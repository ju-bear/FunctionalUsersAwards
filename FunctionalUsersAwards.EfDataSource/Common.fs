module FunctionalUsersAwards.EfDataSource.Common
open System.Collections.Generic
open FunctionalUsersAwards.EfContext
open Dto

let toAwardDto (award: Award) = { Id = award.Id; Title = award.Title }
let toUserDto (user: User) = { Id = user.Id; Username = user.Username; Awards = user.Awards |> Seq.map (fun x -> x.Award |> toAwardDto) |> Seq.toList }
let fromAwardDto (award: AwardDto) = new Award(Id = award.Id, Title = award.Title)
let fromUserDto (user: UserDto) = new User(Id = user.Id, Username = user.Username)