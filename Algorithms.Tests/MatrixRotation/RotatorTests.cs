using System.Collections.Generic;
using Algorithms.MatrixRotation;
using FluentAssertions;
using Moq;
using Xunit;

namespace Algorithms.Tests.MatrixRotation
{
    public class RotatorTests
    {
        private readonly Mock<IRowExtractor> _rowExtractorMock = new Mock<IRowExtractor>();
        private readonly Mock<IElementShifter> _elementShifterMock =  new Mock<IElementShifter>();
        private readonly IRotator _rotator;
        private readonly List<List<int>> _matrix = new List<List<int>> {new List<int>()};
        private readonly List<INode<int>> _rows =  new List<INode<int>>();
        private readonly int[][] _rotationResult = new int[1][];

        public RotatorTests()
        {
            _rowExtractorMock.Setup(x => x.Extract(_matrix, 1, 0)).Returns(_rows);
            _rowExtractorMock.Setup(x => x.ToMatrix(_rows, 1, 0)).Returns(_rotationResult);
            _rotator = new Rotator(_rowExtractorMock.Object, _elementShifterMock.Object);
        }

        [Fact]
        public void RotateTest()
        {
            var result = _rotator.Rotate(_matrix, 1);
            result.Should().BeEquivalentTo(_rotationResult);
            _rowExtractorMock.Verify(x=>x.Extract(_matrix, 1, 0), Times.Once);
            _rowExtractorMock.Verify(x => x.ToMatrix(_rows, 1, 0), Times.Once);
            _elementShifterMock.Verify(x=>x.Shift(_rows, 1), Times.Once);
        }
    }
}