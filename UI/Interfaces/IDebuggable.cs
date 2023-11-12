using ParticleLibrary.UI.Primitives.Complex;

namespace ParticleLibrary.UI.Interfaces
{
    internal interface IDebuggable
    {
        public abstract void DebugRender(Box box, ref float colorIntensity, float alpha);
    }
}
