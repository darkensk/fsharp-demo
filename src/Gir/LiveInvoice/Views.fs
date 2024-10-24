module Gir.LiveInvoice.Views

open Giraffe.ViewEngine
open Gir.Layout

let template (liveInvoiceBundleUrl: string) (liveInvoiceId: string) =
    div
        [ _id "root"; _style "padding: 20px;"; _data "liveInvoiceId" liveInvoiceId ]

        [ h1 [ _class "settings-heading" ] [ str "Live Invoice" ]
          script [ _type "module"; _crossorigin ""; _src liveInvoiceBundleUrl ] [] ]


let liveInvoiceView (liveInvoiceBundleUrl: string) (liveInvoiceId: string) =
    // layout <| [ template liveInvoiceBundleUrl liveInvoiceId ]
    html
        []
        [ head
              []
              [ meta [ _charset "UTF-8" ]
                meta [ _name "description"; _content "" ]
                meta [ _httpEquiv "X-UA-Compatible"; _content "IE=edge" ]
                meta
                    [ _name "viewport"
                      _content "width=device-width, initial-scale=1, shrink-to-fit=no" ]
                title [] [ str "Avarda - GirShop" ] ]
          body [] [ template liveInvoiceBundleUrl liveInvoiceId ] ]
