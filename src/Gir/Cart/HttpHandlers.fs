module Gir.Cart.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Decoders
open Gir.Encoders
open Gir.Utils
open CheckoutIntegration
open Views


let cartEventHandler (products: Product list) (ctx: HttpContext) cartEvent =
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
            match List.tryFind (fun prod -> prod.ProductId = p) products with
            | Some product ->
                let newItem =
                    { Id = p
                      Qty = 1
                      ProductDetail = product }

                let newCart = { decodedCart with Items = currentItems @ [ newItem ] }
                ctx.Session.SetString("cart", cartEncoder newCart)
            | None -> ctx.Session.SetString("cart", cartEncoder decodedCart)

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

            let newItemsWithoutZeroQ =
                List.fold (fun acc x ->
                    if x.Qty = 0 then acc else acc @ [ x ]) [] newItems

            let newCart = { decodedCart with Items = newItemsWithoutZeroQ }
            ctx.Session.SetString("cart", cartEncoder newCart)
        | None -> ctx.Session.SetString("cart", cartEncoder decodedCart)
    | Clear -> ctx.Session.SetString("cart", "{'items': []}")

let cartHandler checkoutFrontendBundleUrl getPurchaseToken getProducts next ctx =
    let purchaseToken = getPurchaseToken (getCartState ctx) ctx
    let cartState = getCartState ctx
    htmlView (cartView cartState checkoutFrontendBundleUrl purchaseToken <| getProducts()) next ctx

let addToCartHandler productId getProducts next (ctx: HttpContext) =
    task {
        do productId
           |> CartEvent.Add
           |> cartEventHandler (getProducts()) ctx

        return! next ctx
    }

let removeFromCartHandler productId getProducts next (ctx: HttpContext) =
    task {
        do productId
           |> CartEvent.Remove
           |> cartEventHandler (getProducts()) ctx

        return! next ctx
    }

let clearCartHandler getProducts next (ctx: HttpContext) =
    task {
        ctx.Session.Remove("purchaseId")
        do CartEvent.Clear |> cartEventHandler (getProducts()) ctx

        return! next ctx
    }

let reclaimHandler backendUrl (checkoutFrontendBundleUrl: string) merchantToken next (ctx: HttpContext) =
    let purchaseToken = reclaimPurchaseToken backendUrl (merchantToken()) ctx
    let cartState = getCartState ctx
    htmlView (cartView cartState checkoutFrontendBundleUrl purchaseToken []) next ctx

let updateItemsHandler backendUrl merchantToken (next: HttpFunc) (ctx: HttpContext) =
    task {
        let cartState = getCartState ctx
        do updateItems backendUrl cartState (merchantToken()) ctx |> ignore
        return! next ctx
    }
