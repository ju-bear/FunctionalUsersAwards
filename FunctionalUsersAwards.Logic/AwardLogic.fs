module AwardLogic

open Dto
open UtilityTypes

let get createAward (getAwardDtosFromDataSource: unit -> AwardDto list) = getAwardDtosFromDataSource 
                                                                          >> List.map (fun x -> (x.Id, createAward x)) 
                                                                          >> Result.flattenList
                                                       
let getById createAward getAwardFromDataSource = getAwardFromDataSource >> createAward

let add toDto addAwardToDataSource = toDto >> addAwardToDataSource