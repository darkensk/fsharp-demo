module Gir.Test.Views

open Giraffe.ViewEngine
open Gir.Layout
open System

open System




let template (checkoutFrontendBundleUrl: string) (purchaseToken: string) (partnerShippingBundleUrl: string) =
    div
        [ _id "checkout-form"
          _style "padding: 20px;"
          _data "purchaseToken" purchaseToken ]

        [ script [ _type "application/javascript"; _src "/js/checkout-integration-simple.js" ] []
          script
              [ _type "application/javascript" ]
              [ rawText
                <| sprintf """initCheckout("%s", "%s");""" checkoutFrontendBundleUrl purchaseToken ]
          script [ _src partnerShippingBundleUrl; _async; _crossorigin "annonymous" ] [] ]

let testCheckoutView (checkoutFrontendBundleUrl: string) (purchaseToken: string) (partnerShippingBundleUrl: string) =
    [ template checkoutFrontendBundleUrl purchaseToken partnerShippingBundleUrl ]
    |> layout
