namespace Algorithms.MatrixRotation
{
    public interface INode<T>
    {
        T Value { get; }
        INode<T> Next { get; set;}
        int Count {get; }
        INode<T> this[int index] 
        { 
            get;
        } 
    }
}