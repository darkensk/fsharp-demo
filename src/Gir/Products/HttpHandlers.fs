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
    (getPartPaymentWidgetToken: string -> Task<PartPaymentWidgetState>)
    (getPartnerAccessToken: Market -> Task<string>)
    (partPaymentWidgetBundleUrl: string)
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
                isPartPaymentWidgetEnabledGlobally partPaymentWidgetBundleUrl
                && settings.PartPaymentWidgetSettings.Enabled
            then
                match Session.tryGetPartPaymentWidgeState ctx with
                | Some partPaymentWidgetState ->
                    let decodedPartPaymentWidgetState =
                        initPartPaymentWidgetDecoder partPaymentWidgetState

                    return!
                        htmlView
                            (productDetailView
                                settings
                                cartState
                                product
                                partPaymentWidgetBundleUrl
                                (Some decodedPartPaymentWidgetState))
                            next
                            ctx
                | None ->
                    let! partnerToken = getPartnerAccessToken settings.Market

                    let! initPartPaymentWidgetResponse = getPartPaymentWidgetToken partnerToken

                    Session.setPartPaymentWidgetState ctx (partPaymentWidgetStateEncoder initPartPaymentWidgetResponse)

                    return!
                        htmlView
                            (productDetailView
                                settings
                                cartState
                                product
                                partPaymentWidgetBundleUrl
                                (Some initPartPaymentWidgetResponse))
                            next
                            ctx
            else
                return! htmlView (productDetailView settings cartState product partPaymentWidgetBundleUrl None) next ctx
        | None -> return! (setStatusCode 404 >=> text "Not Found") next ctx
    }
