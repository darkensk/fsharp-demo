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
