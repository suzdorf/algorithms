using System.Collections.Generic;
using System.Linq;
using Algorithms.CountStrings;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class UnitTest2
    {
        private readonly Serializer _serializer = new Serializer();
        private readonly NfaCompiler _compiler = new NfaCompiler();

        [Fact]
        public void Test1()
        {
           Solution.Test("((ab)|(ba))", 2);
        }

        [Fact]
        public void SequentialRegexpTest()
        {
            const string regExp = "(ab)";

            var node = _serializer.Serialize(regExp);
            var nfa = _compiler.Compile(node);

            var firstStateTran = nfa.Start.Transitions;
            var secondStateTran = firstStateTran.First().State.Transitions;

            firstStateTran.Count.Should().Be(1);
            firstStateTran.First().Value.Should().Be('a');
            secondStateTran.Count.Should().Be(1);
            secondStateTran.First().Value.Should().Be('b');
            secondStateTran.First().State.Should().BeEquivalentTo(nfa.End);
        }

        [Fact]
        public void StarRegexpTest()
        {
            const string regExp = "((b*)(ab)*)";

            var node = _serializer.Serialize(regExp);
            var nfa = _compiler.Compile(node);

            nfa.Start.Transitions.Count.Should().Be(1);
            nfa.Start.Transitions[0].Value.Should().Be('b');
            nfa.Start.Transitions[0].State.Transitions.Count.Should().Be(2);
            nfa.Start.Transitions[0].State.Transitions[0].Should().BeEquivalentTo(nfa.Start.Transitions[0]);
            nfa.Start.Transitions[0].State.Transitions[1].Value.Should().Be('a');
            nfa.Start.Transitions[0].State.Transitions[1].State.Transitions.Count.Should().Be(1);
            nfa.Start.Transitions[0].State.Transitions[1].State.Transitions[0].Value.Should().Be('b');
            nfa.Start.Transitions[0].State.Transitions[1].State.Transitions[0].State.Transitions.Count.Should().Be(1);
            nfa.Start.Transitions[0].State.Transitions[1].State.Transitions[0].State.Transitions[0].Should().BeEquivalentTo(nfa.Start.Transitions[0].State.Transitions[1]);
            nfa.Start.Transitions[0].State.Transitions[1].State.Transitions[0].State.Should().BeEquivalentTo(nfa.End);
        }

        [Fact]
        public void ParallelRegExpTest()
        {
            const string regExp = "((ab)|(ba))";

            var node = _serializer.Serialize(regExp);
            var nfa = _compiler.Compile(node);

            nfa.Start.Transitions.Count.Should().Be(2);
            nfa.Start.Transitions[0].Value.Should().Be('a');
            nfa.Start.Transitions[0].State.Transitions.Count.Should().Be(1);
            nfa.Start.Transitions[0].State.Transitions[0].Value.Should().Be('b');
            nfa.Start.Transitions[0].State.Transitions[0].State.Should().BeEquivalentTo(nfa.End);
            nfa.Start.Transitions[1].Value.Should().Be('b');
            nfa.Start.Transitions[1].State.Transitions.Count.Should().Be(1);
            nfa.Start.Transitions[1].State.Transitions[0].Value.Should().Be('a');
            nfa.Start.Transitions[1].State.Transitions[0].State.Should().BeEquivalentTo(nfa.End);
        }
    }
}