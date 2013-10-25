using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    public interface ICameraProvider
    {
        Matrix World {get;}
        Matrix View {get;}
        Matrix Projection { get; }
        Matrix WorldViewProjection { get; }
        Viewport Viewport { get; }
    }

    public interface IStereoCameraProvider : ICameraProvider
    {
        Matrix LeftProjection { get; }
        Matrix RightProjection { get; }
    }
}
