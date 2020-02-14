module Gir.Index.Views

open Giraffe.GiraffeViewEngine

open Gir.Shared.Layout
open Domain

let productDiv (product: Product) =
    div [ _class "single-products-catagory clearfix" ]
        [ a [ _href "#" ]
              [ img
                  [ _src product.Img
                    _alt "" ]
                div [ _class "hover-content" ]
                    [ div [ _class "line" ] []
                      p [] [ str <| sprintf "From $%.0f" product.Price ]
                      h4 [] [ str product.Name ] ] ] ]



let tempProducts =
    let createProduct name price img =
        { Name = name
          Price = price
          Img = img }
    [ createProduct "Modern Chair" 180. "/img/bg-img/1.jpg"
      createProduct "Minimalistic Plant Pot" 50. "/img/bg-img/2.jpg"
      createProduct "Night Stand" 250. "/img/bg-img/4.jpg"
      createProduct "Plant Pot" 30. "/img/bg-img/5.jpg"
      createProduct "Small Table" 180. "/img/bg-img/6.jpg"
      createProduct "Metallic Chair" 317. "/img/bg-img/7.jpg"
      createProduct "Rocking Chair" 317. "/img/bg-img/8.jpg"
      createProduct "Modern Chair" 180. "/img/bg-img/1.jpg"
      createProduct "Minimalistic Plant Pot" 50. "/img/bg-img/2.jpg"
      createProduct "Home Deco" 250. "/img/bg-img/9.jpg" ]



let template (productList: Product list) =
    let products = productList |> List.map productDiv
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
                headerView
                div [ _class "products-catagories-area clearfix" ]
                    [ div [ _class "amado-pro-catagory clearfix" ] products ] ]
          subscribeSectionView
          footerView ]


let indexView = [ template tempProducts ] |> layout
