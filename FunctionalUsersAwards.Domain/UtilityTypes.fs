module UtilityTypes

type NonEmptyString = NonEmptyString of string

module NonEmptyString = 
    let create str = match str with
                     | "" | null -> None
                     | _ -> Some (NonEmptyString str)
                     
    let getValue (NonEmptyString str) = str
    
module Result =
    let (>>=) s f = Result.bind f s
    let apply f s = match f, s with
                    | Ok f, Ok s -> Ok (f s)
                    | Ok f, Error s -> Error s
                    | Error f, Ok s -> Error f
                    | Error f, Error s -> Error (f @ s)
    let (<*>) f s = apply f s
    let (<!>) s f = Result.map f s
    
module Option = 
    let (>>=) s f = Option.bind f s
    let optionToResult error opt = Option.defaultValue error opt 