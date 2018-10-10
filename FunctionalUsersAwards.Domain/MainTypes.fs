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
    
    let private processOneAward accum value = match value |> snd with
                                              | Ok v -> accum |> Result.map (fun x -> v :: x)
                                              | Error err -> match accum with
                                                             | Ok _ -> [value |> fst, err] |> Error
                                                             | Error v1 -> [value |> fst, err] @ v1 |> Error
                                                                        
    let private createAwardList (createAward: AwardDto -> Result<Award, AwardError list>) (list: AwardDto list) = 
        list 
        |> List.map (fun x -> (x.Id, x |> createAward))
        |> List.fold processOneAward (Ok [])
    
    let create createUserId createUsername createAward (userDto: UserDto) = 
        createFromDomain 
       <!> (userDto.Id |> createUserId |> Ok) 
       <*> (createUsername userDto.Username |> Result.mapError (UserError.UsernameError >> List.wrap))
       <*> (userDto.Awards |> createAwardList createAward |> Result.mapError (AwardListError >> List.wrap))