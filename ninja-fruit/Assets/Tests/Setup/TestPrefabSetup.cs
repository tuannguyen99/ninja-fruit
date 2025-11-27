using UnityEngine;

namespace NinjaFruit.Tests.Setup
{
    /// <summary>
    /// Editor setup utility to create TestFruit.prefab for Play Mode tests
    /// Run this once per project: Window > Ninja Fruit > Setup Test Prefab
    /// </summary>
    public class TestPrefabSetup
    {
        /// <summary>
        /// Creates TestFruit.prefab in Assets/Resources/Prefabs/TestFruit.prefab
        /// This prefab is required for Play Mode tests (TEST-011 through TEST-014)
        /// </summary>
        public static void CreateTestFruitPrefab()
        {
            string prefabPath = "Assets/Resources/Prefabs";
            string fullPrefabPath = prefabPath + "/TestFruit.prefab";

            // Create Resources/Prefabs directory if needed
            if (!System.IO.Directory.Exists(prefabPath))
            {
                System.IO.Directory.CreateDirectory(prefabPath);
                Debug.Log($"[TestSetup] Created directory: {prefabPath}");
            }

            // Create GameObject
            GameObject testFruit = new GameObject("TestFruit");
            testFruit.tag = "Fruit"; // Requires "Fruit" tag to exist in project

            // Add Rigidbody2D
            Rigidbody2D rb = testFruit.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f;
            rb.mass = 1.0f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // Add CircleCollider2D
            CircleCollider2D collider = testFruit.AddComponent<CircleCollider2D>();
            collider.radius = 0.3f;
            collider.isTrigger = false;

            // Save as prefab
            #if UNITY_EDITOR
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(testFruit, fullPrefabPath);
            Debug.Log($"[TestSetup] Created TestFruit prefab at: {fullPrefabPath}");
            
            // Cleanup the temporary GameObject
            Object.DestroyImmediate(testFruit);
            #endif
        }

        /// <summary>
        /// Verifies required game tags exist
        /// </summary>
        public static void VerifyRequiredTags()
        {
            string[] requiredTags = { "Fruit" };
            
            foreach (string tag in requiredTags)
            {
                if (!TagExists(tag))
                {
                    CreateTag(tag);
                    Debug.Log($"[TestSetup] Created tag: {tag}");
                }
            }
        }

        private static bool TagExists(string tag)
        {
            #if UNITY_EDITOR
            var tagManager = UnityEditor.AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
            if (tagManager == null) return false;
            var so = new UnityEditor.SerializedObject(tagManager);
            var tagsProp = so.FindProperty("tags");
            if (tagsProp == null) return false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                var t = tagsProp.GetArrayElementAtIndex(i).stringValue;
                if (t == tag) return true;
            }
            return false;
            #else
            return true;
            #endif
        }

        private static void CreateTag(string tag)
        {
            #if UNITY_EDITOR
            var tagManager = UnityEditor.AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
            if (tagManager == null) return;

            var so = new UnityEditor.SerializedObject(tagManager);
            var tagsProperty = so.FindProperty("tags");
            if (tagsProperty == null) return;

            // Avoid duplicates
            for (int i = 0; i < tagsProperty.arraySize; i++)
            {
                if (tagsProperty.GetArrayElementAtIndex(i).stringValue == tag)
                    return;
            }

            // Append new tag at the end
            int insertIndex = tagsProperty.arraySize;
            tagsProperty.InsertArrayElementAtIndex(insertIndex);
            tagsProperty.GetArrayElementAtIndex(insertIndex).stringValue = tag;
            so.ApplyModifiedProperties();
            #endif
        }
    }
}
