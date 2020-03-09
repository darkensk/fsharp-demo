module Gir.Products.HttpHandlers

open Giraffe
open Views

let listHandler cartState getProducts =
    htmlView (listView cartState <| getProducts())


let detailHandler cartState getProductById id =
    match getProductById id with
    | Some p -> htmlView (productDetailView cartState p)
    | None -> setStatusCode 404 >=> text "Not Found"


