using Odyssey.Engine;
using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public struct HitDescription
    {
        public HitDescription(IInteractive3D match, float distanceSquared, Vector3 intersectionPoint)
            : this()
        {
            Match = match;
            DistanceSquared = distanceSquared;
            IntersectionPoint = intersectionPoint;
        }

        public float DistanceSquared { get; private set; }
        public Vector3 IntersectionPoint { get; private set; }
        public IInteractive3D Match { get; private set; } 
    }

    public abstract class InteractionPanel : PanelBase
    {
        SortedDictionary<float, HitDescription> matches;

        protected InteractionPanel(string controlTag)
            : base(controlTag, "Empty")
        {
            IsFocusable = true;
            matches = new SortedDictionary<float, HitDescription>();
        }

        public  ICameraProvider CameraProvider { get; set; }
        public IInteractiveItemsProvider SceneProvider { get; set; }
        protected IInteractive3D CurrentMatch { get; set; }

        protected IEnumerable<HitDescription> Matches
        {
            get
            {
                return matches.Values;
            }
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        protected bool PickTarget(Vector2 position, out Vector3 intersection)
        {
            Contract.Requires<NullReferenceException>(SceneProvider != null, "SceneProvider is null.");
            Contract.Requires<NullReferenceException>(CameraProvider != null, "CameraProvider is null.");
            var tempMatches = new SortedDictionary<float, HitDescription>();
            Matrix mViewProjection = CameraProvider.View * CameraProvider.Projection;
            Ray pickRay = GetPickRay(position, CameraProvider);
            Vector3 intersectionPoint = default(Vector3);
            lock(this)
            foreach (IInteractive3D iObject in SceneProvider.Items)
            {
                if (iObject.IsHitTestVisible && iObject.Intersects(pickRay, out intersectionPoint))
                {
                    float distanceSquared =(intersectionPoint - pickRay.Position).LengthSquared();
                    HitDescription hDesc = new HitDescription(iObject, distanceSquared, intersectionPoint);
                    tempMatches.Add(distanceSquared, hDesc);
                }
            }

            var closestHit = tempMatches.FirstOrDefault();

            if (closestHit.Value.Match != null)
            {
                CurrentMatch = closestHit.Value.Match;
                intersection = closestHit.Value.IntersectionPoint;
            }
            else
            {
                CurrentMatch = null;
                intersection = default(Vector3);
            }
            matches = new SortedDictionary<float,HitDescription>(tempMatches);

            return (CurrentMatch != null);
        }

        public static Ray GetPickRay(Vector2 position, ICameraProvider camera)
        {
            return Ray.GetPickRay((int)position.X, (int)position.Y, camera.Viewport, camera.View * camera.Projection);
        }
    }
}