module UserAwardLogic

open LogicValidation

let addAwardToUser addAwardToUserInDataSource userId awardIds = match addAwardToUserInDataSource userId awardIds with
                                                                | Ok id -> Ok id
                                                                | Error err -> err |> UserAwardLogicError.DataSourceError |> Error

