// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System.IO
open AOC2020


[<EntryPoint>]
let main argv =
    let filePath file = Path.Combine(Directory.GetCurrentDirectory(), file)

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
        | _ -> failwith "todo"

    printfn "answer %A" answer
    0 // return an integer exit code
