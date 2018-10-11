namespace FunctionalUsersAwards.RestUI.Controllers

open System
open Microsoft.AspNetCore.Mvc
open CompositionRoot
open CompositionRoot
open Dto

[<Route("api/users")>]
[<ApiController>]
type UsersController() =
    inherit ControllerBase()
    
    [<HttpGet>]
    [<Route("")>]
    member this.Get() = match UserLogicRoot.get() with
                        | Ok users -> this.Ok(users |> List.map UserRoot.toDto) :> IActionResult
                        | Error err -> this.Conflict(err) :> IActionResult
                        
    [<HttpGet>]
    [<Route("id")>]
    member this.GetById(id) = match UserLogicRoot.getById id with
                              | Some user -> match user with
                                             | Ok user -> this.Ok(user |> UserRoot.toDto) :> IActionResult
                                             | Error err -> this.Conflict(err) :> IActionResult
                              | None -> this.NotFound() :> IActionResult
                              
    [<HttpPost>]
    [<Route("add")>]
    member this.Add(user : UserDto) = match { user with Id = Guid.NewGuid(); Awards = if box user.Awards = null then [] else user.Awards} |> UserRoot.createUser with
                                      | Ok user -> match UserLogicRoot.add user with
                                                   | Ok user -> this.Ok(user) :> IActionResult
                                                   | Error err -> this.Conflict(err) :> IActionResult
                                      | Error err -> this.Conflict(err) :> IActionResult
    
    [<HttpDelete>]
    [<Route("delete/{id}")>]
    member this.Delete(id) = match UserLogicRoot.delete id with
                             | Some id -> this.Ok() :> IActionResult
                             | None -> this.NotFound() :> IActionResult