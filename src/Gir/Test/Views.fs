module Gir.Test.Views

open Giraffe.ViewEngine
open Gir.Layout
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
          (if not (String.IsNullOrEmpty partnerShippingBundleUrl) then
               script
                   [ _src partnerShippingBundleUrl
                     _type "module"
                     _async
                     _crossorigin "annonymous" ]
                   []
           else
               div [] []) ]

let testCheckoutView (checkoutFrontendBundleUrl: string) (purchaseToken: string) (partnerShippingBundleUrl: string) =
    [ template checkoutFrontendBundleUrl purchaseToken partnerShippingBundleUrl ]
    |> layout
