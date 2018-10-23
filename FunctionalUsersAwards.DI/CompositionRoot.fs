module CompositionRoot
open AdditionalTypes
open MainTypes
open FunctionalUsersAwards.EfContext
open Microsoft.EntityFrameworkCore


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
    
module DataSourceRoot =
    let mutable opts = lazy Unchecked.defaultof<DbContextOptions>
    let getContext() = new UserAwardDbContext(opts.Force())
    
module AwardDataSourceRoot =
    open FunctionalUsersAwards.EfContext
    
    let add = FunctionalUsersAwards.EfDataSource.AwardDataSource.add DataSourceRoot.getContext
    let get = FunctionalUsersAwards.EfDataSource.AwardDataSource.get DataSourceRoot.getContext
    let getById = FunctionalUsersAwards.EfDataSource.AwardDataSource.getById DataSourceRoot.getContext
    let isUnique = FunctionalUsersAwards.EfDataSource.AwardDataSource.isUnique DataSourceRoot.getContext
    let delete = FunctionalUsersAwards.EfDataSource.AwardDataSource.delete DataSourceRoot.getContext
    let areAwardsInDataSource = FunctionalUsersAwards.EfDataSource.AwardDataSource.areAwardsInDataSource DataSourceRoot.getContext
        
module AwardLogicRoot =
    let get = AwardLogic.get AwardRoot.createAward AwardDataSourceRoot.get
    let getById = AwardLogic.getById AwardRoot.createAward AwardDataSourceRoot.getById
    let add = AwardLogic.add AwardRoot.toDto AwardDataSourceRoot.isUnique AwardDataSourceRoot.add
    let delete = AwardLogic.delete AwardDataSourceRoot.delete UserAwardDataSource.hasUsers
    
module UserLogicRoot =
    let get = UserLogic.get UserDataSource.get UserRoot.createUser
    let getById = UserLogic.getById UserDataSource.getById UserRoot.createUser
    let add = UserLogic.add UserRoot.toDto UserDataSource.isUnique AwardDataSource.areAwardsInDataSource UserDataSource.add
    let delete = UserLogic.delete UserDataSource.delete
    
module UserAwardLogicRoot =
    let addAwardsToUser = UserAwardLogic.addAwardToUser UserAwardDataSource.addAwardsToUser UserDataSource.exists AwardDataSource.areAwardsInDataSource