using System;
using System.Collections.Generic;
using System.Linq;



namespace Algorithms.CountStrings
{
    public class Solution
    {
        private const char Eps = (char)0;
        private static readonly char[] alphabet = { 'a', 'b' };

        private class Transition
        {
            public State State { get; set; }

            public Transition(State state)
            {
                State = state;
            }
        }

        private class State
        {
            private static int nextId = 0;

            public int Id { get; }

            public bool IsFinal { get; set; }

            private readonly Dictionary<char, List<Transition>> transitions = new Dictionary<char, List<Transition>>();

            public State()
            {
                Id = ++nextId;
            }

            public void AddTransition(char letter, State state)
            {
                List<Transition> letterTransitions;
                if (!transitions.TryGetValue(letter, out letterTransitions))
                {
                    letterTransitions = new List<Transition>();
                    transitions[letter] = letterTransitions;
                }

                letterTransitions.Add(new Transition(state));
            }

            public IEnumerable<Transition> GetTransitions()
            {
                return transitions.SelectMany(kv => kv.Value);
            }

            public IEnumerable<Transition> GetTransitions(char letter)
            {
                List<Transition> letterTransitions;
                if (!transitions.TryGetValue(letter, out letterTransitions))
                {
                    return Enumerable.Empty<Transition>();
                }

                return letterTransitions;
            }
        }

        private abstract class Automaton
        {
            public List<State> States { get; } = new List<State>();

            public abstract bool Match(string s);
        }

        private class NFA : Automaton
        {
            public static NFA CreateLetter(char letter)
            {
                var nfa = new NFA();
                var finalState = new State();
                var initialState = new State();
                initialState.AddTransition(letter, finalState);
                nfa.States.Add(initialState);
                nfa.States.Add(finalState);

                return nfa;
            }

            public static NFA CreateStar(NFA child)
            {
                var nfa = new NFA();
                var initialState = new State();
                var finalState = new State();
                initialState.AddTransition(Eps, finalState);
                initialState.AddTransition(Eps, child.States.First());

                child.States.Last().AddTransition(Eps, child.States.First());
                child.States.Last().AddTransition(Eps, finalState);

                nfa.States.Add(initialState);
                nfa.States.AddRange(child.States);
                nfa.States.Add(finalState);

                return nfa;
            }

            public static NFA CreateSequence(NFA left, NFA right)
            {
                var nfa = new NFA();
                foreach (var t in left.States
                    .Take(left.States.Count - 1)
                    .SelectMany(s => s.GetTransitions()))
                {
                    if (t.State == left.States.Last())
                    {
                        t.State = right.States.First();
                    }
                }

                nfa.States.AddRange(left.States.Take(left.States.Count - 1));
                nfa.States.AddRange(right.States);

                return nfa;
            }

            public static NFA CreateOr(NFA left, NFA right)
            {
                var nfa = new NFA();

                var initialState = new State();
                initialState.AddTransition(Eps, left.States.First());
                initialState.AddTransition(Eps, right.States.First());

                var finalState = new State();
                left.States.Last().AddTransition(Eps, finalState);
                right.States.Last().AddTransition(Eps, finalState);

                nfa.States.Add(initialState);
                nfa.States.AddRange(left.States);
                nfa.States.AddRange(right.States);
                nfa.States.Add(finalState);

                return nfa;
            }

            public override bool Match(string s)
            {
                var possibleStates = new HashSet<State>() { States.First() };
                var newPossibleStates = new HashSet<State>();
                var epsTransitions = new List<Transition>();
                var newEpsTransitions = new List<Transition>();

                var index = 0;
                while (true)
                {
                    newPossibleStates.Clear();
                    newPossibleStates.UnionWith(possibleStates);

                    foreach (var possibleState in possibleStates)
                    {
                        epsTransitions.Clear();
                        epsTransitions.AddRange(possibleState.GetTransitions(Eps));
                        while (true)
                        {
                            newEpsTransitions.Clear();
                            foreach (var t in epsTransitions)
                            {
                                if (newPossibleStates.Add(t.State))
                                {
                                    newEpsTransitions.AddRange(t.State.GetTransitions(Eps));
                                }
                            }

                            if (newEpsTransitions.Count == 0)
                            {
                                break;
                            }

                            epsTransitions.Clear();
                            epsTransitions.AddRange(newEpsTransitions);
                        }
                    }

                    possibleStates.Clear();
                    possibleStates.UnionWith(newPossibleStates);

                    if (index == s.Length)
                    {
                        break;
                    }

                    newPossibleStates.Clear();
                    foreach (var possibleState in possibleStates)
                    {
                        var letterTransitions = possibleState.GetTransitions(s[index]);
                        foreach (var t in letterTransitions)
                        {
                            newPossibleStates.Add(t.State);
                        }
                    }

                    possibleStates.Clear();
                    possibleStates.UnionWith(newPossibleStates);

                    ++index;
                }

                return possibleStates.Contains(States.Last());
            }

