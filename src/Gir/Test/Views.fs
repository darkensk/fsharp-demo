module Gir.Test.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout


let template (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    div
        [ _id "checkout-form"
          _style "padding: 20px;" ]
        [ script [ _type "application/javascript" ] [ rawText <| sprintf """
              (function(e, t, n, a, s, c, o, i, r) {
                e[a] =
                  e[a] ||
                  function() {
                    (e[a].q = e[a].q || []).push(arguments);
                  };
                e[a].i = s;
                i = t.createElement(n);
                i.async = 1;
                i.src = o + "?v=" + c + "&ts=" + 1 * new Date();
                r = t.getElementsByTagName(n)[0];
                r.parentNode.insertBefore(i, r);
              })(
                window,
                document,
                "script",
                "avardaCheckoutInit",
                "avardaCheckout",
                "1.0.0",
                "%s"
              );

              var handleByMerchantCallback = function(avardaCheckoutInstance) {
                console.log("Handle external payment here");

                // Un-mount Checkout 3.0 frontend app from the page when external payment is handled
                avardaCheckoutInstance.unmount();
                // Display success message instead of Checkout 3.0 frontend application
                document.getElementById("checkout-form").innerHTML =
                  "<br><h2>External payment handled by partner!</h2><br>";
              };

              const redirectUrlCallback = () =>
                window.location.origin;

              window.avardaCheckoutInit({
                purchaseJwt: "%s",
                rootElementId: "checkout-form",
                redirectUrl: redirectUrlCallback,
                styles: {},
                disableFocus: true,
                handleByMerchantCallback: handleByMerchantCallback
              });
              """                                                 checkoutFrontendBundleUrl purchaseToken ] ]

let testCheckoutView (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    [ template checkoutFrontendBundleUrl purchaseToken ] |> layout
