﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticleLibrary.Core
{
    public abstract class GParticle
    {
        public abstract GRenderQuad Quad { get; set; }
    }
}