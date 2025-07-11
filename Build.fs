open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext ()

let srcPath = Path.getFullName "src"
let appSrcPath = srcPath </> "Gir"

// Targets
let clean proj =
    [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "Clean" (fun _ -> appSrcPath |> clean)


Target.create "Run" (fun _ ->
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "Development"
    [ "app", Tools.dotnet "watch run" appSrcPath ] |> runParallel)


let dependencies = [ "Clean" ==> "Run" ]

[<EntryPoint>]
let main args = runOrDefault "Run" args
