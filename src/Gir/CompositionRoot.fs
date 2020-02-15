module Gir.CompositionRoot

open Microsoft.Extensions.Configuration

type CompositionRoot = {
    GetMerchantToken : unit -> string
    GetPurchaseToken : unit -> string
}

module CompositionRoot =
    let compose (cfg:IConfigurationRoot) : CompositionRoot =
        
        let getMerchantToken () = Cart.CheckoutIntegration.getCachedToken cfg.["clientId"] cfg.["clientSecret"]
        
        {
            GetMerchantToken = getMerchantToken
            GetPurchaseToken = getMerchantToken >> Cart.CheckoutIntegration.getPurchaseToken
        }
       