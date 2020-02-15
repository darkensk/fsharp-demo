module Gir.CompositionRoot

open Microsoft.Extensions.Configuration

type CompositionRoot = {
    CheckoutFrontendBundle : string
    GetMerchantToken : unit -> string
    GetPurchaseToken : unit -> string
}

module CompositionRoot =
    let compose (cfg:IConfigurationRoot) : CompositionRoot =
        let url = cfg.["checkoutBackendApiUrl"] + "/api/partner/tokens"
        let getMerchantToken () = Cart.CheckoutIntegration.getCachedToken url cfg.["clientId"] cfg.["clientSecret"]
        
        {
            CheckoutFrontendBundle = cfg.["checkoutFrontendBundleUrl"]
            GetMerchantToken = getMerchantToken
            GetPurchaseToken = getMerchantToken >> Cart.CheckoutIntegration.getPurchaseToken
        }
       