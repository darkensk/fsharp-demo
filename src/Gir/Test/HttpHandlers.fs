module Gir.Test.HttpHandlers

open Giraffe
open Views

let testCheckoutHandler (checkoutFrontendBundleUrl: string) (purchaseToken: string) =
    htmlView <| testCheckoutView checkoutFrontendBundleUrl purchaseToken
