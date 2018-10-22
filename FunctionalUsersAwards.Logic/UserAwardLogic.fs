module UserAwardLogic

open LogicValidation
open UtilityTypes
open Result

let addAwardToUser addAwardToUserInDataSource isUserInDatasource areAwardsInDataSource userId awardIds =
    resultBuilder {
        let! userInDataSource = isUserInDatasource userId |> boolToResult () (UserAwardLogicError.UserNotInDataSource userId)
        let! awardsInDataSource = areAwardsInDataSource awardIds |> Result.mapError UserAwardLogicError.SomeAwardsAreNotInDataSource
        return! addAwardToUserInDataSource userId awardIds |> Result.mapError UserAwardLogicError.DataSourceError 
    }