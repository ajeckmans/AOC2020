module AOC2020.day9

open System.IO

let getInvalid input =
    input
    |> List.windowed 26
    |> List.find (fun w ->
        (w.[..^1]
         |> List.ofSeq
         |> List.combinations 2
         |> List.filter (fun x -> (List.distinct x).Length = x.Length)
         |> List.map List.sum)
        |> List.contains w.[^0]
        |> not)
    |> List.last

let solvePart1 filepath = File.ReadLines filepath |> Seq.map int64 |> List.ofSeq |> getInvalid

let solvePart2 filepath =
    let input = File.ReadLines filepath |> Seq.map int64 |> List.ofSeq

    let invalid = getInvalid input

    let rec find chunkSize list =
        let sets = list |> List.windowed chunkSize

        match sets |> List.tryFind (fun x -> (List.sum x) = invalid) with
        | Some result ->
            let sorted = List.sort result
            sorted.[0] + sorted.[^0]
        | None -> find (chunkSize + 1) list

    find 2 input
