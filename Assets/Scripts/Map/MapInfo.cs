using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapInfo", menuName = "Map List Info", order = 51)]
public class MapInfo : ScriptableObject
{
    public List<MapInfo.Param> param = new List<MapInfo.Param>();

    [Serializable]
    public class Param
    {
        public string MapName;
        [Tooltip("Map number.")]
        public int No;
        public string AssetBundleName;
        public string AssetName;
        public bool isGate;
        public bool is2D;
        public bool isWarning;
        [Tooltip("Used by MapShoesSetting")]
        public int State;
        public int LookFor;
        public bool isOutdoors;
        public bool isFreeH;
        public bool isSpH;
        [Header("Thumbnails")]
        public string ThumbnailBundle;
        public string ThumbnailAsset;
    }
}
