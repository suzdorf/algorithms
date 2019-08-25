using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public interface IElementShifter
    {
        void Shift(List<INode<int>> rows, int r);
    }
}