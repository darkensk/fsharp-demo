module Gir.Cart.CheckoutIntegration

open Microsoft.IdentityModel.JsonWebTokens
open Microsoft.IdentityModel.Tokens
open Thoth.Json.Net
open FSharp.Data
open Gir.Domain
open Gir.Encoders
open Gir.Decoders
open Gir.Encoders

let mutable partnerAccessTokenCache: string option = None

let mutable purchaseIdCache: string option = None

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

let initPaymentDecoder s =
    match Decode.fromString initPaymentPayloadDecoder s with
    | Ok v ->
        purchaseIdCache <- Some v.PurchaseId
        v.Jwt
    | Error e -> failwithf "Cannot decode init payment, error = %A" e


let reclaimPurchaseToken partnerToken =
    let purchaseId =
        match purchaseIdCache with
        | Some purchaseId -> purchaseId
        | None -> failwith "Cannot reclaim token, purchase not initialized"

    let bearerString = "Bearer " + partnerToken
    let url =
        sprintf "https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/payments/%s/token" purchaseId
    Http.RequestString
        (url,
         headers =
             [ ("Content-Type", "application/json")
               ("Authorization", bearerString) ], httpMethod = "GET")
    |> decodePurchaseToken

let getPurchaseToken (cartState: CartState) partnerToken =
    if (List.isEmpty cartState.Items) then
        ""
    else
        match purchaseIdCache with
        | Some _ -> reclaimPurchaseToken partnerToken
        | None ->
            let encodedPaymentPayload = paymentPayloadEncoder cartState.Items

            let bearerString = "Bearer " + partnerToken
            Http.RequestString
                ("https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/payments",
                 headers =
                     [ ("Content-Type", "application/json")
                       ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload, httpMethod = "POST")
            |> initPaymentDecoder



let updateItems cartState partnerToken =
    if (List.isEmpty cartState.Items) then
        purchaseIdCache <- None
        ""
    else
        match purchaseIdCache with
        | Some purchaseId ->
            let encodedPaymentPayload = paymentPayloadEncoder cartState.Items
            let bearerString = "Bearer " + partnerToken
            let url =
                sprintf "https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/payments/%s/items"
                    purchaseId
            Http.RequestString
                (url,
                 headers =
                     [ ("Content-Type", "application/json")
                       ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload, httpMethod = "PUT")
        | None -> getPurchaseToken cartState partnerToken
