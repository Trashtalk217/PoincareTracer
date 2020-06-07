using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Template
{

	class MyApplication
	{

		// member variables
		public Surface screen;
		private Light[] _lights;
		private IPrimitive[] _primitives;

		public int Width { get; set; }
		public int Height { get; set; }
        public Vector3[,] preBlend;
        public Vector2 center;

		// initialize
		public void Init()
		{
            Vector3 lightColor = new Vector3(10);
            preBlend = new Vector3[screen.width + 1, screen.height + 1];
            _primitives = new IPrimitive[]
            {
                new Circle(0, 0.5f, 0.1f, 255, 255, 255),
                new Circle(0.4f, -0.2f, 0.2f, 255, 255, 255),
                new Circle(-0.4f, -0.2f, 0.2f, 255, 255, 255)
            };
            center = new Vector2(0.5f, 0.5f);

            _lights = new Light[25];
            for (int i = -12; i < 13; i++)
            {
                _lights[i + 12] = new Light(Tools.Complete(new Vector2(i, 0)), lightColor);
            }
        }

		// tick: renders one frame
        public void Tick()
        {
            for(int i = 0; i < _lights.Length; i++)
            {
                _lights[i].Location = Tools.MoveUp(0.05f, _lights[i].Location);
            }

            Parallel.For(0, (screen.width + 1) * (screen.height + 1), k => 
            {
                int x = k % (screen.width + 1);
                int y = k / (screen.width + 1);
                float tx = (float)2 * x / (screen.width + 1) - 1;
                float ty = -((float)2 * y / (screen.height + 1) - 1);

                Vector2 location = new Vector2(tx, ty);
                Vector3 pixelColor = new Vector3(0, 0, 0);

                if (location.LengthSquared() < 1)
                {
                    for (int i = 0, im = _lights.Length; i < im; i++)
                    {
                        Light light = _lights[i];
                        Ray ray = new Ray(location, Tools.Project(light.Location));
                        bool occluded = false;
                        float shade = 0;

                        for (int j = 0, jm = _primitives.Length; !occluded && j < jm; j++)
                            if (_primitives[j].Intersects(ray, out bool locked))
                            {
                                occluded = true;
                                shade *= locked ? 0 : 0.5f;
                            }

                        if (!occluded)
                            shade = LightAttenuation(ray.Distance);

                        pixelColor += light.Color * shade;
                    }
                }
                preBlend[x, y] = pixelColor;
            });

            // blend!
            Parallel.For(0, screen.height * screen.width, k =>
            {
                int x = k % screen.width;
                int y = k / screen.width;

                Vector3 pixelColor = new Vector3(0, 0, 0);
                pixelColor += preBlend[x, y];
                pixelColor += preBlend[x + 1, y];
                pixelColor += preBlend[x, y + 1];
                pixelColor += preBlend[x + 1, y + 1];
                pixelColor /= 4;

                screen.Plot(x, y, MixColor(pixelColor));
            });
            
            /*
            Ray ray2 = new Ray(new Vector2(0, 0.25f), new Vector2(0.25f, 0));
            screen.Box(500 - 2, 400 - 2, 500 + 2, 400 + 2, MixColor(new Vector3(0, 0, 255)));
            screen.Box(400 - 2, 300 - 2, 400 + 2, 300 + 2, MixColor(new Vector3(0, 0, 255)));
            ray2.Draw(screen);
            Console.WriteLine(_primitives[0].Intersects(ray2, out bool temp));
            */           
        }

        public static int MixColor(Vector3 color)
        {
            if (color.X > 255) color.X = 255;
            if (color.Y > 255) color.Y = 255;
            if (color.Z > 255) color.Z = 255;

            return ((int)color.X << 16) + ((int)color.Y << 8) + (int)color.Z;

		}
		public static float Crossproduct(Vector2 v, Vector2 w)
		{

			return v.X * w.Y - v.Y * w.X;

		}
		public static float LightAttenuation(float d)
		{

			if (d == 0)
				return 1;
			else
				return 1 / (float)Math.Sqrt(d);
		}
	}
}
