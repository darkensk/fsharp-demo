module Gir.ProductDetail.HttpHandlers

open Giraffe
open Gir.ProductDetail.Views

let productDetailHandler next ctx =
    htmlView productDetailView next ctx