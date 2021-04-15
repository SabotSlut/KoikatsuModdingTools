#if UNITY_EDITOR

using H;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Map.Editor
{
    [ExecuteInEditMode]
    public class MappingHelper : MonoBehaviour
    {
        public MapInfo MapInfo;
        public Transform HPoint_Container;
        public HDisplayType HPoint_DisplayType = HDisplayType.FirstPosition;
        public bool HPoint_DisplayAxis = true;

        void OnDrawGizmos()
        {
            var spawn = GameObject.FindGameObjectWithTag("Action/WarpPoint");
            if (spawn)
            {
                Gizmos.DrawIcon(spawn.transform.position + Vector3.up * 0.5f, "icon_warppoint");
                GizmosHelper.DrawAxis(spawn.transform);
            }

            if (HPoint_Container)
            {
                foreach (var hpd in HPoint_Container.GetComponentsInChildren<HPointData>())
                {
                    if (HPoint_DisplayAxis)
                    {
                        GizmosHelper.DrawAxis(hpd.transform);
                    }

                    GizmosHelper.HPoint(hpd, HPoint_DisplayType);
                }
            }
        }

        public enum HDisplayType
        {
            None,
            Icon,
            Shape,
            FirstPosition,
            AllPositions,
        }
    }
}

#endif
