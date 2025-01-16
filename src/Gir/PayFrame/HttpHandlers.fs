module Gir.PayFrame.HttpHandlers

open Views
open Giraffe
open Gir.Domain
open Gir.Utils
open Microsoft.AspNetCore.Http

let payFrameHandler (getProducts: unit -> Product list) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        return! htmlView (payFrameView settings cartState <| getProducts ()) next ctx

    }
