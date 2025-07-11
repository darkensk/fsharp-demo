open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext ()

let publishPath = Path.getFullName "publish"
let srcPath = Path.getFullName "src"
let appSrcPath = srcPath </> "Gir"
let appPublishPath = publishPath </> "app"

// Targets
let clean proj =
    [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "Clean" (fun _ -> appSrcPath |> clean)

Target.create "Publish" (fun _ ->
    [ appPublishPath ] |> Shell.cleanDirs
    let publishArgs = sprintf "publish -c Release -o \"%s\"" appPublishPath
    run Tools.dotnet publishArgs appSrcPath
    [ appPublishPath </> "appsettings.Development.json" ] |> File.deleteAll)

Target.create "Run" (fun _ ->
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "Development"
    [ "app", Tools.dotnet "watch run" appSrcPath ] |> runParallel)


let dependencies = [ "Clean" ==> "Publish"; "Clean" ==> "Run" ]

[<EntryPoint>]
let main args = runOrDefault "Run" args
