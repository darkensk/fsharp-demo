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
open System.Threading.Tasks

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

let cartHandler
    checkoutFrontendBundleUrl
    (getPurchaseToken: CartState -> string -> Task<InitializePaymentResponse>)
    getProducts
    (getPartnerAccessToken: unit -> Task<string>)
    next
    ctx
    =
    task {
        let cartState = Session.getCartState ctx
        if List.isEmpty cartState.Items then
            return! htmlView (cartView cartState checkoutFrontendBundleUrl "" <| getProducts()) next ctx
        else
            let! partnerToken = getPartnerAccessToken()
            let! initPurchaseResponse = getPurchaseToken cartState partnerToken
            do Session.setPurchaseId ctx initPurchaseResponse.PurchaseId |> ignore
            return! htmlView
                        (cartView cartState checkoutFrontendBundleUrl initPurchaseResponse.Jwt <| getProducts()) next
                        ctx
    }

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
        Session.deletePurchaseId |> ignore
        do CartEvent.Clear |> cartEventHandler (getProducts()) ctx

        return! next ctx
    }

let reclaimHandler
    backendUrl
    (checkoutFrontendBundleUrl: string)
    (getPartnerAccessToken: unit -> Task<string>)
    next
    (ctx: HttpContext)
    =
    // Zoberem si partner token
    // Vytiahnem si z CTX purchase ID
    // Spravim reclaim na zaklade ulozeneho purchaseId
    // vytiahnem si cart state aby som vedel zobrazit view
    // Poslem reclaimnuty purchase do view
    // Vidim Checkout3 v completed stepe
    task {
        let! partnerToken = getPartnerAccessToken()
        let maybePurchaseId = Session.tryGetPurchaseId ctx
        let cartState = getCartState ctx

        match maybePurchaseId with
        | Some v ->
            let! purchaseToken = reclaimPurchaseToken backendUrl partnerToken v
            return! htmlView (cartView cartState checkoutFrontendBundleUrl purchaseToken []) next ctx
        | None -> return! htmlView (cartView cartState checkoutFrontendBundleUrl "" []) next ctx
    }

let updateItemsHandler backendUrl (getPartnerAccessToken: unit -> Task<string>) next (ctx: HttpContext) =
    task {
        let! partnerToken = getPartnerAccessToken()
        let cartState = Session.getCartState ctx
        let sessionPurchaseId = Session.tryGetPurchaseId ctx
        match sessionPurchaseId with
        | Some v -> do! updateItems backendUrl cartState partnerToken v
        | None ->
            let! initPaymentResponse = getPurchaseToken backendUrl cartState partnerToken
            do Session.setPurchaseId ctx initPaymentResponse.PurchaseId |> ignore

        return! next ctx
    }

let completedHandler next (ctx: HttpContext) =
    task {
        Session.deleteCartState |> ignore
        Session.deletePurchaseId |> ignore
        return! next ctx
    }
