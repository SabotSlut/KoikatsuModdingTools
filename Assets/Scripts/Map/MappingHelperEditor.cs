#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ActionGame.MapObject;
using ActionGame.Point;
using Assets.Map.Editor;
using H;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    [CustomEditor(typeof(MappingHelper))]
    public class MappingHelperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MappingHelper helper = target as MappingHelper;

            if (helper == null)
            {
                GUILayout.Label("MappingHelperEditor somehow detached from the MappingHelper.");
                return;
            }

            bool canUpdateHCategories = true;
            List<string> warnings = new List<string>();

            GameObject mapContainer = helper.gameObject.scene.GetRootGameObjects().FirstOrDefault(gameObject => gameObject.name == "Map");
            Kind kind = null;
            GateGroup gateGroup = null;
            MapInfo.Param mapInfo = null;

            if (mapContainer == null)
            {
                warnings.Add("Can't find root GameObject called \"Map\".");
                canUpdateHCategories = false;
            }
            else
            {
                gateGroup = mapContainer.GetComponent<GateGroup>();
                kind = mapContainer.GetComponentInChildren<Kind>();
            }

            if (kind == null)
            {
                warnings.Add("Can't find \"Kind\" GameObject with Kind component under \"Map\".");
                canUpdateHCategories = false;
            }

            if (helper.MapInfo == null)
            {
                warnings.Add("MapInfo is not set.");
            }
            else if (helper.MapInfo.param.Count < 1)
            {
                warnings.Add("MapInfo must have at least one param.");
            }
            else
            {
                mapInfo = helper.MapInfo.param[0];

                if (mapInfo.isGate && gateGroup == null)
                {
                    warnings.Add("Can't find GateGroup component on \"Map\" GameObject.");
                }
            }

            if (helper.HPoint_Container == null)
            {
                canUpdateHCategories = false;
            }

            if (!canUpdateHCategories)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("Update H Categories") && canUpdateHCategories)
            {
                var categories = new List<int>();
                foreach (HPointData hpoint in helper.HPoint_Container.GetComponentsInChildren<HPointData>(true))
                {
                    foreach (int category in hpoint._categorys)
                    {
                        if (!categories.Contains(category))
                        {
                            categories.Add(category);
                        }
                    }
                }

                categories.Sort();

                kind.categoryes = categories.ToArray();

                Debug.Log("Updated H Categories.");
            }

            GUI.enabled = true;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (warnings.Count > 0)
            {
                EditorGUILayout.LabelField("Warnings:", EditorStyles.boldLabel);
                foreach (string warning in warnings)
                {
                    EditorGUILayout.LabelField(warning);
                }

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            base.OnInspectorGUI();
        }
    }
}

#endif
