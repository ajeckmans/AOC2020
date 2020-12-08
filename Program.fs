// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO
open AOC2020
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

let filePath file = Path.Combine(Directory.GetCurrentDirectory(), file)
//
//type Benchmark () =
//    let mutable puzzleInput : string [] = [||]
//
//    [<GlobalSetup>]
//    member self.SetupData() =
//        puzzleInput <- File.ReadAllLines (filePath "day1\input.txt")
//
//    [<Benchmark>]
//    member self.``two_combinations`` () = day1.solve puzzleInput 2
//
//    [<Benchmark>]
//    member self.``tree_combinations`` () = day1.solve puzzleInput 3
//
//let defaultSwitch () = BenchmarkSwitcher [| typeof<Benchmark>  |]

[<EntryPoint>]
let main argv =
    //    defaultSwitch().Run argv |> ignore

    let answer: obj =
        match argv.[0] with
        | "day1part1" -> day1.solve (filePath "day1\input.txt") 2 :> obj
        | "day1part2" -> day1.solve (filePath "day1\input.txt") 3 :> obj
        | "day2part1" -> day2.solve (filePath "day2\input.txt") day2.part1 :> obj
        | "day2part2" -> day2.solve (filePath "day2\input.txt") day2.part2 :> obj
        | "day3part1" -> day3.solve (filePath "day3\input.txt") [ (3, 1) ] :> obj
        | "day3part2" -> day3.solve (filePath "day3\input.txt") [ (1, 2) ] :> obj
        | "day4" -> day4.solve (filePath "day4\input.txt") :> obj
        | "day5part1" -> day5.solvePart1 (filePath "day5\input.txt") :> obj
        | "day5part2" -> day5.solvePart2 (filePath "day5\input.txt") :> obj
        | "day6part1" -> day6.solvePart1 (filePath "day6\input.txt") :> obj
        | "day6part2" -> day6.solvePart2 (filePath "day6\input.txt") :> obj
        | "day7part1" -> day7.solvePart1 (filePath "day7\input.txt") :> obj
        | "day7part2" -> day7.solvePart2 (filePath "day7\input.txt") :> obj
        | _ -> failwith "todo"

    printfn "answer %A" answer
    0 // return an integer exit code
