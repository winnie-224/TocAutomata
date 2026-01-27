using System.Collections.Generic;

namespace TocAutomata.Models;

public class Automaton
{
    public List<State> States { get; set; }
    public List<Transition> Transitions { get; set; }
    public string StartState { get; set; }
}