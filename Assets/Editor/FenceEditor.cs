using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fence))]
public class FenceEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("spin 90 degrees", GUILayout.Height(30)))
		{
			(target as Fence).InstantOrthographicSpin();
		}
	}
}
