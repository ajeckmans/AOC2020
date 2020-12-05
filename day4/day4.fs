module AOC2020.day4

open System
open System.IO
open FSharp.Text.RegexProvider
open FSharp.Text.RegexExtensions

// Domain

type BirthYear =
    | ValidBirthYear of int
    | InvalidBirthYear of int
    | Unknown
    static member FromString s =
        let parsed = s |> int
        if parsed >= 1920 && parsed <= 2002 then ValidBirthYear parsed else InvalidBirthYear parsed

type IssueYear =
    | ValidIssueYear of int
    | InvalidIssueYear of int
    | Unknown
    static member FromString s =
        let parsed = s |> int
        if parsed >= 2010 && parsed <= 2020 then ValidIssueYear parsed else InvalidIssueYear parsed

type ExpirationYear =
    | ValidExpirationYear of int
    | InvalidExpirationYear of int
    | Unknown
    static member FromString s =
        let parsed = s |> int
        if parsed >= 2020 && parsed <= 2030 then ValidExpirationYear parsed else InvalidExpirationYear parsed

type HeightRegexType = Regex< @"(?<number>\d+)(?<unit>in|cm)$" >
let HeightRegex = HeightRegexType()

type Height =
    | ValidHeight of int * string
    | InvalidHeight of string
    | Unknown
    static member FromString s =
        let parsed = HeightRegex.TypedMatch(s)

        if parsed.Success then
            let unit = parsed.unit.Value
            let height = parsed.number.AsInt

            match (unit, height) with
            | ("cm" as unit, h) when h >= 150 && h <= 193 -> ValidHeight(h, unit)
            | ("in" as unit, h) when h >= 59 && h <= 76 -> ValidHeight(h, unit)
            | _ -> InvalidHeight s
        else
            InvalidHeight s

type HairColorRegexType = Regex< @"^#[0-9a-f]{6}$" >
let HairColorRegex = HairColorRegexType()

type HairColor =
    | ValidHairColor of string
    | InvalidHairColor
    | Unknown
    static member FromString s = if HairColorRegex.IsMatch(s) then ValidHairColor s else InvalidHairColor

type EyeColor =
    | ValidEyeColor of string
    | InvalidEyeColor
    | Unknown
    static member FromString s =
        if [ "amb"; "blu"; "brn"; "gry"; "grn"; "hzl"; "oth" ] |> List.contains s then ValidEyeColor s else InvalidEyeColor

type PassportIDRegexType = Regex< @"^[0-9]{9}$" >
let PassportIDRegex = PassportIDRegexType()

type PassportID =
    | ValidPassportID of string
    | InvalidPassportID
    | Unknown
    static member FromString s = if PassportIDRegex.IsMatch(s) then ValidPassportID s else InvalidPassportID


type PassportData =
    { BirthYear: BirthYear
      IssueYear: IssueYear
      ExpirationYear: ExpirationYear
      Height: Height
      HairColor: HairColor
      EyeColor: EyeColor
      PassportId: PassportID
      cid: string option }
    static member Default =
        { BirthYear = BirthYear.Unknown
          IssueYear = IssueYear.Unknown
          ExpirationYear = ExpirationYear.Unknown
          Height = Height.Unknown
          HairColor = HairColor.Unknown
          EyeColor = EyeColor.Unknown
          PassportId = PassportID.Unknown
          cid = None }

    member this.IsValid =
        match this with
        | { BirthYear = BirthYear.ValidBirthYear _
            IssueYear = IssueYear.ValidIssueYear _
            ExpirationYear = ExpirationYear.ValidExpirationYear _
            Height = Height.ValidHeight _
            HairColor = HairColor.ValidHairColor _
            EyeColor = EyeColor.ValidEyeColor _
            PassportId = PassportID.ValidPassportID _
            cid = _ } -> true
        | _ -> false

// Solver

let MapToPassportData (s: string) =
    s.Split(" ", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
    |> Seq.fold (fun (passport: PassportData) c ->
        let cp = c.Split(":")

        if not (String.IsNullOrWhiteSpace(cp.[1])) then
            match cp.[0] with
            | "byr" -> { passport with BirthYear = BirthYear.FromString cp.[1] }
            | "iyr" -> { passport with IssueYear = IssueYear.FromString cp.[1] }
            | "eyr" -> { passport with ExpirationYear = ExpirationYear.FromString cp.[1] }
            | "hgt" -> { passport with Height = Height.FromString cp.[1] }
            | "hcl" -> { passport with HairColor = HairColor.FromString cp.[1] }
            | "ecl" -> { passport with EyeColor = EyeColor.FromString cp.[1] }
            | "pid" -> { passport with PassportId = PassportID.FromString cp.[1] }
            | "cid" -> { passport with cid = Some cp.[1] }
            | _ -> failwith "todo"
        else
            passport) PassportData.Default

let solve filePath =
    File.ReadAllLines filePath
    |> Array.fold (fun state elem ->
        match elem, state with
        | "", _ -> "" :: state
        | s, [] -> s :: state
        | s, "" :: rest -> s :: rest
        | s, first :: rest -> first + " " + s :: rest) []
    |> List.map MapToPassportData
    |> List.filter (fun e -> e.IsValid)
    |> List.length
