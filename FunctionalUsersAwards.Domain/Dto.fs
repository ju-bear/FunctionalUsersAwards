module Dto
open System

type AwardDto = {
    Id: Guid
    Title: string
}

type UserDto = {
    Id: Guid
    Username: string
    Awards: AwardDto list
}