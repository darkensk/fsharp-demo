module Gir.Settings.Views

open Giraffe.ViewEngine
open Gir.Layout
open Gir.Domain
open Gir.Utils

let rowStyles =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 10px 0px 0px; height: 50px;"

let helpTextRowStyles =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 20px 10px 20px; height: 20px; color: grey; font-size: 12px; font-weight: 100;"

let columnStyles =
    _style "display: flex; flex-direction: column; padding: 20px 0px 0px 20px;"

let labelStyles =
    "text-overflow: ellipsis; overflow: hidden; margin: 0px;"

let checkboxLabelStyles =
    "text-overflow: ellipsis; overflow: hidden; margin: 0px; cursor: pointer;"

let checkboxView (inputId: string) (inputLabel: string) (helpText: string option) (isChecked: bool) (isEnabled: bool) =
    let checkedAttribute = if isChecked then [ _checked ] else []
    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    div [] [
        div [ rowStyles ] [
            label [ _for inputId; _style checkboxLabelStyles ] [
                str inputLabel
            ]
            div [ _style "display: flex; flex-direction-row; min-width: 60px;" ] [
                input (
                    [ _style "width: 30px; height: 30px; cursor: pointer;"
                      _type "checkbox"
                      _id inputId
                      _name inputId
                      _value "true" ]
                    @ checkedAttribute @ disabledAttribute
                )
            ]
        ]
        (helpText
         |> Option.map (fun ht -> div [ helpTextRowStyles ] [ str ht ])
         |> Option.defaultValue (div [] []))
    ]

let textareaView (areaId: string) (areaLabel: string) =
    div [ rowStyles ] [
        label [ _for areaId; _style labelStyles ] [
            str areaLabel
        ]
        textarea [ _id areaId
                   _name areaId
                   _value ""
                   _form "settings" ] []
    ]

let selectView
    (selectId: string)
    (selectLabel: string)
    (selectOptions: (string * bool) list)
    (selectedOption: string)
    (isEnabled: bool)
    =
    let selectedOptionAttribute option =
        if option = selectedOption then
            [ _selected ]
        else
            []

    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    div [ rowStyles ] [
        label [ _for selectId; _style labelStyles ] [
            str selectLabel
        ]
        select
            ([ _name selectId; _id selectId ]
             @ disabledAttribute)
            (List.map
                (fun (o, isDisabled) ->
                    option
                        ([ _value o
                           if isDisabled then _disabled ]
                         @ selectedOptionAttribute o)
                        [ str o ])
                selectOptions)
    ]

let inputView inputType inputStyle inputId inputLabel helpText value isEnabled minMax =
    let disabledAttribute = if isEnabled then [] else [ _disabled ]
    let minMaxAttributes = 
        match minMax with 
            | Some (min, max) -> [ _min min; _max max; ]
            | None -> []

    div [] [
        div [ rowStyles ] [
            label [ _for inputId; _style labelStyles ] [
                str inputLabel
            ]
            input (
                [ _type inputType
                  _style inputStyle
                  _id inputId
                  _name inputId
                  _value value ]
                @ disabledAttribute
                @ minMaxAttributes
            )
        ]
        (helpText
        |> Option.map (fun ht -> div [ helpTextRowStyles ] [ str ht ])
        |> Option.defaultValue (div [] []))
    ]

let textInput =
    inputView "text" "width: 50%;
                        height: 50px;
                        background-color: #fff;
                        color: #000;
                        font-size: 14px;
                        border: 1px solid #e8e8e8;
                        border-radius: 5px;
                        padding: 0 10px;"

let numberInput = 
    inputView "number" "width: 20%;
                        height: 50px;
                        background-color: #fff;
                        color: #000;
                        font-size: 14px;
                        border: 1px solid #e8e8e8;
                        border-radius: 5px;
                        padding: 0 10px;"

