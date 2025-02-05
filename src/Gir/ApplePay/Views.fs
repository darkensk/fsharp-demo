module Gir.ApplePay.Views

open Giraffe.ViewEngine

let applePayButton = tag "apple-pay-button"

let _buttonStyleAttribute = attr "buttonstyle"

let _localeAttribute = attr "locale"

let applePayView =
    div
        []
        [ style
              []
              [ rawText
                    "        .container {
            display: flex;
            flex-direction: row;
            gap: 16px;
            padding: 16px;
        }
        .textarea-container {
            display: flex;
            flex-direction: column;
            width: 300px;
        }
        .textarea-container label {
            margin-bottom: 8px;
            font-weight: bold;
        }
        .textarea-container textarea {
            width: 100%;
            height: 150px;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            resize: none;
        }
        @media (max-width: 600px) {
            .container {
                flex-direction: column;
            }
        }" ]
          script
              [ _crossorigin "true"
                _src "https://applepay.cdn-apple.com/jsapi/1.latest/apple-pay-sdk.js" ]
              []

          script
              [ _crossorigin "true"
                _type "application/javascript"
                _src "https://dvsg0gir0shop0temp.z6.web.core.windows.net/assets/apple-pay-integration.js" ]
              []

          applePayButton
              [ _buttonStyleAttribute "black"
                _type "plain"
                _localeAttribute "en-US"
                _onclick "onApplePayButtonClicked()" ]
              []
          div
              [ _class "container" ]
              [ div
                    [ _class "textarea-container" ]
                    [ label
                          []
                          [ rawText "Tokenized ApplePay token:"
                            textarea [ _id "tokenized-apple-pay-token" ] [] ] ] ] ]
