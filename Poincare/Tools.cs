using System;
using System.Numerics;

namespace Template
{
    public static class Tools
    {
        static public float HypLength(Vector2 p, Vector2 q)
        {
            float pq = Vector2.DistanceSquared(p, q);
            float pSquared = Vector2.Dot(p, p);
            float qSquared = Vector2.Dot(q, q);
            return Acosh(1 + (2 * pq / ((1 - pSquared) * (1 - qSquared))));
        }

        static public float HypLength(Vector3 p, Vector3 q)
        {
            return Acosh(p.Z * q.Z - p.X * q.X - p.Y * q.Y);
        }

        static public Vector2 Project(Vector3 p)
        {
            return new Vector2(p.X / (p.Z + 1), p.Y / (p.Z + 1));
        }

        static public Vector3 Complete(Vector2 p)
        {
            return new Vector3(p.X, p.Y, (float)Math.Sqrt(1 + p.X * p.X + p.Y * p.Y));
        }

        static public Vector3 ReverseProject(Vector2 p)
        {
            float lambda = 2f / (1 - p.X * p.X - p.Y * p.Y);
            return new Vector3(lambda * p.X, lambda * p.Y, lambda - 1);
        }

        static public float Acosh(float x)
        {
            return (float)Math.Log(x + Math.Sqrt(x * x - 1));
        }

        static public Vector3 MoveRight(float dist, Vector3 p) 
        {
            float cosh = (float)Math.Cosh(dist);
            float sinh = (float)Math.Sinh(dist);
            // matrix multiplication
            return new Vector3(sinh * p.Z + cosh * p.X, p.Y, cosh * p.Z + sinh * p.X);
        }

        static public Vector3 MoveUp(float dist, Vector3 p)
        {
            float cosh = (float)Math.Cosh(dist);
            float sinh = (float)Math.Sinh(dist);
            // matrix multiplication
            return new Vector3(p.X, sinh * p.Z + cosh * p.Y, cosh * p.Z + sinh * p.Y);
        }
    }
}
