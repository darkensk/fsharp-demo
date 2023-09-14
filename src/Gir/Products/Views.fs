module Gir.Products.Views

open Giraffe.ViewEngine
open Gir.Layout
open Gir.Domain
open Gir.Utils



let avardaPaymentWidget = tag "avarda-payment-widget"

let _priceAttribute = attr "price"

let _languageAttribute = attr "lang"

let _widgetJwtAttribute = attr "data-widget-jwt"

let _paymentIdAttribute = attr "data-payment-id"

let _customStylesAttribute = attr "data-custom-styles"

let rawCustomStyles (apiPublicUrl: string) =
    let styles =
        """
    {
  "buttons": {
    "primary": {
      "base": {
        "backgroundColor": "#fbb710",
        "color": "#ffffff",
        "borderColor": "#fbb710"
      },
      "hover": {
        "backgroundColor": "#131212",
        "borderColor": "#131212",
        "color": "#ffffff"
      },
      "boxShadow": {
        "hOffset": 0,
        "vOffset": 0,
        "blur": 0,
        "spread": 0,
        "color": "#000000"
      },
      "disabled": {
        "backgroundColor": "#FDE9B7",
        "borderColor": "#FDE9B7",
        "color": "#ffffff"
      },
      "borderWidth": 0,
      "fontSize": 18,
      "lineHeight": 56,
      "minHeight": 55,
      "fontWeight": 400,
      "padding": {
        "top": 10,
        "right": 16,
        "bottom": 10,
        "left": 16
      },
      "borderRadius": 0
    },
    "secondary": {
      "base": {
        "backgroundColor": "#131212",
        "borderColor": "#131212",
        "color": "#ffffff"
      },
      "hover": {
        "backgroundColor": "#131212",
        "borderColor": "#131212",
        "color": "#ffffff"
      },
      "boxShadow": {
        "hOffset": 0,
        "vOffset": 0,
        "blur": 0,
        "spread": 0,
        "color": "#000000"
      },
      "disabled": {
        "backgroundColor": "#FDE9B7",
        "borderColor": "#FDE9B7",
        "color": "#ffffff"
      },
      "borderWidth": 0,
      "fontSize": 18,
      "lineHeight": 56,
      "minHeight": 55,
      "fontWeight": 400,
      "padding": {
        "top": 10,
        "right": 16,
        "bottom": 10,
        "left": 16
      },
      "borderRadius": 0
    }
  },
  "fontFamilies": [
    "HelveticaNeue-Medium",
    "sans-serif"
  ],
  "headings": {
    "h1": {
      "fontSize": 30,
      "lineHeight": 32,
      "display": "block",
      "fontWeight": 400,
      "color": "#000000",
      "alignment": "left"
    },
    "h2": {
      "fontSize": 30,
      "lineHeight": 32,
      "display": "block",
      "fontWeight": 400,
      "color": "#000000",
      "alignment": "left"
    },
    "h3": {
      "fontSize": 18,
      "lineHeight": 20,
      "display": "block",
      "fontWeight": 400,
      "color": "#000000",
      "alignment": "left"
    },
    "h4": {
      "fontSize": 16,
      "lineHeight": 16,
      "display": "block",
      "fontWeight": 400,
      "color": "#000000",
      "alignment": "left"
    }
  },
  "input": {
    "height": 50,
    "fontSize": 16,
    "fontWeight": 400,
    "backgroundColorValid": "#ffffff",
    "backgroundColorInvalid": "#fee7e7",
    "borderColor": "#b2b2b2",
    "borderWidth": 1,
    "borderRadius": 0,
    "focusOutlineColor": "#fbb710",
    "disabled": {
      "backgroundColor": "#f5f5f5",
      "borderColor": "#aeaeae",
      "color": "#aeaeae"
    },
    "placeholderColor": "#aeaeae"
  },
  "links": {
    "default": {
      "fontSize": 13,
      "fontWeight": 400,
      "color": "#777777",
      "textDecoration": "underline",
      "hover": {
        "color": "#fbb710",
        "textDecoration": "underline"
      },
      "disabled": {
        "color": "#aeaeae",
        "textDecoration": "underline"
      }
    },
    "blue": {
      "fontSize": 13,
      "fontWeight": 400,
      "color": "#131212",
      "textDecoration": "underline",
      "hover": {
        "color": "#fbb710",
        "textDecoration": "underline"
      },
      "disabled": {
        "color": "#aeaeae",
        "textDecoration": "underline"
      }
    },
    "biggerBlue": {
      "fontSize": 16,
      "fontWeight": 400,
      "color": "#131212",
      "textDecoration": "underline",
      "hover": {
        "color": "#fbb710",
        "textDecoration": "underline"
      },
      "disabled": {
        "color": "#aeaeae",
        "textDecoration": "underline"
      }
    },
    "smallNoDecoration": {
      "fontSize": 11,
      "fontWeight": 400,
      "color": "#4b4b4b",
      "textDecoration": "none",
      "hover": {
        "color": "#fbb710",
        "textDecoration": "none"
      },
      "disabled": {
        "color": "#aeaeae",
        "textDecoration": "underline"
      }
    }
  },
  "select": {
    "base": {
      "backgroundColor": "#ffffff",
      "color": "#4b4b4b",
      "borderColor": "#b2b2b2",
      "selectArrowUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/default_selectArrow.svg"
    },
    "disabled": {
      "backgroundColor": "#f5f5f5",
      "color": "#aeaeae",
      "borderColor": "#aeaeae",
      "selectArrowUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/default_selectArrow.svg"
    },
    "fontSize": 16,
    "lineHeight": 30,
    "height": 50,
    "borderWidth": 1,
    "fontWeight": 400
  },
  "labels": {
    "active": {
      "color": "#4c4c4c"
    },
    "disabled": {
      "color": "#aeaeae"
    },
    "error": {
      "color": "#e20000"
    }
  },
  "footer": {
    "fontSize": 13,
    "fontWeight": 400,
    "color": "#b2b2b2",
    "iconLabelColor": "#fbb710"
  },
  "icons": {
    "card": {
      "color": "#fbb710",
      "width": 91,
      "height": 24
    },
    "loanPayment": {
      "backgroundUrl": "[apiPublicUrl]/img/core-img/logo.png",
      "width": 30,
      "height": 30
    },
    "partPayment": {
      "color": "#fbb710",
      "width": 30,
      "height": 30
    },
    "invoice": {
      "backgroundUrl": "[apiPublicUrl]/img/core-img/logo.png",
      "width": 30,
      "height": 30
    },
    "directBank": {
      "color": "#fbb710",
      "width": 30,
      "height": 30
    },
    "payOnDelivery": {
      "color": "#fbb710",
      "width": 30,
      "height": 30
    }
  },
  "paymentMethods": {
    "selected": {
      "labelColor": "#fbb710",
      "borderColor": "#fbb710",
      "borderWidth": 1,
      "backgroundColor": "#FEFAF0",
      "partPaymentPaymentTermSelect": {
        "selected": {
          "labelColor": "#131212",
          "borderColor": "#131212",
          "backgroundColor": "#FDE9B7"
        },
        "unselected": {
          "labelColor": "#000000",
          "borderColor": "#b2b2b2",
          "backgroundColor": "#ffffff"
        }
      },
      "bulletIconColor": "#00a0ba",
      "radioButtonColor": "#fbb710"
    },
    "unselected": {
      "labelColor": "#000000",
      "backgroundColor": "",
      "borderColor": "#b2b2b2",
      "radioButtonColor": "#fbb710"
    }
  },
  "amountToPayColor": "#fbb710",
  "backgroundBorderRadius": 0,
  "commonBorderColor": "#b2b2b2",
  "paymentSection": {
    "activeBorderWidth": 2,
    "borderRadius": 5
  },
  "checkbox": {
    "primary": {
      "width": 20,
      "height": 20,
      "checked": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryChecked.svg"
      },
      "unchecked": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryUnchecked.svg"
      },
      "checkedDisabled": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryCheckedDisabled.svg"
      },
      "uncheckedDisabled": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryUncheckedDisabled.svg"
      },
      "checkedInvalid": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedChecked.svg"
      },
      "uncheckedInvalid": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedUnchecked.svg"
      },
      "focusOutlineColor": "#fbb710",
      "checkedLabelColor": "#fbb710"
    },
    "secondary": {
      "width": 20,
      "height": 20,
      "checked": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryChecked.svg"
      },
      "unchecked": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryUnchecked.svg"
      },
      "checkedDisabled": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryCheckedDisabled.svg"
      },
      "uncheckedDisabled": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryUncheckedDisabled.svg"
      },
      "checkedInvalid": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedChecked.svg"
      },
      "uncheckedInvalid": {
        "backgroundUrl": "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedUnchecked.svg"
      },
      "focusOutlineColor": "#fbb710",
      "checkedLabelColor": "#fbb710"
    }
  },
  "spinnerColor": "#aeaeae"
}
    """

    styles.Replace("[apiPublicUrl]", apiPublicUrl)


