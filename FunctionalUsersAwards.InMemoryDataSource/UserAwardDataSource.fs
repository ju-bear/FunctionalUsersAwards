module UserAwardDataSource

let hasUsers id = Common.userMap 
                  |> Map.filter (fun key value -> value.Awards |> List.tryFind (fun x -> x.Id = id) |> Option.isSome)
                  |> Map.toList
                  |> (fun x -> List.length x > 0, x |> List.map (fun (u, y) -> u))
                  
let addAwardsToUser userId awardIds = if AwardDataSource.areAwardsInDataSource awardIds |> fst 
                                      then if Common.userMap |> Map.containsKey userId 
                                           then Common.userMap <- 
                                                                 let awards = Common.awardMap |> Map.toList |> List.filter (fun (x, y) -> awardIds |> List.contains x) |> List.map (fun (x, y) -> y)
                                                                 let user = Common.userMap |> Map.find userId
                                                                 Common.userMap |> Map.add userId { user with Awards = user.Awards @ (awards |> List.except user.Awards) }
                                                Ok userId
                                           else Error "User was not found in data source"
                                      else Error "Not all awards are in database"

