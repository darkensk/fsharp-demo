module Gir.Decoders

open Thoth.Json.Net
open Gir.Domain


let productDecoder =
    Decode.object (fun get ->
        { ProductId = get.Required.Field "productId" Decode.int
          Name = get.Required.Field "name" Decode.string
          Price = get.Required.Field "price" Decode.float
          Img = get.Required.Field "img" Decode.string })


let cartItemDecoder =
    Decode.object (fun get ->
        { Id = get.Required.Field "id" Decode.int
          Qty = get.Required.Field "qty" Decode.int
          ProductDetail = get.Required.Field "product" productDecoder })

let cartDecoder s =
    let decoder =
        Decode.object (fun get ->
            { Items = get.Optional.Field "items" (Decode.list cartItemDecoder) |> Option.defaultValue [] })
    match Decode.fromString decoder s with
    | Ok i -> i
    | Error e -> failwithf "Cannot parse cart, error = %A" e
