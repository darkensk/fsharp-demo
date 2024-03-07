module Gir.Decoders

open Thoth.Json.Net
open Domain


let partnerAccessTokenDecoder =
    Decode.field "token" Decode.string |> Decode.fromString

let purchaseTokenDecoder = Decode.field "jwt" Decode.string |> Decode.fromString

let shippingParametersDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { Height = get.Required.Field "height" Decode.int
          Length = get.Required.Field "length" Decode.int
          Width = get.Required.Field "width" Decode.int
          Weight = get.Required.Field "weight" Decode.int
          Attributes = get.Required.Field "attributes" (Decode.list Decode.string) })

let productDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { ProductId = get.Required.Field "productId" Decode.int
          Name = get.Required.Field "name" Decode.string
          Price = get.Required.Field "price" Decode.decimal
          Img = get.Required.Field "img" Decode.string
          BigImg = get.Required.Field "bigImg" Decode.string
          ShippingParameters = get.Required.Field "shippingParameters" shippingParametersDecoder })

let cartItemDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { Id = get.Required.Field "id" Decode.int
          Qty = get.Required.Field "qty" Decode.int
          ProductDetail = get.Required.Field "product" productDecoder })

let cartDecoder (cartString: string) =
    let decoder =
        Decode.object (fun (get: Decode.IGetters) ->
            { Items =
                get.Optional.Field "items" (Decode.list cartItemDecoder)
                |> Option.defaultValue [] })

    match Decode.fromString decoder cartString with
    | Ok cartState -> cartState
    | Error e -> failwithf "Cannot decode cart, error = %A" e

let decodePartnerAccessToken =
    partnerAccessTokenDecoder
    >> function
        | Ok partnerAccessToken -> partnerAccessToken
        | Error e -> failwithf "Cannot decode partner access token, error = %A" e

let decodePurchaseToken =
    purchaseTokenDecoder
    >> function
        | Ok purchaseToken -> purchaseToken
        | Error e -> failwithf "Cannot decode purchase token, error = %A" e

let initPaymentResponseDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { PurchaseId = get.Required.Field "purchaseId" Decode.string
          Jwt = get.Required.Field "jwt" Decode.string })

let initPaymentDecoder (initPaymentResponseString: string) =
    match Decode.fromString initPaymentResponseDecoder initPaymentResponseString with
    | Ok initPaymentResponse -> initPaymentResponse
    | Error e -> failwithf "Cannot decode init payment, error = %A" e

let extrasDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { ExtraTermsAndConditions = get.Optional.Field "extraTermsAndConditions" Decode.string })

let extraCheckoutFlagsDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { DisableFocus = get.Required.Field "disableFocus" Decode.bool
          BeforeSubmitCallbackEnabled = get.Required.Field "beforeSubmitCallbackEnabled" Decode.bool
          DeliveryAddressChangedCallbackEnabled =
            get.Required.Field "deliveryAddressChangedCallbackEnabled" Decode.bool
          CustomStyles = get.Required.Field "customStyles" Decode.bool
          IncludePaymentFeeInTotalPrice = get.Required.Field "includePaymentFeeInTotalPrice" Decode.bool
          ShippingOptionChangedCallbackEnabled = get.Required.Field "shippingOptionChangedCallbackEnabled" Decode.bool
          PaymentMethodChangedCallbackEnabled = get.Required.Field "paymentMethodChangedCallbackEnabled" Decode.bool
          ModeChangedCallbackEnabled = get.Required.Field "modeChangedCallbackEnabled" Decode.bool
          HideAvardaLogo = get.Required.Field "hideAvardaLogo" Decode.bool
          Extras = get.Required.Field "extras" extrasDecoder })

let extraInitSettingsDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
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
          AgeValidation = (get.Required.Field "ageValidation" Decode.string) |> stringToAgeValidation
          EmailInvoice = (get.Required.Field "emailInvoice" Decode.string) |> stringToCheckboxState
          UseCustomTermsAndConditionsUrl = get.Required.Field "useCustomTermsAndConditionsUrl" Decode.bool
          UseCustomIntegrityConditionsUrl = get.Required.Field "useCustomIntegrityConditionsUrl" Decode.bool
          HideUnsupportedRecurringPaymentMethods =
            get.Required.Field "hideUnsupportedRecurringPaymentMethods" Decode.bool
          UseCustomSmsNewsletterSubscriptionText =
            get.Required.Field "useCustomSmsNewsletterSubscriptionText" Decode.bool
          UseCustomEmailNewsletterSubscriptionText =
            get.Required.Field "useCustomEmailNewsletterSubscriptionText" Decode.bool
          SkipEmailZipEntry = get.Required.Field "skipEmailZipEntry" Decode.bool })

