using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;

namespace NinjaFruit.Tests.Utilities
{
    public static class UITestHelpers
    {
        public static Canvas CreateTestCanvas()
        {
            GameObject canvasObj = new GameObject("TestCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // Create EventSystem with InputSystemUIInputModule (for New Input System)
            var existingEventSystem = Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
            if (existingEventSystem == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
                
                // Use InputSystemUIInputModule instead of StandaloneInputModule
                eventSystemObj.AddComponent<InputSystemUIInputModule>();
            }
            
            return canvas;
        }
        
        public static TextMeshProUGUI CreateTextElement(Transform parent, string name)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);
            TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
            text.fontSize = 24;
            return text;
        }
        
        public static Image CreateImageElement(Transform parent, string name)
        {
            GameObject imgObj = new GameObject(name);
            imgObj.transform.SetParent(parent, false);
            Image img = imgObj.AddComponent<Image>();
            // Create a simple white sprite for testing
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            img.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
            return img;
        }
    }
}
