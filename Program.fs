// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open AOC2020


[<EntryPoint>]
let main argv =
    let filePath = Path.Combine(Directory.GetCurrentDirectory(), argv.[0])

    // let answer = day1.solve filePath (argv.[1] |> int)
    // let answer = day2.solve filePath day2.part1
    let answer = day2.solve filePath day2.part2

    printfn "answer %A" answer
    0 // return an integer exit code
