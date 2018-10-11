module CompositionRoot
open AdditionalTypes
open MainTypes

module AwardRoot = 
    let maxTitleLength = 55
    let createId = AwardId.createWithValue
    let createTitle = AwardTitle.create (AwardTitle.validate maxTitleLength)
    let createAward = Award.create createId createTitle
    let toDto = Award.toDto

module UserRoot =
    let maxUsernameLength = 55
    let createId = UserId.createWithValue
    let createUsername = Username.create (Username.validate maxUsernameLength)
    let createUser = User.create User.validateUser createId createUsername AwardRoot.createAward
    let toDto = User.toDto AwardRoot.toDto
    
module AwardLogicRoot =
    let get = AwardLogic.get AwardRoot.createAward AwardDataSource.get
    let getById = AwardLogic.getById AwardRoot.createAward AwardDataSource.getById
    let add = AwardLogic.add AwardRoot.toDto AwardDataSource.isUnique AwardDataSource.add
    let delete = AwardLogic.delete AwardDataSource.delete AwardDataSource.hasUsers
    
module UserLogicRoot =
    let get = UserLogic.get UserDataSource.get UserRoot.createUser
    let getById = UserLogic.getById UserDataSource.getById UserRoot.createUser
    let add = UserLogic.add UserRoot.toDto UserDataSource.isUnique AwardDataSource.areAwardsInDataSource UserDataSource.add
    let delete = UserLogic.delete UserDataSource.delete