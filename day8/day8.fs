module AOC2020.day8

open System.IO
open FParsec

// ################
// Domain
// ################

type Expression =
    | ACC of int32
    | JMP of int32
    | NOP of int32

type Program = { Instructions: Expression list }

// ################
// Parser
// ################

let instruction = pstring "acc" <|> pstring "jmp" <|> pstring "nop"

let number = pint32

let mapToExpression i =
    match i with
    | ("acc", num) -> ACC num
    | ("jmp", num) -> JMP num
    | ("nop", num) -> NOP num
    | _ -> failwith "unsupported expression"

let expr = ((instruction .>> spaces) .>>. number) |>> mapToExpression

let mapToProgram expressions = { Instructions = expressions }

let code = many (expr .>> opt skipNewline) |>> mapToProgram

let parse s = run code s

// ################
// Execution engine
// ################
type ExecutionContext = { nextLine: int; accumulator: int64; executedLines: int list }

let applyExpression expr state =
    match expr with
    | ACC num ->
        { state with
              executedLines = state.nextLine :: state.executedLines
              accumulator = state.accumulator + int64 num
              nextLine = state.nextLine + 1 }
    | JMP num ->
        { state with
              executedLines = state.nextLine :: state.executedLines
              nextLine = state.nextLine + int num }
    | NOP _ -> { state with executedLines = state.nextLine :: state.executedLines; nextLine = state.nextLine + 1 }

type ExecutionResult =
    | OK of int64
    | TERMINATED of ExecutionContext

let executeProgram program =
    let terminationLine = program.Instructions |> List.length

    let rec execute state =
        if state.nextLine = terminationLine then
            OK state.accumulator
        else
            let currentExpr = program.Instructions.Item state.nextLine

            if state.executedLines |> List.contains state.nextLine then TERMINATED state else execute (applyExpression currentExpr state)

    execute { nextLine = 0; accumulator = 0L; executedLines = [] }

// ################
// Mutator
// ################

let mutateInstruction instruction =
    match instruction with
    | JMP num -> NOP num
    | NOP num -> JMP num
    | instr -> instr

let mutations program =
    seq {
        for i in 0 .. ((List.length program.Instructions) - 1) do
            match i with
            | 0 -> yield { Instructions = (mutateInstruction program.Instructions.[0]) :: program.Instructions.[1..] }
            | i ->
                yield
                    { Instructions =
                          program.Instructions.[..i - 1]
                          @ (mutateInstruction program.Instructions.[i]) :: program.Instructions.[i + 1..] }
    }

// ################
// puzzles
// ################

let solvePart1 filepath =
    let parseResult = File.ReadAllText filepath |> parse

    match parseResult with
    | Success (result, _, _) -> printfn "%A" (executeProgram result)
    | Failure (errorMsg, _, _) -> printfn "%s" errorMsg

let solvePart2 filepath =
    let parseResult = File.ReadAllText filepath |> parse

    match parseResult with
    | Success (result, _, _) ->
        mutations result
        |> Seq.find (fun x ->
            match executeProgram x with
            | OK acc ->
                printfn "%A" acc
                true
            | TERMINATED _ -> false)
        |> ignore

        ()
    | Failure (errorMsg, _, _) -> printfn "%s" errorMsg
