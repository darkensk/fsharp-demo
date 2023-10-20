module Gir.Settings.Views

open Giraffe.ViewEngine
open Gir.Layout
open Gir.Domain
open Gir.Utils
open System

let rowStyles: XmlAttribute =
    _style "display: flex; flex-direction: row; align-items: center; justify-content: space-between; height: 50px;"

let helpTextRowStyles: XmlAttribute =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 20px 10px 20px; height: 20px; color: grey; font-size: 12px; font-weight: 100;"

let columnStyles: XmlAttribute = _style "display: flex; flex-direction: column;"

let labelStyles: string = "text-overflow: ellipsis; overflow: hidden; margin: 0px;"

let checkboxLabelStyles: string =
    "text-overflow: ellipsis; overflow: hidden; margin: 0px; cursor: pointer;"

let checkboxView (inputId: string) (inputLabel: string) (helpText: string option) (isChecked: bool) (isEnabled: bool) =
    let checkedAttribute = if isChecked then [ _checked ] else []
    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    div
        [ _class "settings-row" ]
        [ div
              [ rowStyles ]
              [ label [ _for inputId; _style checkboxLabelStyles ] [ str inputLabel ]
                div
                    [ _style "display: flex; flex-direction-row; min-width: 60px;" ]
                    [ input (
                          [ _style "width: 30px; height: 30px; cursor: pointer;"
                            _type "checkbox"
                            _id inputId
                            _name inputId
                            _value "true" ]
                          @ checkedAttribute
                          @ disabledAttribute
                      ) ] ]
          (helpText
           |> Option.map (fun (helpText: string) -> div [ helpTextRowStyles ] [ str helpText ])
           |> Option.defaultValue (div [] [])) ]

let textareaView
    (areaId: string)
    (areaLabel: string)
    (helpText: string option)
    (value: string option)
    (isEnabled: bool)
    =
    let textareaStyles =
        "resize: none; width: 50%; border: 1px solid #ccc; padding: 5px; font-size: 16px;"

    let textareaRowStyles =
        _style "display: flex; flex-direction: row; align-items: center; justify-content: space-between; height: 100px;"

    let initValue =
        match value with
        | None -> ""
        | Some value_ -> value_

    div
        [ _class "settings-row" ]
        [ div
              [ textareaRowStyles ]
              [ label [ _for areaId; _style labelStyles ] [ str areaLabel ]
                textarea
                    [ _id areaId
                      _name areaId
                      _value initValue
                      _form "settings"
                      _style textareaStyles
                      if not isEnabled then
                          _disabled ]
                    [ str initValue ] ]
          (helpText
           |> Option.map (fun (helpText: string) -> div [ helpTextRowStyles ] [ str helpText ])
           |> Option.defaultValue (div [] [])) ]


let selectView
    (selectId: string)
    (selectLabel: string)
    (selectOptions: (string * bool) list)
    (selectedOption: string)
    (isEnabled: bool)
    =
    let selectedOptionAttribute (option: string) =
        if option = selectedOption then [ _selected ] else []

    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    div
        [ rowStyles; _class "settings-row" ]
        [ label [ _for selectId; _style labelStyles ] [ str selectLabel ]
          select
              ([ _name selectId; _id selectId ] @ disabledAttribute)
              (List.map
                  (fun (option_: string, isDisabled: bool) ->
                      option
                          ([ _value option_
                             if isDisabled then
                                 _disabled ]
                           @ selectedOptionAttribute option_)
                          [ str option_ ])
                  selectOptions) ]

let inputView
    (inputType: string)
    (inputStyle: string)
    (inputId: string)
    (inputLabel: string)
    (helpText: string option)
    (value: string)
    (isEnabled: bool)
    (minMax: (string * string) option)
    =
    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    let minMaxAttributes =
        match minMax with
        | Some(min: string, max: string) -> [ _min min; _max max ]
        | None -> []

    div
        [ _class "settings-row" ]
        [ div
              [ rowStyles ]
              [ label [ _for inputId; _style labelStyles ] [ str inputLabel ]
                input (
                    [ _type inputType; _style inputStyle; _id inputId; _name inputId; _value value ]
                    @ disabledAttribute
                    @ minMaxAttributes
                ) ]
          (helpText
           |> Option.map (fun (helpText: string) -> div [ helpTextRowStyles ] [ str helpText ])
           |> Option.defaultValue (div [] [])) ]

