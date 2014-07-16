using Odyssey.UserInterface.Controls;
using SharpDX;
using System;
using System.Diagnostics.Contracts;

namespace Odyssey.Graphics.Shapes
{
    public class Path : Shape
    {
        public PathGeometry Data { get; set; }

        public override bool Contains(Vector2 cursorLocation)
        {
            return false;
        }

        public override void Render()
        {
            //Device.DrawGeometry(Data, Stroke);
            Device.FillGeometry(Data, Fill);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            if (Data == null)
                throw new InvalidOperationException("'Data' cannot be null");
            ToDispose(Data).Initialize();
            var initializer = new ShapeInitializer(Device);
            initializer.Initialize(this);
            foreach (var resource in initializer.CreatedResources)
                ToDispose(resource);
        }
    }
}