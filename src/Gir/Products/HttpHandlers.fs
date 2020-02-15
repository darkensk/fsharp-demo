module Gir.Products.HttpHandlers

open Giraffe
open Views

let listHandler getProducts =
    htmlView (listView <| getProducts())


let detailHandler getProductById id =
    match getProductById id with
    | Some p -> htmlView (productDetailView p)
    | None -> setStatusCode 404 >=> text "Not Found"


