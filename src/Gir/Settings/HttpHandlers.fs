module Gir.Settings.HttpHandlers

open FSharp.Control.Tasks
open Giraffe
open Microsoft.AspNetCore.Http
open Gir.Domain
open Gir.Utils
open Views


let settingsHandler (next: HttpFunc) (ctx: HttpContext) =
    let settings = Session.getSettings ctx
    (htmlView <| settingsView settings) next ctx

let saveSettingsHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let tryGetValue s = ctx.Request.Form.TryGetValue(s)

        // let getValueFromSeq s =
        //     s |> Seq.cast |> List.ofSeq |> List.head

        let checkboxValue s =
            match tryGetValue s with
            | (true, _) -> true
            | (false, _) -> false

        let boolToCustomStyles s =
            match checkboxValue s with
            | true -> Set "{}"
            | false -> NotSet

        let getValue s = ctx.Request.Form.Item(s).ToString()

        let formData =
            { ExtraCheckoutFlags =
                  { DisableFocus = checkboxValue "disableFocus"
                    BeforeSubmitCallbackEnabled = checkboxValue "beforeSubmitCallbackEnabled"
                    DeliveryAddressChangedCallbackEnabled = checkboxValue "deliveryAddressChangedCallbackEnabled"
                    CustomStyles = boolToCustomStyles "customStyles" }
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
                    EmailInvoice = getValue "emailInvoice" |> stringToCheckboxState }
              Market = getValue "market" |> stringToMarket }

        Session.setSettings ctx formData
        Session.deleteCartState ctx
        Session.deletePurchaseId ctx
        return! next ctx
    }
