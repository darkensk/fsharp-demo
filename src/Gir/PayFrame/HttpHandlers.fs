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

let validationHandler (payFrameBundle: string) (next: HttpFunc) (ctx: HttpContext) =
    if not <| String.IsNullOrEmpty(payFrameBundle) then
        next ctx
    else
        redirectTo false "/" next ctx


let payFrameHandler
    (payFrameBundleUrl: string)
    (defaultSiteKey: string)
    (defaultLanguage: string)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx

        let maybeQuerySiteKey = checkIfNotEmpty "siteKey" defaultSiteKey ctx

        let maybeQueryLanguage = checkIfNotEmpty "language" defaultLanguage ctx

        return! htmlView (payFrameView cartState payFrameBundleUrl maybeQuerySiteKey maybeQueryLanguage) next ctx
    }
