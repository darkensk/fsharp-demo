module Gir.Test.HttpHandlers

open Giraffe
open Microsoft.AspNetCore.Http
open Views


let testCheckoutHandler (checkoutFrontendBundleUrl: string) (purchaseToken: string) (partnerShippingBundleUrl: string) =
    htmlView
    <| testCheckoutView checkoutFrontendBundleUrl purchaseToken partnerShippingBundleUrl

let easterEggHandler (next: HttpFunc) (ctx: HttpContext) =
    let purchaseToken =
        ctx.Request.Form.Item("purchaseJwt").ToString().Replace("\"", "")

    (redirectTo false <| sprintf "/test/%s" purchaseToken) next ctx
