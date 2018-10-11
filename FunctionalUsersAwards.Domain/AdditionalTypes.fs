module AdditionalTypes

open System
open UtilityTypes
open Dto

type AwardId = AwardId of Guid

type AwardTitleError = AwardTitleEmptyError | AwardTitleTooLongError of int

type AwardError = AwardTitleError of AwardTitleError

type UserId = UserId of Guid

type UsernameError = UsernameEmptyError | UsernameTooLongError of int 

type UserError = UsernameError of UsernameError | AwardListError of (Guid * AwardError list) list | AwardsMustBeDistinctError

module AwardId = 
    let create() = Guid.NewGuid >> AwardId
    let createWithValue id = AwardId id
    let getValue (AwardId id) = id

type AwardTitle = AwardTitle of NonEmptyString

module AwardTitle =
    let validate maxLength str = if NonEmptyString.length str > maxLength
                                 then NonEmptyString.length str |> AwardTitleTooLongError |> Error
                                 else Ok str
                                 
    let create validate str = NonEmptyString.create str
                              |> Option.optionToResult AwardTitleEmptyError
                              |> Result.bind validate
                              |> Result.map AwardTitle
                              
    let getValue (AwardTitle str) = str

module UserId =
    let create() = Guid.NewGuid >> UserId
    let createWithValue id = UserId id
    let getValue (UserId id) = id

type Username = Username of NonEmptyString

module Username = 
    let validate maxLength str = if NonEmptyString.length str > maxLength 
                                 then NonEmptyString.length str |> UsernameTooLongError |> Error 
                                 else Ok str 

    let create validate str = NonEmptyString.create str 
                              |> Option.optionToResult UsernameEmptyError 
                              |> Result.bind validate
                              |> Result.map Username  
   
    
    let getValue (Username str) = str