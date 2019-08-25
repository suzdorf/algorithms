using System;
using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public class ElementShifter : IElementShifter
    {
        public void Shift(List<INode<int>> rows, int r)
        {
            for (var i=0; i< rows.Count; i++){
                var row = rows[i];
                var count = rows[i].Count;
                var shiftNumber = r > count ? r % count : r;

                if (r == count || shiftNumber == 0){
                    return;
                }

                rows[i] = row[count - shiftNumber];
                row[count - 1].Next = row[0];
                row[count - shiftNumber - 1].Next = null;
            }
        }
    }
}