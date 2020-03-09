module Gir.CompositionRoot

open Microsoft.Extensions.Configuration
open Gir.Domain

type CompositionRoot = {
    CheckoutFrontendBundle : string
    GetMerchantToken : unit -> string
    GetPurchaseToken : CartState -> string
    GetAllProducts : unit -> Product list
    GetProductById : int -> Product option
}

let dummyProducts =
    let createProduct id name price img =
        { ProductId = id
          Name = name
          Price = price
          Img = img }
    [ createProduct 1 "Modern Chair" 180. "/img/bg-img/1.jpg"
      createProduct 2 "Minimalistic Plant Pot" 50. "/img/bg-img/2.jpg"
      createProduct 3 "Night Stand" 250. "/img/bg-img/4.jpg"
      createProduct 4 "Plant Pot" 30. "/img/bg-img/5.jpg"
      createProduct 5 "Small Table" 180. "/img/bg-img/6.jpg"
      createProduct 6 "Metallic Chair" 317. "/img/bg-img/7.jpg"
      createProduct 7 "Rocking Chair" 317. "/img/bg-img/8.jpg"
      createProduct 8 "Modern Chair" 180. "/img/bg-img/1.jpg"
      createProduct 9 "Minimalistic Plant Pot" 50. "/img/bg-img/2.jpg"
      createProduct 10 "Home Deco" 250. "/img/bg-img/9.jpg" ]

module CompositionRoot =
    let compose (cfg:IConfigurationRoot) : CompositionRoot =
        let url = cfg.["checkoutBackendApiUrl"] + "/api/partner/tokens"
        let getMerchantToken () = Cart.CheckoutIntegration.getCachedToken url cfg.["clientId"] cfg.["clientSecret"]
        
        {
            CheckoutFrontendBundle = cfg.["checkoutFrontendBundleUrl"]
            GetMerchantToken = getMerchantToken
            GetPurchaseToken = (fun cartState -> Cart.CheckoutIntegration.getPurchaseToken cartState <| getMerchantToken() )
            GetAllProducts = fun _ -> dummyProducts
            GetProductById = fun i -> dummyProducts |> List.tryFind (fun x -> x.ProductId = i)
        }
