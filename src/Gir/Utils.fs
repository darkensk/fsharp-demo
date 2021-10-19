module Gir.Utils

open Giraffe.GiraffeViewEngine
open Microsoft.AspNetCore.Http
open Decoders
open Encoders
open Domain


let marketToCurrency (market: Market) =
    match market with
    | Sweden -> "kr"
    | Finland -> "EUR"
    | Norway -> "kr"
    | Denmark -> "kr."
    | Slovakia -> "€"
    | Czechia -> "kč"
    | Poland -> "zł"
    | Latvia -> "€"
    | Estonia -> "€"
    | International -> "€"

let productDiv (settings: Settings) (product: Product) =
    div [ _class "single-products-catagory clearfix" ] [
        a [ _href (sprintf "/product/%i" product.ProductId) ] [
            img [ _src product.Img; _alt "" ]
            div [ _class "hover-content" ] [
                div [ _class "line" ] []
                p [] [
                    str
                    <| sprintf "%M %s" product.Price (marketToCurrency settings.Market)
                ]
                h4 [] [ str product.Name ]
            ]
        ]
    ]

[<RequireQualifiedAccess>]
module Task =
    open FSharp.Control.Tasks
    open System.Threading.Tasks

    let map (fn: 'a -> 'b) (v: Task<'a>) =
        task {
            let! value = v
            return fn value
        }

[<RequireQualifiedAccess>]
module Session =
    let private cartKey = "cart"
    let private purchaseKey = "purchaseId"
    let private settingsKey = "settings"

    let getCartState (ctx: HttpContext) =
        let currentCart =
            match ctx.Session.GetString(cartKey) with
            | null -> "{'items': []}"
            | v -> v

        cartDecoder currentCart

    let setCartState (ctx: HttpContext) (cartState: string) =
        ctx.Session.SetString(cartKey, cartState)

    let deleteCartState (ctx: HttpContext) = ctx.Session.Remove(cartKey)

    let tryGetPurchaseId (ctx: HttpContext) =
        match ctx.Session.GetString(purchaseKey) with
        | null -> None
        | v -> Some v

    let setPurchaseId (ctx: HttpContext) (purchaseId: string) =
        ctx.Session.SetString(purchaseKey, purchaseId)

    let deletePurchaseId (ctx: HttpContext) = ctx.Session.Remove(purchaseKey)

    let getSettings (ctx: HttpContext) =
        match ctx.Session.GetString(settingsKey) with
        | null -> defaultSettings
        | v -> settingsDecoder v

    let setSettings (ctx: HttpContext) (settings: Settings) =
        let encodedSettings = settingsEncoder settings
        ctx.Session.SetString(settingsKey, encodedSettings)
