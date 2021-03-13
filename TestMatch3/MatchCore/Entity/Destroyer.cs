
namespace MatchCore
{
    public class Destroyer:VisualObject
    {
        public DestroyerType Type { get; set; }
        public Destroyer(RendererData data) : base(data)
        {
            Enabled = true;
        }
    }
}