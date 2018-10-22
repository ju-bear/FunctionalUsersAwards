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

let delete deleteFromDataSource id = deleteFromDataSource id 

let add (toDto: User -> UserDto) isUniqueInDataSource areAwardsInDataSource addToDataSource (user: User) = 
    let userDto = user |> toDto
    
    resultBuilder {
        let! uniqueUser = isUniqueInDataSource userDto |> Result.boolToResult userDto UserLogicError.UserAlreadyExists
        let! areAwardsInDataSource = uniqueUser.Awards |> List.map (fun x -> x.Id) |> areAwardsInDataSource |> Result.mapError UserLogicError.SomeAwardsAreNotInDataSource
        let! result = addToDataSource userDto |> Result.mapError UserLogicError.DataSourceError
        return result
    }                                                                  