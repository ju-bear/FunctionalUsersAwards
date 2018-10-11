module AwardDataSource
open Dto

let get() = Common.awardMap |> Map.toList |> List.map (fun (x, y) -> y) 

let getById id = Common.awardMap |> Map.tryFind id

let isUnique (award: AwardDto) = Common.awardMap 
                                             |> Map.exists (fun _ value -> award.Title = value.Title)
                                             |> not
                                             
let areAwardsInDataSource (awards: AwardDto list) =
    let awardsGuids = awards |> List.map (fun x -> x.Id) 
    Common.awardMap
    |> Map.toList
    |> List.map (fun (key, value) -> key)
    |> List.filter (fun x -> awardsGuids |> List.contains x)
    |> (fun resultList -> List.length resultList = List.length awardsGuids, awardsGuids |> List.except resultList)