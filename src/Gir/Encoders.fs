module Gir.Encoders

open Thoth.Json.Net
open Domain
open System


let getPartnerTokenPayloadEncoder (clientId: string) (clientSecret: string) =
    Encode.object
        [ "clientId", Encode.string clientId
          "clientSecret", Encode.string clientSecret ]
    |> Encode.toString 0

let productEncoder (product: Product) =
    Encode.object
        [ "productId", Encode.int product.ProductId
          "name", Encode.string product.Name
          "price", Encode.decimal product.Price
          "img", Encode.string product.Img
          "bigImg", Encode.string product.BigImg ]

let cartItemEncoder (cartItem: CartItem) =
    Encode.object
        [ "id", Encode.int cartItem.Id
          "qty", Encode.int cartItem.Qty
          "product", productEncoder cartItem.ProductDetail ]

let cartEncoder (cartState: CartState) =
    Encode.object [ "items", Encode.list <| List.map (cartItemEncoder) cartState.Items ]
    |> Encode.toString 0

let paymentItemEncoder (productDetail: Product) =

    let productTax = (productDetail.Price * 0.2M)
    let roundedProductTax = Math.Round(productTax, 2)

    Encode.object
        [ "description", Encode.string productDetail.Name
          "notes", Encode.string "-"
          "amount", Encode.decimal productDetail.Price
          "taxCode", Encode.string "20%"
          "taxAmount", Encode.decimal roundedProductTax ]

let languageEncoder = languageToString >> Encode.string

let modeEncoder = checkoutModeToString >> Encode.string

let checkboxStateEncoder = checkboxStateToString >> Encode.string

let backendNotificationStateEncoder (apiPublicUrl: string) =
    function
    | NotSet -> Encode.nil
    | ShouldSucceed -> apiPublicUrl + "/be2be/succeed" |> Encode.string
    | ShouldFail -> apiPublicUrl + "/be2be/fail" |> Encode.string

let selectedPaymentMethodEncoder =
    function
    | Selected pm -> pm |> paymentMethodsToString |> Encode.string
    | NotSelected -> "" |> Encode.string

let ageValidationEncoder ageValidation =
    ageValidation |> ageValidationToString |> Encode.string

let ageValidationEncoderExternal =
    function
    | Disabled -> Encode.nil
    | Enabled limit -> Encode.int limit

let stringOrNullDecoder (condition: bool) (stringValue) =
    if condition then Encode.string stringValue else Encode.nil

let extraInitSettingsEncoderForInitPayment (apiPublicUrl: string) (initSettings: ExtraInitSettings) =
    Encode.object
        [ "language", languageEncoder initSettings.Language
          "mode", modeEncoder initSettings.Mode
          "differentDeliveryAddress", checkboxStateEncoder initSettings.DifferentDeliveryAddress
          "selectedPaymentMethod", selectedPaymentMethodEncoder initSettings.SelectedPaymentMethod
          "displayItems", Encode.bool initSettings.DisplayItems
          "recurringPayments", checkboxStateEncoder initSettings.RecurringPayments
          "smsNewsletterSubscription", checkboxStateEncoder initSettings.SmsNewsletterSubscription
          "emailNewsletterSubscription", checkboxStateEncoder initSettings.EmailNewsletterSubscription
          "completedNotificationUrl", backendNotificationStateEncoder apiPublicUrl initSettings.BackendNotification
          "enableB2BLink", Encode.bool initSettings.EnableB2BLink
          "enableCountrySelector", Encode.bool initSettings.EnableCountrySelector
          "showThankYouPage", Encode.bool initSettings.ShowThankYouPage
          "ageValidation", ageValidationEncoderExternal initSettings.AgeValidation
          "emailInvoice", checkboxStateEncoder initSettings.EmailInvoice
          "termsAndConditionsUrl",
          stringOrNullDecoder initSettings.UseCustomTermsAndConditionsUrl (apiPublicUrl + "/docs/terms_conditions.pdf")
          "integrityConditionsUrl",
          stringOrNullDecoder initSettings.UseCustomIntegrityConditionsUrl (apiPublicUrl + "/docs/integrity_policy.pdf")
          "hideUnsupportedRecurringPaymentMethods", Encode.bool initSettings.HideUnsupportedRecurringPaymentMethods
          "smsNewsletterSubscriptionText",
          stringOrNullDecoder initSettings.UseCustomSmsNewsletterSubscriptionText "Custom SMS Subscription label"
          "emailNewsletterSubscriptionText",
          stringOrNullDecoder initSettings.UseCustomEmailNewsletterSubscriptionText "Custom Email Subscription label"
          "skipEmailZipEntry", Encode.bool initSettings.SkipEmailZipEntry ]

