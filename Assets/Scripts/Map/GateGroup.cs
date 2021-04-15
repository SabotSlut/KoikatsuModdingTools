using System.Collections.Generic;
using UnityEngine;

namespace ActionGame.Point
{
    public class GateGroup : MonoBehaviour
    {
        [SerializeField]
        public List<Gate> _gateList = new List<Gate>();
        
        public List<Gate> gateList
        {
            get
            {
                return _gateList;
            }
        }
        
        [ContextMenu("Setup")]
        private void Setup()
        {
            _gateList.Clear();
            _gateList.AddRange(FindObjectsOfType<Gate>());
            _gateList.Sort(((a, b) => a.ID.CompareTo(b.ID)));
        }
    }
}
