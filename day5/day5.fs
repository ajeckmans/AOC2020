module AOC2020.day5

open System.IO

let charsToByte chars =
    chars
    |> Seq.rev
    |> Seq.fold (fun (state, pos) c -> if c = 'B' || c = 'R' then ((1uy <<< pos) ||| state, pos + 1) else (state, pos + 1)) (0 |> byte, 0)
    |> fst

let fileToRowColumn filePath =
    File.ReadAllLines filePath
    |> Array.map (fun line ->
        let chars = line.ToCharArray()
        (chars |> Seq.take 7 |> charsToByte, chars |> Seq.skip 7 |> charsToByte))
    |> Array.map (fun (row, column) -> (row |> int) * 8 + (column |> int))

let solvePart1 filePath = fileToRowColumn filePath |> Array.sortDescending |> Array.head

let solvePart2 filePath =
    fileToRowColumn filePath
    |> Array.sort
    |> Array.skip 1
    |> Array.pairwise
    |> Array.filter (fun (e1, e2) -> e1 = (e2 - 2))
    |> Array.head
    |> fst
    |> (+) 1
