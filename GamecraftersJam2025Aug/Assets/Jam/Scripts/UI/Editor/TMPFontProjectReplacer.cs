using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Jam.Scripts.UI.Editor
{
    public class FontProjectReplacer : EditorWindow
    {
        TMP_FontAsset newTMPFont;
        Font newUIFont;

        [MenuItem("Tools/Replace Fonts In Project")]
        public static void ShowWindow()
        {
            GetWindow<FontProjectReplacer>("Replace Fonts In Project");
        }

        void OnGUI()
        {
            GUILayout.Label("Replace TMP & UI.Text Fonts in All Scenes & Prefabs", EditorStyles.boldLabel);

            newTMPFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newTMPFont, typeof(TMP_FontAsset), false);
            newUIFont = (Font)EditorGUILayout.ObjectField("New UI.Font", newUIFont, typeof(Font), false);

            if (GUILayout.Button("Replace Fonts In Project"))
            {
                if (newTMPFont == null && newUIFont == null)
                {
                    Debug.LogWarning("Please assign at least one font.");
                    return;
                }

                if (EditorUtility.DisplayDialog("Confirm Font Replacement",
                        "This will replace TMP & UI.Text fonts in ALL scenes and prefabs in the Assets folder. Continue?", "Yes", "Cancel"))
                {
                    ReplaceInProject();
                }
            }
        }

        void ReplaceInProject()
        {
            int totalCount = 0;

            // Prefabs
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            foreach (string guid in prefabGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab)
                    totalCount += ReplaceInObject(prefab, isPrefab: true);
            }

            // Scenes
            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });
            foreach (string guid in sceneGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Scene scene = EditorSceneManager.OpenScene(path);
                totalCount += ReplaceInScene(scene);
                EditorSceneManager.SaveScene(scene);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[FontProjectReplacer] Replaced fonts in {totalCount} objects across all scenes and prefabs (Assets folder only).");
        }

        int ReplaceInObject(GameObject obj, bool isPrefab = false)
        {
            int count = 0;

            // TMP UI
            foreach (var tmp in obj.GetComponentsInChildren<TextMeshProUGUI>(true))
            {
                if (newTMPFont && tmp.font != newTMPFont)
                {
                    Undo.RecordObject(tmp, "Replace TMP Font");
                    tmp.font = newTMPFont;
                    EditorUtility.SetDirty(tmp);
                    count++;
                }
            }

            // TMP 3D
            foreach (var tmp in obj.GetComponentsInChildren<TextMeshPro>(true))
            {
                if (newTMPFont && tmp.font != newTMPFont)
                {
                    Undo.RecordObject(tmp, "Replace TMP Font");
                    tmp.font = newTMPFont;
                    EditorUtility.SetDirty(tmp);
                    count++;
                }
            }

            // Legacy UI Text
            foreach (var uiText in obj.GetComponentsInChildren<Text>(true))
            {
                if (newUIFont && uiText.font != newUIFont)
                {
                    Undo.RecordObject(uiText, "Replace UI.Font");
                    uiText.font = newUIFont;
                    EditorUtility.SetDirty(uiText);
                    count++;
                }
            }

            if (isPrefab && count > 0) PrefabUtility.SavePrefabAsset(obj);
            return count;
        }

        int ReplaceInScene(Scene scene)
        {
            int count = 0;

            foreach (var tmp in FindObjectsOfType<TextMeshProUGUI>(true))
            {
                if (newTMPFont && tmp.font != newTMPFont)
                {
                    Undo.RecordObject(tmp, "Replace TMP Font");
                    tmp.font = newTMPFont;
                    EditorUtility.SetDirty(tmp);
                    count++;
                }
            }

            foreach (var tmp in FindObjectsOfType<TextMeshPro>(true))
            {
                if (newTMPFont && tmp.font != newTMPFont)
                {
                    Undo.RecordObject(tmp, "Replace TMP Font");
                    tmp.font = newTMPFont;
                    EditorUtility.SetDirty(tmp);
                    count++;
                }
            }

            foreach (var uiText in FindObjectsOfType<Text>(true))
            {
                if (newUIFont && uiText.font != newUIFont)
                {
                    Undo.RecordObject(uiText, "Replace UI.Font");
                    uiText.font = newUIFont;
                    EditorUtility.SetDirty(uiText);
                    count++;
                }
            }

            return count;
        }
    }
}
