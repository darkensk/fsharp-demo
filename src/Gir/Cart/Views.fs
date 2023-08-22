module Gir.Cart.Views

open Giraffe.ViewEngine
open Gir.Layout
open Gir.Domain
open Gir.Utils


let initCheckoutInstance (settings: Settings) (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    div
        [ _id "checkout-form"; _style "padding-top: 50px;" ]
        (match purchaseToken with
         | "" -> []
         | _ ->
             [ script [ _type "application/javascript"; _src "/js/checkout-integration.js" ] []
               script
                   [ _type "application/javascript" ]
                   [ rawText
                     <| sprintf
                         """initCheckout("%s", "%s", %b, %b, %b, %b, %b);"""
                         checkoutFrontendBundleUrl
                         purchaseToken
                         settings.ExtraCheckoutFlags.DisableFocus
                         settings.ExtraCheckoutFlags.CustomStyles
                         settings.ExtraCheckoutFlags.BeforeSubmitCallbackEnabled
                         settings.ExtraCheckoutFlags.DeliveryAddressChangedCallbackEnabled
                         settings.ExtraCheckoutFlags.IncludePaymentFeeInTotalPrice ] ])

let cartItemView (settings: Settings) (cartItem: CartItem) =
    tr
        []
        [ td
              [ _class "cart_product_img" ]
              [ a [ _href "#" ] [ img [ _src cartItem.ProductDetail.Img; _alt "Product" ] ] ]
          td [ _class "cart_product_desc" ] [ h5 [] [ str cartItem.ProductDetail.Name ] ]
          td
              [ _class "price" ]
              [ span
                    []
                    [ str
                      <| sprintf "%M %s" cartItem.ProductDetail.Price (marketToCurrency settings.Market) ] ]
          td
              [ _class "qty" ]
              [ div
                    []
                    [ div
                          [ _class "quantity"; _style "display: flex;" ]
                          [ form
                                [ _id "quantityForm"
                                  _method "POST"
                                  _action (sprintf "/product/%s/add" <| string cartItem.ProductDetail.ProductId) ]
                                [ button
                                      [ _class "qty-minus qtyButtons"
                                        _type "submit"
                                        _formaction (
                                            sprintf "/product/%s/remove" <| string cartItem.ProductDetail.ProductId
                                        ) ]
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
                                      [ _class "qty-plus qtyButtons"; _type "submit" ]
                                      [ i [ _class "fa fa-plus" ] [] ] ] ] ] ] ]

let cartSummaryView (settings: Settings) (cartState: CartState) =
    let subTotal =
        List.fold (fun acc x -> acc + (decimal x.Qty * x.ProductDetail.Price)) 0M cartState.Items

    let subTotalString = sprintf "\"%M %s\"" subTotal (marketToCurrency settings.Market)

    div
        [ _class "col-12 col-lg-4" ]
        [ div
              [ _class "cart-summary" ]
              [ h5 [] [ str "Cart Total" ]
                ul
                    [ _class "summary-table" ]
                    [ li
                          []
                          [ span [] [ str "Subtotal:" ]
                            span [ _style "text-transform: none;" ] [ str subTotalString ] ]
                      li [] [ span [] [ str "Delivery:" ]; span [] [ str "Free" ] ]
                      li
                          []
                          [ span [] [ str "Total:" ]
                            span [ _style "text-transform: none;" ] [ str subTotalString ] ] ]
                div
                    [ _class "amado-btn-group mt-30 mb-0"
                      _style "display: flex; flex-direction: column; justify-content: space-evenly;" ]
                    [ a [ _href "#checkout-form"; _class "btn amado-btn mb-15" ] [ str "Checkout" ]
                      a [ _href "/cart/clear"; _class "btn amado-btn active mb-15" ] [ str "Clear Cart" ] ] ] ]

let languageSelectView =
    details
        []
        [ summary [ _class "select-language-summary" ] [ str "Select language of Checkout" ]
          div
              [ _class "select-language-container" ]
              [ button
                    [ _class "select-flag"
                      _id "flag-en"
                      _onclick "avardaCheckout.changeLanguage('English');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/gb.svg"
                            _alt "English language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-se"
                      _onclick "avardaCheckout.changeLanguage('Swedish');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/se.svg"
                            _alt "Swedish language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-no"
                      _onclick "avardaCheckout.changeLanguage('Norwegian');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/no.svg"
                            _alt "Norwegian language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-fi"
                      _onclick "avardaCheckout.changeLanguage('Finnish');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/fi.svg"
                            _alt "Finnish language"
                            _ariaHidden "true" ] ] ] ]


let template
    (settings: Settings)
    (cartState: CartState)
    (products: Product list)
    (checkoutFrontendBundleUrl: string)
    (purchaseToken: string)
    =
    let products = products |> List.map (productDiv settings)

    div
        []
        [ div
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
                    [ _class "main-content-wrapper d-flex clearfix"
                      _id "cart-main-content-wrapper" ]
                    [ div
                          [ _class "mobile-nav" ]
                          [ div
                                [ _class "amado-navbar-brand" ]
                                [ a [ _href "/" ] [ img [ _src "/img/core-img/logo.png"; _alt "" ] ] ]
                            div [ _class "amado-navbar-toggler" ] [ span [] []; span [] []; span [] [] ] ]
                      headerView cartState
                      div
                          [ _class "cart-table-area section-padding-100" ]
                          [ div
                                [ _class "container-fluid" ]
                                [ div
                                      [ _class "row"; _id "cart-product-list" ]
                                      (match purchaseToken with
                                       | "" ->
                                           [ div
                                                 [ _class "col-12 col-lg-8" ]
                                                 [ div
                                                       [ _class "cart-title mt-50" ]
                                                       [ h2 [] [ str "Cart is empty - Try to add something!" ] ] ]
                                             div
                                                 [ _class "col-12 col-lg-12 products-catagories-area clearfix" ]
                                                 [ div [ _class "amado-pro-catagory clearfix" ] products ] ]
                                       | _ ->
                                           [ div
                                                 [ _class "col-12 col-lg-8" ]
                                                 [ div [ _class "cart-title mt-50" ] [ h2 [] [ str "Shopping Cart" ] ]
                                                   div
                                                       [ _class "cart-table clearfix" ]
                                                       [ table
                                                             [ _class "table table-responsive" ]
                                                             [ thead
                                                                   []
                                                                   [ tr
                                                                         []
                                                                         [ th [] []
                                                                           th [] [ str "Name" ]
                                                                           th [] [ str "Price" ]
                                                                           th [] [ str "Quantity" ] ] ]
                                                               tbody
                                                                   []
                                                                   (List.map (cartItemView settings) cartState.Items) ] ] ]
                                             cartSummaryView settings cartState
                                             div
                                                 [ _class "col-12 col-lg-8" ]
                                                 [ initCheckoutInstance settings checkoutFrontendBundleUrl purchaseToken
                                                   languageSelectView ] ]) ] ] ]
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
