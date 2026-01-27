using System;
namespace TocAutomata.Models;

public class AutomatonRequest
{
    public Automaton Automaton { get; set; }
    public string Input { get; set; }
}