let template (partPaymentWidgetBundleUrl: string) (enabledMarkets: Market list) (settings: Settings) (cartState: CartState) =
    let checkboxStateOptions =
        [ ("Hidden", false)
          ("Checked", false)
          ("Unchecked", false) ]

    let { ExtraInitSettings = initSettings
          ExtraCheckoutFlags = checkoutFlags 
          PartPaymentWidgetSettings = partPaymentWidgetSettings } =
        settings

    let marketsOptions =
        List.map (fun m -> (marketToString m, false)) enabledMarkets

    let partPaymentWidgetHelpText = 
        if (isPartPaymentWidgetEnabledGlobally partPaymentWidgetBundleUrl) then None else Some "Requires 'partPaymentWidgetBundleUrl' environment variable specified"

    div [] [
        div [ _class "search-wrapper section-padding-100" ] [
            div [ _class "search-close" ] [
                i [ _class "fa fa-close"
                    _ariaHidden "true" ] []
            ]
            div [ _class "container" ] [
                div [ _class "row" ] [
                    div [ _class "col-12" ] [
                        div [ _class "search-content" ] [
                            form [ _action "#"; _method "get" ] [
                                input [ _type "search"
                                        _name "search"
                                        _id "search"
                                        _placeholder "Type your keyword..." ]
                                button [ _type "submit" ] [
                                    img [ _src "/img/core-img/search.png"
                                          _alt "" ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        div [ _class "main-content-wrapper d-flex clearfix" ] [
            div [ _class "mobile-nav" ] [
                div [ _class "amado-navbar-brand" ] [
                    a [ _href "/" ] [
                        img [ _src "/img/core-img/logo.png"
                              _alt "" ]
                    ]
                ]
                div [ _class "amado-navbar-toggler" ] [
                    span [] []
                    span [] []
                    span [] []
                ]
            ]
            headerView cartState
            div [ _class "single-product-area section-padding-100 clearfix" ] [
                div [ _class "container-fluid" ] [
                    div [ columnStyles ] [
                        h1 [] [ str "Settings Page" ]
                        form [ _id "settings"
                               _action "/settings/save"
                               _method "POST" ] [
                            div [] [ h3 [] [ str "Market" ] ]
                            selectView "market" "Market" marketsOptions (marketToString settings.Market) true
                            div [] [
                                h3 [] [ str "Extra Identifiers" ]
                            ]
                            textInput "orderReference" "Order Reference" None settings.OrderReference true None
                            div [] [
                                h3 [] [ str "Extra Init Options" ]
                            ]
                            selectView
                                "language"
                                "Language"
                                [ ("English", false)
                                  ("Swedish", false)
                                  ("Finnish", false)
                                  ("Norwegian", false)
                                  ("Danish", false)
                                  ("Slovak", true)
                                  ("Czech", true)
                                  ("Latvian", true)
                                  ("Polish", true)
                                  ("Estonian", true) ]
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
                                [ ("NotSet", false)
                                  ("ShouldSucceed", false)
                                  ("ShouldFail", false) ]
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
                            numberInput "ageValidation" "Age Validation" (Some "Insert age limit as number between 0 and 100") (initSettings.AgeValidation |> ageValidationToString) true (Some ("0", "100"))
                            div [] [
                                h3 [] [ str "Extra Checkout Flags" ]
                            ]
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
                            checkboxView "customStyles" "Use Custom Styles" None checkoutFlags.CustomStyles true
                            checkboxView "includePaymentFeeInTotalPrice" "Include Payment Fees in Total Price" None checkoutFlags.IncludePaymentFeeInTotalPrice true
                            div [] [
                                h3 [] [ str "Part Payment Widget" ]
                            ]
                            checkboxView "partPaymentWidgetEnabled" "Enable Part Payment Widget" partPaymentWidgetHelpText partPaymentWidgetSettings.Enabled (isPartPaymentWidgetEnabledGlobally partPaymentWidgetBundleUrl)
                            checkboxView "partPaymentWidgetCustomStyles" "Use Custom Styles in Part Payment Widget" None partPaymentWidgetSettings.CustomStyles (isPartPaymentWidgetEnabledGlobally partPaymentWidgetBundleUrl)
                            div [ _style
                                      "display: flex; flex: 1; flex-wrap: wrap; flex-direction: row; align-items: center; justify-content: space-between; padding: 20px 10px;" ] [
                                a [ _class "btn amado-btn active"
                                    _style "min-width: 200px; width: 30%;"
                                    _href "/" ] [
                                    str "Back to Shop"
                                ]
                                input [ _class "btn amado-btn"
                                        _style "min-width: 200px; width: 30%;"
                                        _type "submit"
                                        _value "Save Settings" ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        subscribeSectionView
        footerView
    ]

let settingsView (partPaymentWidgetBundleUrl: string) (enabledMarkets: Market list) (settings: Settings) (cartState: CartState) =
    [ template partPaymentWidgetBundleUrl enabledMarkets settings cartState ]
    |> layout
