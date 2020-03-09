module Gir.Decoders

open Thoth.Json.Net
open Gir.Domain

let cartDecoder s =
    let decoderCartItem =
        Decode.object (fun get ->
            { Id = get.Required.Field "id" Decode.int
              Qty = get.Required.Field "qty" Decode.int })

    let decoder =
        Decode.object (fun get ->
            { Items = get.Optional.Field "items" (Decode.list decoderCartItem) |> Option.defaultValue [] })
    match Decode.fromString decoder s with
    | Ok i -> i
    | Error e -> failwithf "Cannot parse cart, error = %A" e