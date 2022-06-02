module Gir.Test.Views

open Giraffe.ViewEngine
open Gir.Layout


let template (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    div [ _id "checkout-form"
          _style "padding: 20px;" ] [
        script [ _type "application/javascript"
                 _src "/js/checkout-integration-simple.js" ] []
        script [ _type "application/javascript" ] [
            rawText
            <| sprintf """initCheckout("%s", "%s");""" checkoutFrontendBundleUrl purchaseToken
        ]
    ]

let testCheckoutView (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    [ template checkoutFrontendBundleUrl purchaseToken ]
    |> layout
