module UserDataSource

open Dto
open UtilityTypes

let get() = Common.userMap |> Map.toList |> List.map (fun (x, y) -> y) 

let getById id = Common.userMap |> Map.tryFind id

let isUnique (user: UserDto) = Common.userMap 
                                  |> Map.exists (fun _ value -> user.Username = value.Username)
                                  |> not
                               
let exists userId = Common.userMap |> Map.exists (fun key _ -> userId = key)
                               
let delete id = if Common.userMap |> Map.containsKey id 
                then Common.userMap <- Common.userMap |> Map.remove id
                     Some id
                else None
                     
    
let add (user: UserDto) = isUnique user 
                          |> Result.boolToResult user "User is already in the database" 
                          |> Result.bind (fun x -> Common.userMap <- Common.userMap |> Map.add user.Id user
                                                   Ok user)