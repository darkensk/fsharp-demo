module Gir.Cart.CheckoutIntegration
open Thoth.Json.Net
open FSharp.Data

let mutable tokenCache: string option = None

let parseToken (s: string) =
    s
    |> Decode.fromString (Decode.field "token" Decode.string)
    |> function
    | Ok v -> v
    | Error e -> failwithf "Cannot parse token, error = %A" e

let getAccessToken clientId clientSecret =
    let merchantAccessString =
        Encode.object
            [ "clientId", Encode.string clientId
              "clientSecret", Encode.string clientSecret ]
        |> Encode.toString 0
    Http.RequestString
        ("https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/tokens",
         headers = [ ("Content-Type", "application/json") ], body = TextRequest merchantAccessString) |> parseToken

let isValid t =
    if true then (Some t) else None

let getCachedToken clientId clientSecret =
    tokenCache
    |> Option.bind isValid
    |> Option.defaultWith (fun _ ->
        let token = getAccessToken clientId clientSecret
        tokenCache <- Some token
        token)

let parsePurchaseToken (s: string) =
    s
    |> Decode.fromString (Decode.field "jwt" Decode.string)
    |> function
    | Ok v -> v
    | Error e -> failwithf "Cannot parse purchase token, error = %A" e

let getPurchaseToken merchantToken =
    let newPaymentPayload =
        Encode.object
            [ "language", Encode.string "English"
              "items",
              Encode.list <| List.singleton
                                 (Encode.object
                                     [ "description", Encode.string "Test Item 1"
                                       "notes", Encode.string "Test Note 1"
                                       "amount", Encode.float 100.
                                       "taxCode", Encode.string "20%"
                                       "taxAmount", Encode.float 20. ])
              "orderReference", Encode.string "TEST-AVARDA-ORDER-X"
              "displayItems", Encode.bool true
              "differentDeliveryAddress", Encode.string "Checked"
              "deliveryAddress",
              Encode.object
                  [ "address1", Encode.string "Smetanova"
                    "address2", Encode.string ""
                    "zip", Encode.string "30593"
                    "city", Encode.string "Halmstad"
                    "country", Encode.string "SE"
                    "firstName", Encode.string "Rudolf"
                    "lastName", Encode.string "Halmstad" ] ]
        |> Encode.toString 0

    let bearerString = "Bearer " + merchantToken
    Http.RequestString
        ("https://avdonl-t-checkout.westeurope.cloudapp.azure.com/api/partner/payments",
         headers =
             [ ("Content-Type", "application/json")
               ("Authorization", bearerString) ], body = TextRequest newPaymentPayload)
    |> parsePurchaseToken