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
      ApiPublicUrl: string }

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
    let compose (cfg: IConfigurationRoot): CompositionRoot =
        let url =
            cfg.["checkoutBackendApiUrl"]
            + "/api/partner/tokens"

        let getPartnerAccessToken (market: Market) =
            let getCachedToken =
                Cart.CheckoutIntegration.getCachedToken url market

            match market with
            | Sweden -> getCachedToken cfg.["swedenClientId"] cfg.["swedenClientSecret"]
            | Finland -> getCachedToken cfg.["finlandClientId"] cfg.["finlandClientSecret"]

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
          ApiPublicUrl = cfg.["apiPublicUrl"] }