let paymentWidgetSettingsDecoder: Decoder<PaymentWidgetSettings> =
    Decode.object (fun (get: Decode.IGetters) -> { Enabled = get.Required.Field "enabled" Decode.bool })

let additionalFeaturesDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { PartnerShippingEnabled = get.Required.Field "partnerShippingEnabled" Decode.bool })

let aprWidgetSettingsDecoder: Decoder<AprWidgetSettings> =
    Decode.object (fun (get: Decode.IGetters) -> { Enabled = get.Required.Field "enabled" Decode.bool })

let sharedWidgetSettingsDecoder =
    Decode.object (fun (get: Decode.IGetters) -> { CustomStyles = get.Required.Field "customStyles" Decode.bool })

let shippingSettingsDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { IncludeShippingParameters = get.Required.Field "includeShippingParameters" Decode.bool
          IncludeDefaultShippingItem = get.Required.Field "includeDefaultShippingItem" Decode.bool })

let decodeSettings =
    Decode.object (fun (get: Decode.IGetters) ->
        { ExtraCheckoutFlags = get.Required.Field "extraCheckoutFlags" extraCheckoutFlagsDecoder
          ExtraInitSettings = get.Required.Field "extraInitSettings" extraInitSettingsDecoder
          Market = get.Required.Field "market" Decode.string |> stringToMarket
          OrderReference =
            (get.Optional.Field "orderReference" Decode.string)
            |> Option.defaultValue defaultSettings.OrderReference
          PaymentWidgetSettings = get.Required.Field "paymentWidgetSettings" paymentWidgetSettingsDecoder
          AdditionalFeatures = get.Required.Field "additionalFeatures" additionalFeaturesDecoder
          AprWidgetSettings = get.Required.Field "aprWidgetSettings" aprWidgetSettingsDecoder
          SharedWidgetSettings = get.Required.Field "sharedWidgetCustomStyles" sharedWidgetSettingsDecoder
          ShippingSettings = get.Required.Field "shippingSettings" shippingSettingsDecoder })

let settingsDecoder (settingsString: string) =
    match Decode.fromString decodeSettings settingsString with
    | Ok settings -> settings
    | Error e -> failwithf "Cannot decode settings, error = %A" e


let decodeExtraIdentifiers =
    Decode.object (fun (get: Decode.IGetters) -> { OrderReference = get.Required.Field "orderReference" Decode.string })


let decodeCurrentB2BStep: (string -> JsonValue -> Result<CurrentB2BStep, DecoderError>) =
    Decode.object (fun (get: Decode.IGetters) ->
        { Current = (get.Required.Field "current" Decode.string) |> stringToB2BStep })

let decodeCurrentB2CStep: (string -> JsonValue -> Result<CurrentB2CStep, DecoderError>) =
    Decode.object (fun (get: Decode.IGetters) ->
        { Current = (get.Required.Field "current" Decode.string) |> stringToB2CStep })

let decodeB2BData: (string -> JsonValue -> Result<B2BData, DecoderError>) =
    Decode.object (fun (get: Decode.IGetters) -> { Step = get.Required.Field "step" decodeCurrentB2BStep })

let decodeB2CData: (string -> JsonValue -> Result<B2CData, DecoderError>) =
    Decode.object (fun (get: Decode.IGetters) -> { Step = get.Required.Field "step" decodeCurrentB2CStep })

let decodePaymentStatus =
    Decode.object (fun (get: Decode.IGetters) ->
        { PurchaseId = get.Required.Field "purchaseId" Decode.string
          ExtraIdentifiers = get.Required.Field "extraIdentifiers" decodeExtraIdentifiers
          Mode = get.Required.Field "mode" Decode.string
          B2B = get.Optional.Field "b2B" decodeB2BData
          B2C = get.Optional.Field "b2C" decodeB2CData })


let getPaymentStatusDecoder (paymentStatusString: string) =
    match Decode.fromString decodePaymentStatus paymentStatusString with
    | Ok paymentStatus -> paymentStatus
    | Error e -> failwithf "Cannot decode get payment status, error = %A" e


let initPaymentWidgetStateDecoder =
    Decode.object (fun (get: Decode.IGetters) ->
        { PaymentId = get.Required.Field "paymentId" Decode.string
          WidgetJwt = get.Required.Field "widgetJwt" Decode.string })

let initPaymentWidgetDecoder (paymentWidgetStateString: string) =
    match Decode.fromString initPaymentWidgetStateDecoder paymentWidgetStateString with
    | Ok paymentWidgetState -> paymentWidgetState
    | Error e -> failwithf "Cannot decode init payment widget, error = %A" e
