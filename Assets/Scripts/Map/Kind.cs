using UnityEngine;

namespace ActionGame.MapObject
{
    public class Kind : MonoBehaviour
    {
        [SerializeField]
        [Header("H Scene Category Numbers")]
        private int[] _categoryes;

        [Header("Targets")]
        [SerializeField]
        private Transform[] _targets;

        [SerializeField]
        [Header("Groups")]
        private Transform[] _groups;

        [Header("Offset Position")]
        [SerializeField]
        internal Vector3 _offsetPos;

        [SerializeField]
        [Header("Offset Rotation")]
        internal Vector3 _offsetAngle;

        public int[] categoryes
        {
            set
            {
                this._categoryes = value;
            }
            get
            {
                return this._categoryes;
            }
        }
    }
}
