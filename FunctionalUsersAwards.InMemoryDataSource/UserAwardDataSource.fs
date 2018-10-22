module UserAwardDataSource

open AwardDataSource
open UtilityTypes

let hasUsers id = Common.userMap 
                  |> Map.filter (fun key value -> value.Awards |> List.tryFind (fun x -> x.Id = id) |> Option.isSome)
                  |> Map.toList
                  |> (fun x -> List.length x > 0, x |> List.map (fun (u, y) -> u))
                  
                  
let addAwardsToUser userId awardIds =
    Result.resultBuilder {
        let! areAwardsInDataSource = AwardDataSource.areAwardsInDataSource awardIds |> Result.mapError (fun x -> "Not all awards are in data source")
        let! userExists = UserDataSource.exists userId |> Result.boolToResult () "User was not found in data source" 
        Common.userMap <- 
                         let awards = Common.awardMap |> Map.toList |> List.filter (fun (x, y) -> awardIds |> List.contains x) |> List.map (fun (x, y) -> y)
                         let user = Common.userMap |> Map.find userId
                         Common.userMap |> Map.add userId { user with Awards = user.Awards @ (awards |> List.except user.Awards) }
        return userId
    }