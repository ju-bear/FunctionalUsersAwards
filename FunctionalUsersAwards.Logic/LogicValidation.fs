module LogicValidation

open System

type AwardLogicError = AwardAlreadyExists | DataSourceError of string | AwardHasUsers of Guid list | AwardWasNotFound

type UserLogicError = UserAlreadyExists | SomeAwardsAreNotInDataSource of Guid list | DataSourceError of string