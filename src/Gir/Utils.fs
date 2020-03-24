module Gir.Utils

open Giraffe.GiraffeViewEngine
open Microsoft.AspNetCore.Http
open Gir.Decoders
open Gir.Domain


let getCartState (ctx: HttpContext) =
    let sessionCart = ctx.Session.GetString("cart")

    let currentCart =
        if isNull sessionCart then "{'items': []}" else sessionCart

    cartDecoder currentCart

let productDiv (product: Product) =
    div [ _class "single-products-catagory clearfix" ]
        [ a [ _href (sprintf "/product/%i" product.ProductId) ]
              [ img
                  [ _src product.Img
                    _alt "" ]
                div [ _class "hover-content" ]
                    [ div [ _class "line" ] []
                      p [] [ str <| sprintf "%.0f kr" product.Price ]
                      h4 [] [ str product.Name ] ] ] ]

[<RequireQualifiedAccess>]
module Task =
    open FSharp.Control.Tasks
    open System.Threading.Tasks

    let map (fn: 'a -> 'b) (v: Task<'a>) =
        task {
            let! value = v
            return fn value }

[<RequireQualifiedAccess>]
module Session =
    let private cartKey = "cart"
    let private purchaseKey = "purchaseId"

    let getCartState (ctx: HttpContext) =
        let currentCart =
            match ctx.Session.GetString(cartKey) with
            | null -> "{'items': []}"
            | v -> v

        cartDecoder currentCart

    let deleteCartState (ctx: HttpContext) = ctx.Session.Remove(cartKey)

    let tryGetPurchaseId (ctx: HttpContext) =
        match ctx.Session.GetString(purchaseKey) with
        | null -> None
        | v -> Some v

    let setPurchaseId (ctx: HttpContext) purchaseId = ctx.Session.SetString(purchaseKey, purchaseId)

    let deletePurchaseId (ctx: HttpContext) = ctx.Session.Remove(purchaseKey)
