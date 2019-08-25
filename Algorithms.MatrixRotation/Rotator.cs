using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public class Rotator : IRotator
    {
        private readonly IRowExtractor _rowExtractor;
        private readonly IElementShifter _elementShifter;

        public Rotator(IRowExtractor rowExtractor, IElementShifter elementShifter)
        {
            _rowExtractor = rowExtractor;
            _elementShifter = elementShifter;
        }

        public int[][] Rotate(List<List<int>> matrix, int r)
        {
            var m = matrix.Count;
            var n = matrix[0].Count;

            var result = _rowExtractor.Extract(matrix, m,
                n);

            _elementShifter.Shift(result, r);

            return _rowExtractor.ToMatrix(result, m, n);
        }
    }
}