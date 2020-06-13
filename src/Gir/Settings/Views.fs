module Gir.Settings.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout
open Gir.Domain


let rowStyles =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 50px; height: 50px;"

let columnStyles =
    _style "display: flex; flex-direction: column; padding: 20px 0px; width: 500px;"

let checkboxView (inputId: string) (inputLabel: string) (isChecked: bool) (isEnabled: bool) =
    let checkedAttribute = if isChecked then [ _checked ] else []
    let disabledAttribute = if isEnabled then [] else [ _disabled ]
    div [ rowStyles ]
        [ label [ _for inputId ] [ str inputLabel ]
          input
              ([ _style "width: 30px; height: 30px;"
                 _type "checkbox"
                 _id inputId
                 _name inputId
                 _value "true" ]
               @ checkedAttribute
               @ disabledAttribute) ]

let textareaView (areaId: string) (areaLabel: string) =
    div [ rowStyles ]
        [ label [ _for areaId ] [ str areaLabel ]
          textarea
              [ _id areaId
                _name areaId
                _value ""
                _form "settings" ] [] ]

let selectView
    (selectId: string)
    (selectLabel: string)
    (selectOptions: string list)
    (selectedOption: string)
    (isEnabled: bool)
    =
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
              [ div [] [ h3 [] [ str "Market" ] ]
                selectView "market" "Market" [ "Sweden"; "Finland" ] (marketToString settings.Market) true
                div [] [ h3 [] [ str "Extra Init Options" ] ]
                selectView "language" "Language"
                    [ "English"
                      "Swedish"
                      "Finnish"
                      "Norwegian"
                      "Estonian"
                      "Danish" ] (languageToString initSettings.Language) true
                selectView "mode" "Mode" [ "b2c"; "b2b" ] "b2c" false
                selectView "differentDeliveryAddress" "Different Delivery Address" checkboxStateOptions
                    (checkboxStateToString initSettings.DifferentDeliveryAddress) false
                checkboxView "displayItems" "Display Items" initSettings.DisplayItems true
                selectView "recurringPayments" "Recurring Payments" checkboxStateOptions
                    (checkboxStateToString initSettings.RecurringPayments) false
                selectView "smsNewsletterSubscription" "SMS Newsletter Subscription" checkboxStateOptions
                    (checkboxStateToString initSettings.SmsNewsletterSubscription) false
                selectView "emailNewsletterSubscription" "Email Newsletter Subscription" checkboxStateOptions
                    (checkboxStateToString initSettings.EmailNewsletterSubscription) false
                selectView "emailInvoice" "Email Invoice" checkboxStateOptions
                    (checkboxStateToString initSettings.EmailInvoice) false
                div [] [ h3 [] [ str "Extra Checkout Flags" ] ]
                checkboxView "disableFocus" "Disable Focus" checkoutFlags.DisableFocus true
                checkboxView "beforeSubmitCallbackEnabled" "Before Submit Callback Enabled"
                    checkoutFlags.BeforeSubmitCallbackEnabled true
                checkboxView "deliveryAddressChangedCallbackEnabled" "Delivery Address Changed Callback Enabled"
                    checkoutFlags.DeliveryAddressChangedCallbackEnabled true
                checkboxView "customStyles" "Use Custom Styles" (customStylesToBool checkoutFlags.CustomStyles) false
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
