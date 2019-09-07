using System.Collections.Generic;

namespace Algorithms.CountStrings
{
    public class Node : IContent
    {
        public Node Parent { get; set; }
        public List<IContent> Content { get; } =  new List<IContent>();
    }
}