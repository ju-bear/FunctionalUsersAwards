module UtilityTypes

type NonEmptyString = NonEmptyString of string

module NonEmptyString = 
    let create str = match str with
                     | "" | null -> None
                     | _ -> Some (NonEmptyString str)
                     
    let getValue (NonEmptyString str) = str
    
    let length (NonEmptyString str) = String.length str
    
module Result =
    let (>>=) s f = Result.bind f s
    let apply f s = match f, s with
                    | Ok f, Ok s -> Ok (f s)
                    | Ok f, Error s -> Error s
                    | Error f, Ok s -> Error f
                    | Error f, Error s -> Error (f @ s)
    let (<*>) f s = apply f s
    let (<!>) f s = Result.map f s
    
    let boolToResult okValue errorValue b = if b then okValue |> Ok else errorValue |> Error 
    
    let flatten v = match v with
                    | Ok res -> res
                    | Error err -> Error err
                    
    let private processOne accum value = match value |> snd with
                                         | Ok v -> accum |> Result.map (fun x -> v :: x)
                                         | Error err -> match accum with
                                                        | Ok _ -> [value |> fst, err] |> Error
                                                        | Error accumError -> [value |> fst, err] @ accumError |> Error
                                                
    let flattenList list = List.fold processOne (Ok []) list
    
    type ResultBuilder() =
        member x.Bind(s, f) = Result.bind f s
        member x.Return(v) = Ok v
        member x.ReturnFrom(v) = v
    
    let resultBuilder = new ResultBuilder() 
    
module Option = 
    let (>>=) s f = Option.bind f s
    let optionToResult error opt = opt |> Option.map Ok |> Option.defaultValue (Error error) 
    
module List =
    let wrap value = [value]