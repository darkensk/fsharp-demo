module Gir.Settings.HttpHandlers

open Giraffe
open Views
open Gir.Domain

let settingsHandler (settings: Settings) next ctx =
    (htmlView <| settingsView settings) next ctx
