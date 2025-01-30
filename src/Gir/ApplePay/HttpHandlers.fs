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
                // local network
                url = "http://checkout-dot-com-api/api/apple/payment-session",
                // dev
                // url = "https://checkout-dot-com.local/api/apple/payment-session",
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