let paymentWidgetScriptView
    (paymentWidgetBundleUrl: string)
    (paymentWidgetSettings: PaymentWidgetSettings)
    (paymentWidgetState: PaymentWidgetState option)
    (apiPublicUrl: string)
    =
    match paymentWidgetState with
    | Some state ->
        let { PaymentId = paymentId
              WidgetJwt = widgetJwt } =
            state

        let bundleUrl =
            paymentWidgetBundleUrl
            + "?ts="
            + System.DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()

        if paymentWidgetSettings.CustomStyles then
            script
                [ _async
                  _crossorigin "annonymous"
                  _src bundleUrl
                  _paymentIdAttribute paymentId
                  _widgetJwtAttribute widgetJwt
                  _customStylesAttribute (rawCustomStyles apiPublicUrl) ]
                []
        else
            script
                [ _async
                  _crossorigin "annonymous"
                  _src bundleUrl
                  _paymentIdAttribute paymentId
                  _widgetJwtAttribute widgetJwt ]
                []
    | None -> div [] []


let listTemplate (settings: Settings) (cartState: CartState) (productList: Product list) =
    let products = productList |> List.map (productDiv settings)

    div
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
              [ _class "main-content-wrapper d-flex clearfix" ]
              [ div
                    [ _class "mobile-nav" ]
                    [ div
                          [ _class "amado-navbar-brand" ]
                          [ a [ _href "/" ] [ img [ _src "/img/core-img/logo.png"; _alt "" ] ] ]
                      div [ _class "amado-navbar-toggler" ] [ span [] []; span [] []; span [] [] ] ]
                headerView cartState
                div
                    [ _class "products-catagories-area clearfix" ]
                    [ div [ _class "amado-pro-catagory clearfix" ] products ] ]
          subscribeSectionView
          footerView ]

