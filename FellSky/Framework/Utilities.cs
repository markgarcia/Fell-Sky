﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FellSky
{
    public static class Utilities
    {
        public static void DecomposeMatrix2D(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

        public static float GetLesserAngleDifference(float a1, float a2)
        {
            return (float)Math.Atan2(Math.Sin(a1 - a2), Math.Cos(a1 - a2));
        }

        public static Vector2 CreateVector2FromAngle(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
