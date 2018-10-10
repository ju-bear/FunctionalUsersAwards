module MainTypes
open AdditionalTypes
open UtilityTypes
open Result
open Dto

type Award = {
    Id: AwardId
    Title: AwardTitle
}

module Award =
    let createFromDomain id title = { Id = id; Title = title }
    
    let create createAwardId createAwardTitle (awardDto: AwardDto) = createFromDomain 
                                                                    <!> (awardDto.Id |> createAwardId |> Ok) 
                                                                    <*> createAwardTitle awardDto.Title 
                                                                        |> Result.mapError List.wrap

type User = {
    Id: UserId
    Username: Username
    Awards: Award list
}