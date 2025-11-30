using UnityEngine;
using System.Collections.Generic;

namespace NinjaFruit.UI
{
    /// <summary>
    /// SwipeVisualizer - Draws a visual trail when the player swipes
    /// Shows where the swipe line is for better feedback
    /// </summary>
    public class SwipeVisualizer : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] private Color trailColor = Color.white;
        [SerializeField] private float trailWidth = 0.1f;
        [SerializeField] private float trailDuration = 0.2f;
        [SerializeField] private Material lineMaterial;

        private LineRenderer currentLine;
        private List<Vector3> currentPoints = new List<Vector3>();
        private bool isDrawing = false;
        private float lineCreationTime;

        private Camera mainCamera;
        private List<TrailInfo> activeTrails = new List<TrailInfo>();

        private class TrailInfo
        {
            public GameObject gameObject;
            public LineRenderer lineRenderer;
            public float creationTime;
        }

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // Detect mouse input for drawing trail
            try
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartDrawing();
                }
                else if (Input.GetMouseButton(0) && isDrawing)
                {
                    ContinueDrawing();
                }
                else if (Input.GetMouseButtonUp(0) && isDrawing)
                {
                    StopDrawing();
                }
            }
            catch (System.InvalidOperationException)
            {
                // Input System package conflict - ignore
            }

            // Fade out existing trails
            FadeTrails();
        }

        private void StartDrawing()
        {
            isDrawing = true;
            currentPoints.Clear();

            // Create new line renderer
            GameObject lineObj = new GameObject("SwipeTrail");
            currentLine = lineObj.AddComponent<LineRenderer>();
            
            // Configure line renderer
            currentLine.startWidth = trailWidth;
            currentLine.endWidth = trailWidth;
            currentLine.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
            currentLine.startColor = trailColor;
            currentLine.endColor = trailColor;
            currentLine.useWorldSpace = true;
            currentLine.sortingOrder = 100; // Draw on top

            lineCreationTime = Time.time;

            // Add to active trails
            activeTrails.Add(new TrailInfo
            {
                gameObject = lineObj,
                lineRenderer = currentLine,
                creationTime = lineCreationTime
            });

            // Add first point
            Vector3 worldPos = GetMouseWorldPosition();
            currentPoints.Add(worldPos);
            UpdateLineRenderer();
        }

        private void ContinueDrawing()
        {
            if (currentLine == null) return;

            Vector3 worldPos = GetMouseWorldPosition();
            
            // Only add point if it's far enough from the last point
            if (currentPoints.Count == 0 || Vector3.Distance(worldPos, currentPoints[currentPoints.Count - 1]) > 0.1f)
            {
                currentPoints.Add(worldPos);
                UpdateLineRenderer();
            }
        }

        private void StopDrawing()
        {
            isDrawing = false;
            
            // Line will fade out automatically
        }

        private void UpdateLineRenderer()
        {
            if (currentLine == null || currentPoints.Count == 0) return;

            currentLine.positionCount = currentPoints.Count;
            currentLine.SetPositions(currentPoints.ToArray());
        }

        private Vector3 GetMouseWorldPosition()
        {
            if (mainCamera == null) return Vector3.zero;

            Vector3 mousePos = Vector3.zero;
            try
            {
                mousePos = Input.mousePosition;
            }
            catch (System.InvalidOperationException)
            {
                // Input System conflict
                return Vector3.zero;
            }

            mousePos.z = 10f; // Distance from camera
            return mainCamera.ScreenToWorldPoint(mousePos);
        }

        private void FadeTrails()
        {
            // Fade out existing trails
            for (int i = activeTrails.Count - 1; i >= 0; i--)
            {
                var trail = activeTrails[i];
                var line = trail.lineRenderer;
                
                // Fade based on time
                float age = Time.time - trail.creationTime;
                float alpha = 1f - (age / trailDuration);
                
                if (alpha <= 0f)
                {
                    Destroy(trail.gameObject);
                    activeTrails.RemoveAt(i);
                }
                else
                {
                    Color color = line.startColor;
                    color.a = alpha;
                    line.startColor = color;
                    line.endColor = color;
                }
            }
        }
    }
}
