module AOC2020.day7

open System.Collections.Generic
open System.ComponentModel
open System.IO
open FParsec

let betweenSpaces s = spaces >>. s .>> spaces

let bag =
    opt (betweenSpaces pint32) .>>. (charsTillString " bag" true 10000 .>> opt (pstring "s"))
    .>> opt (pchar '.' <|> pchar ',' <|> pchar ' ')

let lineParser = (bag .>> (pstring "contain")) .>>. many bag

let solvePart1 filePath =
    let mutable canBePutInMapping = Dictionary<string, string list>()

    let addToMapping value container =
        match canBePutInMapping.TryGetValue(value) with
        | true, res -> canBePutInMapping.[value] <- (container :: res) |> List.distinct
        | false, res -> canBePutInMapping.[value] <- [ container ]

    File.ReadLines filePath
    |> Seq.iter (fun line ->
        match run lineParser line with
        | Success ((container, containees), _, _) ->
            for (_num, color) in containees do
                addToMapping color (container |> snd)
        | Failure (errorMsg, _, _) -> ())

    let rec handler list state =
        match list with
        | [] -> state
        | x :: xs ->
            let (found, newToAdd) = canBePutInMapping.TryGetValue x

            if found then handler (newToAdd @ xs) (x :: state) else handler xs (x :: state)

    (handler [ "shiny gold" ] [] |> List.distinct |> List.length) - 1

let solvePart2 filePath =
    let mutable canHoldMapping = Dictionary<string, (int * string) list>()

    let addToMapping value container =
        match canHoldMapping.TryGetValue(container) with
        | true, res -> canHoldMapping.[container] <- (value :: res) |> List.distinct
        | false, res -> canHoldMapping.[container] <- [ value ]

    File.ReadLines filePath
    |> Seq.iter (fun line ->
        match run lineParser line with
        | Success ((container, containees), _, _) ->
            for (num, color) in containees do
                addToMapping (num.Value, color) (container |> snd)
        | Failure (errorMsg, _, _) -> ())

    let rec handler (current: string): int =
        let (found, contains) = canHoldMapping.TryGetValue(current)
        if found then contains |> List.sumBy (fun (num, bag) -> (handler bag * num) + num) else 0

    handler "shiny gold"
