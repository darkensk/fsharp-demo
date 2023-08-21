module Gir.Decoders

open Thoth.Json.Net
open Domain


let partnerAccessTokenDecoder =
    Decode.field "token" Decode.string |> Decode.fromString

let purchaseTokenDecoder = Decode.field "jwt" Decode.string |> Decode.fromString

let productDecoder =
    Decode.object (fun get ->
        { ProductId = get.Required.Field "productId" Decode.int
          Name = get.Required.Field "name" Decode.string
          Price = get.Required.Field "price" Decode.decimal
          Img = get.Required.Field "img" Decode.string })

let cartItemDecoder =
    Decode.object (fun get ->
        { Id = get.Required.Field "id" Decode.int
          Qty = get.Required.Field "qty" Decode.int
          ProductDetail = get.Required.Field "product" productDecoder })

let cartDecoder (s: string) =
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

let initPaymentDecoder (s: string) =
    match Decode.fromString initPaymentPayloadDecoder s with
    | Ok v ->
        { PurchaseId = v.PurchaseId
          Jwt = v.Jwt }
    | Error e -> failwithf "Cannot decode init payment, error = %A" e

let extraCheckoutFlagsDecoder =
    Decode.object (fun get ->
        { DisableFocus = get.Required.Field "disableFocus" Decode.bool
          BeforeSubmitCallbackEnabled = get.Required.Field "beforeSubmitCallbackEnabled" Decode.bool
          DeliveryAddressChangedCallbackEnabled =
            get.Required.Field "deliveryAddressChangedCallbackEnabled" Decode.bool
          CustomStyles = get.Required.Field "customStyles" Decode.bool
          IncludePaymentFeeInTotalPrice = get.Required.Field "includePaymentFeeInTotalPrice" Decode.bool })

let extraInitSettingsDecoder =
    Decode.object (fun get ->
        { Language = (get.Required.Field "language" Decode.string) |> stringToLanguage
          Mode = (get.Required.Field "mode" Decode.string) |> stringToCheckoutMode
          DifferentDeliveryAddress =
            (get.Required.Field "differentDeliveryAddress" Decode.string)
            |> stringToCheckboxState
          SelectedPaymentMethod =
            (get.Required.Field "selectedPaymentMethod" Decode.string)
            |> stringToSelectedPaymentMethod
          DisplayItems = get.Required.Field "displayItems" Decode.bool
          RecurringPayments = (get.Required.Field "recurringPayments" Decode.string) |> stringToCheckboxState
          SmsNewsletterSubscription =
            (get.Required.Field "smsNewsletterSubscription" Decode.string)
            |> stringToCheckboxState
          EmailNewsletterSubscription =
            (get.Required.Field "emailNewsletterSubscription" Decode.string)
            |> stringToCheckboxState
          BackendNotification =
            (get.Required.Field "completedNotificationUrl" Decode.string)
            |> stringToBackendNotificationState
          EnableB2BLink = get.Required.Field "enableB2BLink" Decode.bool
          EnableCountrySelector = get.Required.Field "enableCountrySelector" Decode.bool
          ShowThankYouPage = get.Required.Field "showThankYouPage" Decode.bool
          AgeValidation = (get.Required.Field "ageValidation" Decode.string) |> stringToAgeValidation })

let paymentWidgetSettingsDecoder =
    Decode.object (fun get ->
        { Enabled = get.Required.Field "enabled" Decode.bool
          CustomStyles = get.Required.Field "customStyles" Decode.bool })

let decodeSettings =
    Decode.object (fun get ->
        { ExtraCheckoutFlags = get.Required.Field "extraCheckoutFlags" extraCheckoutFlagsDecoder
          ExtraInitSettings = get.Required.Field "extraInitSettings" extraInitSettingsDecoder
          Market = get.Required.Field "market" Decode.string |> stringToMarket
          OrderReference =
            (get.Optional.Field "orderReference" Decode.string)
            |> Option.defaultValue defaultSettings.OrderReference
          PaymentWidgetSettings = get.Required.Field "paymentWidgetSettings" paymentWidgetSettingsDecoder })

let settingsDecoder (s: string) =
    match Decode.fromString decodeSettings s with
    | Ok v ->
        { ExtraCheckoutFlags = v.ExtraCheckoutFlags
          ExtraInitSettings = v.ExtraInitSettings
          Market = v.Market
          OrderReference = v.OrderReference
          PaymentWidgetSettings = v.PaymentWidgetSettings }
    | Error e -> failwithf "Cannot decode settings, error = %A" e


let decodeExtraIdentifiers =
    Decode.object (fun get -> { OrderReference = get.Required.Field "orderReference" Decode.string })


let decodePaymentStatus =
    Decode.object (fun get ->
        { PurchaseId = get.Required.Field "purchaseId" Decode.string
          ExtraIdentifiers = get.Required.Field "extraIdentifiers" decodeExtraIdentifiers
          Mode = get.Required.Field "mode" Decode.string })


let getPaymentStatusDecoder (s: string) =
    match Decode.fromString decodePaymentStatus s with
    | Ok v ->
        { PurchaseId = v.PurchaseId
          ExtraIdentifiers = v.ExtraIdentifiers
          Mode = v.Mode }
    | Error e -> failwithf "Cannot decode get payment status, error = %A" e


let initPaymentWidgetPayloadDecoder =
    Decode.object (fun get ->
        { PaymentId = get.Required.Field "paymentId" Decode.string
          WidgetJwt = get.Required.Field "widgetJwt" Decode.string })

let initPaymentWidgetDecoder (s: string) =
    match Decode.fromString initPaymentWidgetPayloadDecoder s with
    | Ok v ->
        { PaymentId = v.PaymentId
          WidgetJwt = v.WidgetJwt }
    | Error e -> failwithf "Cannot decode init part payment widget, error = %A" e
