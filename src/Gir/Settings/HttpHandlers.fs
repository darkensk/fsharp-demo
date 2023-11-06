module Gir.Settings.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Utils
open Views


let settingsHandler
    (paymentWidgetBundleUrl: string)
    (enabledMarkets: Market list)
    (partnerShippingBundleUrl: string)
    (next: HttpFunc)
    (ctx: HttpContext)
    =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx

    (htmlView
     <| settingsView paymentWidgetBundleUrl enabledMarkets settings cartState partnerShippingBundleUrl)
        next
        ctx

let saveSettingsHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let tryGetValue fieldName = ctx.Request.Form.TryGetValue(fieldName)

        let checkboxValue fieldName =
            match tryGetValue fieldName with
            | (true, _) -> true
            | (false, _) -> false

        let getValue fieldName =
            ctx.Request.Form.Item(fieldName).ToString()

        let getTextAreaValue fieldName =
            match getValue fieldName with
            | "" -> None
            | " " -> None
            | value -> Some value

        let formData =
            { ExtraCheckoutFlags =
                { DisableFocus = checkboxValue "disableFocus"
                  BeforeSubmitCallbackEnabled = checkboxValue "beforeSubmitCallbackEnabled"
                  DeliveryAddressChangedCallbackEnabled = checkboxValue "deliveryAddressChangedCallbackEnabled"
                  CustomStyles = checkboxValue "customStyles"
                  IncludePaymentFeeInTotalPrice = checkboxValue "includePaymentFeeInTotalPrice"
                  ShippingOptionChangedCallbackEnabled = checkboxValue "shippingOptionChangedCallbackEnabled"
                  PaymentMethodChangedCallbackEnabled = checkboxValue "paymentMethodChangedCallbackEnabled"
                  ModeChangedCallbackEnabled = checkboxValue "modeChangedCallbackEnabled"
                  HideAvardaLogo = checkboxValue "hideAvardaLogo"
                  Extras = { ExtraTermsAndConditions = getTextAreaValue "extraTermsAndConditions" } }
              ExtraInitSettings =
                { Language = getValue "language" |> stringToLanguage
                  Mode = getValue "mode" |> stringToCheckoutMode
                  DifferentDeliveryAddress = getValue "differentDeliveryAddress" |> stringToCheckboxState
                  SelectedPaymentMethod = NotSelected
                  DisplayItems = checkboxValue "displayItems"
                  RecurringPayments = getValue "recurringPayments" |> stringToCheckboxState
                  SmsNewsletterSubscription = getValue "smsNewsletterSubscription" |> stringToCheckboxState
                  EmailNewsletterSubscription = getValue "emailNewsletterSubscription" |> stringToCheckboxState
                  BackendNotification = getValue "completedNotificationUrl" |> stringToBackendNotificationState
                  EnableB2BLink = checkboxValue "enableB2BLink"
                  EnableCountrySelector = checkboxValue "enableCountrySelector"
                  ShowThankYouPage = checkboxValue "showThankYouPage"
                  AgeValidation = getValue "ageValidation" |> stringToAgeValidation
                  EmailInvoice = getValue "emailInvoice" |> stringToCheckboxState
                  UseCustomTermsAndConditionsUrl = checkboxValue "useCustomTermsAndConditionsUrl"
                  UseCustomIntegrityConditionsUrl = checkboxValue "useCustomIntegrityConditionsUrl"
                  HideUnsupportedRecurringPaymentMethods = checkboxValue "hideUnsupportedRecurringPaymentMethods"
                  UseCustomSmsNewsletterSubscriptionText = checkboxValue "useCustomSmsNewsletterSubscriptionText"
                  UseCustomEmailNewsletterSubscriptionText = checkboxValue "useCustomEmailNewsletterSubscriptionText"
                  SkipEmailZipEntry = checkboxValue "skipEmailZipEntry" }
              Market = getValue "market" |> stringToMarket
              OrderReference = getValue "orderReference"
              PaymentWidgetSettings = { Enabled = checkboxValue "paymentWidgetEnabled" }
              AdditionalFeatures = { PartnerShippingEnabled = checkboxValue "partnerShippingEnabled" }
              AprWidgetSettings = { Enabled = checkboxValue "aprWidgetEnabled" }
              SharedWidgetSettings = { CustomStyles = checkboxValue "sharedWidgetCustomStyles" } }

        Session.setSettings ctx formData
        Session.deleteCartState ctx
        Session.deletePurchaseId ctx
        Session.deletePaymentWidgetState ctx
        return! next ctx
    }
