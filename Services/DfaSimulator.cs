using TocAutomata.Models;

namespace TocAutomata.Services;

public class DfaSimulator
{
    public bool Run(Automaton dfa, string input)
    {
        var current = dfa.StartState;
        foreach(var symbol in input)
        {
            var transition = dfa.Transitions.FirstOrDefault(t => t.From == current && t.Symbol == symbol);
            if(transition == null)
            {
                return false;
            }
            current = transition.To;
        }
        return dfa.States.First(s => s.Name == current).IsFinal;
    }
}