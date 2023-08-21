module Gir.Products.PaymentWidgetIntegration

open FSharp.Data
open FSharp.Control.Tasks
open Gir.Decoders
open Gir.Utils


let getPaymentWidgetToken (backendUrl: string) (partnerToken: string) =
    task {
        let bearerString = "Bearer " + partnerToken

        return!
            Http.AsyncRequestString(
                sprintf "%s/api/paymentwidget/partner/init" backendUrl,
                headers = [ ("Content-Type", "application/json"); ("Authorization", bearerString) ],
                httpMethod = "GET"
            )
            |> Async.StartAsTask
            |> Task.map initPaymentWidgetDecoder
    }
