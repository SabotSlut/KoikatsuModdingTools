#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using ActionGame.Point;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GateInjector : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public string TargetGate;
    public Transform SpawnPoint;
    public BoxCollider CollisionBox;
    public bool UseOnCollision = true;

    private static readonly Vector3 modelOffset = new Vector3(0, 1.145f, 0);

    void OnDrawGizmos()
    {
        if (CollisionBox != null)
        {
            DrawBoxCollider(CollisionBox, new Color(0, 0.5f, 0, 0.5f));
        }

        if (SpawnPoint != null)
        {
            DrawSpawnPoint(SpawnPoint, new Color(0, 0.5f, 0, 0.5f));
        }
    }

    private void DrawSpawnPoint(Transform trans, Color color)
    {
        Mesh mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Gizmos/hpoint_1_f.obj", typeof(Mesh));
        if (mesh != null)
        {
            Gizmos.color = color;
            Gizmos.DrawWireMesh(mesh, trans.position + modelOffset, trans.rotation);
        }
    }

    private static void DrawBoxCollider(BoxCollider box, Color color)
    {
        var size = box.transform.rotation * box.size;
        if (size.x < 0) { size.x *= -1; }
        if (size.y < 0) { size.y *= -1; }
        if (size.z < 0) { size.z *= -1; }

        Gizmos.color = color;
        Gizmos.DrawCube(box.transform.position + box.transform.rotation * box.center, size);
    }
}

#endif
