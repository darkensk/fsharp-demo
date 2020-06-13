module Gir.Cart.CheckoutIntegration

open FSharp.Data
open FSharp.Control.Tasks
open Microsoft.IdentityModel.JsonWebTokens
open Gir.Domain
open Gir.Encoders
open Gir.Decoders
open Gir.Utils


let mutable partnerAccessTokenCache: string option = None
let mutable marketCache: string option = None

let getRequestPartnerAccessToken url clientId clientSecret =
    task {
        let getPartnerAccessTokenPayload =
            getPartnerTokenPayloadEncoder clientId clientSecret

        return! Http.AsyncRequestString
                    (url,
                     headers = [ ("Content-Type", "application/json") ],
                     body = TextRequest getPartnerAccessTokenPayload,
                     httpMethod = "POST")
                |> Async.StartAsTask
                |> Task.map decodePartnerAccessToken
    }

let isValid (t: string) =
    let handler = JsonWebTokenHandler()
    let jwt = handler.ReadJsonWebToken(t)
    if jwt.ValidTo > System.DateTime.UtcNow then (Some t) else None

let getCachedToken (url: string) (market: Market) (clientId: string) (clientSecret: string) =
    task {
        let marketString = marketToString market

        let isCorrectMarket =
            match marketCache with
            | Some cachedMarket -> marketString = cachedMarket
            | None -> false

        if isCorrectMarket then
            let validToken =
                partnerAccessTokenCache |> Option.bind isValid

            match validToken with
            | Some v -> return v
            | None ->
                let! token = getRequestPartnerAccessToken url clientId clientSecret
                partnerAccessTokenCache <- Some token
                marketCache <- Some marketString
                return token
        else
            let! token = getRequestPartnerAccessToken url clientId clientSecret
            partnerAccessTokenCache <- Some token
            marketCache <- Some marketString
            return token
    }

let reclaimPurchaseToken (backendUrl: string) (partnerToken: string) (sessionPurchaseId: string) =
    task {
        let bearerString = "Bearer " + partnerToken

        let url =
            sprintf "%s/api/partner/payments/%s/token" backendUrl sessionPurchaseId

        return! Http.AsyncRequestString
                    (url,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ],
                     httpMethod = "GET")
                |> Async.StartAsTask
                |> Task.map decodePurchaseToken
    }

let getPurchaseToken (backendUrl: string) (cartState: CartState) (partnerToken: string) (settings: Settings) =
    task {
        let encodedPaymentPayload =
            paymentPayloadEncoder settings cartState.Items

        let bearerString = "Bearer " + partnerToken

        return! Http.AsyncRequestString
                    (sprintf "%s/api/partner/payments" backendUrl,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ],
                     body = TextRequest encodedPaymentPayload,
                     httpMethod = "POST")
                |> Async.StartAsTask
                |> Task.map initPaymentDecoder
    }

let updateItems
    (backendUrl: string)
    (settings: Settings)
    (cartState: CartState)
    (partnerToken: string)
    (sessionPurchaseId: string)
    =
    task {
        let encodedPaymentPayload =
            paymentPayloadEncoder settings cartState.Items

        let bearerString = "Bearer " + partnerToken

        let url =
            sprintf "%s/api/partner/payments/%s/items" backendUrl sessionPurchaseId

        return! Http.AsyncRequestString
                    (url,
                     headers =
                         [ ("Content-Type", "application/json")
                           ("Authorization", bearerString) ],
                     body = TextRequest encodedPaymentPayload,
                     httpMethod = "PUT")
                |> Async.StartAsTask
                |> Task.map ignore
    }
