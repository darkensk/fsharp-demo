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
      EnabledMarkets: Market list }

let dummyProducts =
    let createProduct id name price img =
        { ProductId = id
          Name = name
          Price = price
          Img = img }

    [ createProduct 1 "Modern Chair" 180m "/img/bg-img/1.jpg"
      createProduct 2 "Minimalistic Plant Pot" 10M "/img/bg-img/2.jpg"
      createProduct 3 "Night Stand" 250M "/img/bg-img/4.jpg"
      createProduct 4 "Plant Pot" 3M "/img/bg-img/5.jpg"
      createProduct 5 "Small Table" 120M "/img/bg-img/6.jpg"
      createProduct 6 "Metallic Chair" 317M "/img/bg-img/7.jpg"
      createProduct 7 "Rocking Chair" 100M "/img/bg-img/8.jpg"
      createProduct 8 "Modern Chair" 50M "/img/bg-img/1.jpg"
      createProduct 9 "Minimalistic Plant Pot" 5M "/img/bg-img/2.jpg"
      createProduct 10 "Home Deco" 30M "/img/bg-img/9.jpg" ]

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
              Estonia ]

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

        let hasCredentials market =
            market
            |> credentialsByMarket
            |> (fun (clientId, clientSecret) ->
                (cfg.[clientId] = null || cfg.[clientSecret] = null)
                |> not)

        let enabledMarkets = List.filter hasCredentials allMarkets

        let url =
            cfg.["checkoutBackendApiUrl"]
            + "/api/partner/tokens"

        let getPartnerAccessToken (market: Market) =
            let getCachedToken =
                Cart.CheckoutIntegration.getCachedToken url market

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

        { CheckoutFrontendBundle = cfg.["checkoutFrontendBundleUrl"]
          CheckoutBackendApiUrl = cfg.["checkoutBackendApiUrl"]
          GetPartnerAccessToken = getPartnerAccessToken
          GetPurchaseToken =
              Cart.CheckoutIntegration.getPurchaseToken cfg.["checkoutBackendApiUrl"] cfg.["apiPublicUrl"]
          GetAllProducts = fun _ -> dummyProducts
          GetProductById =
              fun i ->
                  dummyProducts
                  |> List.tryFind (fun x -> x.ProductId = i)
          ReclaimPurchaseToken = Cart.CheckoutIntegration.reclaimPurchaseToken cfg.["checkoutBackendApiUrl"]
          ApiPublicUrl = cfg.["apiPublicUrl"]
          EnabledMarkets = enabledMarkets }
