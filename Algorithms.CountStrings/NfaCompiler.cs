using System.Collections.Generic;
using System.Linq;

namespace Algorithms.CountStrings
{
    public class NfaCompiler
    {
        public NfaEntity Compile(Node node)
        {
            var result = new NfaEntity{
                Start = new State(),
                End = new State()
            };

            var current = result.Start;
            var starTransition = new List<Transition>();

            foreach (var content in node.Content)
            {
                if (content is Char character)
                {
                    switch (character.Value)
                    {
                        case Symbols.Star:
                            current.Transitions.AddRange(starTransition);
                            break;
                        case Symbols.VerticalBar:
                            result.Last.State = result.End;
                            result.Last = null;
                            current = result.Start;
                            break;
                        default:
                            result.Last = new Transition { State = new State(), Value = character.Value };
                            current.Transitions.Add(result.Last);
                            current = result.Last.State;
                            starTransition = new List<Transition> { result.Last };
                            break;
                    }
                }
                else
                {
                    var innerNfa = Compile(content as Node);

                    if (result.Last != null){
                        result.Last.State.Transitions.AddRange(innerNfa.Start.Transitions);
                    }
                    else
                    {
                        if (result.Start.Transitions.Any())
                        {
                            result.Start.Transitions.AddRange(innerNfa.Start.Transitions);
                        }
                        else
                        {
                            result.Start = innerNfa.Start;
                        }
                    }

                    starTransition = innerNfa.Start.Transitions;
                    result.Last = innerNfa.Last;

                    current = innerNfa.End;
                }
            }

            result.End = current;

            return result;
        }
    }
}