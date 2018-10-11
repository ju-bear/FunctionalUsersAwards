module AwardDataSource
open Dto

let get() = Common.awardMap |> Map.toList |> List.map (fun (x, y) -> y) 

let getById id = Common.awardMap |> Map.tryFind id

let isUnique (award: AwardDto) = Common.awardMap 
                                             |> Map.exists (fun _ value -> award.Title = value.Title)
                                             |> not
                                             
let delete id = if Common.awardMap |> Map.containsKey id 
                then Common.awardMap <- Common.awardMap |> Map.remove id
                     Some id
                else None 

let hasUsers id = Common.userMap 
                  |> Map.filter (fun key value -> value.Awards |> List.tryFind (fun x -> x.Id = id) |> Option.isSome)
                  |> Map.toList
                  |> (fun x -> List.length x > 0, x |> List.map (fun (u, y) -> u))
                                             
let areAwardsInDataSource (awards: AwardDto list) =
    let awardsGuids = awards |> List.map (fun x -> x.Id) 
    Common.awardMap
    |> Map.toList
    |> List.map (fun (key, value) -> key)
    |> List.filter (fun x -> awardsGuids |> List.contains x)
    |> (fun resultList -> List.length resultList = List.length awardsGuids, awardsGuids |> List.except resultList)
    
let add (award: AwardDto) = 
    if isUnique award 
    then 
        Common.awardMap <- Common.awardMap |> Map.add award.Id award
        Ok award
    else Error "Award is already in the database" 