module Gir.Cart.HttpHandlers

open Giraffe
open Gir.Cart.Views
open Gir.Domain
open FSharp.Control.Tasks
open Microsoft.AspNetCore.Http
open Gir.Decoders
open Gir.Encoders
open Gir.Utils



let cartEventHandler (ctx: HttpContext) cartEvent =
    let sessionCart = ctx.Session.GetString("cart")

    let currentCart =
        if isNull sessionCart then "{'items': []}" else sessionCart

    match cartEvent with
    | Add p ->
        let decodedCart = cartDecoder currentCart
        let currentItems = decodedCart.Items
        let isInTheCart = List.tryFind (fun i -> i.Id = p) currentItems
        match isInTheCart with
        | Some i ->
            let incrementQty changedItem currentItem =
                if currentItem.Id = changedItem.Id
                then { currentItem with Qty = currentItem.Qty + 1 }
                else currentItem

            let newItems = List.map (incrementQty i) currentItems
            let newCart = { decodedCart with Items = newItems }
            ctx.Session.SetString("cart", cartEncoder newCart)
        | None ->
            let newItem =
                { Id = p
                  Qty = 1 }

            let newCart = { decodedCart with Items = currentItems @ [ newItem ] }
            ctx.Session.SetString("cart", cartEncoder newCart)

    | Remove p ->
        let decodedCart = cartDecoder currentCart
        let currentItems = decodedCart.Items
        let isInTheCart = List.tryFind (fun i -> i.Id = p) currentItems
        match isInTheCart with
        | Some i ->
            let decrementQty changedItem currentItem =
                if currentItem.Id = changedItem.Id
                then { currentItem with Qty = currentItem.Qty - 1 }
                else currentItem

            let newItems = List.map (decrementQty i) currentItems
            let newCart = { decodedCart with Items = newItems }
            ctx.Session.SetString("cart", cartEncoder newCart)
        | None -> ctx.Session.SetString("cart", cartEncoder decodedCart)
    | Clear -> ctx.Session.SetString("cart", "{'items': []}")


let cartHandler checkoutFrontendBundleUrl getPurchaseToken next ctx =
    let token = getPurchaseToken()
    let cartState = getCartState ctx
    htmlView (cartView cartState checkoutFrontendBundleUrl token) next ctx

let addToCartHandler productId next (ctx: HttpContext) =
    task {
        do productId
           |> CartEvent.Add
           |> cartEventHandler ctx

        return! next ctx
    }

let removeFromCartHandler productId next (ctx: HttpContext) =
    task {
        do productId
           |> CartEvent.Remove
           |> cartEventHandler ctx

        return! next ctx
    }

let clearCartHandler next (ctx: HttpContext) =
    task {
        do CartEvent.Clear |> cartEventHandler ctx

        return! next ctx
    }
