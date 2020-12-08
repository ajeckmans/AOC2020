module AOC2020.day8

open System.Collections.Generic
open System.IO
open FParsec

// Parser
let instruction = pstring "acc" <|> pstring "jmp" <|> pstring "nop"

let number = pint32

let expr = (instruction .>> spaces) .>>. number

let code = many (expr .>> opt skipNewline)

let parse s = run code s

// Execution engine

type ExecutionContext = { nextLine: int; accumulator: int64; executedLines: int list }

let applyExpression (expr: string * int32) state =
    match expr with
    | ("acc", num) ->
        { state with
              executedLines = state.nextLine :: state.executedLines
              accumulator = state.accumulator + int64 num
              nextLine = state.nextLine + 1 }
    | ("jmp", num) ->
        { state with
              executedLines = state.nextLine :: state.executedLines
              nextLine = state.nextLine + int num }
    | ("nop", _) -> { state with executedLines = state.nextLine :: state.executedLines; nextLine = state.nextLine + 1 }
    | _ -> failwith "unsupported expression"

type ExecutionResult =
    | OK of int64
    | TERMINATED of ExecutionContext

let executeCode instructions =
    let terminationLine = instructions |> List.length

    let rec execute state =
        if state.nextLine = terminationLine then
            OK state.accumulator
        else
            let currentExpr = instructions.Item state.nextLine

            if state.executedLines |> List.contains state.nextLine then TERMINATED state else execute (applyExpression currentExpr state)

    execute { nextLine = 0; accumulator = 0L; executedLines = [] }


// puzzles

let solvePart1 filepath =
    let parseResult = File.ReadAllText filepath |> parse

    match parseResult with
    | Success (result, _, _) -> printfn "%A" (executeCode result)
    | Failure (errorMsg, _, _) -> printfn "%s" errorMsg

let mutateInstruction instruction =
    match instruction with
    | ("jmp", num) -> ("nop", num)
    | ("nop", num) -> ("jmp", num)
    | instr -> instr

let mutations instructions =
    seq {
        for i in 0 .. ((List.length instructions) - 1) do
            match i with
            | 0 -> yield (mutateInstruction instructions.[0]) :: instructions.[1..]
            | i -> yield instructions.[..i - 1] @ (mutateInstruction instructions.[i]) :: instructions.[i + 1..]
    }

let solvePart2 filepath =
    let parseResult = File.ReadAllText filepath |> parse

    match parseResult with
    | Success (result, _, _) ->
        let possibles = mutations result |> Seq.toList

        possibles
        |> Seq.find (fun x ->
            match executeCode x with
            | OK acc ->
                printfn "%A" acc
                true
            | TERMINATED _ -> false)
        |> ignore

        ()
    | Failure (errorMsg, _, _) -> printfn "%s" errorMsg
