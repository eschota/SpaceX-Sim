using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIWindRose : Graphic
{
    [SerializeField]
    private Color lineGoodColor = Color.green;
    [SerializeField]
    private Color lineBadColor = Color.red;
    [SerializeField]
    private float thickness = 1f;
    [SerializeField]
    private float lerpSpeed = 1f;
    [SerializeField]
    private Vector2[] points = new Vector2[0];
    [SerializeField]
    private InfoTypeIcon[] infoTypeIcons;

    private Vector2[] nextPoints = new Vector2[0];
    private Dictionary<PlaceInfoType, RectTransform> typeToIcon = new Dictionary<PlaceInfoType, RectTransform>();

    protected override void Awake()
    {
        for (int i = 0; i < infoTypeIcons.Length; i++)
        {
            typeToIcon[infoTypeIcons[i].type] = infoTypeIcons[i].icon;
        }
    }

    public void UpdateValues(float[] values, PlaceInfoType[] types)
    {
        Vector2[] points = new Vector2[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, (float)i / values.Length * 360f);
            points[i] = rotation * new Vector2(0, .1f + values[i] * .9f);
            typeToIcon[types[i]].anchoredPosition = rotation * Vector2.up * rectTransform.rect.height / 2f;
        }
        for (int i = 0; i < infoTypeIcons.Length; i++)
        {
            infoTypeIcons[i].icon.gameObject.SetActive(Array.IndexOf(types, infoTypeIcons[i].type) != -1);
        }
        nextPoints = points;
    }

    private void Update()
    {
        if (!Application.isPlaying) return;

        if (points.Length != nextPoints.Length)
        {
            points = nextPoints;
        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Vector2.Lerp(points[i], nextPoints[i], lerpSpeed * Time.unscaledDeltaTime);
            }
        }
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Length < 2) return;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 point = points[i];
            DrawVerticesForPoint(point, vh);
        }

        for (int i = 0; i < points.Length; i++)
        {
            int iNext = i + 1;
            if (iNext >= points.Length) iNext = 0;
            int index = i * 2;
            int nextIndex = iNext * 2;
            bool isReversed = Vector2.Dot(points[i].normalized, points[iNext].normalized) < 0f;
            vh.AddTriangle(index + 0, index + 1, nextIndex + (isReversed ? 0 : 1));
            vh.AddTriangle(nextIndex + 1, nextIndex + 0, index + (isReversed ? 1 : 0));
        }
    }

    private void DrawVerticesForPoint(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = Color.Lerp(lineBadColor, lineGoodColor, point.magnitude);

        Vector3 dirFromCenter = point.normalized;

        vertex.position = new Vector2(dirFromCenter.x * -thickness / 2f + rectTransform.rect.width * .5f * point.x, dirFromCenter.y * -thickness / 2f + rectTransform.rect.height * .5f * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector2(dirFromCenter.x * thickness / 2f + rectTransform.rect.width * .5f * point.x, dirFromCenter.y * thickness / 2f + rectTransform.rect.height * .5f * point.y);
        vh.AddVert(vertex);

    }

    [Serializable]
    private class InfoTypeIcon
    {
        public PlaceInfoType type;
        public RectTransform icon;
    }
}