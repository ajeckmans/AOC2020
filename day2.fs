module AOC2020.day2

open System.IO

type passwordRequirement = { num1: int; num2: int; char: char; password: string }

let part1 passwordRequirement =
    let count =
        passwordRequirement.password.ToCharArray()
        |> Array.sumBy (fun c -> if c = passwordRequirement.char then 1 else 0)

    count >= passwordRequirement.num1 && count <= passwordRequirement.num2

let part2 passwordRequirement =
    let p1 = passwordRequirement.password.[passwordRequirement.num1 - 1] = passwordRequirement.char
    let p2 = passwordRequirement.password.[passwordRequirement.num2 - 1] = passwordRequirement.char
    p1 <> p2

let solve filePath passwordValid =
    let parseLine (line: string) =
        let components = line.Split(' ', '-')

        { num1 = components.[0] |> int
          num2 = components.[1] |> int
          char = components.[2].[0]
          password = components.[3] }

    File.ReadAllLines filePath |> Array.map parseLine |> Array.filter passwordValid |> Array.length
