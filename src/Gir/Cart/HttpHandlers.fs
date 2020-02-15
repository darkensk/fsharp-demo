module Gir.Cart.HttpHandlers

open Giraffe
open Gir.Cart.Views

let cartHandler getPurchaseToken next ctx =
    let token = getPurchaseToken()
    htmlView (cartView token) next ctx
