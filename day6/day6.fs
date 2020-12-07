module AOC2020.day6

open System
open System.IO

let solvePart1 filePath =
    let groups =
        (File.ReadAllText filePath)
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)

    groups
    |> Seq.map (fun line -> line.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
    |> Seq.map (fun e -> (e |> Seq.collect (fun x -> x.ToCharArray())) |> Seq.distinct |> Seq.length)
    |> Seq.sum

let solvePart2 filePath =
    let groups =
        (File.ReadAllText filePath)
            .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)

    let groupAnswers =
        groups
        |> Seq.map (fun line ->
            let persons = line.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            let answers = persons |> Seq.collect (fun x -> x.ToCharArray()) |> Seq.groupBy id

            answers |> Seq.filter (fun (_, values) -> values |> Seq.length = persons.Length) |> Seq.length)


    groupAnswers |> Seq.sum
