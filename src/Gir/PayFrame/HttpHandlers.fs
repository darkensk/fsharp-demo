module Gir.PayFrame.HttpHandlers

open Views
open Giraffe
open Gir.Domain
open Gir.Utils
open Microsoft.AspNetCore.Http
open System


let checkIfNotEmpty (queryParamKey: string) (defaultValue: string) (ctx: HttpContext) =
    match ctx.TryGetQueryStringValue queryParamKey with
    | None -> defaultValue
    | Some value -> if String.IsNullOrEmpty(value) then defaultValue else value

let payFrameHandler
    (payFrameBundleUrl: string)
    (defaultSiteKey: string)
    (defaultDomain: string)
    (defaultLanguage: string)
    (getProducts: unit -> Product list)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        let maybeQuerySiteKey = checkIfNotEmpty "siteKey" defaultSiteKey ctx

        let maybeQueryDomain = checkIfNotEmpty "domain" defaultDomain ctx


        let maybeQueryLanguage = checkIfNotEmpty "language" defaultLanguage ctx


        return!
            htmlView
                (payFrameView settings cartState payFrameBundleUrl maybeQuerySiteKey maybeQueryDomain maybeQueryLanguage
                 <| getProducts ())
                next
                ctx
    }
