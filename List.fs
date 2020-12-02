module List

let rec combinations n l =
    match n, l with
    | 0, _ -> [ [] ]
    | _, [] -> []
    | k, (x :: xs) -> List.map ((@) [ x ]) (combinations (k - 1) xs) @ combinations k xs