            private HashSet<State> GetEpsTransitions(State state)
            {
                var possibleStates = new HashSet<State>() { state };
                var newPossibleStates = new HashSet<State>();
                var epsTransitions = new List<Transition>();
                var newEpsTransitions = new List<Transition>();

                newPossibleStates.Clear();
                newPossibleStates.UnionWith(possibleStates);

                foreach (var possibleState in possibleStates)
                {
                    epsTransitions.Clear();
                    epsTransitions.AddRange(possibleState.GetTransitions(Eps));
                    while (true)
                    {
                        newEpsTransitions.Clear();
                        foreach (var t in epsTransitions)
                        {
                            if (newPossibleStates.Add(t.State))
                            {
                                newEpsTransitions.AddRange(t.State.GetTransitions(Eps));
                            }
                        }

                        if (newEpsTransitions.Count == 0)
                        {
                            break;
                        }

                        epsTransitions.Clear();
                        epsTransitions.AddRange(newEpsTransitions);
                    }
                }

                possibleStates.Clear();
                possibleStates.UnionWith(newPossibleStates);

                return possibleStates;
            }

            public DFA ToDFA()
            {
                var dfaStates = new List<Tuple<State, HashSet<State>>> { Tuple.Create(new State(), GetEpsTransitions(States.First())) };
                var currentStates = new List<Tuple<State, HashSet<State>>> { dfaStates.First() };
                var newStates = new List<Tuple<State, HashSet<State>>>();
                var nextStates = new HashSet<State>();

                while (true)
                {
                    foreach (var currentState in currentStates)
                    {
                        foreach (var letter in alphabet)
                        {
                            nextStates.Clear();

                            foreach (var nfaState in currentState.Item2)
                            {
                                var letterTransitions = nfaState.GetTransitions(letter);
                                foreach (var transition in letterTransitions)
                                {
                                    nextStates.Add(transition.State);
                                    nextStates.UnionWith(GetEpsTransitions(transition.State));
                                }
                            }

                            var newState = dfaStates.FirstOrDefault(s => s.Item2.SetEquals(nextStates))?.Item1;
                            if (newState == null)
                            {
                                newState = new State();
                                dfaStates.Add(Tuple.Create(newState, new HashSet<State>(nextStates)));
                                newStates.Add(dfaStates.Last());
                            }

                            if (!currentState.Item1.GetTransitions(letter).Any())
                            {
                                currentState.Item1.AddTransition(letter, newState);
                            }
                        }
                    }

                    if (!newStates.Any())
                    {
                        break;
                    }

                    currentStates.Clear();
                    currentStates.AddRange(newStates);
                    newStates.Clear();
                }

                var dfa = new DFA();
                foreach (var dfaState in dfaStates)
                {
                    if (dfaState.Item2.Contains(States.Last()))
                    {
                        dfaState.Item1.IsFinal = true;
                    }

                    dfa.States.Add(dfaState.Item1);
                }

                return dfa;
            }
        }

        private class DFA : Automaton
        {
            public override bool Match(string s)
            {
                var currentState = States.First();
                foreach (var letter in s)
                {
                    var transition = currentState.GetTransitions(letter).FirstOrDefault();
                    if (transition == null)
                    {
                        return false;
                    }

                    currentState = transition.State;
                }

                return currentState.IsFinal;
            }

