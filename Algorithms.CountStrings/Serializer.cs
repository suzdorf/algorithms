namespace Algorithms.CountStrings
{
    public class Serializer
    {
        public Node Serialize(string input)
        {
            Node current = null;

            foreach (var ch in input)
            {
                switch (ch)
                {
                    case Symbols.LeftBracket:
                        var newNode = new Node();

                        if (current == null){
                            current = newNode;
                        }else{
                            current.Content.Add(newNode);
                            newNode.Parent = current;
                            current = newNode;
                        }
                        break;
                    case Symbols.RightBracket:
                        if (current.Parent!=null){
                            current = current.Parent;
                        }
                        break;
                    default:
                        current.Content.Add(new Char { Value = ch} );
                        break;
                }
            }

            return current;
        }
    }
}