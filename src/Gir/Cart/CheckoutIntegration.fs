module Gir.Cart.CheckoutIntegration

open FSharp.Data
open FSharp.Control.Tasks
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.IdentityModel.JsonWebTokens
open Microsoft.IdentityModel.Tokens
open Thoth.Json.Net
open Gir.Domain
open Gir.Encoders
open Gir.Decoders
open Gir.Utils


let mutable partnerAccessTokenCache: string option = None

let decodePartnerAccessToken =
    partnerAccessTokenDecoder
    >> function
    | Ok v -> v
    | Error e -> failwithf "Cannot decode partner access token, error = %A" e

let getRequestPartnerAccessToken url clientId clientSecret =
    let getPartnerAccessTokenPayload = getPartnerTokenPayloadEncoder clientId clientSecret
    Http.AsyncRequestString
        (url, headers = [ ("Content-Type", "application/json") ], body = TextRequest getPartnerAccessTokenPayload,
         httpMethod = "POST")
    |> Async.StartAsTask
    |> Task.map decodePartnerAccessToken

let createValidationParameters =
    let validationParameters = TokenValidationParameters()
    validationParameters.ValidateLifetime <- true
    validationParameters

let isValid t =
    let handler = JsonWebTokenHandler()
    let validationResult = handler.ValidateToken(t, createValidationParameters)
    if validationResult.IsValid then (Some t) else None

let getCachedToken url clientId clientSecret =
    task {
        let validToken = partnerAccessTokenCache |> Option.bind isValid
        match validToken with
        | Some v -> return v
        | None ->
            let! token = getRequestPartnerAccessToken url clientId clientSecret
            partnerAccessTokenCache <- Some token
            return token
    }

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
        { PurchaseId = v.PurchaseId
          Jwt = v.Jwt }
    | Error e -> failwithf "Cannot decode init payment, error = %A" e

let reclaimPurchaseToken backendUrl partnerToken sessionPurchaseId =
    task {
        let bearerString = "Bearer " + partnerToken
        let url = sprintf "%s/api/partner/payments/%s/token" backendUrl sessionPurchaseId
        return! Http.AsyncRequestString
                    (url,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ], httpMethod = "GET")
                |> Async.StartAsTask
                |> Task.map decodePurchaseToken
    }

let getPurchaseToken backendUrl (cartState: CartState) partnerToken =
    task {
        let encodedPaymentPayload = paymentPayloadEncoder cartState.Items

        let bearerString = "Bearer " + partnerToken

        return! Http.AsyncRequestString
                    (sprintf "%s/api/partner/payments" backendUrl,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload,
                     httpMethod = "POST")
                |> Async.StartAsTask
                |> Task.map initPaymentDecoder
    }


let updateItems backendUrl cartState partnerToken sessionPurchaseId =
    task {
        let encodedPaymentPayload = paymentPayloadEncoder cartState.Items
        let bearerString = "Bearer " + partnerToken
        let url = sprintf "%s/api/partner/payments/%s/items" backendUrl sessionPurchaseId
        return! Http.AsyncRequestString
                    (url,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload,
                     httpMethod = "PUT")
                |> Async.StartAsTask
                |> Task.map ignore
    }
