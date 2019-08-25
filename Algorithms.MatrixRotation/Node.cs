namespace Algorithms.MatrixRotation
{
    public class Node<T> : INode<T>
    {
        public T Value { get; set; }
        public INode<T> Next { get; set; }
        public int Count => 1 + (Next == null ? 0 : Next.Count);
        public INode<T> this[int index] 
        { 
            get{
                if (index == 0) return this;

                INode<T> result = this;

                for (var i = 1; i<= index; i++){
                    result = result.Next;
                }

                return result;
            }
        } 
    }
}