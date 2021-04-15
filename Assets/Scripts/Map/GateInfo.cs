using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionGame.Point;
using Illusion.Extensions;
using UnityEngine;

namespace ActionGame.Point
{
    public class GateInfo : MonoBehaviour
    {
        public Vector3 playerHitSize = Vector3.one;
        public Vector3 heroineHitSize = Vector3.one;
        public Vector3 iconHitSize = Vector3.one;
        public int seType = -1;
        public int ID;
        public int mapNo;
        public int linkID;
        public Vector3 pos;
        public Vector3 ang;
        public string Name;
        public Vector3 playerPos;
        public Vector3 playerAng;
        public Vector3 playerHitPos;
        public Vector3 heroineHitPos;
        public Vector3 iconPos;
        public Vector3 iconHitPos;
        public int moveType;

        public GateInfo(Gate gate)
        {
            ID = gate.ID;
            mapNo = gate.mapNo;
            linkID = gate.linkID;
            pos = gate.transform.position;
            ang = gate.transform.eulerAngles;
            Name = gate.name;
            playerPos = gate.playerTrans.localPosition;
            playerAng = gate.playerTrans.localEulerAngles;
            playerHitPos = gate.playerHitBox.center;
            playerHitSize = gate.playerHitBox.size;
            heroineHitPos = gate.heroineHitBox.center;
            heroineHitSize = gate.heroineHitBox.size;
            iconPos = gate.canvas.GetComponent<RectTransform>().anchoredPosition3D;
            iconHitPos = gate.iconHitBox.center;
            iconHitSize = gate.iconHitBox.size;
            moveType = gate.moveType;
            seType = gate.seType;
            calc = new Dictionary<int, Vector3[]>();
        }

        public GateInfo(List<string> list)
        {
            ID = int.Parse(list[0]);
            mapNo = int.Parse(list[1]);
            linkID = int.Parse(list[2]);
            pos = list[3].GetVector3();
            ang = list[4].GetVector3();
            Name = list[5];
            playerPos = list[6].GetVector3();
            playerAng = list[7].GetVector3();
            playerHitPos = list[8].GetVector3();
            playerHitSize = list[9].GetVector3();
            heroineHitPos = list[10].GetVector3();
            heroineHitSize = list[11].GetVector3();
            iconPos = list[12].GetVector3();
            iconHitPos = list[13].GetVector3();
            iconHitSize = list[14].GetVector3();
            moveType = int.Parse(list[15]);
            seType = int.Parse(list[16]);
            calc = new Dictionary<int, Vector3[]>();
        }

        public Dictionary<int, Vector3[]> calc { get; set; }

        public static List<string> Convert(Gate gate)
        {
            return new GateInfo(gate).Convert();
        }

        public static void Convert(List<string> list, ref Gate gate)
        {
            GateInfo gateInfo = new GateInfo(list);
            gate.ID = gateInfo.ID;
            gate.mapNo = gateInfo.mapNo;
            gate.linkID = gateInfo.linkID;
            Transform transform = gate.transform;
            transform.position = gateInfo.pos;
            transform.eulerAngles = gateInfo.ang;
            gate.name = gateInfo.Name;
            gate.playerTrans.localPosition = gateInfo.playerPos;
            gate.playerTrans.localEulerAngles = gateInfo.playerAng;
            BoxCollider playerHitBox = gate.playerHitBox;
            playerHitBox.center = gateInfo.playerHitPos;
            playerHitBox.size = gateInfo.playerHitSize;
            BoxCollider heroineHitBox = gate.heroineHitBox;
            heroineHitBox.center = gateInfo.heroineHitPos;
            heroineHitBox.size = gateInfo.heroineHitSize;
            gate.canvas.GetComponent<RectTransform>().anchoredPosition3D = gateInfo.iconPos;
            BoxCollider iconHitBox = gate.iconHitBox;
            iconHitBox.center = gateInfo.iconHitPos;
            iconHitBox.size = gateInfo.iconHitSize;
            gate.moveType = gateInfo.moveType;
            gate.seType = gateInfo.seType;
            gateInfo.calc = new Dictionary<int, Vector3[]>();
        }

        public static List<GateInfo> Create(List<ExcelData.Param> list)
        {
            List<GateInfo> gateInfoList = new List<GateInfo>();
            GateInfo gateInfo1 = null;
            foreach (ExcelData.Param obj in list)
            {
                if (obj.list[0] == "@")
                {
                    gateInfo1 = null;
                }
                else if (gateInfo1 == null)
                {
                    GateInfo gateInfo2 = new GateInfo(obj.list);
                    gateInfoList.Add(gateInfo2);
                    gateInfo1 = gateInfo2;
                }
                else
                {
                    Dictionary<int, Vector3[]> calc = gateInfo1.calc;
                    int index = int.Parse(obj.list[0]);
                    IEnumerable<string> source = obj.list.Skip(1);
                    Vector3[] array = source.Select(StringExtensions.GetVector3).ToArray();
                    calc[index] = array;
                }
            }

            return gateInfoList;
        }

        public static List<List<string>> Create(List<GateInfo> list)
        {
            List<List<string>> ret = new List<List<string>>();
            list.Sort((a, b) => a.ID.CompareTo(b.ID));
            list.ForEach(p =>
            {
                ret.Add(new List<string>() {"@"});
                ret.Add(p.Convert());
                foreach (KeyValuePair<int, Vector3[]> keyValuePair in p.calc.OrderBy(v => v.Key))
                {
                    List<string> stringList = new List<string>();
                    stringList.Add(keyValuePair.Key.ToString());
                    foreach (Vector3 self in keyValuePair.Value)
                    {
                        stringList.Add(self.Convert());
                    }

                    ret.Add(stringList);
                }
            });
            return ret;
        }

        public List<string> Convert()
        {
            return new List<string>
            {
                ID.ToString(),
                mapNo.ToString(),
                linkID.ToString(),
                pos.Convert(true),
                ang.Convert(true),
                Name,
                playerPos.Convert(true),
                playerAng.Convert(true),
                playerHitPos.Convert(true),
                playerHitSize.Convert(true),
                heroineHitPos.Convert(true),
                heroineHitSize.Convert(true),
                iconPos.Convert(true),
                iconHitPos.Convert(true),
                iconHitSize.Convert(true),
                moveType.ToString(),
                seType.ToString()
            };
        }
    }
}
