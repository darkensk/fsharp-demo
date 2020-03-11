module Gir.Layout

open Giraffe.GiraffeViewEngine
open Gir.Domain


let _ariaHidden = attr "aria-hidden"

let _ariaLabel = attr "aria-label"

let _ariaControls = attr "aria-controls"

let _dataTarget = attr "data-target"

let _dataToggle = attr "data-toggle"

let _ariaExpanded = attr "aria-expanded"

let _ariaCurrent = attr "aria-current"

let _dataSlideTo = attr "data-slide-to"

let _dataRide = attr "data-ride"

let headerView (cartState: CartState) =
    let cartSize = List.fold (fun acc x -> acc + x.Qty) 0 cartState.Items
    let cartSizeString = "\"" + (string cartSize) + "\""

    header [ _class "header-area clearfix" ]
        [ div [ _class "nav-close" ]
              [ i
                  [ _class "fa fa-close"
                    _ariaHidden "true" ] [] ]
          div [ _class "logo" ]
              [ a [ _href "/" ]
                    [ img
                        [ _src "/img/core-img/logo.png"
                          _alt "" ] ] ]
          nav [ _class "amado-nav" ]
              [ ul []
                    [ li [] [ a [ _href "/" ] [ str "Home" ] ]
                      li [] [ a [ _href "/" ] [ str "Shop" ] ]
                      li [] [ a [ _href "/" ] [ str "Product" ] ]
                      li [ _class "active" ] [ a [ _href "/cart/" ] [ str "Cart" ] ]
                      li [] [ a [ _href "/cart/" ] [ str "Checkout" ] ] ] ]
          div [ _class "amado-btn-group mt-30 mb-100" ]
              [ a
                  [ _href "#"
                    _class "btn amado-btn mb-15" ] [ str "%Discount%" ]
                a
                    [ _href "#"
                      _class "btn amado-btn active" ] [ str "New this week" ] ]
          div [ _class "cart-fav-search mb-100" ]
              [ a
                  [ _href "/cart/"
                    _class "cart-nav" ]
                    [ img
                        [ _src "/img/core-img/cart.png"
                          _alt "" ]
                      str "Cart "
                      span [] [ str cartSizeString ] ]
                a
                    [ _href "#"
                      _class "fav-nav" ]
                    [ img
                        [ _src "/img/core-img/favorites.png"
                          _alt "" ]
                      str "Favourite" ]
                a
                    [ _href "#"
                      _class "search-nav" ]
                    [ img
                        [ _src "/img/core-img/search.png"
                          _alt "" ]
                      str "Search" ] ]
          div [ _class "social-info d-flex justify-content-between" ]
              [ a [ _href "#" ]
                    [ i
                        [ _class "fa fa-pinterest"
                          _ariaHidden "true" ] [] ]
                a [ _href "#" ]
                    [ i
                        [ _class "fa fa-instagram"
                          _ariaHidden "true" ] [] ]
                a [ _href "#" ]
                    [ i
                        [ _class "fa fa-facebook"
                          _ariaHidden "true" ] [] ]
                a [ _href "#" ]
                    [ i
                        [ _class "fa fa-twitter"
                          _ariaHidden "true" ] [] ] ] ]

