module Gir.Settings.HttpHandlers

open Giraffe
open Views

let settingsHandler next ctx = (htmlView <| settingsView) next ctx
