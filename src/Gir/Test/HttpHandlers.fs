module Gir.Test.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Views


let testCheckoutHandler (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    htmlView
    <| testCheckoutView checkoutFrontendBundleUrl purchaseToken

let easterEggHandler (checkoutFrontendBundleUrl: string) next (ctx: HttpContext) =
    let purchaseToken =
        ctx.Request.Form.Item("purchaseJwt").ToString()

    htmlView (testCheckoutView checkoutFrontendBundleUrl purchaseToken) next ctx