let extraInitSettingsEncoderForSettings (initSettings: ExtraInitSettings) =
    Encode.object
        [ "language", languageEncoder initSettings.Language
          "mode", modeEncoder initSettings.Mode
          "differentDeliveryAddress", checkboxStateEncoder initSettings.DifferentDeliveryAddress
          "selectedPaymentMethod", selectedPaymentMethodEncoder initSettings.SelectedPaymentMethod
          "displayItems", Encode.bool initSettings.DisplayItems
          "recurringPayments", checkboxStateEncoder initSettings.RecurringPayments
          "smsNewsletterSubscription", checkboxStateEncoder initSettings.SmsNewsletterSubscription
          "emailNewsletterSubscription", checkboxStateEncoder initSettings.EmailNewsletterSubscription
          "completedNotificationUrl",
          initSettings.BackendNotification
          |> backendNotificationStateToString
          |> Encode.string
          "enableB2BLink", Encode.bool initSettings.EnableB2BLink
          "enableCountrySelector", Encode.bool initSettings.EnableCountrySelector
          "showThankYouPage", Encode.bool initSettings.ShowThankYouPage
          "ageValidation", ageValidationEncoder initSettings.AgeValidation
          "emailInvoice", checkboxStateEncoder initSettings.EmailInvoice
          "useCustomTermsAndConditionsUrl", Encode.bool initSettings.UseCustomTermsAndConditionsUrl
          "useCustomIntegrityConditionsUrl", Encode.bool initSettings.UseCustomIntegrityConditionsUrl
          "hideUnsupportedRecurringPaymentMethods", Encode.bool initSettings.HideUnsupportedRecurringPaymentMethods
          "useCustomSmsNewsletterSubscriptionText", Encode.bool initSettings.UseCustomSmsNewsletterSubscriptionText
          "useCustomEmailNewsletterSubscriptionText", Encode.bool initSettings.UseCustomEmailNewsletterSubscriptionText
          "skipEmailZipEntry", Encode.bool initSettings.SkipEmailZipEntry ]

let paymentPayloadEncoder (apiPublicUrl: string) (settings: Settings) (items: CartItem list) =
    if List.isEmpty items then
        ""
    else
        let productsList =
            // Checkout 3 will soon allow quantity
            // - remove this fold and send quantity in init payment instead
            List.fold
                (fun acc x ->
                    acc
                    @ [ for i in 1 .. x.Qty do
                            { ProductId = x.ProductDetail.ProductId
                              Name = x.ProductDetail.Name
                              Price = x.ProductDetail.Price
                              Img = x.ProductDetail.Img
                              BigImg = x.ProductDetail.BigImg } ])
                []
                items

        Encode.object
            [ "checkoutSetup", extraInitSettingsEncoderForInitPayment apiPublicUrl settings.ExtraInitSettings
              "items", Encode.list <| List.map (paymentItemEncoder) productsList
              "extraIdentifiers", Encode.object [ "orderReference", Encode.string settings.OrderReference ] ]
        |> Encode.toString 0

let extrasEncoder (extras: Extras) =
    Encode.object [ "extraTermsAndConditions", Encode.option Encode.string extras.ExtraTermsAndConditions ]


let extraCheckoutFlagsEncoder (checkoutFlags: ExtraCheckoutFlags) =
    Encode.object
        [ "disableFocus", Encode.bool checkoutFlags.DisableFocus
          "beforeSubmitCallbackEnabled", Encode.bool checkoutFlags.BeforeSubmitCallbackEnabled
          "deliveryAddressChangedCallbackEnabled", Encode.bool checkoutFlags.DeliveryAddressChangedCallbackEnabled
          "customStyles", Encode.bool checkoutFlags.CustomStyles
          "includePaymentFeeInTotalPrice", Encode.bool checkoutFlags.IncludePaymentFeeInTotalPrice
          "shippingOptionChangedCallbackEnabled", Encode.bool checkoutFlags.ShippingOptionChangedCallbackEnabled
          "paymentMethodChangedCallbackEnabled", Encode.bool checkoutFlags.PaymentMethodChangedCallbackEnabled
          "modeChangedCallbackEnabled", Encode.bool checkoutFlags.ModeChangedCallbackEnabled
          "hideAvardaLogo", Encode.bool checkoutFlags.HideAvardaLogo
          "extras", extrasEncoder checkoutFlags.Extras ]

let paymentWidgetSettingsEncoder (paymentWidgetSettings: PaymentWidgetSettings) =
    Encode.object
        [ "enabled", Encode.bool paymentWidgetSettings.Enabled
          "customStyles", Encode.bool paymentWidgetSettings.CustomStyles ]

let settingsEncoder (settings: Settings) =
    Encode.object
        [ "extraCheckoutFlags", extraCheckoutFlagsEncoder settings.ExtraCheckoutFlags
          "extraInitSettings", extraInitSettingsEncoderForSettings settings.ExtraInitSettings
          "market", settings.Market |> marketToString |> Encode.string
          "orderReference", Encode.string settings.OrderReference
          "paymentWidgetSettings", paymentWidgetSettingsEncoder settings.PaymentWidgetSettings ]
    |> Encode.toString 0


let paymentWidgetStateEncoder (state: PaymentWidgetState) =
    Encode.object
        [ "paymentId", Encode.string state.PaymentId
          "widgetJwt", Encode.string state.WidgetJwt ]
    |> Encode.toString 0
