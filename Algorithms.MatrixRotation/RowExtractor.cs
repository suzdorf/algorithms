using System.Collections.Generic;

namespace Algorithms.MatrixRotation
{
    public class RowExtractor : IRowExtractor
    {
        private readonly IDirectionGenerator _directionGenerator;

        public RowExtractor(IDirectionGenerator directionGenerator)
        {
            _directionGenerator = directionGenerator;
        }

        public List<INode<int>> Extract(List<List<int>> matrix, int m, int n)
        {
            var result = new List<INode<int>>();
            Node<int> lastNode = null;

            for (var i = 0; i < m / 2 && i < n / 2 ; i++)
            {
                var x = i;
                var y = i;
                var started = false;

                while (!started || !(x == i && y == i))
                {
                    var nextNode = new Node<int> { Value = matrix[y][x] };

                    if (!started){
                        result.Add(nextNode);
                        started = true;
                    }
                    else{
                        lastNode.Next = nextNode;
                    }
                    
                    lastNode = nextNode;

                    var direction = _directionGenerator.Get(x, y, m, n);

                    switch (direction)
                    {
                        case Direction.Up:
                            y--;
                            break;
                        case Direction.Down:
                            y++;
                            break;
                        case Direction.Left:
                            x--;
                            break;
                        case Direction.Right:
                            x++;
                            break;
                        case Direction.None:
                            break;
                    }
                }
            }


            return result;
        }

        public int[][] ToMatrix(List<INode<int>> rows, int m, int n)
        {
           var result = new int[m][];

           for (var i = 0; i<m; i++){
               result[i] = new int[n];
           }

           var j = 0;
           foreach (var row in rows)
           {
               var x = j;
               var y = j;
               var element = row;

               while (element != null){
                    result[y][x] = element.Value;
                    var direction = _directionGenerator.Get(x, y, m, n);

                    switch (direction)
                    {
                        case Direction.Up:
                            y--;
                            break;
                        case Direction.Down:
                            y++;
                            break;
                        case Direction.Left:
                            x--;
                            break;
                        case Direction.Right:
                            x++;
                            break;
                        case Direction.None:
                            break;
                    }

                    element = element.Next;
               }

               j++;
           }

           return result;
        }
    }
}
