module Gir.Products.HttpHandlers

open FSharp.Control.Tasks
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Giraffe
open Gir.Domain
open Gir.Utils
open Gir.Encoders
open Gir.Decoders
open Views


let listHandler (getProducts: unit -> Product list) (next: HttpFunc) (ctx: HttpContext) =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx
    htmlView (listView settings cartState <| getProducts ()) next ctx

let detailHandler
    (getPaymentWidgetToken: string -> Task<PaymentWidgetState>)
    (getPartnerAccessToken: Market -> Task<string>)
    (paymentWidgetBundleUrl: string)
    (getProductById: int -> Product option)
    (id: int)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx


        match getProductById id with
        | Some product ->
            if
                isPaymentWidgetEnabledGlobally paymentWidgetBundleUrl
                && settings.PaymentWidgetSettings.Enabled
            then
                match Session.tryGetPaymentWidgeState ctx with
                | Some paymentWidgetState ->
                    let decodedPaymentWidgetState = initPaymentWidgetDecoder paymentWidgetState

                    return!
                        htmlView
                            (productDetailView
                                settings
                                cartState
                                product
                                paymentWidgetBundleUrl
                                (Some decodedPaymentWidgetState))
                            next
                            ctx
                | None ->
                    let! partnerToken = getPartnerAccessToken settings.Market

                    let! initPaymentWidgetResponse = getPaymentWidgetToken partnerToken

                    Session.setPaymentWidgetState ctx (paymentWidgetStateEncoder initPaymentWidgetResponse)

                    return!
                        htmlView
                            (productDetailView
                                settings
                                cartState
                                product
                                paymentWidgetBundleUrl
                                (Some initPaymentWidgetResponse))
                            next
                            ctx
            else
                return! htmlView (productDetailView settings cartState product paymentWidgetBundleUrl None) next ctx
        | None -> return! (setStatusCode 404 >=> text "Not Found") next ctx
    }
