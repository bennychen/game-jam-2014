using UnityEditor;
using UnityEngine;
using GameJam.Utils;

public class GameObjectEdit : EditorWindow
{
    [MenuItem("GameJam/Game Object/Create Prefab")]
    public static void DoCreatePrefabs()
    {
        Transform[] transforms = Selection.transforms;
        if (transforms.Length > 0 && EditorUtility.DisplayDialog("Create prefabs?",
            "Are you sure you want to create prefabs for selected game object?", "Create", "Do Not Create"))
        {
            GameObject gameObject = null;
            foreach (Transform t in transforms)
            {
                gameObject = PrefabUtility.CreatePrefab("Assets/Prefabs/" + t.gameObject.name + ".prefab",
                    t.gameObject, ReplacePrefabOptions.ConnectToPrefab);
            }
            if (gameObject != null)
            {
                Selection.activeGameObject = gameObject;
            }
        }
    }

    [MenuItem("GameJam/Game Object/Print Object Distance")]
    private static void DoPrintObjectDistance()
    {
        if (Selection.transforms.Length == 2)
        {
            Debug.Log("Distance from [" + Selection.transforms[0].name + 
                      "] to [" + Selection.transforms[1].name + "] = " + (Selection.transforms[1].position - Selection.transforms[0].position));
        }
    }

#if ENABLE_TK2D
    [MenuItem("GameJam/Game Object/Print Sprite Bounds")]
    private static void DoPrintSpriteBounds()
    {
        GameObject gameObject = Selection.activeGameObject;
        if (gameObject != null)
        {
            tk2dSprite sprite = gameObject.GetComponent<tk2dSprite>();
            if (sprite != null)
            {
                PrintBounds(sprite.transform, sprite.GetUntrimmedBounds());
            }
        }
    }

    [MenuItem("GameJam/Game Object/Print Batched Sprite Bounds")]
    private static void DoPrintBatchedSpriteBounds()
    {
        GameObject gameObject = Selection.activeGameObject;
        if (gameObject != null)
        {
            tk2dStaticSpriteBatcher sprite = gameObject.GetComponent<tk2dStaticSpriteBatcher>();
            if (sprite != null)
            {
                PrintBounds(sprite.transform, sprite.GetBounds());
            }
        }
    }
#endif

    [MenuItem("GameJam/Game Object/Print Camera Size")]
    private static void DoPrintCameraSize()
    {
        GameObject gameObject = Selection.activeGameObject;
        if (gameObject != null)
        {
            Camera camera = gameObject.GetComponent<Camera>();
            if (camera != null)
            {
                Debug.Log("Camera width = " + CameraUtil.GetCameraHalfWidth(camera) * 2 + 
                          ", height = " + CameraUtil.GetCameraHalfHeight(camera) * 2);
            }
        }
    }

    private static void PrintBounds(Transform transform, Bounds bounds)
    {
        Debug.Log("Bounds=" + bounds.ToString("F4") +
                  ",Min=" + (transform.localPosition + bounds.center - bounds.size / 2).ToString("F4") +
                  ",Max=" + (transform.localPosition + bounds.center + bounds.size / 2).ToString("F4"));
    }
}
