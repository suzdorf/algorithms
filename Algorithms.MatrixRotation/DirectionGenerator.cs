namespace Algorithms.MatrixRotation
{
    public class DirectionGenerator : IDirectionGenerator
    {
        public Direction Get(int x, int y, int m, int n)
        {
            var z = m - n;

            if (x > y && x + y <= n - 1 && y <= m / 2 - 1)
            {
                return Direction.Left;
            }

            if (x <= y && x + y < m - 1 && x <= n / 2 - 1)
            {
                return Direction.Down;
            }

            if (x >= y - z && x + y > n - 1 && x >= n / 2 - 1)
            {
                return Direction.Up;
            }

            if (x < y - z && x + y >= m - 1 && y >= m / 2 - 1)
            {
                return Direction.Right;
            }

            return Direction.None;
        }
    }
}