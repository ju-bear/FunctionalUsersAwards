module UserLogic

open UtilityTypes
open Dto
open Result
open Option
open MainTypes
open LogicValidation

let get (getUsersFromDataSource: unit -> UserDto list) createUser = getUsersFromDataSource 
                                                                    >> List.map (fun x -> (x.Id, createUser x)) 
                                                                    >> Result.flattenList
                                                                    
let getById getUserFromDataSource createUser id = getUserFromDataSource id |> Option.map createUser

let add toDto isUniqueInDataSource areAwardsInDataSource addToDataSource (user: User) = 
    let userDto = user |> toDto
    if isUniqueInDataSource userDto
    then 
        let areAwardsInDataSourceResult = areAwardsInDataSource userDto.Awards
        if areAwardsInDataSourceResult |> fst 
        then addToDataSource userDto |> Result.mapError UserLogicError.DataSourceError
        else areAwardsInDataSourceResult |> snd |> SomeAwardsAreNotInDataSource |> Error
    else Error UserAlreadyExists                                                                  