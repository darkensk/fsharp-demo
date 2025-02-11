module Gir.PayFrame.Views

open Giraffe.ViewEngine
open Gir.Layout
open Gir.Domain

let template (cartState: CartState) (payFrameBundle: string) (siteKey: string) (language: string) =

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
                          [ _id "pay-frame"; _class "pay-frame-view section-padding-100" ]
                          [ script [ _type "application/javascript"; _src "/js/pay-frame-integration.js" ] []
                            script
                                [ _type "application/javascript" ]
                                [ rawText
                                  <| sprintf """initPayFrame("%s","%s", "%s");""" payFrameBundle siteKey language ]
                            div [] [] ] ]
                subscribeSectionView
                footerView ] ]




let payFrameView (cartState: CartState) (payFrameBundle: string) (siteKey: string) (language: string) =
    [ template cartState payFrameBundle siteKey language ] |> layout
