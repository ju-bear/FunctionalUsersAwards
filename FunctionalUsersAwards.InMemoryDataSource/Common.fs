module Common
open Dto
open System

let mutable userMap : Map<Guid, UserDto> = Map.empty

let mutable awardMap : Map<Guid, AwardDto> = Map.empty

