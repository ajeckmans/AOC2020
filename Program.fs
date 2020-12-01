// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO

let rec comb n l =
    match n, l with
    | 0, _ -> [ [] ]
    | _, [] -> []
    | k, (x :: xs) -> List.map ((@) [ x ]) (comb (k - 1) xs) @ comb k xs

[<EntryPoint>]
let main argv =
    let filePath = Path.Combine(Directory.GetCurrentDirectory(), argv.[0])
    let combinationCount = argv.[1] |> int

    let puzzleInput = File.ReadAllLines filePath |> Array.toList |> List.map int

    let answer =
        puzzleInput
        |> comb combinationCount
        |> List.filter (fun l -> List.sum l = 2020)
        |> List.head
        |> List.fold (*) 1

    printfn "answer %A" answer
    0 // return an integer exit code
