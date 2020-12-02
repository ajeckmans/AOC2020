module AOC2020.day2

open System.IO
open FSharp.Text.RegexProvider
open FSharp.Text.RegexExtensions

type LineRegex = Regex< @"(?<num1>\d+)-(?<num2>\d+) (?<char>.{1}): (?<password>.+)$" >

type LineInput = { num1: int; num2: int; char: char; password: string }

let part1 lineInput =
    let count = lineInput.password.ToCharArray() |> Array.sumBy (fun c -> if c = lineInput.char then 1 else 0)

    count >= lineInput.num1 && count <= lineInput.num2

let part2 lineInput =
    let p1 = lineInput.password.[lineInput.num1 - 1] = lineInput.char
    let p2 = lineInput.password.[lineInput.num2 - 1] = lineInput.char
    p1 <> p2

let solve filePath passwordValid =
    let matcher = LineRegex()

    let parseLine (line: string) =
        let parsed = matcher.TypedMatch(line)

        { num1 = parsed.num1.AsInt
          num2 = parsed.num2.AsInt
          char = parsed.char.AsChar
          password = parsed.password.Value }

    File.ReadAllLines filePath |> Array.map parseLine |> Array.filter passwordValid |> Array.length
