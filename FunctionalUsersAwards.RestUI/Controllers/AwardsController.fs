namespace FunctionalUsersAwards.Controllers
open System
open CompositionRoot
open Dto
open FunctionalUsersAwards.EfContext
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
                              
    [<Route("{id}")>]
    [<HttpGet>]
    member this.GetAward(id) = match AwardLogicRoot.getById id with
                               | Some award -> match award with
                                               | Ok award -> this.Ok(award |> AwardRoot.toDto) :> IActionResult
                                               | Error err -> this.Conflict(err) :> IActionResult
                               | None -> this.NotFound() :> IActionResult


    [<Route("add")>]
    [<HttpPost>]                                
    member this.AddAward(award: AwardDto) = match AwardRoot.createAward { award with Id = Guid.NewGuid() } with
                                            | Ok award -> match AwardLogicRoot.add award with
                                                          | Ok dto -> this.Ok(award |> AwardRoot.toDto) :> IActionResult
                                                          | Error err -> this.Conflict(err) :> IActionResult
                                            | Error err -> this.Conflict(err) :> IActionResult
                                            
    [<Route("delete/{id}")>]
    [<HttpDelete>]
    member this.Delete(id) = match AwardLogicRoot.delete id with
                             | Ok id -> this.Ok() :> IActionResult
                             | Error err -> this.Conflict(err) :> IActionResult