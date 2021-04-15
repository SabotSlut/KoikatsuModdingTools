using System;
using UnityEngine;

namespace H
{
    // Token: 0x0200078A RID: 1930
    public class HPointData : MonoBehaviour
    {
        [SerializeField]
        [Header("H Scene Category Numbers")]
        public int[] _categorys;

        private Transform _selfTransform;

        [SerializeField]
        [Header("Targets")]
        private string[] _targets;

        public Transform[] _objTargets = new Transform[0];

        [Header("Group")]
        [SerializeField]
        private string[] _groups;

        public Transform[] _objGroups = new Transform[0];

        [Header("Offset Position")]
        [SerializeField]
        internal Vector3 _offsetPos;

        [SerializeField]
        [Header("Offset Angle")]
        internal Vector3 _offsetAngle;

        [SerializeField]
        [Header("Experience Required (0: Virgins Allowed, 1: Virgins Banned)")]
        private int _experience;
    }
}
