using Point = SharpDX.Mathematics.Vector2;
using Real = System.Single;

namespace Odyssey.Geometry.Primitives
{
    public class BSpline : BSplineBase<Point>
    {
        public BSpline(int degree = 2) : base(degree) {}

        public override BSplineBase<Point> Derivative()
        {
            var newCPs = new Point[Count - 1];

            BSpline derivative = new BSpline(Degree - 1);

            for (int i = 0; i < newCPs.Length; i++)
            {
                newCPs[i] = (this[i + 1] - this[i]) * (Degree / (KnotVector[i + Degree + 1] - KnotVector[i + 1]));
            }

            derivative.AddPoints(newCPs);

            var newKnotVector = new Real[KnotVector.Count - 2];
            for (int i = 0; i < newKnotVector.Length; i++)
            {
                newKnotVector[i] = KnotVector[i + 1];
            }
            derivative.KnotVector.AddRange(newKnotVector);

            return derivative;
        }

        protected override Point DeBoor(int k, int i, Real t)
        {
            if (k == 0)
            {
                return this[i];
            }
            else
            {
                double alpha = (t - KnotVector[i]) / (KnotVector[i + Degree + 1 - k] - KnotVector[i]);
                return DeBoor(k - 1, i - 1, t) * (1 - (Real)alpha) + DeBoor(k - 1, i, t) * (Real)alpha;
            }
        }
    }
}
