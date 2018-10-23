module AwardLogic

open Dto
open UtilityTypes
open Option
open Result
open System
open LogicValidation

let get createAward (getAwardDtosFromDataSource: unit -> AwardDto list) = getAwardDtosFromDataSource 
                                                                          >> List.map (fun x -> (x.Id, createAward x)) 
                                                                          >> Result.flattenList
                                                       
let getById createAward getAwardFromDataSource id = getAwardFromDataSource id |> Option.map createAward 

let delete (deleteFromDataSource: Guid -> Guid option) hasUsers id =
    hasUsers id 
    |> Result.mapError AwardHasUsers 
    |> Result.bind (fun id -> deleteFromDataSource id |> Option.optionToResult AwardWasNotFound)

let add toDto isUniqueInDataSource addAwardToDataSource award = 
    let awardDto = award |> toDto
    
    if isUniqueInDataSource awardDto
    then addAwardToDataSource awardDto |> Result.mapError AwardLogicError.DataSourceError
    else Error AwardAlreadyExists