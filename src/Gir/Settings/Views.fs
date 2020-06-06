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


let inputView (inputId: string) (inputLabel: string) =
    div [ rowStyles ]
        [ label [ _for inputId ] [ str inputLabel ]
          input
              [ _type "checkbox"
                _id inputId
                _name inputId
                _value "true" ] ]

let textareaView (areaId: string) (areaLabel: string) =
    div [ rowStyles ]
        [ label [ _for areaId ] [ str areaLabel ]
          textarea
              [ _id areaId
                _name areaId
                _value ""
                _form "settings" ] [] ]

let selectView (selectId: string) selectLabel (selectOptions) =
    div [ rowStyles ]
        [ label [ _for selectId ] [ str selectLabel ]
          select [ _name selectId; _id selectId ] (List.map (fun o -> option [ _value o ] [ str o ]) selectOptions) ]


let template (settings: Settings) =
    let checkboxStateOptions = [ "Hidden"; "Checked"; "Unchecked" ]

    div [ columnStyles ]
        [ form [ _id "settings" ]
              [ div [] [ h3 [] [ str "Extra Init Options" ] ]
                selectView "language" "Language"
                    [ "English"
                      "Swedish"
                      "Finnish"
                      "Norwegian"
                      "Estonian"
                      "Danish" ]
                selectView "mode" "Mode" [ "b2c"; "b2b" ]
                selectView "differentDeliveryAddress" "Different Delivery Address" checkboxStateOptions
                inputView "displayItems" "Display Items"
                selectView "recurringPayments" "Recurring Payments" checkboxStateOptions
                selectView "smsNewsletterSubscription" "SMS Newsletter Subscription" checkboxStateOptions
                selectView "emailNewsletterSubscription" "Email Newsletter Subscription" checkboxStateOptions
                selectView "emailInvoice" "Email Invoice" checkboxStateOptions
                div [] [ h3 [] [ str "Extra Checkout Flags" ] ]
                inputView "disableFocus" "Disable Focus"
                inputView "beforeSubmitCallbackEnabled" "Before Submit Callback Enabled"
                inputView "deliveryAddressChangedCallbackEnabled" "Delivery Address Changed Callback Enabled"
                textareaView "customStyles" "Custom Styles"
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