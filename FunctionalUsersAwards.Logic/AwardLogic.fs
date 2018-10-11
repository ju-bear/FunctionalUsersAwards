module AwardLogic

open Dto
open UtilityTypes

let getAwards createAward getAwardDtosFromDataSource = getAwardDtosFromDataSource() 
                                                       |> List.map (fun x -> (x.Id, createAward x))
                                                       |> Result.flattenList