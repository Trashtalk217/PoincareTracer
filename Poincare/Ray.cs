using System;
using System.Numerics;

namespace Template
{
	public struct Ray
	{

        public Vector2 Origin;
        public Vector2 End;
        public Vector2 Center { get; private set; }
        public float Radius { get; private set; }
        public float Distance { get; private set; }
        public Vector2 Direction;

        public Ray(Vector2 origin, Vector2 end)
        {
            Direction = new Vector2(0, 0);
            Origin = origin;
            End = end;
            Center = FindCenter(origin, end);
            Radius = Vector2.Distance(Center, origin);
            Distance = Tools.HypLength(origin, end);
        }

        private static Vector2 FindCenter(Vector2 origin, Vector2 end)
        {
            // now we get the distance between (0, 0) and the midpoint between origin and it's inversion
            // same goes for the end
            // everything is scaled by o.Length and e.Length for speed
            float mOffset = (origin.LengthSquared() + 1) / 2;
            float nOffset = (end.LengthSquared() + 1) / 2;
            // now we have two lines
            // m has normalO and mOffset: normalO dot vector = mOffset
            // n has normalE and nOffset: normalE dot vector = nOffset
            // we need to find where these lines cross, that's the center
            // solve it using Cramer's rule:
            float determinant = origin.X * end.Y - origin.Y * end.X;
            if (determinant != 0)
            {
                float detX = mOffset * end.Y - nOffset * origin.Y;
                float detY = origin.X * nOffset - end.X * mOffset;
                return new Vector2(detX / determinant, detY / determinant);
            }
            else
            {
                // the lines are parallel and origin and end lie on a diameter
                return new Vector2(0, 0);
            }
        }

        public void Draw(Surface screen)
        {
            double Pi2 = 2 * Math.PI;

            for (double theta = 0, dtheta = 0.0001f; theta < Pi2; theta += dtheta)
            {
                float x = Center.X + Radius * (float)Math.Cos(theta),
                    y = Center.Y + Radius * (float)Math.Sin(theta);
                int tx = (int)Math.Round((x + 1) / 2 * screen.width);
                int ty = (int)Math.Round((-y + 1) / 2 * screen.height);

                if (x * x + y * y <= 1)
                    screen.pixels[tx + ty * screen.width] = MyApplication.MixColor(new Vector3(255, 0, 0));
            }
        }
    }
}
