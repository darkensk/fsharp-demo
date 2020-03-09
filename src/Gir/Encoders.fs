module Gir.Encoders

open Gir.Domain
open Thoth.Json.Net


let cartEncoder cartState =
    let encodeCartItem cartItem =
        Encode.object
            [ "id", Encode.int cartItem.Id
              "qty", Encode.int cartItem.Qty ]
    Encode.object [ "items", Encode.list <| List.map (encodeCartItem) cartState.Items ] |> Encode.toString 0