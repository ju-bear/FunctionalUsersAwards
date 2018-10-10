module AdditionalTypes

open System
open UtilityTypes

type UserId = UserId of Guid

module UserId =
    let create() = Guid.NewGuid >> UserId
    let createWithValue id = UserId id
    let getValue (UserId id) = id

type Username = Username of NonEmptyString

type AwardId = AwardId of Guid

module AwardId = 
    let create() = Guid.NewGuid >> AwardId
    let createWithValue id = AwardId id
    let getValue (AwardId id) = id

type AwardTitle = AwardTitle of NonEmptyString