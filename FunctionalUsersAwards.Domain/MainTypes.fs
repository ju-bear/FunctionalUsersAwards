module MainTypes
open AdditionalTypes

type Award = {
    Id: AwardId
    Title: AwardTitle
}

type User = {
    Id: UserId
    Username: Username
    Awards: Award list
}

