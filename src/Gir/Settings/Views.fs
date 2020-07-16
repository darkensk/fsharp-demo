module Gir.Settings.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout
open Gir.Domain


let rowStyles =
    _style
        "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 0px 50px; height: 50px;"

let columnStyles =
    _style "display: flex; flex-direction: column; padding: 20px 0px 0px 20px;"

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

    let disabledAttribute = if isEnabled then [] else [ _disabled ]

    div [ rowStyles ]
        [ label [ _for selectId ] [ str selectLabel ]
          select
              ([ _name selectId; _id selectId ]
               @ disabledAttribute)
              (List.map (fun o -> option ([ _value o ] @ selectedOptionAttribute o) [ str o ]) selectOptions) ]

let inputView inputId inputLabel value isEnabled =
    let disabledAttribute = if isEnabled then [] else [ _disabled ]
    div [ rowStyles ]
        [ label [ _for inputId ] [ str inputLabel ]
          input
              ([ _type "text"
                 _style "width: 50%;
                        height: 50px;
                        background-color: #fff;
                        color: #000;
                        font-size: 14px;
                        border: 1px solid #e8e8e8;
                        border-radius: 5px;
                        padding: 0 10px;"
                 _id inputId
                 _name inputId
                 _value value ]
               @ disabledAttribute) ]

let template (settings: Settings) (cartState: CartState) =
    let checkboxStateOptions = [ "Hidden"; "Checked"; "Unchecked" ]

    let { ExtraInitSettings = initSettings; ExtraCheckoutFlags = checkoutFlags } = settings

    div []
        [ div [ _class "search-wrapper section-padding-100" ]
              [ div [ _class "search-close" ]
                    [ i
                        [ _class "fa fa-close"
                          _ariaHidden "true" ] [] ]
                div [ _class "container" ]
                    [ div [ _class "row" ]
                          [ div [ _class "col-12" ]
                                [ div [ _class "search-content" ]
                                      [ form [ _action "#"; _method "get" ]
                                            [ input
                                                [ _type "search"
                                                  _name "search"
                                                  _id "search"
                                                  _placeholder "Type your keyword..." ]
                                              button [ _type "submit" ]
                                                  [ img
                                                      [ _src "/img/core-img/search.png"
                                                        _alt "" ] ] ] ] ] ] ] ]
          div [ _class "main-content-wrapper d-flex clearfix" ]
              [ div [ _class "mobile-nav" ]
                    [ div [ _class "amado-navbar-brand" ]
                          [ a [ _href "/" ]
                                [ img
                                    [ _src "/img/core-img/logo.png"
                                      _alt "" ] ] ]
                      div [ _class "amado-navbar-toggler" ] [ span [] []; span [] []; span [] [] ] ]
                headerView cartState
                div [ _class "single-product-area section-padding-100 clearfix" ]
                    [ div [ _class "container-fluid" ]
                          [ div [ columnStyles ]
                                [ form
                                    [ _id "settings"
                                      _action "/settings/save"
                                      _method "POST" ]
                                      [ div [] [ h3 [] [ str "Market" ] ]
                                        selectView "market" "Market" [ "Sweden"; "Finland" ]
                                            (marketToString settings.Market) true
                                        div [] [ h3 [] [ str "Extra Identifiers" ] ]
                                        inputView "orderReference" "Order Reference" settings.OrderReference true
                                        div [] [ h3 [] [ str "Extra Init Options" ] ]
                                        selectView "language" "Language"
                                            [ "English"
                                              "Swedish"
                                              "Finnish"
                                              "Norwegian"
                                              "Estonian"
                                              "Danish" ] (languageToString initSettings.Language) true
                                        selectView "mode" "Mode" [ "b2c"; "b2b" ] "b2c" true
                                        selectView "differentDeliveryAddress" "Different Delivery Address"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.DifferentDeliveryAddress) true
                                        checkboxView "displayItems" "Display Items" initSettings.DisplayItems true
                                        selectView "recurringPayments" "Recurring Payments" checkboxStateOptions
                                            (checkboxStateToString initSettings.RecurringPayments) true
                                        selectView "smsNewsletterSubscription" "SMS Newsletter Subscription"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.SmsNewsletterSubscription) true
                                        selectView "emailNewsletterSubscription" "Email Newsletter Subscription"
                                            checkboxStateOptions
                                            (checkboxStateToString initSettings.EmailNewsletterSubscription) true
                                        div [] [ h3 [] [ str "Extra Checkout Flags" ] ]
                                        checkboxView "disableFocus" "Disable Focus" checkoutFlags.DisableFocus true
                                        checkboxView "beforeSubmitCallbackEnabled" "Before Submit Callback Enabled"
                                            checkoutFlags.BeforeSubmitCallbackEnabled true
                                        checkboxView "deliveryAddressChangedCallbackEnabled"
                                            "Delivery Address Changed Callback Enabled"
                                            checkoutFlags.DeliveryAddressChangedCallbackEnabled true
                                        checkboxView "customStyles" "Use Custom Styles" checkoutFlags.CustomStyles true
                                        div
                                            [ _style
                                                "display: flex; flex-direction: row; align-items: center; justify-content: space-between; padding: 20px 50px;" ]
                                            [ a
                                                [ _class "btn amado-btn active"
                                                  _href "/" ] [ str "Back to Shop" ]
                                              input
                                                  [ _class "btn amado-btn"
                                                    _type "submit"
                                                    _value "Save Settings" ] ] ] ] ] ] ]
          subscribeSectionView
          footerView ]

let settingsView (settings: Settings) (cartState: CartState) =
    [ template settings cartState ] |> layout
