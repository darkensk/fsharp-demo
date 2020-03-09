module Gir.Utils

open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Decoders


let getCartState (ctx: HttpContext) =
    let sessionCart = ctx.Session.GetString("cart")

    let currentCart =
        if isNull sessionCart then "{'items': []}" else sessionCart
    
    cartDecoder currentCart