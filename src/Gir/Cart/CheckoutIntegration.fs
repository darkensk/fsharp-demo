module Gir.Cart.CheckoutIntegration

open Thoth.Json.Net
open FSharp.Data
open Gir.Domain
open Gir.Encoders

let mutable tokenCache: string option = None

let parseToken (s: string) =
    s
    |> Decode.fromString (Decode.field "token" Decode.string)
    |> function
    | Ok v -> v
    | Error e -> failwithf "Cannot parse token, error = %A" e

let getMerchantToken url clientId clientSecret =
    let merchantAccessString =
        Encode.object
            [ "clientId", Encode.string clientId
              "clientSecret", Encode.string clientSecret ]
        |> Encode.toString 0
    Http.RequestString
        (url, headers = [ ("Content-Type", "application/json") ], body = TextRequest merchantAccessString) |> parseToken

let isValid t =
    if true then (Some t) else None

let getCachedToken url clientId clientSecret =
    tokenCache
    |> Option.bind isValid
    |> Option.defaultWith (fun _ ->
        let token = getMerchantToken url clientId clientSecret
        tokenCache <- Some token
        token)

let parsePurchaseToken (s: string) =
    s
    |> Decode.fromString (Decode.field "jwt" Decode.string)
    |> function
    | Ok v -> v
    | Error e -> failwithf "Cannot parse purchase token, error = %A" e

let getPurchaseToken (cartState: CartState) merchantToken =
    if (List.isEmpty cartState.Items) then
        ""
    else
        let encodedPaymentPayload = paymentPayloadEncoder cartState.Items

        let bearerString = "Bearer " + merchantToken
        Http.RequestString
            ("https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/payments",
             headers =
                 [ ("Content-Type", "application/json")
                   ("Authorization", bearerString) ], body = TextRequest encodedPaymentPayload)
        |> parsePurchaseToken
