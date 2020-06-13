module Gir.Test.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Views


let testCheckoutHandler (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    htmlView
    <| testCheckoutView checkoutFrontendBundleUrl purchaseToken

let easterEggHandler (next: HttpFunc) (ctx: HttpContext) =
    let purchaseToken =
        ctx.Request.Form.Item("purchaseJwt").ToString()

    (redirectTo false
     <| sprintf "/test/%s" purchaseToken) next ctx
