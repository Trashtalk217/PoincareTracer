using System;
using System.Numerics;

namespace Template
{

	public class Light
	{
        public Vector3 Location;
		public Vector3 Color { get; set; }

		public Light(Vector3 loc, Vector3 col)
		{
			Location = loc;
			Color = col;
		}
	}
}
