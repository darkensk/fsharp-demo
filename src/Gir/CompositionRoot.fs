module Gir.CompositionRoot

open Microsoft.Extensions.Configuration
open System.Threading.Tasks
open Domain

type CompositionRoot =
    { CheckoutFrontendBundle: string
      CheckoutBackendApiUrl: string
      GetPartnerAccessToken: Market -> Task<string>
      GetPurchaseToken: CartState -> string -> Settings -> Task<InitializePaymentResponse>
      GetAllProducts: unit -> Product list
      GetProductById: int -> Product option
      ReclaimPurchaseToken: string -> string -> Task<string>
      ApiPublicUrl: string
      EnabledMarkets: Market list
      PaymentWidgetBundle: string
      GetPaymentWidgetToken: string -> Task<PaymentWidgetState>
      PartnerShippingBundle: string }

let dummyProducts: Product list =
    let createProduct id name price img bigImg =
        { ProductId = id
          Name = name
          Price = price
          Img = img
          BigImg = bigImg }

    [ createProduct 1 "Modern Chair" 180M "/img/bg-img/1.jpg" "/img/product-img/pro-big-1.jpg"
      createProduct 2 "Minimalistic Plant Pot" 10M "/img/bg-img/2.jpg" "/img/product-img/pro-big-2.jpg"
      createProduct 3 "Night Stand" 2500M "/img/bg-img/4.jpg" "/img/product-img/pro-big-4.jpg"
      createProduct 4 "Plant Pot" 3M "/img/bg-img/5.jpg" "/img/product-img/pro-big-5.jpg"
      createProduct 5 "Large Table" 1100M "/img/bg-img/6.jpg" "/img/product-img/pro-big-6.jpg"
      createProduct 6 "Metallic Chair" 317M "/img/bg-img/7.jpg" "/img/product-img/pro-big-7.jpg"
      createProduct 7 "Rocking Chair" 100M "/img/bg-img/8.jpg" "/img/product-img/pro-big-8.jpg"
      createProduct 8 "Modern Chair" 50M "/img/bg-img/1.jpg" "/img/product-img/pro-big-1.jpg"
      createProduct 9 "Minimalistic Plant Pot - Discounted" 5M "/img/bg-img/2.jpg" "/img/product-img/pro-big-2.jpg"
      createProduct 10 "Table Lamp" 30.50M "/img/bg-img/10.jpg" "/img/product-img/pro-big-10.jpg"
      createProduct 11 "Expensive Dinning Set" 1800.99M "/img/bg-img/11.jpg" "/img/product-img/pro-big-11.jpg"
      createProduct 12 "Plant Deco Set Wall" 2500M "/img/bg-img/12.jpg" "/img/product-img/pro-big-12.jpg"
      createProduct 13 "Punk Chair" 10.99M "/img/bg-img/13.jpg" "/img/product-img/pro-big-13.jpg"
      createProduct 14 "Huge Leather Sofa" 3999M "/img/bg-img/14.jpg" "/img/product-img/pro-big-14.jpg"
      createProduct 15 "Day Stand" 199M "/img/bg-img/15.jpg" "/img/product-img/pro-big-15.jpg"
      createProduct 16 "Cool Chair" 100M "/img/bg-img/16.jpg" "/img/product-img/pro-big-16.jpg"
      createProduct 17 "Cozy Cabinets" 50M "/img/bg-img/17.jpg" "/img/product-img/pro-big-17.jpg" ]

module CompositionRoot =
    let compose (cfg: IConfigurationRoot) : CompositionRoot =
        let allMarkets =
            [ Sweden
              Finland
              Norway
              Denmark
              Czechia
              Slovakia
              Poland
              Latvia
              Estonia
              International
              Germany
              Austria ]

        let credentialsByMarket market =
            match market with
            | Sweden -> ("swedenClientId", "swedenClientSecret")
            | Finland -> ("finlandClientId", "finlandClientSecret")
            | Norway -> ("norwayClientId", "norwayClientSecret")
            | Denmark -> ("denmarkClientId", "denmarkClientSecret")
            | Slovakia -> ("slovakiaClientId", "slovakiaClientSecret")
            | Czechia -> ("czechiaClientId", "czechiaClientSecret")
            | Poland -> ("polandClientId", "polandClientSecret")
            | Latvia -> ("latviaClientId", "latviaClientSecret")
            | Estonia -> ("estoniaClientId", "estoniaClientSecret")
            | International -> ("internationalClientId", "internationalClientSecret")
            | Germany -> ("germanyClientId", "germanyClientSecret")
            | Austria -> ("austriaClientId", "austriaClientSecret")

        let hasCredentials market =
            market
            |> credentialsByMarket
            |> (fun (clientId: string, clientSecret: string) ->
                (cfg.[clientId] = null || cfg.[clientSecret] = null) |> not)

        let enabledMarkets = List.filter hasCredentials allMarkets

        let url = cfg.["checkoutBackendApiUrl"] + "/api/partner/tokens"

        let getPartnerAccessToken (market: Market) =
            let getCachedToken = Cart.CheckoutIntegration.getCachedToken url market

            match market with
            | Sweden -> getCachedToken cfg.["swedenClientId"] cfg.["swedenClientSecret"]
            | Finland -> getCachedToken cfg.["finlandClientId"] cfg.["finlandClientSecret"]
            | Norway -> getCachedToken cfg.["norwayClientId"] cfg.["norwayClientSecret"]
            | Denmark -> getCachedToken cfg.["denmarkClientId"] cfg.["denmarkClientSecret"]
            | Slovakia -> getCachedToken cfg.["slovakiaClientId"] cfg.["slovakiaClientSecret"]
            | Czechia -> getCachedToken cfg.["czechiaClientId"] cfg.["czechiaClientSecret"]
            | Poland -> getCachedToken cfg.["polandClientId"] cfg.["polandClientSecret"]
            | Latvia -> getCachedToken cfg.["latviaClientId"] cfg.["latviaClientSecret"]
            | Estonia -> getCachedToken cfg.["estoniaClientId"] cfg.["estoniaClientSecret"]
            | International -> getCachedToken cfg.["internationalClientId"] cfg.["internationalClientSecret"]
            | Germany -> getCachedToken cfg.["germanyClientId"] cfg.["germanyClientSecret"]
            | Austria -> getCachedToken cfg.["austriaClientId"] cfg.["austriaClientSecret"]

        { CheckoutFrontendBundle = cfg.["checkoutFrontendBundleUrl"]
          CheckoutBackendApiUrl = cfg.["checkoutBackendApiUrl"]
          GetPartnerAccessToken = getPartnerAccessToken
          GetPurchaseToken =
            Cart.CheckoutIntegration.getPurchaseToken cfg.["checkoutBackendApiUrl"] cfg.["apiPublicUrl"]
          GetAllProducts = fun _ -> dummyProducts
          GetProductById =
            fun (productId: int) ->
                dummyProducts
                |> List.tryFind (fun (product: Product) -> product.ProductId = productId)
          ReclaimPurchaseToken = Cart.CheckoutIntegration.reclaimPurchaseToken cfg.["checkoutBackendApiUrl"]
          ApiPublicUrl = cfg.["apiPublicUrl"]
          EnabledMarkets = enabledMarkets
          PaymentWidgetBundle = cfg.["paymentWidgetBundleUrl"]
          GetPaymentWidgetToken = Products.PaymentWidgetIntegration.getPaymentWidgetToken cfg.["checkoutBackendApiUrl"]
          PartnerShippingBundle = cfg.["partnerShippingBundleUrl"] }
