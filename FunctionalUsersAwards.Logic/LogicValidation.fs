module LogicValidation

open System

type AwardLogicError = AwardAlreadyExists | DataSourceError of string

type UserLogicError = UserAlreadyExists | SomeAwardsAreNotInDataSource of Guid list | DataSourceError of string