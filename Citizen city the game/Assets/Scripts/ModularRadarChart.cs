using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class ModularRadarChart : MaskableGraphic
{
    [Header("Chart Sizing")]
    public float chartRadius = 140f;

    [Header("Edge Icon Distribution")]
    [Tooltip("Assign the 5 Icon GameObjects here in order: AUT, STR, BOM, POP, HUI")]
    public RectTransform[] edgeIcons = new RectTransform[5];
    [Tooltip("Extra pixel padding pushing the icons past the outer web boundary line.")]
    public float iconPaddingMargin = 25f;

    [Header("Web Background Fill")]
    public bool showBackgroundFill = true;
    public Color webBackgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.15f);

    [Header("Outer Web Frame")]
    [Range(0.01f, 10f)] public float outerWebWidth = 3f;
    public Color outerWebColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    [Header("Inner Grid Lines")]
    [Tooltip("How many nested inner rings to draw under the outer boundary.")]
    [Range(1, 10)] public int gridLevels = 4;
    [Range(0.01f, 10f)] public float innerWebWidth = 2f;
    public Color innerWebColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

    [Header("Dot Settings")]
    public bool showDots = true;
    public float dotRadius = 3f;

    [Header("Outline Settings")]
    public bool showScoreOutline = true;
    [Range(0.01f, 10f)] public float scoreOutlineWidth = 3f;

    [Header("Player Assignment")]
    [Range(0, 3)]
    [SerializeField] private int playerIndex = 0;

    [Header("Data (Managed by Score Manager)")]
    [SerializeField] private List<float> normalizedScores = new List<float>();

    private const float RotationOffsetRad = -90f * Mathf.Deg2Rad;

    // The UNO-style identity palette
    private static readonly Color[] PlayerPalette = new Color[]
    {
        new Color(0.00f, 0.49f, 1.00f, 1f), // Player 1: Blue (#007EFF)
        new Color(1.00f, 0.00f, 0.00f, 1f), // Player 0: Red (#FF0000)
        new Color(0.00f, 0.61f, 0.07f, 1f), // Player 2: Green (#009B12)
        new Color(1.00f, 0.68f, 0.00f, 1f)  // Player 3: Yellow (#FFAD00)
    };

    private Color CurrentPlayerSolidColor => PlayerPalette[Mathf.Clamp(playerIndex, 0, 3)];
    private Color CurrentPlayerFillColor
    {
        get
        {
            Color solid = CurrentPlayerSolidColor;
            return new Color(solid.r, solid.g, solid.b, 0.35f);
        }
    }

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

    public void UpdateChartData(int assignedPlayerIndex, List<float> rawScores, float globalMaxValue)
    {
        this.playerIndex = assignedPlayerIndex;
        normalizedScores.Clear();
        float divisor = globalMaxValue <= 0 ? 10f : globalMaxValue;

        foreach (float score in rawScores)
        {
            normalizedScores.Add(Mathf.Clamp01(score / divisor));
        }

        // Dynamically shift icon layout positions to match current math parameters
        PositionEdgeIcons();

        SetVerticesDirty();
    }

    private void PositionEdgeIcons()
    {
        if (edgeIcons == null || edgeIcons.Length == 0) return;

        int categoriesCount = normalizedScores.Count > 0 ? normalizedScores.Count : 5;
        float angleStep = 360f / categoriesCount;
        float placementRadius = chartRadius + iconPaddingMargin;

        for (int i = 0; i < edgeIcons.Length; i++)
        {
            if (edgeIcons[i] == null) continue;

            // Compute exact trajectory angle for the edge tip vector
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            Vector2 directionalVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Set the anchored position relative to the chart center (0,0)
            edgeIcons[i].anchoredPosition = directionalVector * placementRadius;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (normalizedScores.Count < 3) return;

        int categoriesCount = normalizedScores.Count;
        float angleStep = 360f / categoriesCount;

        // 1. Draw the absolute base background polygon fill first
        if (showBackgroundFill)
        {
            DrawBackgroundTotalFill(vh, categoriesCount, angleStep);
        }

        // 2. Draw Background Grid Structure
        DrawWebGrid(vh, categoriesCount, angleStep);

        // 3. Draw Translucent Player Filled Polygon
        DrawScorePolygon(vh, categoriesCount, angleStep);

        // 4. Draw Solid Player Perimeter Outline
        if (showScoreOutline)
        {
            DrawScoreOutline(vh, categoriesCount, angleStep);
        }

        // 5. Draw Solid Player Dots
        if (showDots)
        {
            DrawScoreCircles(vh, categoriesCount, angleStep);
        }
    }

    private void DrawBackgroundTotalFill(VertexHelper vh, int count, float angleStep)
    {
        int baseIndex = vh.currentVertCount;

        UIVertex centerVert = UIVertex.simpleVert;
        centerVert.color = webBackgroundColor;
        centerVert.position = Vector2.zero;
        vh.AddVert(centerVert);

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;

            UIVertex bgVert = UIVertex.simpleVert;
            bgVert.color = webBackgroundColor;
            bgVert.position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * chartRadius;
            vh.AddVert(bgVert);
        }

        for (int i = 0; i < count; i++)
        {
            int current = i + 1;
            int next = ((i + 1) % count) + 1;
            vh.AddTriangle(baseIndex, baseIndex + current, baseIndex + next);
        }
    }

    private void DrawWebGrid(VertexHelper vh, int count, float angleStep)
    {
        for (int level = 1; level <= gridLevels; level++)
        {
            float levelRadiusFraction = (float)level / gridLevels;
            float currentRadius = chartRadius * levelRadiusFraction;

            bool isOuterFrame = (level == gridLevels);
            float activeWidth = isOuterFrame ? outerWebWidth : innerWebWidth;
            Color activeColor = isOuterFrame ? outerWebColor : innerWebColor;

            for (int i = 0; i < count; i++)
            {
                float currentAngle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
                float nextAngle = (((i + 1) % count) * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;

                Vector2 startPos = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius;
                Vector2 endPos = new Vector2(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle)) * currentRadius;

                DrawLine(vh, startPos, endPos, activeColor, activeWidth);
            }
        }

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            Vector2 outerEdgePoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * chartRadius;

            DrawLine(vh, Vector2.zero, outerEdgePoint, innerWebColor * 1.2f, innerWebWidth);
        }
    }

    private void DrawScorePolygon(VertexHelper vh, int count, float angleStep)
    {
        int baseIndex = vh.currentVertCount;
        Color fillColor = CurrentPlayerFillColor;

        UIVertex centerVert = UIVertex.simpleVert;
        centerVert.color = fillColor;
        centerVert.position = Vector2.zero;
        vh.AddVert(centerVert);

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float currentRadius = normalizedScores[i] * chartRadius;

            UIVertex scoreVert = UIVertex.simpleVert;
            scoreVert.color = fillColor;
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
        Color outlineColor = CurrentPlayerSolidColor;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float nextAngle = (((i + 1) % count) * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;

            float currentRadius = normalizedScores[i] * chartRadius;
            float nextRadius = normalizedScores[(i + 1) % count] * chartRadius;

            Vector2 startPos = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * currentRadius;
            Vector2 endPos = new Vector2(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle)) * nextRadius;

            DrawLine(vh, startPos, endPos, outlineColor, scoreOutlineWidth);
        }
    }

    private void DrawScoreCircles(VertexHelper vh, int count, float angleStep)
    {
        const int segments = 12;
        float segmentAngleStep = 360f / segments;
        Color dotColor = CurrentPlayerSolidColor;

        for (int i = 0; i < count; i++)
        {
            float angle = (i * angleStep * Mathf.Deg2Rad) + RotationOffsetRad;
            float currentRadius = normalizedScores[i] * chartRadius;

            Vector2 circleCenter = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * currentRadius;
            int startVertIndex = vh.currentVertCount;

            UIVertex centerVert = UIVertex.simpleVert;
            centerVert.color = dotColor;
            centerVert.position = circleCenter;
            vh.AddVert(centerVert);

            for (int j = 0; j < segments; j++)
            {
                float rad = j * segmentAngleStep * Mathf.Deg2Rad;
                UIVertex ringVert = UIVertex.simpleVert;
                ringVert.color = dotColor;
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

    // Fallback alignment engine context safety routine
    protected void OnValidate()
    {
        PositionEdgeIcons();
    }
}