module Gir.CompositionRoot

open System
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
      PartnerShippingBundle: string
      PayFrameBundle: string
      PayFrameSiteKey: string
      PayFrameLanguage: string
      PayFrameUseV2: bool
      AppleDeveloperMerchantidDomainAssociation: string }

let dummyProducts: Product list =
    let createProduct id name price img bigImg shippingParameters =
        { ProductId = id
          Name = name
          Price = price
          Img = img
          BigImg = bigImg
          ShippingParameters = shippingParameters }

    [ createProduct
          1
          "Modern Chair"
          180M
          "/img/bg-img/1.jpg"
          "/img/product-img/pro-big-1.jpg"
          { Height = 9000
            Length = 6000
            Width = 3000
            Weight = 1000
            Attributes = [] }
      createProduct
          2
          "Minimalistic Plant Pot"
          10M
          "/img/bg-img/2.jpg"
          "/img/product-img/pro-big-2.jpg"
          { Height = 2000
            Length = 4000
            Width = 4000
            Weight = 300
            Attributes = [] }
      createProduct
          3
          "Night Stand"
          2500M
          "/img/bg-img/4.jpg"
          "/img/product-img/pro-big-4.jpg"
          { Height = 8000
            Length = 5000
            Width = 3000
            Weight = 2000
            Attributes = [] }
      createProduct
          4
          "Plant Pot"
          3M
          "/img/bg-img/5.jpg"
          "/img/product-img/pro-big-5.jpg"
          { Height = 10000
            Length = 8000
            Width = 5000
            Weight = 1500
            Attributes = [] }
      createProduct
          5
          "Large Table"
          1100M
          "/img/bg-img/6.jpg"
          "/img/product-img/pro-big-6.jpg"
          { Height = 8500
            Length = 16000
            Width = 8000
            Weight = 20000
            Attributes = [] }
      createProduct
          6
          "Metallic Chair"
          317M
          "/img/bg-img/7.jpg"
          "/img/product-img/pro-big-7.jpg"
          { Height = 8500
            Length = 5500
            Width = 4000
            Weight = 1200
            Attributes = [] }
      createProduct
          7
          "Rocking Chair"
          100M
          "/img/bg-img/8.jpg"
          "/img/product-img/pro-big-8.jpg"
          { Height = 9500
            Length = 6500
            Width = 5000
            Weight = 5000
            Attributes = [] }
      createProduct
          8
          "Modern Chair"
          50M
          "/img/bg-img/1.jpg"
          "/img/product-img/pro-big-1.jpg"
          { Height = 8800
            Length = 5800
            Width = 3300
            Weight = 1100
            Attributes = [] }
      createProduct
          9
          "Minimalistic Plant Pot - Discounted"
          5M
          "/img/bg-img/2.jpg"
          "/img/product-img/pro-big-2.jpg"
          { Height = 11500
            Length = 4200
            Width = 3800
            Weight = 700
            Attributes = [] }
      createProduct
          10
          "Table Lamp"
          30.50M
          "/img/bg-img/10.jpg"
          "/img/product-img/pro-big-10.jpg"
          { Height = 6000
            Length = 2000
            Width = 2000
            Weight = 300
            Attributes = [] }
      createProduct
          11
          "Expensive Dining Set"
          1800.99M
          "/img/bg-img/11.jpg"
          "/img/product-img/pro-big-11.jpg"
          { Height = 7800
            Length = 11000
            Width = 7000
            Weight = 20000
            Attributes = [] }
      createProduct
          12
          "Plant Deco Set Wall"
          2500M
          "/img/bg-img/12.jpg"
          "/img/product-img/pro-big-12.jpg"
          { Height = 16000
            Length = 8000
            Width = 3000
            Weight = 15000
            Attributes = [] }
      createProduct
          13
          "Punk Chair"
          10.99M
          "/img/bg-img/13.jpg"
          "/img/product-img/pro-big-13.jpg"
          { Height = 9200
            Length = 5700
            Width = 3700
            Weight = 1100
            Attributes = [] }
      createProduct
          14
          "Huge Leather Sofa"
          3999M
          "/img/bg-img/14.jpg"
          "/img/product-img/pro-big-14.jpg"
          { Height = 8000
            Length = 20000
            Width = 10000
            Weight = 100000
            Attributes = [] }
      createProduct
          15
          "Day Stand"
          199M
          "/img/bg-img/15.jpg"
          "/img/product-img/pro-big-15.jpg"
          { Height = 7000
            Length = 9000
            Width = 4000
            Weight = 2200
            Attributes = [] }
      createProduct
          16
          "Cool Chair"
          100M
          "/img/bg-img/16.jpg"
          "/img/product-img/pro-big-16.jpg"
          { Height = 9400
            Length = 6200
            Width = 4200
            Weight = 1600
            Attributes = [] } ]


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

        let credentials market = cfg.GetSection(nameof(CheckoutOptions) + ":" + market.ToString()).Get<ClientConfig>()
        
        let hasCredentials market =
            credentials market
            |> Option.ofObj
            |> Option.exists (fun clientConfig ->
                not (String.IsNullOrWhiteSpace(clientConfig.ClientId)) &&
                not (String.IsNullOrWhiteSpace(clientConfig.ClientSecret))
        )
            
        let enabledMarkets = List.filter hasCredentials allMarkets
        
        let checkoutOptions = cfg.GetSection(nameof(CheckoutOptions)).Get<CheckoutOptions>()

        let url = checkoutOptions.BackendApiUrl + "/api/partner/tokens"

        let getPartnerAccessToken (market: Market) =
            let clientConfig = credentials market
            Cart.CheckoutIntegration.getCachedToken url market clientConfig
            
        let payFrameOptions = cfg.GetSection(nameof(PayFrameOptions)).Get<PayFrameOptions>()
        
        let paymentWidgetBundleOptions = cfg.GetSection(nameof(PaymentWidgetOptions)).Get<PaymentWidgetOptions>()
        
        let partnerShippingOptions = cfg.GetSection(nameof(PartnerShippingOptions)).Get<PartnerShippingOptions>()

        { CheckoutFrontendBundle = checkoutOptions.FrontendBundleUrl
          CheckoutBackendApiUrl = checkoutOptions.BackendApiUrl
          GetPartnerAccessToken = getPartnerAccessToken
          GetPurchaseToken =
            Cart.CheckoutIntegration.getPurchaseToken checkoutOptions.BackendApiUrl checkoutOptions.PublicUrl
          GetAllProducts = fun _ -> dummyProducts
          GetProductById =
            fun (productId: int) ->
                dummyProducts
                |> List.tryFind (fun (product: Product) -> product.ProductId = productId)
          ReclaimPurchaseToken = Cart.CheckoutIntegration.reclaimPurchaseToken checkoutOptions.BackendApiUrl
          ApiPublicUrl = checkoutOptions.PublicUrl
          EnabledMarkets = enabledMarkets
          PaymentWidgetBundle = paymentWidgetBundleOptions.FrontendBundleUrl
          GetPaymentWidgetToken = Products.PaymentWidgetIntegration.getPaymentWidgetToken checkoutOptions.BackendApiUrl
          PartnerShippingBundle = partnerShippingOptions.FrontendBundleUrl
          PayFrameBundle = payFrameOptions.FrontendBundleUrl
          PayFrameSiteKey = payFrameOptions.SiteKey
          PayFrameLanguage = payFrameOptions.Language
          PayFrameUseV2 = payFrameOptions.FrontendBundleUrl.Contains("v2")
          AppleDeveloperMerchantidDomainAssociation = cfg["apple-developer-merchantid-domain-association"] }
