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
                                                                    
    let toDto (award: Award) : AwardDto = { Id = award.Id |> AwardId.getValue 
                                            Title = award.Title |> AwardTitle.getValue |> NonEmptyString.getValue }  

type User = {
    Id: UserId
    Username: Username
    Awards: Award list
}

module User = 
    open UtilityTypes

    let validateUser (user: User) = if user.Awards |> List.distinctBy (fun x -> x.Title) = user.Awards
                                    then Ok user
                                    else Error (AwardsMustBeDistinctError)

    let createFromDomain validate id username awards = validate { Id = id; Username = username; Awards = awards }
    
    let toDto toAwardDto (user: User) : UserDto = { Id = user.Id |> UserId.getValue 
                                                    Username = user.Username |> Username.getValue |> NonEmptyString.getValue
                                                    Awards = user.Awards |> List.map toAwardDto } 
    
    let create validateUser createUserId createUsername createAward (userDto: UserDto) = 
        createFromDomain validateUser
       <!> (userDto.Id |> createUserId |> Ok) 
       <*> (createUsername userDto.Username |> Result.mapError (UserError.UsernameError >> List.wrap))
       <*> (userDto.Awards |> List.map (fun x -> (x.Id, x |> createAward)) |> Result.flattenList |> Result.mapError (AwardListError >> List.wrap))
       |> Result.map (Result.mapError List.wrap)
       |> Result.flatten