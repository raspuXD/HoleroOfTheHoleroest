using UnityEngine;
using System.Collections.Generic;

public class PlacementArea : MonoBehaviour
{
    public LayerMask pieceLayer;

    // List of colliders instead of a single one
    public List<Collider2D> areaColliders = new List<Collider2D>();

    void Start()
    {
        // Automatically populate colliders from children if not manually set
        if (areaColliders.Count == 0)
        {
            areaColliders.AddRange(GetComponentsInChildren<Collider2D>());
        }
    }

    public float GetOverlapScore(GameObject piece)
    {
        Collider2D pieceCollider = piece.GetComponent<Collider2D>();
        if (pieceCollider == null || areaColliders.Count == 0) return 0f;

        Bounds pieceBounds = pieceCollider.bounds;
        float totalOverlapArea = 0f;

        // Calculate overlap with each placement area collider
        foreach (var areaCollider in areaColliders)
        {
            Bounds areaBounds = areaCollider.bounds;

            float xOverlap = Mathf.Max(0, Mathf.Min(pieceBounds.max.x, areaBounds.max.x) - Mathf.Max(pieceBounds.min.x, areaBounds.min.x));
            float yOverlap = Mathf.Max(0, Mathf.Min(pieceBounds.max.y, areaBounds.max.y) - Mathf.Max(pieceBounds.min.y, areaBounds.min.y));

            totalOverlapArea += xOverlap * yOverlap;
        }

        float pieceArea = pieceBounds.size.x * pieceBounds.size.y;
        float baseScore = totalOverlapArea / pieceArea;

        // Penalty for overlapping other pieces
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(pieceBounds.center, pieceBounds.size, 0f, pieceLayer);
        float overlapPenalty = 0f;

        foreach (var other in overlaps)
        {
            if (other.gameObject != piece)
            {
                Bounds otherBounds = other.bounds;

                float xO = Mathf.Max(0, Mathf.Min(pieceBounds.max.x, otherBounds.max.x) - Mathf.Max(pieceBounds.min.x, otherBounds.min.x));
                float yO = Mathf.Max(0, Mathf.Min(pieceBounds.max.y, otherBounds.max.y) - Mathf.Max(pieceBounds.min.y, otherBounds.min.y));

                float intersectArea = xO * yO;
                overlapPenalty += intersectArea / pieceArea;
            }
        }

        float finalScore = Mathf.Clamp01(baseScore - overlapPenalty);
        return finalScore;
    }
}