let footerView =
    footer [ _class "footer_area clearfix" ]
        [ div [ _class "container" ]
              [ div [ _class "row align-items-center" ]
                    [ div [ _class "col-12 col-lg-4" ]
                          [ div [ _class "single_widget_area" ]
                                [ div [ _class "footer-logo mr-50" ]
                                      [ a [ _href "/" ]
                                            [ img
                                                [ _src "/img/core-img/logo2.png"
                                                  _alt "" ] ] ]
                                  p [ _class "copywrite" ]
                                      [ str "Copyright ©"
                                        script [] [ str "document.write(new Date().getFullYear());" ]
                                        str " All rights reserved | This template is made with "
                                        i
                                            [ _class "fa fa-heart-o"
                                              _ariaHidden "true" ] []
                                        str " by "
                                        a
                                            [ _href "https://colorlib.com"
                                              _target "_blank" ] [ str "Colorlib" ] ] ] ]
                      div [ _class "col-12 col-lg-8" ]
                          [ div [ _class "single_widget_area" ]
                                [ div [ _class "footer_menu" ]
                                      [ nav [ _class "navbar navbar-expand-lg justify-content-end" ]
                                            [ button
                                                [ _class "navbar-toggler"
                                                  _type "button"
                                                  _dataToggle "collapse"
                                                  _dataTarget "#footerNavContent"
                                                  _ariaControls "footerNavContent"
                                                  _ariaExpanded "false"
                                                  _ariaLabel "Toggle navigation" ] [ i [ _class "fa fa-bars" ] [] ]
                                              div
                                                  [ _class "collapse navbar-collapse"
                                                    _id "footerNavContent" ]
                                                  [ ul [ _class "navbar-nav ml-auto" ]
                                                        [ li [ _class "nav-item active" ]
                                                              [ a
                                                                  [ _class "nav-link"
                                                                    _href "/" ] [ str "Home" ] ]
                                                          li [ _class "nav-item" ]
                                                              [ a
                                                                  [ _class "nav-link"
                                                                    _href "/" ] [ str "Shop" ] ]
                                                          li [ _class "nav-item" ]
                                                              [ a
                                                                  [ _class "nav-link"
                                                                    _href "/" ] [ str "Product" ] ]
                                                          li [ _class "nav-item" ]
                                                              [ a
                                                                  [ _class "nav-link"
                                                                    _href "/cart/" ] [ str "Cart" ] ]
                                                          li [ _class "nav-item" ]
                                                              [ a
                                                                  [ _class "nav-link"
                                                                    _href "/cart/" ] [ str "Checkout" ] ] ] ] ] ] ] ] ] ] ]

let subscribeSectionView =
    section [ _class "newsletter-area section-padding-100-0" ]
        [ div [ _class "container" ]
              [ div [ _class "row align-items-center" ]
                    [ div [ _class "col-12 col-lg-6 col-xl-7" ]
                          [ div [ _class "newsletter-text mb-100" ]
                                [ h2 []
                                      [ str "Subscribe for a"
                                        span [] [ str " 25% Discount" ] ]
                                  p []
                                      [ str
                                          "Nulla ac convallis lorem, eget euismod nisl. Donec in libero sit amet mi vulputate consectetur. Donec auctor interdum purus, ac finibus massa bibendum nec." ] ] ]
                      div [ _class "col-12 col-lg-6 col-xl-5" ]
                          [ div [ _class "newsletter-form mb-100" ]
                                [ form []
                                      [ input
                                          [ _type "email"
                                            _name "email"
                                            _class "nl-email"
                                            _placeholder "Your E-mail" ]
                                        input
                                            [ _type "submit"
                                              _value "Subscribe" ] ] ] ] ] ] ]

let layout (content: XmlNode List) =
    html []
        [ head []
              [ meta [ _charset "UTF-8" ]
                meta
                    [ _name "description"
                      _content "" ]
                meta
                    [ _httpEquiv "X-UA-Compatible"
                      _content "IE=edge" ]
                meta
                    [ _name "viewport"
                      _content "width=device-width, initial-scale=1, shrink-to-fit=no" ]
                title [] [ str "GirShop" ]
                link
                    [ _rel "stylesheet"
                      _type "text/css"
                      _href "/css/core-style.css" ]
                link
                    [ _rel "stylesheet"
                      _type "text/css"
                      _href "/css/main.css" ] ]
          body []
              (content @ [ script [ _src "/js/jquery/jquery-2.2.4.min.js" ] []
                           script [ _src "/js/popper.min.js" ] []
                           script [ _src "/js/bootstrap.min.js" ] []
                           script [ _src "/js/plugins.js" ] []
                           script [ _src "/js/active.js" ] [] ]) ]
