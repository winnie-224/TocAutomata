using System;
using Microsoft.AspNetCore.Mvc;
using TocAutomata.Models;
using TocAutomata.Services;

namespace TocAutomata.Controllers;


[ApiController]
[Route("api/automata")]
public class AutomataController : ControllerBase
{
    [HttpPost("dfa/run")]
    public IActionResult RunDfa([FromBody] AutomatonRequest request)
    {
        var simulator = new DfaSimulator();
        bool result = simulator.Run(request.Automaton, request.Input);

        return Ok(new { accepted = result });
    }
}
