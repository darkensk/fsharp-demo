module Gir.Products.HttpHandlers

open Giraffe
open Views
open Gir.Utils

let listHandler getProducts next ctx =
    let cartState = getCartState ctx
    htmlView (listView cartState <| getProducts()) next ctx


let detailHandler getProductById id next ctx =
    let cartState = getCartState ctx
    match getProductById id with
    | Some p -> htmlView (productDetailView cartState p) next ctx
    | None -> (setStatusCode 404 >=> text "Not Found") next ctx
