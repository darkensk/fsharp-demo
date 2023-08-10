module Gir.Cart.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open System.Threading.Tasks
open Gir.Domain
open Gir.Encoders
open Gir.Utils
open CheckoutIntegration
open Views


let cartEventHandler (sessionCart: CartState) (products: Product list) (cartEvent: CartEvent) =
    match cartEvent with
    | Add p ->
        let currentItems = sessionCart.Items

        let isInTheCart = List.tryFind (fun i -> i.Id = p) currentItems

        match isInTheCart with
        | Some i ->
            let incrementQty changedItem currentItem =
                if currentItem.Id = changedItem.Id then
                    { currentItem with
                        Qty = currentItem.Qty + 1 }
                else
                    currentItem

            let newItems = List.map (incrementQty i) currentItems
            let newCart = { sessionCart with Items = newItems }
            newCart
        | None ->
            match List.tryFind (fun prod -> prod.ProductId = p) products with
            | Some product ->
                let newItem =
                    { Id = p
                      Qty = 1
                      ProductDetail = product }

                let newCart =
                    { sessionCart with
                        Items = currentItems @ [ newItem ] }

                newCart
            | None -> sessionCart

    | Remove p ->
        let currentItems = sessionCart.Items

        let isInTheCart = List.tryFind (fun i -> i.Id = p) currentItems

        match isInTheCart with
        | Some i ->
            let decrementQty changedItem currentItem =
                if currentItem.Id = changedItem.Id then
                    { currentItem with
                        Qty = currentItem.Qty - 1 }
                else
                    currentItem

            let newItems = List.map (decrementQty i) currentItems

            let newItemsWithoutZeroQ =
                List.fold (fun acc x -> if x.Qty = 0 then acc else acc @ [ x ]) [] newItems

            let newCart =
                { sessionCart with
                    Items = newItemsWithoutZeroQ }

            newCart
        | None -> sessionCart
    | Clear -> { Items = [] }

let cartHandler
    (checkoutFrontendBundleUrl: string)
    (getPurchaseToken: CartState -> string -> Settings -> Task<InitializePaymentResponse>)
    (getProducts: unit -> Product list)
    (getPartnerAccessToken: Market -> Task<string>)
    (getReclaimToken: string -> string -> Task<string>)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        if List.isEmpty cartState.Items then
            return! htmlView (cartView settings cartState checkoutFrontendBundleUrl "" <| getProducts ()) next ctx
        else
            let! partnerToken = getPartnerAccessToken settings.Market

            match Session.tryGetPurchaseId ctx with
            | Some v ->
                let! purchaseToken = getReclaimToken partnerToken v

                return!
                    htmlView
                        (cartView settings cartState checkoutFrontendBundleUrl purchaseToken
                         <| getProducts ())
                        next
                        ctx
            | None ->
                let! initPurchaseResponse = getPurchaseToken cartState partnerToken settings
                let purchaseToken = initPurchaseResponse.Jwt
                Session.setPurchaseId ctx initPurchaseResponse.PurchaseId

                return!
                    htmlView
                        (cartView settings cartState checkoutFrontendBundleUrl purchaseToken
                         <| getProducts ())
                        next
                        ctx
    }

let encodeAndSaveCart (ctx: HttpContext) = cartEncoder >> Session.setCartState ctx

let addToCartHandler (productId: int) (getProducts: unit -> Product list) next (ctx: HttpContext) =
    task {
        let sessionCart = Session.getCartState ctx

        do
            productId
            |> CartEvent.Add
            |> cartEventHandler sessionCart (getProducts ())
            |> encodeAndSaveCart ctx

        return! next ctx
    }

let removeFromCartHandler (productId: int) (getProducts: unit -> Product list) next (ctx: HttpContext) =
    task {
        let sessionCart = Session.getCartState ctx

        do
            productId
            |> CartEvent.Remove
            |> cartEventHandler sessionCart (getProducts ())
            |> encodeAndSaveCart ctx

        return! next ctx
    }

let clearCartHandler (getProducts: unit -> Product list) next (ctx: HttpContext) =
    task {
        Session.deletePurchaseId ctx
        let sessionCart = Session.getCartState ctx

        do
            CartEvent.Clear
            |> cartEventHandler sessionCart (getProducts ())
            |> encodeAndSaveCart ctx

        return! next ctx
    }

let updateItemsHandler
    (backendUrl: string)
    (apiPublicUrl: string)
    (getPartnerAccessToken: Market -> Task<string>)
    next
    (ctx: HttpContext)
    =
    task {
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        if List.isEmpty cartState.Items then
            return! next ctx
        else
            let! partnerToken = getPartnerAccessToken settings.Market
            let sessionPurchaseId = Session.tryGetPurchaseId ctx

            match sessionPurchaseId with
            | Some v -> do! updateItems backendUrl apiPublicUrl settings cartState partnerToken v
            | None ->
                let! initPaymentResponse = getPurchaseToken backendUrl apiPublicUrl cartState partnerToken settings
                Session.setPurchaseId ctx initPaymentResponse.PurchaseId

            return! next ctx
    }

let completedHandler (backendUrl: string) (getPartnerAccessToken: Market -> Task<string>) next (ctx: HttpContext) =
    task {
        let settings = Session.getSettings ctx
        let purchaseId = Session.tryGetPurchaseId ctx

        match purchaseId with
        | Some id ->
            let! partnerToken = getPartnerAccessToken settings.Market
            let! getPaymentStatusResponse = getPaymentStatus backendUrl partnerToken id
            Session.deleteCartState ctx
            Session.deletePurchaseId ctx

        | None ->
            Session.deleteCartState ctx
            Session.deletePurchaseId ctx

        return! next ctx
    }

let sessionExpiredHandler
    (backendUrl: string)
    (apiPublicUrl: string)
    (getPartnerAccessToken: Market -> Task<string>)
    next
    (ctx: HttpContext)
    =
    task {
        // Re-use cart and initialize a new purchase
        let cartState = Session.getCartState ctx
        let settings = Session.getSettings ctx

        if List.isEmpty cartState.Items then
            return! next ctx
        else
            let! partnerToken = getPartnerAccessToken settings.Market
            let! initPaymentResponse = getPurchaseToken backendUrl apiPublicUrl cartState partnerToken settings
            Session.setPurchaseId ctx initPaymentResponse.PurchaseId
            return! next ctx
    }
