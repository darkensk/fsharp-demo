module Gir.Decoders

open Thoth.Json.Net
open Gir.Domain


let partnerAccessTokenDecoder =
    Decode.field "token" Decode.string
    |> Decode.fromString

let purchaseTokenDecoder =
    Decode.field "jwt" Decode.string
    |> Decode.fromString

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
            { Items =
                  get.Optional.Field "items" (Decode.list cartItemDecoder)
                  |> Option.defaultValue [] })

    match Decode.fromString decoder s with
    | Ok i -> i
    | Error e -> failwithf "Cannot decode cart, error = %A" e

let decodePartnerAccessToken =
    partnerAccessTokenDecoder
    >> function
    | Ok v -> v
    | Error e -> failwithf "Cannot decode partner access token, error = %A" e

let decodePurchaseToken =
    purchaseTokenDecoder
    >> function
    | Ok v -> v
    | Error e -> failwithf "Cannot decode purchase token, error = %A" e

let initPaymentPayloadDecoder =
    Decode.object (fun get ->
        { PurchaseId = get.Required.Field "purchaseId" Decode.string
          Jwt = get.Required.Field "jwt" Decode.string })

let initPaymentDecoder s =
    match Decode.fromString initPaymentPayloadDecoder s with
    | Ok v ->
        { PurchaseId = v.PurchaseId
          Jwt = v.Jwt }
    | Error e -> failwithf "Cannot decode init payment, error = %A" e

let boolToCustomStyles b = if b then Set "{}" else NotSet

let stringToCustomStyles s =
    match s with
    | _ -> NotSet

let extraCheckoutFlagsDecoder =
    Decode.object (fun get ->
        { DisableFocus = get.Required.Field "disableFocus" Decode.bool
          BeforeSubmitCallbackEnabled = get.Required.Field "beforeSubmitCallbackEnabled" Decode.bool
          DeliveryAddressChangedCallbackEnabled = get.Required.Field "deliveryAddressChangedCallbackEnabled" Decode.bool
          CustomStyles =
              (get.Required.Field "customStyles" Decode.string)
              |> stringToCustomStyles })

let extraInitSettingsDecoder =
    Decode.object (fun get ->
        { Language =
              (get.Required.Field "language" Decode.string)
              |> stringToLanguage
          Mode =
              (get.Required.Field "mode" Decode.string)
              |> stringToCheckoutMode
          DifferentDeliveryAddress =
              (get.Required.Field "differentDeliveryAddress" Decode.string)
              |> stringToCheckboxState
          SelectedPaymentMethod =
              (get.Required.Field "selectedPaymentMethod" Decode.string)
              |> stringToSelectedPaymentMethod
          DisplayItems = get.Required.Field "displayItems" Decode.bool
          RecurringPayments =
              (get.Required.Field "recurringPayments" Decode.string)
              |> stringToCheckboxState
          SmsNewsletterSubscription =
              (get.Required.Field "smsNewsletterSubscription" Decode.string)
              |> stringToCheckboxState
          EmailNewsletterSubscription =
              (get.Required.Field "emailNewsletterSubscription" Decode.string)
              |> stringToCheckboxState
          EmailInvoice =
              (get.Required.Field "emailInvoice" Decode.string)
              |> stringToCheckboxState })

let decodeSettings =
    Decode.object (fun get ->
        { ExtraCheckoutFlags = get.Required.Field "extraCheckoutFlags" extraCheckoutFlagsDecoder
          ExtraInitSettings = get.Required.Field "extraInitSettings" extraInitSettingsDecoder
          Market =
              get.Required.Field "market" Decode.string
              |> stringToMarket })

let settingsDecoder s =
    match Decode.fromString decodeSettings s with
    | Ok v ->
        { ExtraCheckoutFlags = v.ExtraCheckoutFlags
          ExtraInitSettings = v.ExtraInitSettings
          Market = v.Market }
    | Error e -> failwithf "Cannot decode settings, error = %A" e
