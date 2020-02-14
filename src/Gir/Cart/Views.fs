module Gir.Cart.Views

open Giraffe.GiraffeViewEngine
open Gir.Shared.Layout


let initCheckoutInstance (purchaseToken: string) =
    div
        [ _id "checkout-form"
          _class "hey" ]
        [ h2 [] [ rawText purchaseToken ]
          script [ _type "application/javascript" ] [ rawText <| sprintf """
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
                "https://avdonl0t0checkout0fe.blob.core.windows.net/frontend/static/js/main.js"
              );
              var handleByMerchantCallback = function(avardaCheckoutInstance) {
                console.log("Handle external payment here");

                // Un-mount Checkout 3.0 frontend app from the page when external payment is handled
                avardaCheckoutInstance.unmount();
                // Display success message instead of Checkout 3.0 frontend application
                document.getElementById("checkout-form").innerHTML =
                  "<br><h2>External payment handled by partner!</h2><br>";
              };

              window.avardaCheckoutInit({
                accessToken: "%s",
                rootElementId: "checkout-form",
                redirectUrl: "",
                styles: {},
                disableFocus: true,
                handleByMerchantCallback: handleByMerchantCallback
              });
            """                                                   purchaseToken ] ]

let template (purchaseToken: string) =
    div []
        [ div []
              [ div [ _class "search-wrapper section-padding-100" ]
                    [ div [ _class "search-close" ]
                          [ i
                              [ _class "fa fa-close"
                                _ariaHidden "true" ] [] ]
                      div [ _class "container" ]
                          [ div [ _class "row" ]
                                [ div [ _class "col-12" ]
                                      [ div [ _class "search-content" ]
                                            [ form
                                                [ _action "#"
                                                  _method "get" ]
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
                            div [ _class "amado-navbar-toggler" ]
                                [ span [] []
                                  span [] []
                                  span [] [] ] ]
                      header()
                      div [ _class "cart-table-area section-padding-100" ]
                          [ div [ _class "container-fluid" ]
                                [ div [ _class "row" ]
                                      [ div [ _class "col-12 col-lg-8" ]
                                            [ div [ _class "cart-title mt-50" ] [ h2 [] [ str "Shopping Cart" ] ]
                                              div [ _class "cart-table clearfix" ]
                                                  [ table [ _class "table table-responsive" ]
                                                        [ thead []
                                                              [ tr []
                                                                    [ th [] []
                                                                      th [] [ str "Name" ]
                                                                      th [] [ str "Price" ]
                                                                      th [] [ str "Quantity" ] ] ]
                                                          tbody []
                                                              [ tr []
                                                                    [ td [ _class "cart_product_img" ]
                                                                          [ a [ _href "#" ]
                                                                                [ img
                                                                                    [ _src "/img/bg-img/cart1.jpg"
                                                                                      _alt "Product" ] ] ]
                                                                      td [ _class "cart_product_desc" ]
                                                                          [ h5 [] [ str "White Modern Chair" ] ]
                                                                      td [ _class "price" ]
                                                                          [ span [] [ str "$130" ] ]
                                                                      td [ _class "qty" ]
                                                                          [ div [ _class "qty-btn d-flex" ]
                                                                                [ p [] [ str "Qty" ]
                                                                                  div [ _class "quantity" ]
                                                                                      [ span
                                                                                          [ _class "qty-minus"
                                                                                            _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-minus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ]
                                                                                        input
                                                                                            [ _type "number"
                                                                                              _class "qty-text"
                                                                                              _id "qty"
                                                                                              _step "1"
                                                                                              _min "1"
                                                                                              _max "300"
                                                                                              _name "quantity"
                                                                                              _value "1" ]
                                                                                        span
                                                                                            [ _class "qty-plus"
                                                                                              _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-plus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ] ] ] ] ]
                                                                tr []
                                                                    [ td [ _class "cart_product_img" ]
                                                                          [ a [ _href "#" ]
                                                                                [ img
                                                                                    [ _src "/img/bg-img/cart2.jpg"
                                                                                      _alt "Product" ] ] ]
                                                                      td [ _class "cart_product_desc" ]
                                                                          [ h5 [] [ str "Minimal Plant Pot" ] ]
                                                                      td [ _class "price" ]
                                                                          [ span [] [ str "$10" ] ]
                                                                      td [ _class "qty" ]
                                                                          [ div [ _class "qty-btn d-flex" ]
                                                                                [ p [] [ str "Qty" ]
                                                                                  div [ _class "quantity" ]
                                                                                      [ span
                                                                                          [ _class "qty-minus"
                                                                                            _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-minus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ]
                                                                                        input
                                                                                            [ _type "number"
                                                                                              _class "qty-text"
                                                                                              _id "qty2"
                                                                                              _step "1"
                                                                                              _min "1"
                                                                                              _max "300"
                                                                                              _name "quantity"
                                                                                              _value "1" ]
                                                                                        span
                                                                                            [ _class "qty-plus"
                                                                                              _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-plus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ] ] ] ] ]
                                                                tr []
                                                                    [ td [ _class "cart_product_img" ]
                                                                          [ a [ _href "#" ]
                                                                                [ img
                                                                                    [ _src "/img/bg-img/cart3.jpg"
                                                                                      _alt "Product" ] ] ]
                                                                      td [ _class "cart_product_desc" ]
                                                                          [ h5 [] [ str "Minimal Plant Pot" ] ]
                                                                      td [ _class "price" ]
                                                                          [ span [] [ str "$10" ] ]
                                                                      td [ _class "qty" ]
                                                                          [ div [ _class "qty-btn d-flex" ]
                                                                                [ p [] [ str "Qty" ]
                                                                                  div [ _class "quantity" ]
                                                                                      [ span
                                                                                          [ _class "qty-minus"
                                                                                            _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-minus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ]
                                                                                        input
                                                                                            [ _type "number"
                                                                                              _class "qty-text"
                                                                                              _id "qty3"
                                                                                              _step "1"
                                                                                              _min "1"
                                                                                              _max "300"
                                                                                              _name "quantity"
                                                                                              _value "1" ]
                                                                                        span
                                                                                            [ _class "qty-plus"
                                                                                              _onclick "" ]
                                                                                            [ i
                                                                                                [ _class "fa fa-plus"
                                                                                                  _ariaHidden "true" ]
                                                                                                  [] ] ] ] ] ] ] ] ] ]
                                        div [ _class "col-12 col-lg-4" ]
                                            [ div [ _class "cart-summary" ]
                                                  [ h5 [] [ str "Cart Total" ]
                                                    ul [ _class "summary-table" ]
                                                        [ li []
                                                              [ span [] [ str "subtotal:" ]
                                                                span [] [ str "$140.00" ] ]
                                                          li []
                                                              [ span [] [ str "delivery:" ]
                                                                span [] [ str "Free" ] ]
                                                          li []
                                                              [ span [] [ str "total:" ]
                                                                span [] [ str "$140.00" ] ] ]
                                                    div [ _class "cart-btn mt-100" ]
                                                        [ a
                                                            [ _href "/cart/"
                                                              _class "btn amado-btn w-100" ] [ str "Checkout" ] ] ] ] ] ] ] ]
                section [ _class "newsletter-area section-padding-100-0" ]
                    [ div [ _class "container" ]
                          [ div [ _class "row align-items-center" ]
                                [ div [ _class "col-12 col-lg-6 col-xl-7" ]
                                      [ div [ _class "newsletter-text mb-100" ]
                                            [ h2 []
                                                  [ str "Subscribe for a "
                                                    span [] [ str " 25% Discount" ] ]
                                              p [] [ str "Nulla ac convallis lorem, eget euismod nisl. Donec in libero sit
                      amet mi vulputate consectetur. Donec auctor interdum purus, ac
                      finibus massa bibendum nec." ] ] ]
                                  div [ _class "col-12 col-lg-6 col-xl-5" ]
                                      [ div [ _class "newsletter-form mb-100" ]
                                            [ form
                                                [ _action "#"
                                                  _method "post" ]
                                                  [ input
                                                      [ _type "email"
                                                        _name "email"
                                                        _class "nl-email"
                                                        _placeholder "Your E-mail" ]
                                                    input
                                                        [ _type "submit"
                                                          _value "Subscribe" ] ] ] ] ] ] ]
                footer() ] ]

let cartView (purchaseToken: string) = [ template purchaseToken ] |> layout
