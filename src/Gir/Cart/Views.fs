module Gir.Cart.Views

open Giraffe.GiraffeViewEngine
open Gir.Layout
open Gir.Domain
open Gir.Utils


let initCheckoutInstance (settings: Settings) (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    div
        [ _id "checkout-form"
          _style "padding-top: 50px;" ]
        (match purchaseToken with
         | "" -> []
         | _ ->
             [ script [ _type "application/javascript" ]
                   [ rawText
                     <| sprintf """
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

              const completedUrl = window.location.origin + "/cart/completed";
              const sessionExpiredUrl = window.location.origin + "/cart/sessionExpired";
              const cartUrl = window.location.origin + "/cart/";

              const completedCallback = () => {
                fetch(completedUrl)
                  .then(response => {
                    return response.text();
                  })
                  .then(data => {
                    console.log(data);
                  });
                window.location.replace(cartUrl);
              };

              var handleByMerchantCallback = function(avardaCheckoutInstance) {
                console.log("Handle external payment here");

                // Un-mount Checkout 3.0 frontend app from the page when external payment is handled
                avardaCheckoutInstance.unmount();
                // Display success message instead of Checkout 3.0 frontend application
                document.getElementById("checkout-form").innerHTML =
                  "<br><h2>External payment handled by partner!</h2><br>";

                completedCallback();
              };

              const sessionExpired = () => {
                fetch(sessionExpiredUrl)
                  .then(response => {
                    return response.text();
                  })
                  .then(data => {
                    console.log(data);
                  });
                window.location.replace(cartUrl);
              };

              var sessionTimedOutCallback = function(avardaCheckoutInstance) {
                  console.log("Session Timed Out - Handle here!");

                  // Un-mount Checkout 3.0 frontend app from the page
                  avardaCheckoutInstance.unmount();
                  // Start Session Expired handling process
                  sessionExpired();
              };

              const redirectUrlCallback = () =>
                window.location.origin + "/cart/#checkout-form";

              window.avardaCheckoutInit({
                "purchaseJwt": "%s",
                "rootElementId": "checkout-form",
                "redirectUrl": redirectUrlCallback,
                "styles": {},
                "disableFocus": %b,
                "handleByMerchantCallback": handleByMerchantCallback,
                "completedPurchaseCallback": completedCallback,
                "sessionTimedOutCallback": sessionTimedOutCallback,
              });
              """        checkoutFrontendBundleUrl purchaseToken settings.ExtraCheckoutFlags.DisableFocus ] ])

let cartItemView (cartItem: CartItem) =
    tr []
        [ td [ _class "cart_product_img" ]
              [ a [ _href "#" ]
                    [ img
                        [ _src cartItem.ProductDetail.Img
                          _alt "Product" ] ] ]
          td [ _class "cart_product_desc" ] [ h5 [] [ str cartItem.ProductDetail.Name ] ]
          td [ _class "price" ] [ span [] [ str (string cartItem.ProductDetail.Price + " kr") ] ]
          td [ _class "qty" ]
              [ div []
                    [ div
                        [ _class "quantity"
                          _style "display: flex;" ]
                          [ form
                              [ _id "quantityForm"
                                _method "POST"
                                _action
                                    (sprintf "/product/%s/add"
                                     <| string cartItem.ProductDetail.ProductId) ]
                                [ button
                                    [ _class "qty-minus qtyButtons"
                                      _type "submit"
                                      _formaction
                                          (sprintf "/product/%s/remove"
                                           <| string cartItem.ProductDetail.ProductId) ]
                                      [ i [ _class "fa fa-minus" ] [] ]
                                  input
                                      [ _type "number"
                                        _class "qty-text"
                                        _id "qty"
                                        _step "1"
                                        _min "1"
                                        _max "300"
                                        _name "quantity"
                                        _value (string cartItem.Qty)
                                        _disabled ]
                                  button
                                      [ _class "qty-plus qtyButtons"
                                        _type "submit" ] [ i [ _class "fa fa-plus" ] [] ] ] ] ] ] ]

let cartSummaryView (cartState: CartState) =
    let subTotal =
        List.fold (fun acc x -> acc + (float x.Qty * x.ProductDetail.Price)) 0. cartState.Items

    let subTotalString = "\"" + string subTotal + " kr\""
    div [ _class "col-12 col-lg-4" ]
        [ div [ _class "cart-summary" ]
              [ h5 [] [ str "Cart Total" ]
                ul [ _class "summary-table" ]
                    [ li []
                          [ span [] [ str "Subtotal:" ]
                            span [ _style "text-transform: none;" ] [ str subTotalString ] ]
                      li []
                          [ span [] [ str "Delivery:" ]
                            span [] [ str "Free" ] ]
                      li []
                          [ span [] [ str "Total:" ]
                            span [ _style "text-transform: none;" ] [ str subTotalString ] ] ]
                div
                    [ _class "amado-btn-group mt-30 mb-0"
                      _style "display: flex; flex-direction: column; justify-content: space-evenly;" ]
                    [ a
                        [ _href "#checkout-form"
                          _class "btn amado-btn mb-15" ] [ str "Checkout" ]
                      a
                          [ _href "/cart/clear"
                            _class "btn amado-btn active mb-15" ] [ str "Clear Cart" ] ] ] ]

let template
    (settings: Settings)
    (cartState: CartState)
    (products: Product list)
    (checkoutFrontendBundleUrl: string)
    (purchaseToken: string)
    =
    let products = products |> List.map productDiv
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
                      div [ _class "cart-table-area section-padding-100" ]
                          [ div [ _class "container-fluid" ]
                                [ div
                                    [ _class "row"
                                      _id "cart-product-list" ]
                                      (match purchaseToken with
                                       | "" ->
                                           [ div [ _class "col-12 col-lg-8" ]
                                                 [ div [ _class "cart-title mt-50" ]
                                                       [ h2 [] [ str "Cart is empty - Try to add something!" ] ] ]
                                             div [ _class "col-12 col-lg-12 products-catagories-area clearfix" ]
                                                 [ div [ _class "amado-pro-catagory clearfix" ] products ] ]
                                       | _ ->
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
                                                               tbody [] (List.map (cartItemView) cartState.Items) ] ] ]
                                             cartSummaryView cartState
                                             div [ _class "col-12 col-lg-8" ]
                                                 [ initCheckoutInstance settings checkoutFrontendBundleUrl purchaseToken ] ]) ] ] ]
                subscribeSectionView
                footerView ] ]

let cartView
    (settings: Settings)
    (cartState: CartState)
    (checkoutFrontendBundleUrl: string)
    (purchaseToken: string)
    (products: Product list)
    =
    [ template settings cartState products checkoutFrontendBundleUrl purchaseToken ]
    |> layout
