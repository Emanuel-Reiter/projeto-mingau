using UnityEngine;
using UnityEditor;

public class PrefabNameReverter : EditorWindow
{
    [MenuItem("Tools/Prefab/Revert Names to Prefab")]
    public static void ShowWindow()
    {
        GetWindow<PrefabNameReverter>("Revert Prefab Names");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Revert Selected Prefab Instances"))
        {
            RevertSelectedPrefabInstances();
        }
    }

    void RevertSelectedPrefabInstances()
    {
        foreach (GameObject selectedObject in Selection.gameObjects)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(selectedObject))
            {
                GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(selectedObject);
                if (prefabAsset != null)
                {
                    selectedObject.name = prefabAsset.name;
                }
            }
        }
        Debug.Log("Renamed selected prefab instances to their prefab names.");
    }
}