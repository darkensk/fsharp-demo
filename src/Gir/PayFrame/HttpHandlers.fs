module Gir.PayFrame.HttpHandlers

open Views
open Giraffe
open Gir.Domain
open Gir.Utils
open Microsoft.AspNetCore.Http

let payFrameHandler
    (payFrameBudleUrl: string)
    (siteKey: string)
    (getProducts: unit -> Product list)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        return! htmlView (payFrameView settings cartState payFrameBudleUrl siteKey <| getProducts ()) next ctx
    }
