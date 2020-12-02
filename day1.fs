module AOC2020.day1

open System.IO

let solve filePath combinationCount =
    let puzzleInput = File.ReadAllLines filePath |> Array.toList |> List.map int

    puzzleInput
    |> List.combinations combinationCount
    |> List.find (fun l -> List.sum l = 2020)
    |> List.fold (*) 1
