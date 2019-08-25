using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public interface IRowExtractor
    {
        List<INode<int>> Extract(List<List<int>> matrix, int m, int n);
        int[][] ToMatrix(List<INode<int>> rows, int m, int n);
    }
}