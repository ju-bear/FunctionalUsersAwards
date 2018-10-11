module AwardLogic

open Dto
open UtilityTypes
open Option

let get createAward (getAwardDtosFromDataSource: unit -> AwardDto list option) = getAwardDtosFromDataSource() 
                                                                                  |> Option.map (List.map (fun x -> (x.Id, createAward x)) 
                                                                                                 >> Result.flattenList >> Some)
                                                       
let getById createAward getAwardFromDataSource id = getAwardFromDataSource id |> Option.map createAward 

let add toDto isUniqueInDataSource addAwardToDataSource award = award 
                                                                |> toDto 
                                                                |> isUniqueInDataSource 
                                                                >>= (addAwardToDataSource >> Some)