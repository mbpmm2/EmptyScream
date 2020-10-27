
using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(EnemySight))]
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI()
	{
		EnemySight fow = (EnemySight)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.detectionCol.radius*1.2f);
		Vector3 viewAngleA = fow.DirFromAngle(-fow.fovAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.fovAngle / 2, false);

		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.detectionCol.radius * 1.2f);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.detectionCol.radius * 1.2f);

		Handles.color = Color.red;

        if (fow.playerInSight)
        {
			Handles.DrawLine(fow.transform.position, fow.playerController.transform.position);
		}
		
    }

}

#endif