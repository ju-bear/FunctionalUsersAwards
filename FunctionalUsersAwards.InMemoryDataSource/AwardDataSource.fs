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
                                             
let areAwardsInDataSource (awards: System.Guid list) = 
    Common.awardMap
    |> Map.toList
    |> List.map (fun (key, value) -> key)
    |> List.filter (fun x -> awards |> List.contains x)
    |> (fun resultList -> List.length resultList = List.length awards, awards |> List.except resultList)
    
let add (award: AwardDto) = 
    if isUnique award 
    then 
        Common.awardMap <- Common.awardMap |> Map.add award.Id award
        Ok award
    else Error "Award is already in the database" 