module Gir.Index.HttpHandlers

open Giraffe
open Gir.Index.Views

let indexHandler next ctx =
    htmlView indexView next ctx