let listView (settings: Settings) (cartState: CartState) (products: Product list) =
    [ listTemplate settings cartState products ] |> layout


let languageSelectView =
    details
        []
        [ summary [ _class "select-language-summary" ] [ str "Select language of Payment Widget" ]
          div
              [ _class "select-language-container" ]
              [ button
                    [ _class "select-flag"
                      _id "flag-en"
                      _onclick "document.querySelector('avarda-payment-widget').setAttribute('lang', 'en');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/gb.svg"
                            _alt "English language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-se"
                      _onclick "document.querySelector('avarda-payment-widget').setAttribute('lang', 'sv');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/se.svg"
                            _alt "Swedish language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-no"
                      _onclick "document.querySelector('avarda-payment-widget').setAttribute('lang', 'nb');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/no.svg"
                            _alt "Norwegian language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-dk"
                      _onclick "document.querySelector('avarda-payment-widget').setAttribute('lang', 'da');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/dk.svg"
                            _alt "Danish language"
                            _ariaHidden "true" ] ]
                button
                    [ _class "select-flag"
                      _id "flag-fi"
                      _onclick "document.querySelector('avarda-payment-widget').setAttribute('lang', 'fi');" ]
                    [ img
                          [ _class "flag"
                            _src "/img/flags/fi.svg"
                            _alt "Finnish language"
                            _ariaHidden "true" ] ] ] ]


