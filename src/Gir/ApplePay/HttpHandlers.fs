module Gir.ApplePay.HttpHandlers


open Giraffe
open FSharp.Data
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Views
open Gir.Encoders
open Gir.Domain


let applePayHandler (next: HttpFunc) (ctx: HttpContext) = (htmlView <| applePayView) next ctx

let authorizeMerchant validationUrl =
    task {
        let encodedPayload = paymentSessionPayloadEncoder validationUrl

        return!
            Http.AsyncRequestString(
                url = "http://checkout-dot-com-api/api/apple/payment-session",
                headers = [ ("Content-Type", "application/json") ],
                body = TextRequest encodedPayload,
                httpMethod = "POST"
            )
            |> Async.StartAsTask
    }

let swapTokens (payload: SwapTokenPayload) =
    task {
        let encodedPayload = swapTokensPayloadEncoder payload

        return!
            Http.AsyncRequestString(
                url = "http://checkout-dot-com-api/api/token",
                headers = [ ("Content-Type", "application/json") ],
                body = TextRequest encodedPayload,
                httpMethod = "POST"
            )
            |> Async.StartAsTask
    }

type AuthorizeMerchantPayload = { validationUrl: string }

let authorizeMerchantHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! payload = ctx.BindJsonAsync<AuthorizeMerchantPayload>()
            let! authorizeMerchantResponse = authorizeMerchant payload.validationUrl

            return! text authorizeMerchantResponse next ctx
        }

let swapTokensHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! payload = ctx.BindJsonAsync<SwapTokenPayload>()
            let! swapTokenResponse = swapTokens payload

            return! text swapTokenResponse next ctx
        }
