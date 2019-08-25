using Algorithms.MatrixRotation;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests.MatrixRotation
{
    public class DirectionGeneratorTests
    {
        private readonly IDirectionGenerator _directionGenerator = new DirectionGenerator();

        [Theory]
        [InlineData(0, 0, 4, 4, Direction.Down)]
        [InlineData(1, 1, 4, 4, Direction.Down)]
        [InlineData(0, 3, 4, 4, Direction.Right)]
        [InlineData(1, 2, 4, 4, Direction.Right)]
        [InlineData(3, 3, 4, 4, Direction.Up)]
        [InlineData(2, 2, 4, 4, Direction.Up)]
        [InlineData(3, 0, 4, 4, Direction.Left)]
        [InlineData(2, 1, 4, 4, Direction.Left)]
        [InlineData(0, 0, 4, 5, Direction.Down)]
        [InlineData(1, 1, 4, 5, Direction.Down)]
        [InlineData(0, 3, 4, 5, Direction.Right)]
        [InlineData(1, 2, 4, 5, Direction.Right)]
        [InlineData(3, 3, 4, 5, Direction.Right)]
        [InlineData(2, 2, 4, 5, Direction.Right)]
        [InlineData(3, 0, 4, 5, Direction.Left)]
        [InlineData(2, 1, 4, 5, Direction.Left)]
        [InlineData(0, 0, 5, 4, Direction.Down)]
        [InlineData(1, 1, 5, 4, Direction.Down)]
        [InlineData(0, 3, 5, 4, Direction.Down)]
        [InlineData(1, 2, 5, 4, Direction.Down)]
        [InlineData(3, 3, 5, 4, Direction.Up)]
        [InlineData(2, 2, 5, 4, Direction.Up)]
        [InlineData(3, 0, 5, 4, Direction.Left)]
        [InlineData(2, 1, 5, 4, Direction.Left)]
        public void DirectionGenerationTest(int x, int y, int m, int n, Direction result)
        {
            _directionGenerator.Get(x, y, m, n).Should().BeEquivalentTo(result);
        }
    }
}