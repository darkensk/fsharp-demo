module Gir.Settings.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout
open Thoth.Json.Net


type Language =
    | English
    | Swedish
    | Finnish
    | Norwegian
    | Estonian
    | Danish

let languageToString language =
    match language with
    | English -> "English"
    | Swedish -> "Swedish"
    | Finnish -> "Finnish"
    | Norwegian -> "Norwegian"
    | Estonian -> "Estonian"
    | Danish -> "Danish"

let stringToLanguage s =
    match s with
    | "English" -> English
    | "Swedish" -> Swedish
    | "Finnish" -> Finnish
    | "Norwegian" -> Norwegian
    | "Estonian" -> Estonian
    | "Danish" -> Danish
    | _ -> English

type CheckoutMode =
    | B2C
    | B2B

let checkoutModeToString mode =
    match mode with
    | B2C -> "b2c"
    | B2B -> "b2b"

let stringToCheckoutMode s =
    match s with
    | "b2c" -> B2C
    | "b2b" -> B2B
    | _ -> B2C

type CheckboxState =
    | Hidden
    | Checked
    | Unchecked

let checkboxStateToString checkboxState =
    match checkboxState with
    | Hidden -> "Hidden"
    | Checked -> "Checked"
    | Unchecked -> "Unchecked"

let stringToCheckboxState s =
    match s with
    | "Hidden" -> Hidden
    | "Checked" -> Checked
    | "Unchecked" -> Unchecked
    | _ -> Hidden

type PaymentMethod =
    | Loan
    | Invoice
    | Card
    | Direct
    | PayPal
    | Swish
    | PartPayment
    | PayOnDelivery

type SelectedPaymentMethod =
    | Selected of selectedPaymentMethod: PaymentMethod
    | NotSelected

let paymentMethodsToString pm =
    match pm with
    | Loan -> "Loan"
    | Invoice -> "Invoice"
    | Card -> "Card"
    | Direct -> "Direct"
    | PayPal -> "PayPal"
    | Swish -> "Swish"
    | PartPayment -> "PartPayment"
    | PayOnDelivery -> "PayOnDelivery"

let selectedPaymentMethodToString selectedPaymentMethod =
    match selectedPaymentMethod with
    | Selected selectedPaymentMethod -> paymentMethodsToString selectedPaymentMethod
    | NotSelected -> ""


let stringToSelectedPaymentMethod s =
    match s with
    | "Loan" -> Selected Loan
    | "Invoice" -> Selected Invoice
    | "Card" -> Selected Card
    | "Direct" -> Selected Direct
    | "PayPal" -> Selected PayPal
    | "Swish" -> Selected Swish
    | "PartPayment" -> Selected PartPayment
    | "PayOnDelivery" -> Selected PayOnDelivery
    | _ -> NotSelected


type ExtraInitSettings =
    { Language: Language
      Mode: CheckoutMode
      DifferentDeliveryAddress: CheckboxState
      SelectedPaymentMethod: SelectedPaymentMethod
      DisplayItems: bool
      RecurringPayments: CheckboxState
      SmsNewsletterSubscription: CheckboxState
      EmailNewsletterSubscription: CheckboxState
      EmailInvoice: CheckboxState }

type CustomStyles =
    | Set of customStyles: string
    | NotSet

type ExtraCheckoutFlags =
    { DisableFocus: bool
      BeforeSubmitCallbackEnabled: bool
      DeliveryAddressChangedCallbackEnabled: bool
      CustomStyles: CustomStyles }

type Settings =
    { ExtraCheckoutFlags: ExtraCheckoutFlags
      ExtraInitSettings: ExtraInitSettings }

let defaultExtraCheckoutFlags =
    { DisableFocus = true
      BeforeSubmitCallbackEnabled = false
      DeliveryAddressChangedCallbackEnabled = false
      CustomStyles = NotSet }

let defaultExtraInitSettings =
    { Language = English
      Mode = B2C
      DifferentDeliveryAddress = Hidden
      SelectedPaymentMethod = NotSelected
      DisplayItems = true
      RecurringPayments = Hidden
      SmsNewsletterSubscription = Hidden
      EmailNewsletterSubscription = Hidden
      EmailInvoice = Hidden }

let defaultSettings =
    { ExtraCheckoutFlags = defaultExtraCheckoutFlags
      ExtraInitSettings = defaultExtraInitSettings }

let languageEncoder = languageToString >> Encode.string

let modeEncoder = checkoutModeToString >> Encode.string

let checkboxStateEncoder = checkboxStateToString >> Encode.string

let selectedPaymentMethodEncoder selectedPaymentMethod =
    match selectedPaymentMethod with
    | Selected pm -> [ "selectedPaymentMethod", pm |> paymentMethodsToString |> Encode.string ]
    | NotSelected -> []

let customStylesEncoder customStyles =
    match customStyles with
    | Set cs -> Encode.string cs
    | NotSet -> Encode.object []



let extraInitSettingsEncoder initSettings =
    Encode.object
        ([ "language", languageEncoder initSettings.Language
           "mode", modeEncoder initSettings.Mode
           "differentDeliveryAddress", checkboxStateEncoder initSettings.DifferentDeliveryAddress
           "displayItems", Encode.bool initSettings.DisplayItems
           "recurringPayments", checkboxStateEncoder initSettings.RecurringPayments
           "smsNewsletterSubscription", checkboxStateEncoder initSettings.SmsNewsletterSubscription
           "emailNewsletterSubscription", checkboxStateEncoder initSettings.EmailNewsletterSubscription
           "emailInvoice", checkboxStateEncoder initSettings.EmailInvoice ]
         @ (selectedPaymentMethodEncoder initSettings.SelectedPaymentMethod))

let extraCheckoutFlagsEncoder checkoutFlags =
    Encode.object
        [ "disableFocus", Encode.bool checkoutFlags.DisableFocus
          "beforeSubmitCallbackEnabled", Encode.bool checkoutFlags.BeforeSubmitCallbackEnabled
          "deliveryAddressChangedCallbackEnabled", Encode.bool checkoutFlags.DeliveryAddressChangedCallbackEnabled
          "customStyles", customStylesEncoder checkoutFlags.CustomStyles ]

let settingsEncoder settings =
    Encode.object
        [ "extraCheckoutFlags", extraCheckoutFlagsEncoder settings.ExtraCheckoutFlags
          "extraInitSettings", extraInitSettingsEncoder settings.ExtraInitSettings ]
    |> Encode.toString 0

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


let template =
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
                div [ _style "display: flex; flex-direction: row; align-items: center; justify-content: flex-end; padding: 20px 50px;" ]
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

let settingsView = [ template ] |> layout
