
namespace Algorithms.CountStrings
{
    public class NfaEntity
    {
        public State Start { get; set; }
        public State End { get; set; }
        public Transition Last { get; set; }
    }
}