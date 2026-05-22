using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class ModularRadarChart : MaskableGraphic
{
    [Header("Chart Settings")]
    public float chartRadius = 100f;
    [Range(0.01f, 10f)] public float lineWidth = 2f;

    [Header("Dot Settings")]
    public bool showDots = true;
    public float dotRadius = 2.5f;
    public Color dotColor = new Color(0.10f, 0.11f, 0.12f, 0.1f);

    [Header("Outline Settings")]
    public bool showScoreOutline = true;
    [Range(0.01f, 10f)] public float outlineWidth = 2.5f;

    [Header("Data (Managed by Score Manager)")]
    [SerializeField] private List<float> normalizedScores = new List<float>();

    private const float RotationOffsetRad = -90f * Mathf.Deg2Rad;

    public override Texture mainTexture
    {
        get
        {
            if (s_WhiteTexture != null) return s_WhiteTexture;

            Texture2D tex = new Texture2D(2, 2);
            Color[] colors = new Color[] { Color.white, Color.white, Color.white, Color.white };
            tex.SetPixels(colors);
            tex.Apply();
            return tex;
        }
    }

    public void UpdateChartData(List<float> rawScores, float globalMaxValue)
    {
        normalizedScores.Clear();
        float divisor = globalMaxValue <= 0 ? 10f : globalMaxValue;

        foreach (float score in rawScores)
        {
            normalizedScores.Add(Mathf.Clamp01(score / divisor));
        }

        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (normalizedScores.Count < 3) return;

        int categoriesCount = normalizedScores.Count;
        float angleStep = 360f / categoriesCount;

        // 1. Draw Background Outline Web
        DrawWebOutline(vh, categoriesCount, angleStep);

        // 2. Draw Center Player Shaded Shape
        DrawScorePolygon(vh, categoriesCount, angleStep);

        // 3. Draw Outer Score Outline (Same color as the dots!)
        if (showScoreOutline)
        {
            DrawScoreOutline(vh, categoriesCount, angleStep);
        }

        // 4. Draw Dark Circles on Top
        if (showDots)
        {
            DrawScoreCircles(vh, categoriesCount, angleStep);
        }
    }

    private void DrawWebOutline(VertexHelper vh, int count, float angleStep)
    {
        for (int i = 0; i < count; i++)
        {
            float currentAngle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float nextAngle = (((i + 1) % count) * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;

            Vector2 startPos = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * chartRadius;
            Vector2 endPos = new Vector2(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle)) * chartRadius;

            DrawLine(vh, startPos, endPos, Color.gray, lineWidth);
            DrawLine(vh, Vector2.zero, startPos, Color.gray * 0.6f, lineWidth);
        }
    }

    private void DrawScorePolygon(VertexHelper vh, int count, float angleStep)
    {
        int baseIndex = vh.currentVertCount;

        UIVertex centerVert = UIVertex.simpleVert;
        centerVert.color = color;
        centerVert.position = Vector2.zero;
        vh.AddVert(centerVert);

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float currentRadius = normalizedScores[i] * chartRadius;

            UIVertex scoreVert = UIVertex.simpleVert;
            scoreVert.color = color;
            scoreVert.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * currentRadius;
            vh.AddVert(scoreVert);
        }

        for (int i = 0; i < count; i++)
        {
            int current = i + 1;
            int next = ((i + 1) % count) + 1;
            vh.AddTriangle(baseIndex, baseIndex + current, baseIndex + next);
        }
    }

    private void DrawScoreOutline(VertexHelper vh, int count, float angleStep)
    {
        // Cancel out global graphic tint multiplication just like the circles do
        Color calculatedOutlineColor = dotColor;
        if (color.r > 0 && color.g > 0 && color.b > 0 && color.a > 0)
        {
            calculatedOutlineColor = new Color(
                Mathf.Clamp01(dotColor.r / color.r),
                Mathf.Clamp01(dotColor.g / color.g),
                Mathf.Clamp01(dotColor.b / color.b),
                Mathf.Clamp01(dotColor.a / color.a)
            );
        }

        for (int i = 0; i < count; i++)
        {
            float currentAngle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float nextAngle = (((i + 1) % count) * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;

            float currentRadius = normalizedScores[i] * chartRadius;
            float nextRadius = normalizedScores[(i + 1) % count] * chartRadius;

            Vector2 startPos = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius;
            Vector2 endPos = new Vector2(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle)) * nextRadius;

            // Connect score vertices with a line using the dot's matching color target
            DrawLine(vh, startPos, endPos, calculatedOutlineColor, outlineWidth);
        }
    }

    private void DrawScoreCircles(VertexHelper vh, int count, float angleStep)
    {
        const int segments = 12;
        float segmentAngleStep = 360f / segments;

        Color calculatedColor = dotColor;
        if (color.r > 0 && color.g > 0 && color.b > 0 && color.a > 0)
        {
            calculatedColor = new Color(
                Mathf.Clamp01(dotColor.r / color.r),
                Mathf.Clamp01(dotColor.g / color.g),
                Mathf.Clamp01(dotColor.b / color.b),
                Mathf.Clamp01(dotColor.a / color.a)
            );
        }

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float currentRadius = normalizedScores[i] * chartRadius;

            Vector2 circleCenter = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * currentRadius;
            int startVertIndex = vh.currentVertCount;

            UIVertex centerVert = UIVertex.simpleVert;
            centerVert.color = calculatedColor;
            centerVert.position = circleCenter;
            vh.AddVert(centerVert);

            for (int j = 0; j < segments; j++)
            {
                float rad = j * segmentAngleStep * Mathf.Deg2Rad;
                UIVertex ringVert = UIVertex.simpleVert;
                ringVert.color = calculatedColor;
                ringVert.position = circleCenter + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * dotRadius;
                vh.AddVert(ringVert);
            }

            for (int j = 0; j < segments; j++)
            {
                int currentRingVert = startVertIndex + 1 + j;
                int nextRingVert = startVertIndex + 1 + ((j + 1) % segments);

                vh.AddTriangle(startVertIndex, currentRingVert, nextRingVert);
            }
        }
    }

    // Updated DrawLine to take thickness dynamically
    private void DrawLine(VertexHelper vh, Vector2 start, Vector2 end, Color lineColor, float width)
    {
        Vector2 direction = (end - start).normalized;
        Vector2 normal = new Vector2(-direction.y, direction.x) * (width * 0.5f);

        int baseIndex = vh.currentVertCount;
        UIVertex v = UIVertex.simpleVert;
        v.color = lineColor;

        v.position = start - normal; vh.AddVert(v);
        v.position = start + normal; vh.AddVert(v);
        v.position = end + normal; vh.AddVert(v);
        v.position = end - normal; vh.AddVert(v);

        vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        vh.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }
}