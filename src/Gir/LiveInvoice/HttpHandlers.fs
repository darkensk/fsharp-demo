module Gir.LiveInvoice.HttpHandlers

open Giraffe
open Views


let testLiveInvoiceHandler (liveInvoiceBundleUrl: string) (liveInvoiceId: string) =
    htmlView <| liveInvoiceView liveInvoiceBundleUrl liveInvoiceId
