using System.Collections.Generic;
using Algorithms.MatrixRotation;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests.MatrixRotation
{
    public class ElementShifterTests
    {
        private readonly IElementShifter _elementShifter = new ElementShifter();

        [Fact]
        public void ShiftElementsTest()
        {
            var rows = new List<INode<int>>
            {
                new Node<int>
                {
                    Value = 1,
                    Next = new Node<int>
                    {
                        Value = 3,
                        Next = new Node<int>
                        {
                            Value = 4,
                            Next = new Node<int>
                            {
                                Value = 2,
                                Next = null
                            }
                        }
                    }
                }
            };

            _elementShifter.Shift(rows, 2);
            rows[0][0].Value.Should().Be(4);
            rows[0][1].Value.Should().Be(2);
            rows[0][2].Value.Should().Be(1);
            rows[0][3].Value.Should().Be(3);
        }
    }
}