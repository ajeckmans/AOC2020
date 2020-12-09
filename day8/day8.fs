module AOC2020.day8

open System.IO
open FParsec

// ################
// Domain
// ################

type Instruction =
    | ACC of int32
    | JMP of int32
    | NOP of int32

type Program = { Instructions: Instruction list }

// ################
// Parser
// ################

let operationParser = pstring "acc" <|> pstring "jmp" <|> pstring "nop"

let numberParser = pint32

let mapToInstruction i =
    match i with
    | ("acc", num) -> ACC num
    | ("jmp", num) -> JMP num
    | ("nop", num) -> NOP num
    | _ -> failwith "unsupported instruction"

let instructionParser = ((operationParser .>> spaces) .>>. numberParser) |>> mapToInstruction

let mapToProgram instructions = { Instructions = instructions }

let sourceParser = many (instructionParser .>> opt skipNewline) |>> mapToProgram

let parse source = run sourceParser source

// ################
// Execution engine
// ################
type ExecutionContext = { nextLine: int; accumulator: int64; executedLines: int list }

let executeInstruction expr state =
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

    let rec run state instructions =
        if state.nextLine = terminationLine then
            OK state.accumulator
        else
            let currentExpr = instructions |> List.item state.nextLine

            if state.executedLines |> List.contains state.nextLine then TERMINATED state else run (executeInstruction currentExpr state) instructions

    run { nextLine = 0; accumulator = 0L; executedLines = [] } program.Instructions

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
