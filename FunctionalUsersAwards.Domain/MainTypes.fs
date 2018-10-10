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
                                                                    <*> (createAwardTitle awardDto.Title |> Result.mapError (AwardError.AwardTitleError >> List.wrap))

type User = {
    Id: UserId
    Username: Username
    Awards: Award list
}

module User = 
    let createFromDomain id username awards = { Id = id; Username = username; Awards = awards }
    
    let private processOneAward accum value = match value with
                                              | Ok v -> accum |> Result.map (fun x -> v :: x)
                                              | Error err -> accum |> Result.mapError (fun x -> err :: x)
                                                                        
    let private createAwardList (createAward: AwardDto -> Result<Award, AwardError>) list = 
        list 
        |> List.map createAward 
        |> List.fold processOneAward (Ok [])
    
    let create createUserId createUsername createAward (userDto: UserDto) = 
        createFromDomain 
        <!> (userDto.Id |> createUserId |> Ok) 
        <*> (createUsername userDto.Username |> Result.mapError (UserError.UsernameError >> List.wrap)) 
        <*> (userDto.Awards |> createAwardList createAward |> Result.mapError (UserError.AwardListError >> List.wrap))