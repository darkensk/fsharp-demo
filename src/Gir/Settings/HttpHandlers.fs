module Gir.Settings.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Utils
open Views


let settingsHandler (paymentWidgetBundleUrl: string) (enabledMarkets: Market list) (next: HttpFunc) (ctx: HttpContext) =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx

    (htmlView
     <| settingsView paymentWidgetBundleUrl enabledMarkets settings cartState)
        next
        ctx

let saveSettingsHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let tryGetValue s = ctx.Request.Form.TryGetValue(s)

        let checkboxValue s =
            match tryGetValue s with
            | (true, _) -> true
            | (false, _) -> false

        let getValue s = ctx.Request.Form.Item(s).ToString()

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
                  HideAvardaLogo = checkboxValue "hideAvardaLogo" }
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
                  AgeValidation = getValue "ageValidation" |> stringToAgeValidation }
              Market = getValue "market" |> stringToMarket
              OrderReference = getValue "orderReference"
              PaymentWidgetSettings =
                { Enabled = checkboxValue "paymentWidgetEnabled"
                  CustomStyles = checkboxValue "paymentWidgetCustomStyles" } }

        Session.setSettings ctx formData
        Session.deleteCartState ctx
        Session.deletePurchaseId ctx
        return! next ctx
    }
