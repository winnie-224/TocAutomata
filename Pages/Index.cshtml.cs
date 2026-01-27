using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TocAutomata.Models;
using TocAutomata.Services;



public class IndexModel : PageModel
{
    public string? Result { get; set; }
    public string? Error { get; set; }
    public List<State> StatesList { get; set; } = new();
    public List<Transition> TransitionsList { get; set; } = new();
    public List<char> AlphabetList { get; set; } = new();
    public string? ModelStartState { get; set; }
    public string GetTransition(string from, char symbol)
    {
        return TransitionsList
            .FirstOrDefault(t => t.From == from && t.Symbol == symbol)
            ?.To ?? "-";
    }

    public void OnPost(
        string States,
        string StartState,
        string FinalStates,
        string Transitions,
        string Input
        )
    {
        try
        {

            if (string.IsNullOrWhiteSpace(States))
            {
                throw new Exception("States can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(StartState))
            {
                throw new Exception("Start state can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(Transitions))
            {
                throw new Exception("Transitions can not be empty.");
            }

            var stateList = States.Split(',').Select(s => s.Trim()).Where(s=>s!="").ToList();
            var finalList = FinalStates.Split(',').Select(s => s.Trim()).ToList();
            if (!stateList.Contains(StartState.Trim()))
            {
                throw new Exception("Start state must be in States list.");
            }
            var states = stateList.Select(s => new State
            {
                Name = s,
                IsFinal = finalList.Contains(s)
            }).ToList();
            //Parse transitions
            var transitionLines = Transitions.Split('\n');
            var transitions = new List<Transition>();
            foreach (var line in transitionLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var t = line.Split(',');
                if (t.Length != 3)
                {
                    throw new Exception($"Invalid tranisition format:{line}");
                }
                var from = t[0].Trim();
                var symbol = t[1].Trim();
                var to = t[2].Trim();  
                if(symbol.Length != 1)
                {
                    throw new Exception($"Symbol must be 1 character:{line}");
                }
                if(!stateList.Contains(from) || !stateList.Contains(to))
                {
                    throw new Exception($"Transition uses unknown state:{line}");
                }
                transitions.Add(new Transition
                {
                    From = from,
                    Symbol = symbol[0],
                    To = to
                });
            }
            //Build automaton
            var dfa = new Automaton
            {
                StartState = StartState.Trim(),
                States = states,
                Transitions = transitions
            };

            StatesList = states;
            TransitionsList = transitions;
            AlphabetList = transitions.Select(t => t.Symbol).Distinct().ToList();
            ModelStartState = StartState;

            //Run dfa
            var sim = new DfaSimulator();
            Result = sim.Run(dfa, Input) 
                ? "Accepted" 
                : "Rejected";
          

            Error = null;
        }
        catch (Exception e)
        {
            Error = e.Message;
            Result = null;
        }
    }
}