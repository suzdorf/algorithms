namespace Algorithms.MatrixRotation
{
    public interface IDirectionGenerator
    {
        Direction Get(int x, int y, int m, int n);
    }
}