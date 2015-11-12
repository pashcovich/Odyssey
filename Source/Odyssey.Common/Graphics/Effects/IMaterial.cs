using SharpDX;

namespace Odyssey.Graphics.Effects
{
    public interface IMaterial
    {
        float AmbientCoefficient { get;  }
        float DiffuseCoefficient { get;  }
        float SpecularCoefficient { get;  }
        float SpecularPower { get;  }
        Color4 Ambient { get;  }
        Color4 Diffuse { get;  }
        Color4 Specular { get;  }
    }
}