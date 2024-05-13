using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    public RectTransform graphArea;
    public GameObject dataPointPrefab;

    public Vector2[] dataPoints = new Vector2[]
    {
        new Vector2(1, 100), // Day 1: 100 plays
        new Vector2(2, 120), // Day 2: 120 plays
        new Vector2(3, 90),  // Day 3: 90 plays
        new Vector2(4, 150), // Day 4: 150 plays
        new Vector2(5, 200), // Day 5: 200 plays
        new Vector2(6, 180), // Day 6: 180 plays
        new Vector2(7, 210)  // Day 7: 210 plays
    };

    void Start()
    {
        DrawGraph();
    }

    public void DrawGraph()
    {
        for (int i = 0; i < dataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(dataPoints[i]);
            InstantiateDataPoint(anchoredPosition);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(dataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition);
            }
        }
    }

    Vector2 CalculateAnchoredPosition(Vector2 dataPoint)
    {
        float xPercentage = (dataPoint.x - 1f) / 6f;
        float yPercentage = dataPoint.y / 210f;

        return new Vector2(
            xPercentage * graphArea.rect.width,
            yPercentage * graphArea.rect.height
        );
    }

    void InstantiateDataPoint(Vector2 anchoredPosition)
    {
        GameObject dataPoint = Instantiate(dataPointPrefab, graphArea);
        RectTransform dataPointTransform = dataPoint.GetComponent<RectTransform>();
        dataPointTransform.anchoredPosition = anchoredPosition;
    }

    void DrawLineBetweenPoints(Vector2 startPoint, Vector2 endPoint)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(graphArea, false);

        Image lineImage = line.GetComponent<Image>();
        lineImage.color = Color.black;
        lineImage.rectTransform.sizeDelta = new Vector2(Vector2.Distance(startPoint, endPoint), 2f);
        lineImage.rectTransform.pivot = Vector2.zero;
        lineImage.rectTransform.anchorMin = Vector2.zero;
        lineImage.rectTransform.anchorMax = Vector2.zero;
        lineImage.rectTransform.anchoredPosition = startPoint;
        lineImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg);
    }
}
