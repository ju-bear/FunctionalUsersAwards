module CompositionRoot
open AdditionalTypes
open MainTypes

module AwardRoot = 
    let maxTitleLength = 55
    let createDefaultAwardId = AwardId.createWithValue
    let createDefaultAwardTitle = AwardTitle.create (AwardTitle.validate maxTitleLength)
    let createDefaultAward = Award.create createDefaultAwardId createDefaultAwardTitle

module UserRoot =
    let maxUsernameLength = 55
    let createDefaultUserId = UserId.createWithValue
    let createDefaultUsername = Username.create (Username.validate maxUsernameLength)
    let createDefaultUser = User.create createDefaultUserId createDefaultUsername AwardRoot.createDefaultAward