            public SquareMatrix GetAdjacencyMatrix()
            {
                var m = new SquareMatrix(States.Count);
                for (var i = 0; i < States.Count; ++i)
                {
                    for (var j = 0; j < States.Count; ++j)
                    {
                        m[i, j] = (ulong)States[i].GetTransitions().Count(t => t.State == States[j]);
                    }
                }

                return m;
            }
        }

        private static NFA ParseRegex(string r)
        {
            NFA left;
            NFA right;

            int endPos;
            if (r[1] == '(')
            {
                endPos = FindClosingBracket(r, 1);
                left = ParseRegex(r.Substring(1, endPos));
            }
            else
            {
                endPos = 1;
                left = NFA.CreateLetter(r[1]);
            }

            var beginPos = endPos + 1;
            switch (r[beginPos])
            {
                case '*':
                    return NFA.CreateStar(left);

                case '|':
                    if (r[beginPos + 1] == '(')
                    {
                        endPos = FindClosingBracket(r, beginPos + 1);
                        right = ParseRegex(r.Substring(beginPos + 1, endPos - beginPos));
                    }
                    else
                    {
                        right = NFA.CreateLetter(r[beginPos + 1]);
                    }

                    return NFA.CreateOr(left, right);

                case '(':
                    endPos = FindClosingBracket(r, beginPos);
                    right = ParseRegex(r.Substring(beginPos, endPos - beginPos + 1));
                    return NFA.CreateSequence(left, right);

                default:
                    right = NFA.CreateLetter(r[beginPos]);
                    return NFA.CreateSequence(left, right);
            }
        }

        private static int FindClosingBracket(string s, int openIndex)
        {
            var openedBrackets = 0;
            for (var i = openIndex; i < s.Length; ++i)
            {
                if (s[i] == '(')
                {
                    ++openedBrackets;
                }
                else if (s[i] == ')')
                {
                    --openedBrackets;
                    if (openedBrackets == 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private class SquareMatrix
        {
            private readonly int n;

            private readonly ulong[] values;

            public SquareMatrix(int n)
            {
                this.n = n;
                values = new ulong[n * n];
            }

            public SquareMatrix(SquareMatrix other)
                : this(other.n)
            {
                Array.Copy(other.values, values, n * n);
            }

            public static SquareMatrix operator *(SquareMatrix lhs, SquareMatrix rhs)
            {
                var m = new SquareMatrix(lhs.n);
                for (var i = 0; i < lhs.n; ++i)
                {
                    for (var j = 0; j < lhs.n; ++j)
                    {
                        for (var k = 0; k < lhs.n; ++k)
                        {
                            m[i, j] += (lhs[i, k] * rhs[k, j]) % 1000000007;
                            m[i, j] %= 1000000007;
                        }
                    }
                }

                return m;
            }

            public ulong this[int i, int j]
            {
                get { return values[i * n + j]; }
                set { values[i * n + j] = value; }
            }

            public SquareMatrix Exponentiate(int k)
            {
                if (k == 1)
                {
                    return this;
                }

                var square = this * this;
                if ((k % 2) == 0)
                {
                    return square.Exponentiate(k / 2);
                }

                return this * square.Exponentiate((k - 1) / 2);
            }
        }

        static void Main(string[] args)
        {
            var t = int.Parse(Console.ReadLine());
            while (t-- > 0)
            {
                var line = Console.ReadLine().Split(' ');
                var r = line[0];
                var stringLength = int.Parse(line[1]);

                var nfa = ParseRegex(r);
                var dfa = nfa.ToDFA();

                var m = dfa.GetAdjacencyMatrix();
                var result = m.Exponentiate(stringLength);

                var count = 0UL;
                for (var i = 0; i < dfa.States.Count; ++i)
                {
                    if (dfa.States[i].IsFinal)
                    {
                        count += result[0, i];
                        count %= 1000000007;
                    }
                }

                Console.WriteLine(count);
            }
        }

        public static ulong Test(string input, int stringLength){

                var nfa = ParseRegex(input);
                var dfa = nfa.ToDFA();

                var m = dfa.GetAdjacencyMatrix();
                var result = m.Exponentiate(stringLength);

                var count = 0UL;
                for (var i = 0; i < dfa.States.Count; ++i)
                {
                    if (dfa.States[i].IsFinal)
                    {
                        count += result[0, i];
                        count %= 1000000007;
                    }
                }

                return count;
        }
    }
}