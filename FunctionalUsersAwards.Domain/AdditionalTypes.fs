module AdditionalTypes

open System
open UtilityTypes

type UserId = UserId of Guid

type Username = Username of NonEmptyString

type AwardId = AwardId of Guid

type AwardTitle = AwardTitle of NonEmptyString

