module AOC2020.day3

open System.IO

let solve filePath gradients =
    let puzzleInput = File.ReadAllLines filePath |> Array.map (fun x -> x.ToCharArray())

    let rec walk (posX, posY) acc (stepX, stepY) (array2d: char [,]) original =
        let (newPosX, newPosY) = (posX + stepX, posY + stepY)

        match newPosY < Array2D.length1 array2d with
        | false -> acc //end
        | true ->
            let field = if newPosX < Array2D.length2 array2d = false then Array2D.concatenate array2d original else array2d
            let weight = if field.[newPosY, newPosX] = '#' then 1L else 0L

            walk (newPosX, newPosY) (acc + weight) (stepX, stepY) field original

    gradients
    |> Seq.map (fun gradient -> walk (0, 0) 0L gradient (puzzleInput |> array2D) (puzzleInput |> array2D))
    |> Seq.fold (*) 1L
