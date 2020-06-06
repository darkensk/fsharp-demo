module Gir.Encoders

open Thoth.Json.Net
open Gir.Domain


let getPartnerTokenPayloadEncoder (clientId: string) (clientSecret: string) =
    Encode.object
        [ "clientId", Encode.string clientId
          "clientSecret", Encode.string clientSecret ]
    |> Encode.toString 0

let productEncoder (product: Product) =
    Encode.object
        [ "productId", Encode.int product.ProductId
          "name", Encode.string product.Name
          "price", Encode.float product.Price
          "img", Encode.string product.Img ]

let cartItemEncoder (cartItem: CartItem) =
    Encode.object
        [ "id", Encode.int cartItem.Id
          "qty", Encode.int cartItem.Qty
          "product", productEncoder cartItem.ProductDetail ]

let cartEncoder (cartState: CartState) =
    Encode.object
        [ "items",
          Encode.list
          <| List.map (cartItemEncoder) cartState.Items ]
    |> Encode.toString 0

let paymentItemEncoder (productDetail: Product) =
    Encode.object
        [ "description", Encode.string productDetail.Name
          "notes", Encode.string "-"
          "amount", Encode.float productDetail.Price
          "taxCode", Encode.string "20%"
          "taxAmount", Encode.float (productDetail.Price * 0.02) ]

let languageEncoder = languageToString >> Encode.string

let paymentPayloadEncoder (settings: Settings) (items: CartItem list) =
    if List.isEmpty items then
        ""
    else
        let productsList =
            List.fold (fun acc x ->
                acc
                @ [ for i in 1 .. x.Qty do
                        { ProductId = x.ProductDetail.ProductId
                          Name = x.ProductDetail.Name
                          Price = x.ProductDetail.Price
                          Img = x.ProductDetail.Img } ]) [] items

        Encode.object
            [ "language", languageEncoder settings.ExtraInitSettings.Language
              "items",
              Encode.list
              <| List.map (paymentItemEncoder) productsList
              "orderReference", Encode.string "TEST-AVARDA-ORDER-X"
              "displayItems", Encode.bool true ]
        |> Encode.toString 0



let modeEncoder = checkoutModeToString >> Encode.string

let checkboxStateEncoder = checkboxStateToString >> Encode.string

let selectedPaymentMethodEncoder selectedPaymentMethod =
    match selectedPaymentMethod with
    | Selected pm -> [ "selectedPaymentMethod", pm |> paymentMethodsToString |> Encode.string ]
    | NotSelected -> []

let customStylesEncoder customStyles =
    match customStyles with
    | Set cs -> Encode.string cs
    | NotSet -> Encode.object []

let extraInitSettingsEncoder initSettings =
    Encode.object
        ([ "language", languageEncoder initSettings.Language
           "mode", modeEncoder initSettings.Mode
           "differentDeliveryAddress", checkboxStateEncoder initSettings.DifferentDeliveryAddress
           "displayItems", Encode.bool initSettings.DisplayItems
           "recurringPayments", checkboxStateEncoder initSettings.RecurringPayments
           "smsNewsletterSubscription", checkboxStateEncoder initSettings.SmsNewsletterSubscription
           "emailNewsletterSubscription", checkboxStateEncoder initSettings.EmailNewsletterSubscription
           "emailInvoice", checkboxStateEncoder initSettings.EmailInvoice ]
         @ (selectedPaymentMethodEncoder initSettings.SelectedPaymentMethod))

let extraCheckoutFlagsEncoder checkoutFlags =
    Encode.object
        [ "disableFocus", Encode.bool checkoutFlags.DisableFocus
          "beforeSubmitCallbackEnabled", Encode.bool checkoutFlags.BeforeSubmitCallbackEnabled
          "deliveryAddressChangedCallbackEnabled", Encode.bool checkoutFlags.DeliveryAddressChangedCallbackEnabled
          "customStyles", customStylesEncoder checkoutFlags.CustomStyles ]

let settingsEncoder settings =
    Encode.object
        [ "extraCheckoutFlags", extraCheckoutFlagsEncoder settings.ExtraCheckoutFlags
          "extraInitSettings", extraInitSettingsEncoder settings.ExtraInitSettings ]
    |> Encode.toString 0
