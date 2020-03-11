module Gir.Cart.CheckoutIntegration

open FSharp.Data
open Microsoft.AspNetCore.Http
open Microsoft.IdentityModel.JsonWebTokens
open Microsoft.IdentityModel.Tokens
open Thoth.Json.Net
open Gir.Domain
open Gir.Encoders
open Gir.Decoders


let mutable partnerAccessTokenCache: string option = None

let decodePartnerAccessToken =
    partnerAccessTokenDecoder
    >> function
    | Ok v -> v
    | Error e -> failwithf "Cannot decode partner access token, error = %A" e

let getPartnerAccessToken url clientId clientSecret =
    let getPartnerAccessTokenPayload = getPartnerTokenPayloadEncoder clientId clientSecret
    Http.RequestString
        (url, headers = [ ("Content-Type", "application/json") ], body = TextRequest getPartnerAccessTokenPayload,
         httpMethod = "POST") |> decodePartnerAccessToken

let createValidationParameters =
    let validationParameters = TokenValidationParameters()
    validationParameters.ValidateLifetime <- true
    validationParameters

let isValid t =
    let handler = JsonWebTokenHandler()
    let validationResult = handler.ValidateToken(t, createValidationParameters)
    if validationResult.IsValid then (Some t) else None

let getCachedToken url clientId clientSecret =
    partnerAccessTokenCache
    |> Option.bind isValid
    |> Option.defaultWith (fun _ ->
        let token = getPartnerAccessToken url clientId clientSecret
        partnerAccessTokenCache <- Some token
        token)

let decodePurchaseToken =
    purchaseTokenDecoder
    >> function
    | Ok v -> v
    | Error e -> failwithf "Cannot decode purchase token, error = %A" e

let initPaymentPayloadDecoder =
    Decode.object (fun get ->
        { PurchaseId = get.Required.Field "purchaseId" Decode.string
          Jwt = get.Required.Field "jwt" Decode.string })

let initPaymentDecoder (ctx: HttpContext) s =
    match Decode.fromString initPaymentPayloadDecoder s with
    | Ok v ->
        ctx.Session.SetString("purchaseId", v.PurchaseId)
        v.Jwt
    | Error e -> failwithf "Cannot decode init payment, error = %A" e

let reclaimPurchaseToken backendUrl partnerToken (ctx: HttpContext) =
    let sessionPurchaseId = ctx.Session.GetString("purchaseId")

    if isNull sessionPurchaseId then
        failwith "Cannot reclaim token, purchase not initialized"
    else
        let bearerString = "Bearer " + partnerToken
        let url = sprintf "%s/api/partner/payments/%s/token" backendUrl sessionPurchaseId
        Http.RequestString
            (url,
             headers =
                 [ ("Content-Type", "application/json")
                   ("Authorization", bearerString) ], httpMethod = "GET")
        |> decodePurchaseToken

let getPurchaseToken backendUrl (cartState: CartState) partnerToken (ctx: HttpContext) =
    let sessionPurchaseId = ctx.Session.GetString("purchaseId")

    if (List.isEmpty cartState.Items) then
        ""
    else if isNull sessionPurchaseId then
        let encodedPaymentPayload = paymentPayloadEncoder cartState.Items

        let bearerString = "Bearer " + partnerToken
        Http.RequestString
            (sprintf "%s/api/partner/payments" backendUrl,
             headers =
                 [ ("Content-Type", "application/json")
                   ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload, httpMethod = "POST")
        |> initPaymentDecoder ctx
    else
        reclaimPurchaseToken backendUrl partnerToken ctx

let updateItems backendUrl cartState partnerToken (ctx: HttpContext) =
    let sessionPurchaseId = ctx.Session.GetString("purchaseId")
    if (List.isEmpty cartState.Items) then
        ""
    else if isNull sessionPurchaseId then
        getPurchaseToken backendUrl cartState partnerToken ctx
    else
        let encodedPaymentPayload = paymentPayloadEncoder cartState.Items
        let bearerString = "Bearer " + partnerToken
        let url = sprintf "%s/api/partner/payments/%s/items" backendUrl sessionPurchaseId
        Http.RequestString
            (url,
             headers =
                 [ ("Content-Type", "application/json")
                   ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload, httpMethod = "PUT")
