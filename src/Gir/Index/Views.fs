module Gir.Index.Views

open Giraffe.GiraffeViewEngine

open Gir.Shared.Layout


let template =
    div []
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
                div [ _class "products-catagories-area clearfix" ]
                    [ div [ _class "amado-pro-catagory clearfix" ]
                          [ div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/1.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $180" ]
                                              h4 [] [ str "Modern Chair" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/2.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $180" ]
                                              h4 [] [ str "Minimalistic Plant Pot" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/3.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $180" ]
                                              h4 [] [ str "Modern Chair" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/4.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $180" ]
                                              h4 [] [ str "Night Stand" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/5.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $18" ]
                                              h4 [] [ str "Plant Pot" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/6.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $320" ]
                                              h4 [] [ str "Small Table" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/7.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $318" ]
                                              h4 [] [ str "Metallic Chair" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/8.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $318" ]
                                              h4 [] [ str "Modern Rocking Chair" ] ] ] ]
                            div [ _class "single-products-catagory clearfix" ]
                                [ a [ _href "#" ]
                                      [ img
                                          [ _src "/img/bg-img/9.jpg"
                                            _alt "" ]
                                        div [ _class "hover-content" ]
                                            [ div [ _class "line" ] []
                                              p [] [ str "From $318" ]
                                              h4 [] [ str "Home Deco" ] ] ] ] ] ] ]
          section [ _class "newsletter-area section-padding-100-0" ]
              [ div [ _class "container" ]
                    [ div [ _class "row align-items-center" ]
                          [ div [ _class "col-12 col-lg-6 col-xl-7" ]
                                [ div [ _class "newsletter-text mb-100" ]
                                      [ h2 []
                                            [ str "Subscribe for a"
                                              span [] [ str "25% Discount" ] ]
                                        p []
                                            [ str
                                                "Nulla ac convallis lorem, eget euismod nisl. Donec in libero sit amet mi vulputate consectetur. Donec auctor interdum purus, ac finibus massa bibendum nec." ] ] ]
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
                                                    _value "Subscribe" ] ] ] ] ] ] ] ]


let indexView = [ template ] |> layout
