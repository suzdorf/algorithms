using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public interface IRotator
    {
        int[][] Rotate(List<List<int>> matrix, int r);
    }
}