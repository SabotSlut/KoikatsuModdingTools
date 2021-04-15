using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    public static class Gizmos
    {
        public static void Axis(Vector3 pos, Quaternion rot, float len = 0.25f)
        {
            UnityEngine.Gizmos.color = Color.red;
            UnityEngine.Gizmos.DrawRay(pos, rot * Vector3.right * len);
            UnityEngine.Gizmos.color = Color.green;
            UnityEngine.Gizmos.DrawRay(pos, rot * Vector3.up * len);
            UnityEngine.Gizmos.color = Color.blue;
            UnityEngine.Gizmos.DrawRay(pos, rot * Vector3.forward * len);
        }

        public static void Axis(Transform transform, float len = 0.25f)
        {
            Utils.Gizmos.Axis(transform.position, transform.rotation, len);
        }

        public static void PointLine(Vector3[] route, bool isLink = false)
        {
            if (!((IEnumerable<Vector3>) route).Any<Vector3>())
            {
                return;
            }

            ((IEnumerable<Vector3>)route).Aggregate<Vector3>((Func<Vector3, Vector3, Vector3>)((prev, current) =>
            {
                UnityEngine.Gizmos.DrawLine(prev, current);
                return current;
            }));
            if (!isLink)
            {
                return;
            }

            UnityEngine.Gizmos.DrawLine(((IEnumerable<Vector3>)route).Last<Vector3>(), ((IEnumerable<Vector3>)route).First<Vector3>());
        }
    }
}
