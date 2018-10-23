module FunctionalUsersAwards.EfDataSource.UserDataSource
open Dto
open UtilityTypes
open FunctionalUsersAwards.EfContext
open FunctionalUsersAwards.EfDataSource
open Microsoft.EntityFrameworkCore
open System.Collections.Generic

let get (getDbContext: unit -> UserAwardDbContext) () = 
    use context = getDbContext()
    context.Users.Include(fun x -> x.Awards :> IEnumerable<_>).ThenInclude(fun (x: UserAward) -> x.Award) |> Seq.map Common.toUserDto |> Seq.toList 

let getById (getDbContext: unit -> UserAwardDbContext) id =
    use context = getDbContext()
    context.Users.Include(fun x -> x.Awards :> IEnumerable<_>).ThenInclude(fun (x: UserAward) -> x.Award) |> Seq.tryFind id |> Option.map Common.toUserDto

let isUnique (getDbContext: unit -> UserAwardDbContext) (user: UserDto) =
    use context = getDbContext()
    context.Users |> Seq.exists (fun (x: User) -> x.Username = user.Username) |> not 
                               
let exists (getDbContext: unit -> UserAwardDbContext) userId =
    use context = getDbContext()
    context.Users |> Seq.exists (fun (x:User) -> x.Id = userId)
                               
let delete (getDbContext: unit -> UserAwardDbContext) id =
    use context = getDbContext()
    context.Users 
    |> Seq.tryFind (fun x -> x.Id = id) 
    |> Option.bind (fun (x: User) -> context.Users.Remove(x) |> ignore
                                     context.SaveChanges() |> ignore
                                     Some id)
                     
    
let add (getDbContext: unit -> UserAwardDbContext) (user: UserDto) =
    use context = getDbContext() 
    isUnique getDbContext user 
    |> Result.boolToResult user "User is already in the database" 
    |> Result.bind (fun x -> context.Add(user |> Common.fromUserDto) |> ignore
                             context.SaveChanges() |> ignore
                             Ok user)