using System.Collections.Generic;
using Algorithms.MatrixRotation;
using FluentAssertions;
using Moq;
using Xunit;

namespace Algorithms.Tests.MatrixRotation
{
    public class RawExtractorTests
    {
        private readonly Mock<IDirectionGenerator> _directionGeneratorMock = new Mock<IDirectionGenerator>();
        private readonly IRowExtractor _rowExtractor;

        public RawExtractorTests()
        {
            _rowExtractor = new RowExtractor(_directionGeneratorMock.Object);
        }

        [Fact]
        public void ExtractRowsTest()
        {
            var matrix = new List<List<int>>
            {
                new List<int> {1, 2},
                new List<int> {3, 4}
            };

            const int m = 2;
            const int n = 2;

            _directionGeneratorMock.Setup(x => x.Get(0, 0, m, n)).Returns(Direction.Down);
            _directionGeneratorMock.Setup(x => x.Get(0, 1, m, n)).Returns(Direction.Right);
            _directionGeneratorMock.Setup(x => x.Get(1, 1, m, n)).Returns(Direction.Up);
            _directionGeneratorMock.Setup(x => x.Get(1, 0, m, n)).Returns(Direction.Left);

            var result = _rowExtractor.Extract(matrix, m, n);

            result.Count.Should().Be(1);
            result[0].Count.Should().Be(4);
            result[0][0].Value.Should().Be(1);
            result[0][1].Value.Should().Be(3);
            result[0][2].Value.Should().Be(4);
            result[0][3].Value.Should().Be(2);
        }

        [Fact]
        public void ConvertRowsToMatrixTest()
        {
            const int m = 2;
            const int n = 2;

            _directionGeneratorMock.Setup(x => x.Get(0, 0, m, n)).Returns(Direction.Down);
            _directionGeneratorMock.Setup(x => x.Get(0, 1, m, n)).Returns(Direction.Right);
            _directionGeneratorMock.Setup(x => x.Get(1, 1, m, n)).Returns(Direction.Up);
            _directionGeneratorMock.Setup(x => x.Get(1, 0, m, n)).Returns(Direction.Left);

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

            var result = _rowExtractor.ToMatrix(rows, m, n);

            result[0][0].Should().Be(1);
            result[0][1].Should().Be(2);
            result[1][0].Should().Be(3);
            result[1][1].Should().Be(4);
        }
    }
}