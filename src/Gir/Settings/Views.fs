module Gir.Settings.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout
open Thoth.Json.Net
open Gir.Domain


let rowStyles =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 50px; height: 50px;"

let columnStyles =
    _style "display: flex; flex-direction: column; padding: 20px 0px; width: 500px;"


let checkboxView (inputId: string) (inputLabel: string) (isChecked: bool) =
    let checkedAttribute = if isChecked then [ _checked ] else []
    div [ rowStyles ]
        [ label [ _for inputId ] [ str inputLabel ]
          //   input
          //       [ _type "hidden"
          //         _id inputId
          //         _name inputId
          //         _value "false" ]
          input
              ([ _style "width: 30px; height: 30px;"
                 _type "checkbox"
                 _id inputId
                 _name inputId
                 _value "true" ]
               @ checkedAttribute) ]

let textareaView (areaId: string) (areaLabel: string) =
    div [ rowStyles ]
        [ label [ _for areaId ] [ str areaLabel ]
          textarea
              [ _id areaId
                _name areaId
                _value ""
                _form "settings" ] [] ]

let selectView (selectId: string) (selectLabel: string) (selectOptions: string list) (selectedOption: string) =
    let selectedOptionAttribute option =
        if option = selectedOption then [ _selected ] else []

    div [ rowStyles ]
        [ label [ _for selectId ] [ str selectLabel ]
          select [ _name selectId; _id selectId ]
              (List.map (fun o -> option ([ _value o ] @ selectedOptionAttribute o) [ str o ]) selectOptions) ]


let template (settings: Settings) =
    let checkboxStateOptions = [ "Hidden"; "Checked"; "Unchecked" ]

    let { ExtraInitSettings = initSettings; ExtraCheckoutFlags = checkoutFlags } = settings

    div [ columnStyles ]
        [ form
            [ _id "settings"
              _action "/settings/save"
              _method "POST" ]
              [ div [] [ h3 [] [ str "Extra Init Options" ] ]
                selectView "language" "Language"
                    [ "English"
                      "Swedish"
                      "Finnish"
                      "Norwegian"
                      "Estonian"
                      "Danish" ] (languageToString initSettings.Language)
                selectView "mode" "Mode" [ "b2c"; "b2b" ] "b2c"
                selectView "differentDeliveryAddress" "Different Delivery Address" checkboxStateOptions
                    (checkboxStateToString initSettings.DifferentDeliveryAddress)
                checkboxView "displayItems" "Display Items" initSettings.DisplayItems
                selectView "recurringPayments" "Recurring Payments" checkboxStateOptions
                    (checkboxStateToString initSettings.RecurringPayments)
                selectView "smsNewsletterSubscription" "SMS Newsletter Subscription" checkboxStateOptions
                    (checkboxStateToString initSettings.SmsNewsletterSubscription)
                selectView "emailNewsletterSubscription" "Email Newsletter Subscription" checkboxStateOptions
                    (checkboxStateToString initSettings.EmailNewsletterSubscription)
                selectView "emailInvoice" "Email Invoice" checkboxStateOptions
                    (checkboxStateToString initSettings.EmailInvoice)
                div [] [ h3 [] [ str "Extra Checkout Flags" ] ]
                checkboxView "disableFocus" "Disable Focus" checkoutFlags.DisableFocus
                checkboxView "beforeSubmitCallbackEnabled" "Before Submit Callback Enabled"
                    checkoutFlags.BeforeSubmitCallbackEnabled
                checkboxView "deliveryAddressChangedCallbackEnabled" "Delivery Address Changed Callback Enabled"
                    checkoutFlags.DeliveryAddressChangedCallbackEnabled
                checkboxView "customStyles" "Use Custom Styles" (customStylesToBool checkoutFlags.CustomStyles)
                div
                    [ _style
                        "display: flex; flex-direction: row; align-items: center; justify-content: flex-end; padding: 20px 50px;" ]
                    [ input
                        [ _style " background-color: #fbb710;
                                           border: none;
                                           color: white;
                                           padding: 16px 32px;
                                           text-decoration: none;
                                           margin: 4px 2px;
                                           cursor: pointer;"
                          _type "submit"
                          _value "Submit" ] ] ] ]

let settingsView (settings: Settings) = [ template settings ] |> layout
