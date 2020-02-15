module Gir.Cart.HttpHandlers

open Giraffe
open Gir.Cart.Views

let cartHandler checkoutFrontendBundleUrl getPurchaseToken next ctx =
    let token = getPurchaseToken()
    htmlView (cartView checkoutFrontendBundleUrl token) next ctx
