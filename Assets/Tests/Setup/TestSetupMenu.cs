#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using NinjaFruit.Tests.Setup;

public class TestSetupMenu
{
    [MenuItem("Window/Ninja Fruit/Setup Test Prefab")]
    public static void SetupTestPrefab()
    {
        TestPrefabSetup.VerifyRequiredTags();
        TestPrefabSetup.CreateTestFruitPrefab();
        EditorUtility.DisplayDialog(
            "Test Setup Complete",
            "TestFruit.prefab has been created at:\n" +
            "Assets/Resources/Prefabs/TestFruit.prefab\n\n" +
            "You can now run Play Mode tests.",
            "OK"
        );
    }

    [MenuItem("Window/Ninja Fruit/Run All Tests")]
    public static void RunAllTests()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Test Runner");
    }
}
#endif
