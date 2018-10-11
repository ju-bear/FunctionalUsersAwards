module UserDataSource

open Dto

let get() = Common.userMap |> Map.toList |> List.map (fun (x, y) -> y) 

let getById id = Common.userMap |> Map.tryFind id

let isUnique (user: UserDto) = Common.userMap 
                               |> Map.exists (fun _ value -> user.Username = value.Username)
                               |> not
    
let add (user: UserDto) = 
    if isUnique user 
    then 
        Common.userMap <- Common.userMap |> Map.add user.Id user
        Ok user
    else Error "Award is already in the database" 