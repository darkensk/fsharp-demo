module Gir.Products.HttpHandlers

open Giraffe
open Gir.Domain
open Gir.Utils
open Views


let listHandler (getProducts: unit -> Product list) next ctx =
    let cartState = Session.getCartState ctx
    htmlView (listView cartState <| getProducts()) next ctx

let detailHandler (getProductById: int -> Product option) (id: int) next ctx =
    let cartState = Session.getCartState ctx
    match getProductById id with
    | Some p -> htmlView (productDetailView cartState p) next ctx
    | None -> (setStatusCode 404 >=> text "Not Found") next ctx
