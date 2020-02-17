module Gir.Cart.HttpHandlers

open Giraffe
open Gir.Cart.Views
open Gir.Domain
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http

let cartHandler checkoutFrontendBundleUrl getPurchaseToken next ctx =
    let token = getPurchaseToken()
    htmlView (cartView checkoutFrontendBundleUrl token) next ctx

let addToCartHandler productId (cartEventHandler: CartEvent -> unit) next (ctx: HttpContext) =
    task {
        do productId 
            |> CartEvent.Add
            |> cartEventHandler
        
        return! next ctx
    }