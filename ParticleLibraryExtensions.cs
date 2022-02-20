using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ParticleLibrary
{
  public static class ParticleLibraryExtensions
  {
    public static float InRadians(this float degrees) => MathHelper.ToRadians(degrees);
    public static float InDegrees(this float radians) => MathHelper.ToDegrees(radians);
    public static Rectangle AnimationFrame(this Texture2D texture, ref int frame, ref int frameTick, int frameTime, int frameCount, bool frameTickIncrease, int overrideHeight = 0)
    {
      if (frameTick >= frameTime)
      {
        frameTick = -1;
        frame = frame == frameCount - 1 ? 0 : frame + 1;
      }
      if (frameTickIncrease)
        frameTick++;
      return new Rectangle(0, overrideHeight != 0 ? overrideHeight * frame : (texture.Height / frameCount) * frame, texture.Width, texture.Height / frameCount);
    }
  }
}
