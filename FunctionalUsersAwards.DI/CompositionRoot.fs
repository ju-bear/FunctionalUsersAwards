module CompositionRoot
open AdditionalTypes
open MainTypes

module AwardRoot = 
    let maxTitleLength = 55
    let createDefaultAwardId = AwardId.createWithValue
    let createDefaultAwardTitle = AwardTitle.create (AwardTitle.validate maxTitleLength)
    let createDefaultAward = Award.create createDefaultAwardId createDefaultAwardTitle
    let toDto = Award.toDto

module UserRoot =
    let maxUsernameLength = 55
    let createDefaultUserId = UserId.createWithValue
    let createDefaultUsername = Username.create (Username.validate maxUsernameLength)
    let createDefaultUser = User.create User.validateUser createDefaultUserId createDefaultUsername AwardRoot.createDefaultAward
    let toDto = User.toDto AwardRoot.toDto
    
module AwardLogicRoot =
    let get = AwardLogic.get AwardRoot.createDefaultAward AwardDataSource.get
    let getById = AwardLogic.getById AwardRoot.createDefaultAward AwardDataSource.getById
    let add = AwardLogic.add AwardRoot.toDto AwardDataSource.isUnique AwardDataSource.add