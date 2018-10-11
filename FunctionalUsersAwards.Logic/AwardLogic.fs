module AwardLogic

open Dto
open UtilityTypes
open Option
open Result
open LogicValidation

let get createAward (getAwardDtosFromDataSource: unit -> AwardDto list) = getAwardDtosFromDataSource 
                                                                          >> List.map (fun x -> (x.Id, createAward x)) 
                                                                          >> Result.flattenList
                                                       
let getById createAward getAwardFromDataSource id = getAwardFromDataSource id |> Option.map createAward 

let delete deleteFromDataSource hasUsers id =
    let hasUsersResult = hasUsers id 
    if hasUsersResult |> fst
    then hasUsersResult |> snd |> AwardHasUsers |> Error 
    else match deleteFromDataSource id with
         | Some id -> Ok id
         | None -> Error AwardWasNotFound

let add toDto isUniqueInDataSource addAwardToDataSource award = 
    let awardDto = award |> toDto
    
    if isUniqueInDataSource awardDto
    then addAwardToDataSource awardDto |> Result.mapError AwardLogicError.DataSourceError
    else Error AwardAlreadyExists