let detailTemplate
    (settings: Settings)
    (cartState: CartState)
    (product: Product)
    (paymentWidgetBundleUrl: string)
    (paymentWidgetState: PaymentWidgetState option)
    (apiPublicUrl: string)
    =
    let selectedLanguageIsoCode =
        settings.ExtraInitSettings.Language |> languageToIsoCode

    div
        []
        [ div
              [ _class "main-content-wrapper d-flex clearfix" ]
              [ div
                    [ _class "mobile-nav" ]
                    [ div
                          [ _class "amado-navbar-brand" ]
                          [ a [ _href "/" ] [ img [ _src "/img/core-img/logo.png"; _alt "" ] ] ]
                      div [ _class "amado-navbar-toggler" ] [ span [] []; span [] []; span [] [] ] ]
                headerView cartState
                div
                    [ _class "single-product-area section-padding-100 clearfix" ]
                    [ div
                          [ _class "container-fluid" ]
                          [ div
                                [ _class "row" ]
                                [ div
                                      [ _class "col-12" ]
                                      [ nav
                                            [ _ariaLabel "breadcrumb" ]
                                            [ ol
                                                  [ _class "breadcrumb mt-50" ]
                                                  [ li [ _class "breadcrumb-item" ] [ a [ _href "#" ] [ str "Home" ] ]
                                                    li
                                                        [ _class "breadcrumb-item" ]
                                                        [ a [ _href "#" ] [ str "Furniture" ] ]
                                                    li [ _class "breadcrumb-item" ] [ a [ _href "#" ] [ str "Chairs" ] ]
                                                    li
                                                        [ _class "breadcrumb-item active"; _ariaCurrent "page" ]
                                                        [ str product.Name ] ] ] ] ]
                            div
                                [ _class "row" ]
                                [ div
                                      [ _class "col-12 col-lg-7" ]
                                      [ div
                                            [ _class "single_product_thumb" ]
                                            [ div
                                                  [ _id "product_details_slider"
                                                    _class "carousel slide"
                                                    _dataRide "carousel" ]
                                                  [ ol
                                                        [ _class "carousel-indicators" ]
                                                        [ li
                                                              [ _class "active"
                                                                _dataTarget "#product_details_slider"
                                                                _dataSlideTo "0"
                                                                _style (
                                                                    sprintf "background-image: url(%s)" product.BigImg
                                                                ) ]
                                                              []
                                                          li
                                                              [ _dataTarget "#product_details_slider"
                                                                _dataSlideTo "1"
                                                                _style
                                                                    "background-image: url(/img/product-img/pro-big-2.jpg)" ]
                                                              []
                                                          li
                                                              [ _dataTarget "#product_details_slider"
                                                                _dataSlideTo "2"
                                                                _style
                                                                    "background-image: url(/img/product-img/pro-big-3.jpg)" ]
                                                              []
                                                          li
                                                              [ _dataTarget "#product_details_slider"
                                                                _dataSlideTo "3"
                                                                _style
                                                                    "background-image: url(/img/product-img/pro-big-4.jpg)" ]
                                                              [] ]
                                                    div
                                                        [ _class "carousel-inner" ]
                                                        [ div
                                                              [ _class "carousel-item active" ]
                                                              [ a
                                                                    [ _class "gallery_img"; _href product.BigImg ]
                                                                    [ img
                                                                          [ _class "d-block w-100"
                                                                            _src product.BigImg
                                                                            _alt "First slide" ] ] ]
                                                          div
                                                              [ _class "carousel-item" ]
                                                              [ a
                                                                    [ _class "gallery_img"
                                                                      _href "/img/product-img/pro-big-2.jpg" ]
                                                                    [ img
                                                                          [ _class "d-block w-100"
                                                                            _src "/img/product-img/pro-big-2.jpg"
                                                                            _alt "Second slide" ] ] ]
                                                          div
                                                              [ _class "carousel-item" ]
                                                              [ a
                                                                    [ _class "gallery_img"
                                                                      _href "/img/product-img/pro-big-3.jpg" ]
                                                                    [ img
                                                                          [ _class "d-block w-100"
                                                                            _src "/img/product-img/pro-big-3.jpg"
                                                                            _alt "Third slide" ] ] ]
                                                          div
                                                              [ _class "carousel-item" ]
                                                              [ a
                                                                    [ _class "gallery_img"
                                                                      _href "/img/product-img/pro-big-4.jpg" ]
                                                                    [ img
                                                                          [ _class "d-block w-100"
                                                                            _src "/img/product-img/pro-big-4.jpg"
                                                                            _alt "Fourth slide" ] ] ] ] ] ] ]
                                  div
                                      [ _class "col-12 col-lg-5" ]
                                      [ div
                                            [ _class "single_product_desc" ]
                                            [ div
                                                  [ _class "product-meta-data" ]
                                                  [ div [ _class "line" ] []
                                                    p
                                                        [ _class "product-price" ]
                                                        [ str
                                                          <| sprintf
                                                              "%M %s"
                                                              product.Price
                                                              (marketToCurrency settings.Market) ]
                                                    a [ _href "#" ] [ h6 [] [ str product.Name ] ]
                                                    div
                                                        [ _class
                                                              "ratings-review mb-15 d-flex align-items-center justify-content-between" ]
                                                        [ div
                                                              [ _class "ratings" ]
                                                              [ i [ _class "fa fa-star"; _ariaHidden "true" ] []
                                                                i [ _class "fa fa-star"; _ariaHidden "true" ] []
                                                                i [ _class "fa fa-star"; _ariaHidden "true" ] []
                                                                i [ _class "fa fa-star"; _ariaHidden "true" ] []
                                                                i [ _class "fa fa-star"; _ariaHidden "true" ] [] ]
                                                          div
                                                              [ _class "review" ]
                                                              [ a [ _href "#" ] [ str "Write A Review" ] ] ]
                                                    p
                                                        [ _class "avaibility" ]
                                                        [ i [ _class "fa fa-circle" ] []; str " In Stock" ] ]
                                              div
                                                  [ _class "short_overview my-5" ]
                                                  [ p
                                                        []
                                                        [ str
                                                              "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aliquid quae eveniet culpa officia quidem mollitia impedit iste asperiores nisi reprehenderit consequatur, autem, nostrum pariatur enim?" ] ]
                                              form
                                                  [ _class "cart clearfix"
                                                    _method "post"
                                                    _action <| sprintf "/product/%i/add" product.ProductId ]
                                                  [ div
                                                        [ _class "cart-btn d-flex mb-50" ]
                                                        [ p [] [ str "Qty" ]
                                                          div
                                                              [ _class "quantity" ]
                                                              [ span
                                                                    [ _class "qty-minus"; _onclick "" ]
                                                                    [ i
                                                                          [ _class "fa fa-caret-down"
                                                                            _ariaHidden "true" ]
                                                                          [] ]
                                                                input
                                                                    [ _type "number"
                                                                      _class "qty-text"
                                                                      _id "qty"
                                                                      _step "1"
                                                                      _min "1"
                                                                      _max "10"
                                                                      _name "quantity"
                                                                      _value "1" ]
                                                                span
                                                                    [ _class "qty-plus"; _onclick "" ]
                                                                    [ i
                                                                          [ _class "fa fa-caret-up"
                                                                            _ariaHidden "true" ]
                                                                          [] ] ] ]
                                                    button
                                                        [ _type "submit"
                                                          _name "addtocart"
                                                          _value "5"
                                                          _class "btn amado-btn" ]
                                                        [ str "Add to cart" ] ]
                                              br []
                                              if settings.PaymentWidgetSettings.Enabled then
                                                  div
                                                      []
                                                      [ avardaPaymentWidget
                                                            [ _priceAttribute <| sprintf "%M" product.Price
                                                              _languageAttribute selectedLanguageIsoCode ]
                                                            [ str "" ]
                                                        languageSelectView ]
                                              else
                                                  str "" ] ] ] ] ] ]
          subscribeSectionView
          footerView
          paymentWidgetScriptView paymentWidgetBundleUrl settings.PaymentWidgetSettings paymentWidgetState apiPublicUrl ]

let productDetailView
    (settings: Settings)
    (cartState: CartState)
    (product: Product)
    (paymentWidgetBundleUrl: string)
    (paymentWidgetState: PaymentWidgetState option)
    (apiPublicUrl: string)
    =
    [ detailTemplate settings cartState product paymentWidgetBundleUrl paymentWidgetState apiPublicUrl ]
    |> layout
