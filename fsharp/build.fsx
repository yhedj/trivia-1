#r "packages/FAKE/tools/FakeLib.dll"

open Fake

let buildDir = "./build"
let projectsToBuild = !! "./**/*.fsproj"

Target "Build" (fun () ->
    MSBuildRelease buildDir "Build" projectsToBuild
    |> Log "AppBuild-Output: "
)

Target "GoldenMaster" (fun () ->
    [0..100]
    |> Seq.collect (fun i -> 
        ExecProcessRedirected (fun s -> 
            s.FileName <- "./build/Trivia.exe"
            s.Arguments <- string i)
            (System.TimeSpan.FromMinutes(1.0))
        |> snd)
    |> (fun lines -> 
        System.IO.File.WriteAllLines("./goldenMaster.txt", 
            lines |> Seq.map (fun l -> l.Message)))
)

"Build" ==> "GoldenMaster"

RunTargetOrDefault "Build"