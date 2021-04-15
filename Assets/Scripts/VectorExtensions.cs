using UnityEngine;

namespace Illusion.Extensions
{
    public static class VectorExtensions
    {
        private static string[] FormatRemoveSplit(string str)
        {
            return FormatRemove(str).Split(',');
        }

        private static string FormatRemove(string str)
        {
            return str.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
        }

        public static string Convert(this Vector2 self, bool isDefault = true)
        {
            return string.Format(!isDefault ? "{0},{1}" : "({0}, {1})", self[0], self[1]);
        }

        public static string Convert(this Vector3 self, bool isDefault = true)
        {
            return string.Format(!isDefault ? "{0},{1},{2}" : "({0}, {1}, {2})", self[0], self[1], self[2]);
        }

        public static string Convert(this Vector4 self, bool isDefault = true)
        {
            return string.Format(!isDefault ? "{0},{1},{2},{3}" : "({0}, {1}, {2}, {3})", self);
        }

        public static Vector2 Convert(this Vector2 _, string str)
        {
            string[] strArray = FormatRemoveSplit(str);
            Vector2 zero = Vector2.zero;
            for (int index = 0; index < strArray.Length && index < 2; ++index)
            {
                float result;
                if (float.TryParse(strArray[index], out result))
                {
                    zero[index] = result;
                }
            }
            return zero;
        }

        public static Vector3 Convert(this Vector3 _, string str)
        {
            string[] strArray = FormatRemoveSplit(str);
            Vector3 zero = Vector3.zero;
            for (int index = 0; index < strArray.Length && index < 3; ++index)
            {
                float result;
                if (float.TryParse(strArray[index], out result))
                {
                    zero[index] = result;
                }
            }
            return zero;
        }

        public static Vector4 Convert(this Vector4 _, string str)
        {
            string[] strArray = FormatRemoveSplit(str);
            Vector4 zero = Vector4.zero;
            for (int index = 0; index < strArray.Length && index < 4; ++index)
            {
                float result;
                if (float.TryParse(strArray[index], out result))
                {
                    zero[index] = result;
                }
            }
            return zero;
        }

        public static Vector2 Convert(this Vector2 self, float[] fArray)
        {
            return new Vector2(fArray[0], fArray[1]);
        }

        public static Vector3 Convert(this Vector3 self, float[] fArray)
        {
            return new Vector3(fArray[0], fArray[1], fArray[2]);
        }

        public static Vector4 Convert(this Vector4 self, float[] fArray)
        {
            return new Vector4(fArray[0], fArray[1], fArray[2], fArray[3]);
        }

        public static float[] ToArray(this Vector2 self)
        {
            return new[] { self[0], self[1] };
        }

        public static float[] ToArray(this Vector3 self)
        {
            return new[] { self[0], self[1], self[2] };
        }

        public static float[] ToArray(this Vector4 self)
        {
            return new[] { self[0], self[1], self[2], self[3] };
        }

        public static Vector2 Get(this Vector2 self, Vector2 set, bool x = true, bool y = true)
        {
            return new Vector2(!x ? self.x : set.x, !y ? self.y : set.y);
        }

        public static Vector3 Get(this Vector3 self, Vector3 set, bool x = true, bool y = true, bool z = true)
        {
            return new Vector3(!x ? self.x : set.x, !y ? self.y : set.y, !z ? self.z : set.z);
        }

        public static Vector4 Get(
            this Vector4 self,
            Vector4 set,
            bool x = true,
            bool y = true,
            bool z = true,
            bool w = true)
        {
            return new Vector4(!x ? self.x : set.x, !y ? self.y : set.y, !z ? self.z : set.z, !w ? self.w : set.w);
        }

        public static bool isNaN(this Vector2 self)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (float.IsNaN(self[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isNaN(this Vector3 self)
        {
            for (int index = 0; index < 3; ++index)
            {
                if (float.IsNaN(self[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isNaN(this Vector4 self)
        {
            for (int index = 0; index < 4; ++index)
            {
                if (float.IsNaN(self[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isInfinity(this Vector2 self)
        {
            for (int index = 0; index < 2; ++index)
            {
                if (float.IsInfinity(self[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isInfinity(this Vector3 self)
        {
            for (int index = 0; index < 3; ++index)
            {
                if (float.IsInfinity(self[index]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isInfinity(this Vector4 self)
        {
            for (int index = 0; index < 4; ++index)
            {
                if (float.IsInfinity(self[index]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
