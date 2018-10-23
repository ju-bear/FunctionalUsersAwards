module FunctionalUsersAwards.EfDataSource.AwardDataSource
open Dto
open FunctionalUsersAwards.EfContext
open FunctionalUsersAwards.EfDataSource
open FunctionalUsersAwards.EfDataSource
open FunctionalUsersAwards.EfDataSource

let get (getDbContext: unit -> UserAwardDbContext) () = 
    use context = getDbContext()
    context.Awards |> Seq.map Common.toAwardDto |> Seq.toList

let getById (getDbContext: unit -> UserAwardDbContext) id = 
    use context = getDbContext()
    context.Awards |> Seq.tryFind (fun x -> x.Id = id) |> Option.map Common.toAwardDto

let isUnique (getDbContext: unit -> UserAwardDbContext) (award: AwardDto) = 
     use context = getDbContext()
     context.Awards |> Seq.tryFind (fun x -> x.Title = award.Title) |> Option.isNone
                                             
let delete (getDbContext: unit-> UserAwardDbContext) id =
    use context = getDbContext()
    context.Awards |> Seq.tryFind (fun x -> x.Id = id) |> Option.bind (fun x -> context.Awards.Remove(x) |> ignore
                                                                                context.SaveChanges() |> ignore
                                                                                Some id) 
                                             
let areAwardsInDataSource (getDbContext: unit -> UserAwardDbContext) (awards: System.Guid list) = 
    use context = getDbContext()
    context.Awards 
    |> Seq.filter (fun x -> awards |> List.contains x.Id)
    |> (fun (x: Award seq) -> if Seq.length x = List.length awards 
                              then x |> Seq.map Common.toAwardDto |> Seq.toList |> Ok 
                              else awards |> Seq.except (context.Awards |> Seq.map (fun y -> y.Id)) |> Error)
    
let add (getDbContext: unit -> UserAwardDbContext) (award: AwardDto) =
    use context = getDbContext()
    try 
        context.Add(award |> Common.fromAwardDto) |> ignore
        context.SaveChanges() |> ignore
        Ok award
    with
        | ex -> Error ex.Message