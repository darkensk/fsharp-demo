module Gir.Products.HttpHandlers

open Microsoft.AspNetCore.Http
open Giraffe
open Gir.Domain
open Gir.Utils
open Views


let listHandler (getProducts: unit -> Product list) (next: HttpFunc) (ctx: HttpContext) =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx
    htmlView (listView settings cartState <| getProducts ()) next ctx

let detailHandler (getProductById: int -> Product option) (id: int) (next: HttpFunc) (ctx: HttpContext) =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx
    match getProductById id with
    | Some p -> htmlView (productDetailView settings cartState p) next ctx
    | None -> (setStatusCode 404 >=> text "Not Found") next ctx
