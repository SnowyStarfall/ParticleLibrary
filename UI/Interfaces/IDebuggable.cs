using ParticleLibrary.UI.Primitives.Shapes;

namespace ParticleLibrary.UI.Interfaces
{
    internal interface IDebuggable
    {
        public abstract void DebugRender(PrimRectangle rectangle, float alpha);
    }
}
