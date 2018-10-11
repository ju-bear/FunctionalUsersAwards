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
    let validateUser (user: User) = if user.Awards |> List.distinctBy (fun x -> x.Title) = user.Awards
                                    then Ok user
                                    else Error (AwardsMustBeDistinctError)

    let createFromDomain validate id username awards = validate { Id = id; Username = username; Awards = awards }
    
    let private processOneAward accum value = match value |> snd with
                                              | Ok v -> accum |> Result.map (fun x -> v :: x)
                                              | Error err -> match accum with
                                                             | Ok _ -> [value |> fst, err] |> Error
                                                             | Error accumError -> [value |> fst, err] @ accumError |> Error
                                                                        
    let private createAwardList (createAward: AwardDto -> Result<Award, AwardError list>) (list: AwardDto list) = 
        list 
        |> List.map (fun x -> (x.Id, x |> createAward))
        |> List.fold processOneAward (Ok [])
    
    let create validateUser createUserId createUsername createAward (userDto: UserDto) = 
        createFromDomain validateUser
       <!> (userDto.Id |> createUserId |> Ok) 
       <*> (createUsername userDto.Username |> Result.mapError (UserError.UsernameError >> List.wrap))
       <*> (userDto.Awards |> createAwardList createAward |> Result.mapError (AwardListError >> List.wrap))
       |> Result.map (Result.mapError List.wrap)
       |> Result.flatten