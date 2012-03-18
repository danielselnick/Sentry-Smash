using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace towersmash
{
    /// <summary>
    /// Math class to do all the same math functions that xna does.
    /// Sole purpose is for me to learn the math behind all the xna math functions
    /// Everything is done on the stack, not on the heap
    /// </summary>
    public static class math
    {
        static Vector2 vector = new Vector2();
        public static float vectortoradians(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -(double)vector.Y);
        }

        public static Vector2 radianstovector(float radians)
        {
            vector.X = (float)Math.Sin(radians);
            vector.Y = -(float)Math.Cos(radians);
            return vector;
        }
        
        public static void rotatevector(ref Vector2 vector, float radians)
        {
            float length = vector.Length();
            float newRadians = (float)Math.Atan2(vector.X, -(double)vector.Y) + radians;

            vector.X = (float)Math.Sin(newRadians) * length;
            vector.Y = -(float)Math.Cos(newRadians) * length;
        }

        public static Rectangle union(Rectangle rect1, Rectangle rect2)
        {
            //calculate the top
            int rect1top = rect1.X + rect1.Width;
            int rect2top = rect2.X + rect2.Width;
            //if rect1 top > rect2 top then biggest top = rect1 top else bigest top = rect2 top
            int biggesttop = (rect1top > rect2top) ? rect1top : rect2top;

            //calculate the side
            int rect1side = rect1.Y + rect1.Height;
            int rect2side = rect2.Y + rect2.Height;
            //if the side of rectangle 1 is greator than the side of rectangle 2, then the greatest side is from rectangle 1, otherwise it'll be from rectangle 2
            int biggestside = (rect1side > rect2side) ? rect1side : rect2side;

            //if rect1 x < rect 2 x then num3 = rect1 x else num3 = rect 2 x
            int smallestx = (rect1.X < rect2.X) ? rect1.X : rect2.X;
            //if rect1 y < rect 2 y then num4 = rect1 y else num4 = rect 2 y
            int smallesty = (rect1.Y < rect2.Y) ? rect1.Y : rect2.Y;

            //set the values for the result
            rect1.X = smallestx;
            rect1.Y = smallesty;
            rect1.Width = biggesttop - smallestx;
            rect1.Height = biggestside - smallesty;

            return rect1;
        }

        public static float clamp(float value, float min, float max)
        {
            //if the value is above the max, set it to the max
            value = (value > max) ? max : value;
            //if the value is below the min, set it to the min
            value = (value < min) ? min : value;
            return value;
        }
        public static Matrix identity = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        //Note that matrix in xna is column major
        

        public static Matrix createtranslation(float x, float y, float z)
        {
            //create a tranlaste matrix
            //  1   0   0   x
            //  0   1   0   y
            //  0   0   1   z
            //  0   0   0   1
            Matrix matrix = math.identity;
            matrix.M41 = x;
            matrix.M42 = y;
            matrix.M43 = z;
            matrix.M44 = 1;
            return matrix;
        }

        public static Matrix createrotationz(float rotation)
        {
            //create a matrix to rotate around the z axis
            //  cos -sin    0   0
            //  sin -cos    0   0
            //  0   0       1   0
            //  0   0       0   1
            Matrix matrix = math.identity;
            float xrotate = (float)Math.Cos((double)rotation);
            float yrotate = (float)Math.Sin((double)rotation);
            matrix.M11 = xrotate;
            matrix.M12 = yrotate;
            matrix.M21 = -yrotate;
            matrix.M22 = -xrotate;
            return matrix;
        }

        /// <summary>
        /// create a transform scale matrix
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix createscale(float scale)
        {
            //  scale   0       0       0  
            //  0       scale   0       0
            //  0       0       scale   0
            //  0       0       0       1
            Matrix matrix = math.identity;
            matrix.M11 = scale;
            matrix.M22 = scale;
            matrix.M33 = scale;
            return matrix;
        }

        public static Vector2 transform(Vector2 position, Matrix matrix)
        {
            position.X = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
            position.Y = ((position.Y * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
            return position;
        }

        public static Vector2 smoothstep(Vector2 vector0, Vector2 vector1, float value)
        {
            //fancy clamp to make sure the value is between 0 and 1
            value = (value > 1f) ? 1f : ((value < 0f) ? 0f : value);
            //math!
            value = (value * value) * (3f - (2f * value));
            //step the values up to the desired value
            vector0.X = vector0.X + ((vector1.X - vector0.X) * value);
            vector0.Y = vector0.Y + ((vector1.Y - vector0.Y) * value);
            //return new vector
            return vector0;
        }

        public static float smoothstep(float value1, float value2, float amount)
        {
            float num = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            float value = (num * num) * (3f-(2f * num));
            float lerp = (value1 + ((value2 - value1) * amount));
            return lerp;
        }

        //linear interpolation
        public static float lerp(float value1, float value2, float amount)
        {
            return (value1 + ((value2 - value1) * amount));
        }
    }
}
