using System;
using System.Numerics;
namespace Template
{

    public interface IPrimitive
    {
        void Draw(Surface screen);
        bool Intersects(Ray ray, out bool locked);
    }

    public class Circle : IPrimitive
    {

        private Vector2 _centre;
        private float _radius;
        private Vector3 _color;

        public Circle(float x, float y, float r, float R, float G, float B)
        {
            _centre = new Vector2(x, y);
            _radius = r;
            _color = new Vector3(R, G, B);
        }

        public void Draw(Surface screen)
        {
            double Pi2 = 2 * Math.PI;

            for (double theta = 0, dtheta = 0.0001f; theta < Pi2; theta += dtheta)
            {
                float x = _centre.X + _radius * (float)Math.Cos(theta),
                    y = _centre.Y + _radius * (float)Math.Sin(theta);
                int tx = (int)Math.Round((x + 1) / 2 * screen.width);
                int ty = (int)Math.Round((-y + 1) / 2 * screen.height);

                if (x * x + y * y <= 1)
                    screen.pixels[tx + ty * screen.width] = MyApplication.MixColor(new Vector3(255, 0, 0));
            }
        }

        public bool Intersects(Ray ray, out bool locked)
        {
            locked = false;
            if (Vector2.DistanceSquared(ray.Origin, _centre) < _radius * _radius)
            {
                locked = true;
                return true;
            }
            float dSquared = Vector2.DistanceSquared(_centre, ray.Center);
            float upper = ray.Radius + _radius;
            float lower = ray.Radius - _radius;
            if (dSquared < upper * upper && dSquared > lower * lower)
            {
                Vector2 a = ray.Origin - ray.Center;
                Vector2 b = ray.Origin - ray.End;
                Vector2 c = ray.Origin - _centre;

                if ((a.Y * b.X - a.X * b.Y) * (a.Y * c.X - a.X * c.Y) >= 0)
                {
                    if (ray.Distance < Tools.HypLength(ray.Origin, _centre))
                    {
                        return false;
                    }
                    locked = true;
                    return true;
                }
            }
            return false;
        }
    }
}
