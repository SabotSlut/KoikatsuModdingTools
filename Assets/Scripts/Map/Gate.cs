using UnityEngine;

namespace ActionGame.Point
{
    public class Gate : MonoBehaviour
    {
        public int ID = -1;
        public int mapNo = -1;
        public int linkID = -1;
        public int seType = -1;
        [SerializeField]
        private Transform _playerTrans;
        [SerializeField]
        private BoxCollider _playerHitBox;
        [SerializeField]
        private BoxCollider _heroineHitBox;
        [Header("0=>クリックで移動,1=>当たって移動")]
        public int moveType;
        [Header("Icon")]
        [SerializeField]
        private Canvas _canvas;
        [SerializeField]
        private BoxCollider _iconHitBox;
        [SerializeField]
        private bl_MiniMapItem miniMapIcon;
        private string infoText;
        
        public Transform playerTrans
        {
            get
            {
                return this._playerTrans;
            }
        }
        
        public BoxCollider playerHitBox
        {
            get
            {
                return this._playerHitBox;
            }
        }
        
        public BoxCollider heroineHitBox
        {
            get
            {
                return this._heroineHitBox;
            }
        }
        
        public Canvas canvas
        {
            get
            {
                return this._canvas;
            }
        }
        
        public BoxCollider iconHitBox
        {
            get
            {
                return this._iconHitBox;
            }
        }
        
        public void SetData(GateInfo info)
        {
            this.ID = info.ID;
            this.mapNo = info.mapNo;
            this.linkID = info.linkID;
            this.name = info.Name;
            this.transform.SetPositionAndRotation(info.pos, Quaternion.Euler(info.ang));
            this._playerTrans.localPosition = info.playerPos;
            this._playerTrans.localEulerAngles = info.playerAng;
            this._playerHitBox.center = info.playerHitPos;
            this._playerHitBox.size = info.playerHitSize;
            this._heroineHitBox.center = info.heroineHitPos;
            this._heroineHitBox.size = info.heroineHitSize;
            this.moveType = info.moveType;
            this.seType = info.seType;
            this._canvas.GetComponent<RectTransform>().anchoredPosition3D = info.iconPos;
            this._iconHitBox.center = info.iconHitPos;
            this._iconHitBox.size = info.iconHitSize;
        }
        
        public void SetMiniMap(string infoText)
        {
            this.infoText = infoText;
        }
    }
}
