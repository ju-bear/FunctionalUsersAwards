namespace FunctionalUsersAwards.Controllers
open System
open CompositionRoot
open Dto
open Microsoft.AspNetCore.Mvc

[<Route("api/awards")>]
[<ApiController>]
type AwardsController() = 
    inherit ControllerBase()
    
    [<Route("")>]    
    [<HttpGet>]
    member this.GetAwards() = match AwardLogicRoot.get() with
                              | Ok awards -> this.Ok(awards |> List.map AwardRoot.toDto) :> IActionResult
                              | Error err -> this.Conflict(err) :> IActionResult
              
    [<Route("add")>]
    [<HttpPost>]                                
    member this.AddAward(award: AwardDto) = match AwardRoot.createAward { award with Id = Guid.NewGuid() } with
                                            | Ok award -> match AwardLogicRoot.add award with
                                                          | Ok dto -> this.Ok(award |> AwardRoot.toDto) :> IActionResult
                                                          | Error err -> this.Conflict(err) :> IActionResult
                                            | Error err -> this.Conflict(err) :> IActionResult