namespace FunctionalUsersAwards.RestUI.Controllers

open CompositionRoot
open Microsoft.AspNetCore.Mvc
open System

type UserAwardsMapping = { UserId: Guid; AwardsIds: Guid list}


[<Route("api/users-awards")>]
[<ApiController>]
type UsersAwardsController() =
    inherit ControllerBase()
        
    [<HttpPut>]
    [<Route("add-awards-to-user")>]
    member this.addAwardsToUser(userAwardsMapping) = match UserAwardLogicRoot.addAwardsToUser userAwardsMapping.UserId userAwardsMapping.AwardsIds with
                                                     | Ok _ -> this.Ok() :> IActionResult
                                                     | Error err -> this.BadRequest(err) :> IActionResult