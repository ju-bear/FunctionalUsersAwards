module FunctionalUsersAwards.EfDataSource.UserAwardDataSource

open Microsoft.EntityFrameworkCore
open FunctionalUsersAwards.EfContext
open AwardDataSource
open FunctionalUsersAwards.EfContext
open UtilityTypes

let hasUsers (getDbContext: unit -> UserAwardDbContext) id =
    use context = getDbContext()
    context.Awards.Include(fun x -> x.Users) 
    |> Seq.tryFind (fun (x: Award) -> x.Id = id) 
    |> Option.optionToResult [] 
    |> Result.bind (fun (x: Award) -> if x.Users |> Seq.isEmpty then Ok id else x.Users |> Seq.map (fun x -> x.UserId) |> Seq.toList |> Error)
                  
                  
let addAwardsToUser (getDbContext: unit -> UserAwardDbContext) userId awardIds =
    use context = getDbContext()
    Result.resultBuilder {
        let! areAwardsInDataSource = AwardDataSource.areAwardsInDataSource getDbContext awardIds |> Result.mapError (fun x -> "Not all awards are in data source")
        let! user = context.Users.Include(fun (x: User) -> x.Awards) |> Seq.tryFind (fun (x: User) -> x.Id = userId) |> Option.optionToResult "User was not foudn in data source"
        user.Awards.AddRange(awardIds |> Seq.map (fun x -> new UserAward(UserId = userId, AwardId = x))) |> ignore
        context.SaveChanges() |> ignore
        return userId
    }