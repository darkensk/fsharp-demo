module Gir.ProductDetail.Views

open Giraffe.GiraffeViewEngine
open Gir.Shared.Layout

let template =
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
          headerView
          div [ _class "single-product-area section-padding-100 clearfix" ]
              [ div [ _class "container-fluid" ]
                    [ div [ _class "row" ]
                          [ div [ _class "col-12" ]
                                [ nav [ _ariaLabel "breadcrumb" ]
                                      [ ol [ _class "breadcrumb mt-50" ]
                                            [ li [ _class "breadcrumb-item" ] [ a [ _href "#" ] [ str "Home" ] ]
                                              li [ _class "breadcrumb-item" ]
                                                  [ a [ _href "#" ] [ str "Furniture" ] ]
                                              li [ _class "breadcrumb-item" ] [ a [ _href "#" ] [ str "Chairs" ] ]
                                              li
                                                  [ _class "breadcrumb-item active"
                                                    _ariaCurrent "page" ] [ str "white modern chair" ] ] ] ] ]
                      div [ _class "row" ]
                          [ div [ _class "col-12 col-lg-7" ]
                                [ div [ _class "single_product_thumb" ]
                                      [ div
                                          [ _id "product_details_slider"
                                            _class "carousel slide"
                                            _dataRide "carousel" ]
                                            [ ol [ _class "carousel-indicators" ]
                                                  [ li
                                                      [ _class "active"
                                                        _dataTarget "#product_details_slider"
                                                        _dataSlideTo "0"
                                                        _style "url(img/product-img/pro-big-1.jpg)" ] []
                                                    li
                                                        [ _dataTarget "#product_details_slider"
                                                          _dataSlideTo "1"
                                                          _style "background-image: url(/img/product-img/pro-big-2.jpg)" ]
                                                        []
                                                    li
                                                        [ _dataTarget "#product_details_slider"
                                                          _dataSlideTo "2"
                                                          _style "background-image: url(/img/product-img/pro-big-3.jpg)" ]
                                                        []
                                                    li
                                                        [ _dataTarget "#product_details_slider"
                                                          _dataSlideTo "3"
                                                          _style "background-image: url(/img/product-img/pro-big-4.jpg)" ]
                                                        [] ]
                                              div [ _class "carousel-inner" ]
                                                  [ div [ _class "carousel-item active" ]
                                                        [ a
                                                            [ _class "gallery_img"
                                                              _href "/img/product-img/pro-big-1.jpg" ]
                                                              [ img
                                                                  [ _class "d-block w-100"
                                                                    _src "/img/product-img/pro-big-1.jpg"
                                                                    _alt "First slide" ] ] ]
                                                    div [ _class "carousel-item" ]
                                                        [ a
                                                            [ _class "gallery_img"
                                                              _href "/img/product-img/pro-big-2.jpg" ]
                                                              [ img
                                                                  [ _class "d-block w-100"
                                                                    _src "/img/product-img/pro-big-2.jpg"
                                                                    _alt "Second slide" ] ] ]
                                                    div [ _class "carousel-item" ]
                                                        [ a
                                                            [ _class "gallery_img"
                                                              _href "/img/product-img/pro-big-3.jpg" ]
                                                              [ img
                                                                  [ _class "d-block w-100"
                                                                    _src "/img/product-img/pro-big-3.jpg"
                                                                    _alt "Third slide" ] ] ]
                                                    div [ _class "carousel-item" ]
                                                        [ a
                                                            [ _class "gallery_img"
                                                              _href "/img/product-img/pro-big-4.jpg" ]
                                                              [ img
                                                                  [ _class "d-block w-100"
                                                                    _src "/img/product-img/pro-big-4.jpg"
                                                                    _alt "Fourth slide" ] ] ] ] ] ] ]
                            div [ _class "col-12 col-lg-5" ]
                                [ div [ _class "single_product_desc" ]
                                      [ div [ _class "product-meta-data" ]
                                            [ div [ _class "line" ] []
                                              p [ _class "product-price" ] [ str "$180" ]
                                              a [ _href "#" ] [ h6 [] [ str "White Modern Chair" ] ]
                                              div
                                                  [ _class
                                                      "ratings-review mb-15 d-flex align-items-center justify-content-between" ]
                                                  [ div [ _class "ratings" ]
                                                        [ i
                                                            [ _class "fa fa-star"
                                                              _ariaHidden "true" ] []
                                                          i
                                                              [ _class "fa fa-star"
                                                                _ariaHidden "true" ] []
                                                          i
                                                              [ _class "fa fa-star"
                                                                _ariaHidden "true" ] []
                                                          i
                                                              [ _class "fa fa-star"
                                                                _ariaHidden "true" ] []
                                                          i
                                                              [ _class "fa fa-star"
                                                                _ariaHidden "true" ] [] ]
                                                    div [ _class "review" ]
                                                        [ a [ _href "#" ] [ str "Write A Review" ] ] ]
                                              p [ _class "avaibility" ]
                                                  [ i [ _class "fa fa-circle" ] []
                                                    str "In Stock" ] ]
                                        div [ _class "short_overview my-5" ]
                                            [ p []
                                                  [ str
                                                      "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aliquid quae eveniet culpa officia quidem mollitia impedit iste asperiores nisi reprehenderit consequatur, autem, nostrum pariatur enim?" ] ]
                                        form
                                            [ _class "cart clearfix"
                                              _method "post" ]
                                            [ div [ _class "cart-btn d-flex mb-50" ]
                                                  [ p [] [ str "Qty" ]
                                                    div [ _class "quantity" ]
                                                        [ span
                                                            [ _class "qty-minus"
                                                              _onclick "" ]
                                                              [ i
                                                                  [ _class "fa fa-caret-down"
                                                                    _ariaHidden "true" ] [] ]
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
                                                                  [ _class "fa fa-caret-up"
                                                                    _ariaHidden "true" ] [] ] ] ]
                                              button
                                                  [ _type "submit"
                                                    _name "addtocart"
                                                    _value "5"
                                                    _class "btn amado-btn" ] [ str "Add to cart" ] ] ] ] ] ] ] ]


let productDetailView = [ template ] |> layout