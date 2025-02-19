﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Map.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Test
{
    [CustomEditor(typeof(GateInjector))]
    public class GateInjectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GateInjector gate = target as GateInjector;
            MappingHelper helper = null;

            if (gate != null)
            {
                var rootObjects = gate.gameObject.scene.GetRootGameObjects();
                foreach (var gameObject in rootObjects)
                {
                    helper = gameObject.GetComponent<MappingHelper>();

                    if (helper != null)
                    {
                        break;
                    }
                }
            }

            if (gate == null ||
                helper == null ||
                gate.CollisionBox == null ||
                gate.SpawnPoint == null ||
                string.IsNullOrEmpty(gate.name) ||
                gate.name == "GameObject" ||
                string.IsNullOrEmpty(gate.TargetGate) ||
                helper.MapInfo == null)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("Serialize") && GUI.enabled)
            {
                Serialize(gate, helper);
            }

            GUI.enabled = true;

            if (gate == null)
            {
                GUILayout.Label("GateInjectorEditor somehow detached from the GateInjector.");
                return;
            }

            if (helper == null)
            {
                GUILayout.Label("Missing a MappingHelper component on an empty GameObject in the current scene.");
            }
            else if (helper.MapInfo == null)
            {
                GUILayout.Label("The MappingHelper does not have the MapInfo set.");
            }
            else if (helper.MapInfo.param.Count < 1)
            {
                GUILayout.Label("The MapInfo must have at least one param.");
            }

            if (string.IsNullOrEmpty(gate.name))
            {
                GUILayout.Label("Missing a gate name.");
            }

            if (gate.name == "GameObject")
            {
                GUILayout.Label("Gate name must not be \"GameObject\".");
            }

            if (string.IsNullOrEmpty(gate.TargetGate))
            {
                GUILayout.Label("Missing a target gate.");
            }

            if (gate.CollisionBox == null)
            {
                GUILayout.Label("Missing a collision box.");
            }

            if (gate.SpawnPoint == null)
            {
                GUILayout.Label("Missing a spawn point.");
            }
        }

        private static void Serialize(GateInjector gate, MappingHelper helper)
        {
            var mapId = helper.MapInfo.param[0].No;
            System.Diagnostics.Debug.Assert(gate != null, "gate != null");
            System.Diagnostics.Debug.Assert(gate.SpawnPoint != null, "gate.SpawnPoint != null");
            System.Diagnostics.Debug.Assert(gate.CollisionBox != null, "gate.CollisionBox != null");
            StringBuilder sb = new StringBuilder();
            sb.Append("Name,");
            sb.AppendLine(gate.name);
            sb.Append("Target Gate,");
            sb.AppendLine(gate.TargetGate);
            sb.Append("Map ID,");
            sb.AppendLine(mapId.ToString());
            AppendDoubleVector3(sb, "Transform", gate.transform.position, gate.transform.eulerAngles);
            AppendDoubleVector3(sb, "Spawn", gate.SpawnPoint.transform.position, gate.SpawnPoint.transform.eulerAngles);
            AppendDoubleVector3(sb, "Collision", gate.CollisionBox.center, gate.CollisionBox.size);
            sb.Append("Use On Collision,");
            sb.AppendLine(gate.UseOnCollision.ToString());
            string destPath = Path.Combine(Path.GetDirectoryName(SceneManager.GetActiveScene().path), string.Format("{0}.csv", gate.name));
            File.WriteAllText(destPath, sb.ToString());
            AssetDatabase.Refresh();
            var asset = AssetImporter.GetAtPath(destPath);
            var abPath = string.Format("map/list/gates/{0}.unity3d", mapId);
            if (asset)
            {
                asset.SetAssetBundleNameAndVariant(abPath, "");
                Debug.Log(string.Format("Wrote {0} and set its asset bundle to {1}.", destPath, abPath));
            }
            else
            {
                Debug.Log(string.Format("Wrote {0}, but failed to set its asset bundle to {1}.", destPath, abPath));
            }
        }

        private static void AppendDoubleVector3(StringBuilder sb, string key, Vector3 vec31, Vector3 vec32)
        {
            sb.Append(key); sb.Append(",");
            sb.Append(vec31.x.ToString(CultureInfo.InvariantCulture)); sb.Append(",");
            sb.Append(vec31.y.ToString(CultureInfo.InvariantCulture)); sb.Append(",");
            sb.Append(vec31.z.ToString(CultureInfo.InvariantCulture)); sb.Append(",");
            sb.Append(vec32.x.ToString(CultureInfo.InvariantCulture)); sb.Append(",");
            sb.Append(vec32.y.ToString(CultureInfo.InvariantCulture)); sb.Append(",");
            sb.AppendLine(vec32.z.ToString(CultureInfo.InvariantCulture));
        }
    }
}

#endif
