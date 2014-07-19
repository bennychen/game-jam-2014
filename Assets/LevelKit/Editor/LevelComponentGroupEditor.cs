using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelComponentGroup))]
public class LevelComponentGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelComponentGroup componentGroup = target as LevelComponentGroup;

        if (componentGroup == null)
        {
            return;
        }

        DrawDefaultInspector();
        if (GUILayout.Button("Update all component in Group", GUILayout.Height(30)))
        {
            string assetPath = AssetDatabase.GetAssetOrScenePath(componentGroup);
            string assetFolder = assetPath.Remove(assetPath.Length - (componentGroup.name + ".asset").Length);
            UpdateGroup(componentGroup,assetFolder);
        }
        EditorUtility.SetDirty(componentGroup);
    }


    public static void UpdateGroup(LevelComponentGroup group,string path)
    {
        Debug.Log("UpdateComponent:" + path);
        string[] fileEntries = System.IO.Directory.GetFiles(path,
                "*.prefab");
        group.ClearGroup();
        foreach (string fileName in fileEntries)
        {
            GameObject prefab = Resources.LoadAssetAtPath(fileName, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
                LevelComponent component = prefab.GetComponent<LevelComponent>();
                if (component != null)
                {
                    group.AddComponent(component);
                }
            }
        }
    }
}