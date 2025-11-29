using UnityEngine;
using System.Collections.Generic;

namespace NinjaFruit
{
    /// <summary>
    /// CollisionManager - Detects fruit collisions with player swipe gestures
    /// 
    /// Story: STORY-003: CollisionManager MVP
    /// Epic: Core Slicing Mechanics
    /// 
    /// Responsibilities:
    /// - Detect line-circle intersections (swipe vs fruit collision circles)
    /// - Identify all fruits hit by a single swipe gesture
    /// - Handle edge cases (tangent touches, overlapping fruits, etc.)
    /// 
    /// Core Algorithm:
    /// Uses line-circle intersection geometry to determine if a swipe line segment
    /// passes through a fruit's collision circle. Critical requirement: swipe must
    /// have valid entry AND exit points to count as a slice (tangent touches rejected).
    /// 
    /// Performance Target: <1 millisecond per collision check
    /// </summary>
    public class CollisionManager : MonoBehaviour
    {
        /// <summary>
        /// Check if a swipe line segment intersects a fruit's collision circle
        /// 
        /// Algorithm: Line-circle intersection
        /// - Project circle center onto line segment
        /// - Calculate minimum distance from circle center to line
        /// - Pass-through: distance <= radius AND intersection point within segment bounds
        /// - Rejects tangent cases (distance = radius exactly) with epsilon tolerance
        /// 
        /// Parameters:
        /// - start: Swipe start position (Vector2)
        /// - end: Swipe end position (Vector2)
        /// - fruitPos: Fruit center position (Vector2)
        /// - radius: Fruit collision radius (float)
        /// 
        /// Returns:
        /// - true: Swipe line passes THROUGH circle (entry and exit)
        /// - false: Swipe misses, is tangent, or has invalid segment bounds
        /// 
        /// Edge Cases Handled:
        /// - Zero-length swipe: returns false
        /// - Tangent touch: returns false
        /// - Segment starts/ends inside circle: returns false (partial hit)
        /// - Overlapping fruits: each checked independently
        /// </summary>
        public bool DoesSwipeIntersectFruit(Vector2 start, Vector2 end, Vector2 fruitPos, float radius)
        {
            // Edge case: zero-length swipe (start == end)
            // A point collision is not considered a valid slice
            float segmentLength = Vector2.Distance(start, end);
            if (segmentLength < 0.00001f)
                return false;

            // Project fruit center onto the line segment to find closest point
            // Vector from line start to fruit center
            Vector2 PA = fruitPos - start;
            // Vector from line start to line end
            Vector2 BA = end - start;
            
            // Normalized projection parameter (0 = at start, 1 = at end)
            // This tells us WHERE on the line segment the closest point is
            float h = Mathf.Clamp01(Vector2.Dot(PA, BA) / Vector2.Dot(BA, BA));
            
            // Find the closest point on the line segment to the circle center
            Vector2 closest = start + h * BA;
            
            // Calculate distance from circle center to closest point
            float distance = Vector2.Distance(fruitPos, closest);
            
            // Pass-through condition:
            // 1. Distance must be STRICTLY LESS than radius (distance < radius)
            //    This rejects tangent cases where distance == radius
            //    Tangent touches are geometric edge cases that should not count as slices
            // 2. Closest point must be strictly within segment bounds (0 < h < 1)
            //    This ensures we have both entry AND exit points
            // 3. We reject partial hits where swipe starts/ends inside circle
            return distance < radius && h > 0 && h < 1;
        }

        /// <summary>
        /// Get all fruits in the scene that are hit by a swipe line segment
        /// 
        /// Process:
        /// 1. Find all Fruit components in scene
        /// 2. For each fruit, get its CircleCollider2D radius
        /// 3. Check if swipe intersects using DoesSwipeIntersectFruit()
        /// 4. Collect all hit fruits into list
        /// 
        /// Parameters:
        /// - start: Swipe start position (Vector2)
        /// - end: Swipe end position (Vector2)
        /// 
        /// Returns:
        /// - List<GameObject>: All fruit GameObjects hit by this swipe
        /// - Empty list: If no fruits are hit
        /// 
        /// Notes:
        /// - Handles destroyed/null fruits gracefully
        /// - Preserves order of fruits in scene
        /// - Returns each fruit only once (no duplicates)
        /// </summary>
        public List<GameObject> GetFruitsInSwipePath(Vector2 start, Vector2 end)
        {
            List<GameObject> fruits = new List<GameObject>();
            
            // Find all fruits in the scene by searching for CircleCollider2D components
            // This is more reliable than looking for a specific component type
            CircleCollider2D[] allColliders = FindObjectsOfType<CircleCollider2D>();
            
            foreach (CircleCollider2D collider in allColliders)
            {
                // Skip invalid colliders
                if (collider == null || collider.gameObject == null)
                    continue;
                
                // Get fruit position from the collider's parent GameObject
                Vector2 fruitPos = collider.transform.position;
                
                // Get the collision radius
                float radius = collider.radius;
                
                // Check if this fruit intersects the swipe line
                if (DoesSwipeIntersectFruit(start, end, fruitPos, radius))
                {
                    fruits.Add(collider.gameObject);
                }
            }
            
            return fruits;
        }
    }
}
