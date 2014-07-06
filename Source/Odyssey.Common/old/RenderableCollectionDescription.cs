#region #Disclaimer

// /*
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  *
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/
//  * You can alter and use this source code as you wish,
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion #Disclaimer

#region Using directives

using SharpDX.Direct3D11;
using System;


#endregion Using directives

namespace Odyssey.Graphics.Rendering.SceneGraph
{
    public class RenderableCollectionDescription : IComparable<RenderableCollectionDescription>
    {
        public RenderingOrderType RenderingOrderType { get; internal set; }
        public InstanceSemantic InstanceSemantics { get; internal set; }
        public BlendStateDescription BlendState { get; internal set; }
        public RasterizerStateDescription RasterizerState { get; internal set; }
        public DepthStencilStateDescription DepthStencilState { get; internal set; }
        public ShadowMappingDescription ShadowMapping { get; internal set; }

        public RenderableCollectionDescription()
        {
            RenderingOrderType = Rendering.RenderingOrderType.OpaqueGeometry;
            InstanceSemantics = InstanceSemantic.None;
            SharpDX.Direct3D11.RasterizerState = new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = true,
                IsFrontCounterClockwise = true,
                IsMultisampleEnabled = true,
                DepthBias = 100,
                DepthBiasClamp = 0.0f,
                SlopeScaledDepthBias = 1.0f,
            };
            RenderTargetBlendDescription rTargetBlendDesc = new RenderTargetBlendDescription()
            {
                IsBlendEnabled = false,
                RenderTargetWriteMask = ColorWriteMaskFlags.All
            };
            BlendStateDescription bStateDescription = new BlendStateDescription();
            bStateDescription.RenderTarget[0] = rTargetBlendDesc;

            SharpDX.Direct3D11.BlendState = bStateDescription;
            DepthStencilState = new DepthStencilStateDescription
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.LessEqual,
            };
            ShadowMapping = ShadowMappingDescription.NoShadows;
        }

        #region IComparable<RenderableCollectionDescription> Members

        public int CompareTo(RenderableCollectionDescription other)
        {
            if (RenderingOrderType == RenderingOrderType.OpaqueGeometry &&
                other.RenderingOrderType != RenderingOrderType.OpaqueGeometry)
                return -1;
            if (RenderingOrderType != RenderingOrderType.OpaqueGeometry &&
                other.RenderingOrderType == RenderingOrderType.OpaqueGeometry)
                return 1;
            else return 0;
        }

        #endregion IComparable<RenderableCollectionDescription> Members


    }


}