let textInput =
    inputView
        "text"
        "width: 50%;
                        height: 50px;
                        background-color: #fff;
                        color: #000;
                        font-size: 14px;
                        border: 1px solid #e8e8e8;
                        border-radius: 5px;
                        padding: 0 10px;"

let numberInput =
    inputView
        "number"
        "width: 20%;
                        height: 50px;
                        background-color: #fff;
                        color: #000;
                        font-size: 16px;
                        border: 1px solid #e8e8e8;
                        border-radius: 5px;
                        padding: 0 10px;"

let template
    (paymentWidgetBundleUrl: string)
    (enabledMarkets: Market list)
    (settings: Settings)
    (cartState: CartState)
    (partnerShippingBundleUrl: string)
    =
    let checkboxStateOptions =
        [ ("Hidden", false); ("Checked", false); ("Unchecked", false) ]

    let { ExtraInitSettings = initSettings
          ExtraCheckoutFlags = checkoutFlags
          PaymentWidgetSettings = paymentWidgetSettings
          AdditionalFeatures = additionalFeatures
          AprWidgetSettings = aprWidgetSettings
          SharedWidgetSettings = sharedWidgetSettings } =
        settings

    let marketsOptions =
        List.map (fun (market: Market) -> (marketToString market, false)) enabledMarkets

    let paymentWidgetHelpText =
        if (isPaymentWidgetEnabledGlobally paymentWidgetBundleUrl) then
            None
        else
            Some "Requires 'paymentWidgetBundleUrl' environment variable specified"

    div
        []
        [ div
              [ _class "search-wrapper section-padding-100" ]
              [ div [ _class "search-close" ] [ i [ _class "fa fa-close"; _ariaHidden "true" ] [] ]
                div
                    [ _class "container" ]
                    [ div
                          [ _class "row" ]
                          [ div
                                [ _class "col-12" ]
                                [ div
                                      [ _class "search-content" ]
                                      [ form
                                            [ _action "#"; _method "get" ]
                                            [ input
                                                  [ _type "search"
                                                    _name "search"
                                                    _id "search"
                                                    _placeholder "Type your keyword..." ]
                                              button
                                                  [ _type "submit" ]
                                                  [ img [ _src "/img/core-img/search.png"; _alt "" ] ] ] ] ] ] ] ]
          div
              [ _class "main-content-wrapper d-flex clearfix" ]
              [ div
                    [ _class "mobile-nav" ]
                    [ div
                          [ _class "amado-navbar-brand" ]
                          [ a [ _href "/" ] [ img [ _src "/img/core-img/logo.png"; _alt "" ] ] ]
                      div [ _class "amado-navbar-toggler" ] [ span [] []; span [] []; span [] [] ] ]
                headerView cartState
                div
                    [ _class "single-product-area section-padding-100 clearfix" ]
                    [ div
                          [ _class "settings-container" ]
                          [ div
                                [ columnStyles ]
                                [ h1 [ _class "settings-heading" ] [ str "Settings Page" ]
                                  form
                                      [ _id "settings"; _action "/settings/save"; _method "POST" ]
                                      [ div [] [ h3 [ _class "settings-heading" ] [ str "Market" ] ]
                                        selectView
                                            "market"
                                            "Market"
                                            marketsOptions
                                            (marketToString settings.Market)
                                            true
                                        div [] [ h3 [ _class "settings-heading" ] [ str "Extra Identifiers" ] ]
                                        textInput
                                            "orderReference"
                                            "Order Reference"
                                            None
                                            settings.OrderReference
                                            true
                                            None
                                        div [] [ h3 [ _class "settings-heading" ] [ str "Extra Init Options" ] ]
                                        selectView
                                            "language"
                                            "Language"
                                            [ ("English", false)
                                              ("Swedish", false)
                                              ("Finnish", false)
                                              ("Norwegian", false)
                                              ("Danish", false)
                                              ("Slovak", false)
                                              ("Czech", false)
                                              ("Latvian", false)
                                              ("Polish", false)
                                              ("Estonian", false)
                                              ("German", false)
                                              ("Austrian", false) ]
                                            (languageToString initSettings.Language)
                                            true
                                        selectView "mode" "Mode" [ ("b2c", false); ("b2b", false) ] "b2c" true
                                        selectView
                                            "differentDeliveryAddress"
                                            "Different Delivery Address"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.DifferentDeliveryAddress)
                                            true
                                        checkboxView "displayItems" "Display Items" None initSettings.DisplayItems true
                                        selectView
                                            "recurringPayments"
                                            "Recurring Payments"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.RecurringPayments)
                                            false
                                        selectView
                                            "smsNewsletterSubscription"
                                            "SMS Newsletter Subscription"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.SmsNewsletterSubscription)
                                            true
                                        selectView
                                            "emailNewsletterSubscription"
                                            "Email Newsletter Subscription"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.EmailNewsletterSubscription)
                                            true
                                        selectView
                                            "completedNotificationUrl"
                                            "Backend Notification"
                                            [ ("NotSet", false); ("ShouldSucceed", false); ("ShouldFail", false) ]
                                            (backendNotificationStateToString initSettings.BackendNotification)
                                            true
                                        checkboxView
                                            "enableB2BLink"
                                            "Enable B2B Link"
                                            (Some "Requires branch setting enabled: B2B mode")
                                            initSettings.EnableB2BLink
                                            true
                                        checkboxView
                                            "enableCountrySelector"
                                            "Enable Country Selector"
                                            (Some "Requires branch setting enabled: ShowCountryInCheckoutForm")
                                            initSettings.EnableCountrySelector
                                            true
                                        checkboxView
                                            "showThankYouPage"
                                            "Show 'Thank You' Page"
                                            None
                                            initSettings.ShowThankYouPage
                                            true
                                        numberInput
                                            "ageValidation"
                                            "Age Validation"
                                            (Some "Insert age limit as number between 0 and 100")
                                            (initSettings.AgeValidation |> ageValidationToString)
                                            true
                                            (Some("0", "100"))
                                        selectView
                                            "emailInvoice"
                                            "Email Invoice"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.EmailInvoice)
                                            true
                                        checkboxView
                                            "useCustomTermsAndConditionsUrl"
                                            "Use custom Terms and Conditions url"
                                            None
                                            initSettings.UseCustomTermsAndConditionsUrl
                                            true
                                        checkboxView
                                            "useCustomIntegrityConditionsUrl"
                                            "Use custom Integrity Policy url"
                                            None
                                            initSettings.UseCustomIntegrityConditionsUrl
                                            true
                                        checkboxView
                                            "hideUnsupportedRecurringPaymentMethods"
                                            "Hide payment methods that are not eligible for recurring payments"
                                            None
                                            initSettings.HideUnsupportedRecurringPaymentMethods
                                            false
                                        checkboxView
                                            "useCustomSmsNewsletterSubscriptionText"
                                            "Replace label for SMS newsletter"
                                            (Some
                                                "Requires SMS newsletter displayed. Label will be replaced with 'Custom SMS Subscription label'")
                                            initSettings.UseCustomSmsNewsletterSubscriptionText
                                            true
                                        checkboxView
                                            "useCustomEmailNewsletterSubscriptionText"
                                            "Raplace label for email newsletter"
                                            (Some
                                                "Requires Email newsletter displayed. Label will be replaced with 'Custom Email Subscription label'")
                                            initSettings.UseCustomEmailNewsletterSubscriptionText
                                            true
                                        checkboxView
                                            "skipEmailZipEntry"
                                            "Skip email zip entry step"
                                            None
                                            initSettings.SkipEmailZipEntry
                                            true
                                        div [] [ h3 [ _class "settings-heading" ] [ str "Extra Checkout Flags" ] ]
                                        checkboxView "disableFocus" "Disable Focus" None checkoutFlags.DisableFocus true
                                        checkboxView
                                            "beforeSubmitCallbackEnabled"
                                            "Before Submit Callback Enabled"
                                            None
                                            checkoutFlags.BeforeSubmitCallbackEnabled
                                            true
                                        checkboxView
                                            "deliveryAddressChangedCallbackEnabled"
                                            "Delivery Address Changed Callback Enabled"
                                            None
                                            checkoutFlags.DeliveryAddressChangedCallbackEnabled
                                            true
                                        checkboxView
                                            "customStyles"
                                            "Use Custom Styles"
                                            None
                                            checkoutFlags.CustomStyles
                                            true
                                        checkboxView
                                            "includePaymentFeeInTotalPrice"
                                            "Include Payment Fees in Total Price"
                                            None
                                            checkoutFlags.IncludePaymentFeeInTotalPrice
                                            true
                                        checkboxView
                                            "shippingOptionChangedCallbackEnabled"
                                            "Shipping Option Changed Callback Enabled"
                                            None
                                            checkoutFlags.ShippingOptionChangedCallbackEnabled
                                            true
                                        checkboxView
                                            "paymentMethodChangedCallbackEnabled"
                                            "Payment Method Changed Callback Enabled"
                                            None
                                            checkoutFlags.PaymentMethodChangedCallbackEnabled
                                            true
                                        checkboxView
                                            "modeChangedCallbackEnabled"
                                            "Mode Changed Callback Enabled"
                                            None
                                            checkoutFlags.ModeChangedCallbackEnabled
                                            true
                                        checkboxView
                                            "hideAvardaLogo"
                                            "Hide Avarda logo"
                                            None
                                            checkoutFlags.HideAvardaLogo
                                            true
                                        textareaView
                                            "extraTermsAndConditions"
                                            "Extra Terms&Conditions"
                                            (Some
                                                "Make sure you enable either SMS or Email newsletter checkbox in order to display extra t&c")
                                            checkoutFlags.Extras.ExtraTermsAndConditions
                                            true
                                        div [] [ h3 [ _class "settings-heading" ] [ str "Payment and APR Widget" ] ]
                                        checkboxView
                                            "paymentWidgetEnabled"
                                            "Enable Payment Widget"
                                            paymentWidgetHelpText
                                            paymentWidgetSettings.Enabled
                                            (isPaymentWidgetEnabledGlobally paymentWidgetBundleUrl)
                                        textareaView
                                            "aprWidgetEnabled"
                                            "Enable APR Widget"
                                            (Some
                                                "To enable the APR widget, please enter the account class in the text area field. If you need to enable multiple widgets, enter the numbers separated by semicolons, like this: 1;2;3."
                                            )
                                            aprWidgetSettings.AccountClass
                                            (isPaymentWidgetEnabledGlobally paymentWidgetBundleUrl)
                                        checkboxView
                                            "sharedWidgetCustomStyles"
                                            "Use Custom Styles in Widgets"
                                            None
                                            sharedWidgetSettings.CustomStyles
                                            (isPaymentWidgetEnabledGlobally paymentWidgetBundleUrl)
                                        div [] [ h3 [ _class "settings-heading" ] [ str "Additional Features" ] ]
                                        checkboxView
                                            "partnerShippingEnabled"
                                            "Enable Partner Shipping Module"
                                            (Some
                                                "`partnerShippingBundleUrl` has to be provided and shipping module has to be setup on current partner in order to be used")
                                            additionalFeatures.PartnerShippingEnabled
                                            (String.IsNullOrEmpty partnerShippingBundleUrl |> not)
                                        div
                                            [ _style
                                                  "display: flex; flex: 1; flex-wrap: wrap; flex-direction: row; align-items: center; justify-content: space-between; padding: 20px 10px;" ]
                                            [ a
                                                  [ _class "btn amado-btn active"
                                                    _style "min-width: 200px; width: 30%;"
                                                    _href "/" ]
                                                  [ str "Back to Shop" ]
                                              input
                                                  [ _class "btn amado-btn"
                                                    _style "min-width: 200px; width: 30%;"
                                                    _type "submit"
                                                    _value "Save Settings" ] ] ] ] ] ] ]
          subscribeSectionView
          footerView ]

let settingsView
    (paymentWidgetBundleUrl: string)
    (enabledMarkets: Market list)
    (settings: Settings)
    (cartState: CartState)
    (partnerShippingBundleUrl: string)
    =
    [ template paymentWidgetBundleUrl enabledMarkets settings cartState partnerShippingBundleUrl ]
    |> layout