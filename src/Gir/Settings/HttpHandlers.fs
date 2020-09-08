module Gir.Settings.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Utils
open Views


let settingsHandler (next: HttpFunc) (ctx: HttpContext) =
    let cartState = Session.getCartState ctx
    let settings = Session.getSettings ctx
    (htmlView <| settingsView settings cartState) next ctx

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
                    CustomStyles = checkboxValue "customStyles" }
              ExtraInitSettings =
                  { Language = getValue "language" |> stringToLanguage
                    Mode = getValue "mode" |> stringToCheckoutMode
                    DifferentDeliveryAddress =
                        getValue "differentDeliveryAddress"
                        |> stringToCheckboxState
                    SelectedPaymentMethod = NotSelected
                    DisplayItems = checkboxValue "displayItems"
                    RecurringPayments =
                        getValue "recurringPayments"
                        |> stringToCheckboxState
                    SmsNewsletterSubscription =
                        getValue "smsNewsletterSubscription"
                        |> stringToCheckboxState
                    EmailNewsletterSubscription =
                        getValue "emailNewsletterSubscription"
                        |> stringToCheckboxState
                    BackendNotification =
                        getValue "completedNotificationUrl"
                        |> stringToBackendNotificationState }
              Market = getValue "market" |> stringToMarket
              OrderReference = getValue "orderReference" }

        Session.setSettings ctx formData
        Session.deleteCartState ctx
        Session.deletePurchaseId ctx
        return! next ctx
    }
