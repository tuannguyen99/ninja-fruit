using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using NinjaFruit;
using NinjaFruit.Gameplay;
using NinjaFruit.UI;
using UnityEngine.InputSystem.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Automated scene builder for Ninja Fruit
/// Creates a complete playable game scene with all managers and UI
/// </summary>
public class NinjaFruitSceneBuilder : MonoBehaviour
{
    [MenuItem("Ninja Fruit/Build Gameplay Scene")]
    public static void BuildGameplayScene()
    {
        Debug.Log("=== Building Ninja Fruit Gameplay Scene ===");
        
        // Create new scene
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        
        // Setup camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 6;
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.15f); // Dark blue background
        }
        
        // Create Game Manager hierarchy
        GameObject gameRoot = new GameObject("=== GAME MANAGERS ===");
        
        // 1. Game Manager
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.transform.SetParent(gameRoot.transform);
        GameManager gameManager = gameManagerObj.AddComponent<GameManager>();
        
        // 2. Fruit Spawner
        GameObject spawnerObj = new GameObject("FruitSpawner");
        spawnerObj.transform.SetParent(gameRoot.transform);
        spawnerObj.transform.position = new Vector3(0, 6, 0); // Spawn from top
        FruitSpawner spawner = spawnerObj.AddComponent<FruitSpawner>();
        
        // 3. Swipe Detector
        GameObject swipeObj = new GameObject("SwipeDetector");
        swipeObj.transform.SetParent(gameRoot.transform);
        SwipeDetector swipeDetector = swipeObj.AddComponent<SwipeDetector>();
        
        // 4. Collision Manager
        GameObject collisionObj = new GameObject("CollisionManager");
        collisionObj.transform.SetParent(gameRoot.transform);
        CollisionManager collisionManager = collisionObj.AddComponent<CollisionManager>();
        
        // 5. Score Manager
        GameObject scoreObj = new GameObject("ScoreManager");
        scoreObj.transform.SetParent(gameRoot.transform);
        ScoreManager scoreManager = scoreObj.AddComponent<ScoreManager>();
        
        // 6. Game State Controller
        GameObject stateObj = new GameObject("GameStateController");
        stateObj.transform.SetParent(gameRoot.transform);
        GameStateController stateController = stateObj.AddComponent<GameStateController>();
        
        Debug.Log("✓ Game managers created");
        
        // Create UI
        GameObject uiRoot = new GameObject("=== UI ===");
        
        // Canvas
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.transform.SetParent(uiRoot.transform);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Event System
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.transform.SetParent(uiRoot.transform);
        eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObj.AddComponent<InputSystemUIInputModule>();
        
        // HUD Controller
        GameObject hudObj = new GameObject("HUDController");
        hudObj.transform.SetParent(canvasObj.transform);
        NinjaFruit.UI.HUDController hudController = hudObj.AddComponent<NinjaFruit.UI.HUDController>();
        RectTransform hudRect = hudObj.AddComponent<RectTransform>();
        hudRect.anchorMin = Vector2.zero;
        hudRect.anchorMax = Vector2.one;
        hudRect.offsetMin = Vector2.zero;
        hudRect.offsetMax = Vector2.zero;
        
        // Score Text
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(hudObj.transform);
        TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "0";
        scoreText.fontSize = 72;
        scoreText.alignment = TextAlignmentOptions.TopRight;
        scoreText.color = Color.white;
        scoreText.fontStyle = FontStyles.Bold;
        RectTransform scoreRect = scoreTextObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(1, 1);
        scoreRect.anchorMax = new Vector2(1, 1);
        scoreRect.pivot = new Vector2(1, 1);
        scoreRect.anchoredPosition = new Vector2(-50, -50);
        scoreRect.sizeDelta = new Vector2(400, 100);
        
        // Combo Text
        GameObject comboTextObj = new GameObject("ComboText");
        comboTextObj.transform.SetParent(hudObj.transform);
        TextMeshProUGUI comboText = comboTextObj.AddComponent<TextMeshProUGUI>();
        comboText.text = "COMBO 2x!";
        comboText.fontSize = 48;
        comboText.alignment = TextAlignmentOptions.Center;
        comboText.color = Color.yellow;
        comboText.fontStyle = FontStyles.Bold;
        RectTransform comboRect = comboTextObj.GetComponent<RectTransform>();
        comboRect.anchorMin = new Vector2(0.5f, 1);
        comboRect.anchorMax = new Vector2(0.5f, 1);
        comboRect.pivot = new Vector2(0.5f, 1);
        comboRect.anchoredPosition = new Vector2(0, -150);
        comboRect.sizeDelta = new Vector2(600, 80);
        comboTextObj.SetActive(false);
        
        // Lives Hearts
        Image[] hearts = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject heartObj = new GameObject($"Heart{i + 1}");
            heartObj.transform.SetParent(hudObj.transform);
            Image heartImage = heartObj.AddComponent<Image>();
            heartImage.color = Color.red;
            RectTransform heartRect = heartObj.GetComponent<RectTransform>();
            heartRect.anchorMin = new Vector2(0, 1);
            heartRect.anchorMax = new Vector2(0, 1);
            heartRect.pivot = new Vector2(0, 1);
            heartRect.anchoredPosition = new Vector2(50 + (i * 80), -50);
            heartRect.sizeDelta = new Vector2(60, 60);
            hearts[i] = heartImage;
        }
        
        // Wire HUD references
        hudController.SetReferences(scoreText, comboText, hearts);
        hudController.SetManagers(scoreManager, stateController);
        
        Debug.Log("✓ UI created");
        
        // Create Swipe Visualizer
        GameObject swipeVisObj = new GameObject("SwipeVisualizer");
        swipeVisObj.transform.SetParent(uiRoot.transform);
        swipeVisObj.AddComponent<SwipeVisualizer>();
        
        Debug.Log("✓ Swipe visualizer created");
        
        // Create fruit prefab
        CreateFruitPrefab();
        CreateBombPrefab();
        
        // Assign prefabs to spawner
        GameObject fruitPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/FruitPrefab.prefab");
        GameObject bombPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/BombPrefab.prefab");
        if (fruitPrefab != null)
        {
            spawner.SetFruitPrefabs(new GameObject[] { fruitPrefab });
        }
        if (bombPrefab != null)
        {
            // Note: spawner uses resource loading for bombs, but we can set it up
        }
        
        Debug.Log("✓ Prefabs created and assigned");
        
        // Save scene
        string scenePath = "Assets/Scenes/GameplayScene.unity";
        EditorSceneManager.SaveScene(newScene, scenePath);
        
        Debug.Log($"=== Scene created successfully! ===");
        Debug.Log($"Scene saved to: {scenePath}");
        Debug.Log("Press PLAY to test the game!");
        
        // Select the GameManager to show in inspector
        Selection.activeGameObject = gameManagerObj;
    }
    
    private static void CreateFruitPrefab()
    {
        // Create a simple fruit prefab
        GameObject fruitPrefab = new GameObject("FruitPrefab");
        
        // Add visual (sprite renderer)
        SpriteRenderer spriteRenderer = fruitPrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite(Color.red, 128);
        spriteRenderer.sortingOrder = 10;
        
        // Add physics
        Rigidbody2D rb = fruitPrefab.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.5f;
        
        // Add collider
        CircleCollider2D collider = fruitPrefab.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        
        // Add Fruit component
        Fruit fruit = fruitPrefab.AddComponent<Fruit>();
        fruit.Type = FruitType.Apple;
        
        // Save as prefab
        string prefabPath = "Assets/Resources/Prefabs/FruitPrefab.prefab";
        System.IO.Directory.CreateDirectory("Assets/Resources/Prefabs");
        PrefabUtility.SaveAsPrefabAsset(fruitPrefab, prefabPath);
        DestroyImmediate(fruitPrefab);
        
        Debug.Log($"✓ Fruit prefab created at {prefabPath}");
    }
    
    private static void CreateBombPrefab()
    {
        // Create a simple bomb prefab
        GameObject bombPrefab = new GameObject("BombPrefab");
        
        // Add visual (sprite renderer)
        SpriteRenderer spriteRenderer = bombPrefab.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite(Color.black, 128);
        spriteRenderer.sortingOrder = 10;
        
        // Add physics
        Rigidbody2D rb = bombPrefab.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.5f;
        
        // Add collider
        CircleCollider2D collider = bombPrefab.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        
        // Add Bomb component
        bombPrefab.AddComponent<Bomb>();
        
        // Save as prefab
        string prefabPath = "Assets/Resources/Prefabs/BombPrefab.prefab";
        System.IO.Directory.CreateDirectory("Assets/Resources/Prefabs");
        PrefabUtility.SaveAsPrefabAsset(bombPrefab, prefabPath);
        DestroyImmediate(bombPrefab);
        
        Debug.Log($"✓ Bomb prefab created at {prefabPath}");
    }
    
    private static Sprite CreateCircleSprite(Color color, int resolution)
    {
        Texture2D texture = new Texture2D(resolution, resolution);
        Color[] pixels = new Color[resolution * resolution];
        
        Vector2 center = new Vector2(resolution / 2f, resolution / 2f);
        float radius = resolution / 2f - 2;
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);
                
                if (distance <= radius)
                {
                    pixels[y * resolution + x] = color;
                }
                else
                {
                    pixels[y * resolution + x] = Color.clear;
                }
            }
        }
        
        texture.SetPixels(pixels);
        texture.Apply();
        
        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), 100);
    }
}
#endif
