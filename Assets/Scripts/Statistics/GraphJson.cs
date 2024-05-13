using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
public class GraphJson : MonoBehaviour
{
    public RectTransform graphArea;
    public PlayData playData;
    private string filePath;
    public TimesPlayed timesPlayedScript;
    public float height;
    public float width;

    [Header("Datapoint prefab")]
    public GameObject HighScorePoint;
    public GameObject AverageScorePoint;
    public GameObject TimesPlayedPoint;

    private Vector2[] timesPlayedDataPoints = new Vector2[7];
    private Vector2[] averageScoreDataPoints = new Vector2[7];
    private Vector2[] highScoreDataPoints = new Vector2[7];
    private Vector2[] elbowDataPoints = new Vector2[7];
    private Vector2[] wristDataPoints = new Vector2[7];

    void Start()
    {

        filePath = Path.Combine(Application.persistentDataPath, "playData.json");
        timesPlayedScript.LoadData();
        playData = timesPlayedScript.playData;
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DailyPlayData todayData = playData.DailyCounts.Find(x => x.Date == today);
    }


    public void CollectData()
    {
        for (int i = 0; i < 7; i++)
        {
            // Get the date for the current day in the iteration
            string dateToCheck = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");

            // Find data for the current day
            DailyPlayData dailyData = playData.DailyCounts.Find(x => x.Date == dateToCheck);

            // If data exists for the current day
            if (dailyData != null)
            {
                timesPlayedDataPoints[i] = new Vector2(i, dailyData.TodayPlays); // Use dailyData here
                averageScoreDataPoints[i] = new Vector2(i, dailyData.TodayHighScore);
                highScoreDataPoints[i] = new Vector2(i, dailyData.TodayAverageScore);
                elbowDataPoints[i] = new Vector2(i, dailyData.TodayAverageElbowPercent);
                wristDataPoints[i] = new Vector2(i, dailyData.TodayAverageWristPercent);
            }
            else
            {
                timesPlayedDataPoints[i] = new Vector2(i, 0);
                averageScoreDataPoints[i] = new Vector2(i, 0);
                highScoreDataPoints[i] = new Vector2(i, 0);
                elbowDataPoints[i] = new Vector2(i, 0);
                wristDataPoints[i] = new Vector2(i, 0);
            }
        }
    }


    public void DrawGameGraph()
    {
        CollectData();
        //Times played
        for (int i = 0; i < timesPlayedDataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(timesPlayedDataPoints[i]);
            InstantiateDataPoint(anchoredPosition, TimesPlayedPoint);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(timesPlayedDataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition, Color.red);
            }
        }

        //Average Score
        for (int i = 0; i < averageScoreDataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(averageScoreDataPoints[i]);
            InstantiateDataPoint(anchoredPosition, AverageScorePoint);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(averageScoreDataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition, Color.blue);
            }
        }

        //Highscore
        for (int i = 0; i < highScoreDataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(highScoreDataPoints[i]);
            InstantiateDataPoint(anchoredPosition, HighScorePoint);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(highScoreDataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition, Color.black);
            }
        }
    }


    public void DrawPercentGraph()
    {
        CollectData();
        //Elbow Score
        for (int i = 0; i < elbowDataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(elbowDataPoints[i]);
            InstantiateDataPoint(anchoredPosition, AverageScorePoint);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(elbowDataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition, Color.blue);
            }
        }

        //Wrist data
        for (int i = 0; i < wristDataPoints.Length; i++)
        {
            Vector2 anchoredPosition = CalculateAnchoredPosition(wristDataPoints[i]);
            InstantiateDataPoint(anchoredPosition, HighScorePoint);

            if (i > 0)
            {
                Vector2 prevAnchoredPosition = CalculateAnchoredPosition(wristDataPoints[i - 1]);
                DrawLineBetweenPoints(prevAnchoredPosition, anchoredPosition, Color.black);
            }
        }
    }

    Vector2 CalculateAnchoredPosition(Vector2 dataPoint)
    {
        float xPercentage = (dataPoint.x - 1f) / width;
        float yPercentage = dataPoint.y / height;

        return new Vector2(
            xPercentage * graphArea.rect.width,
            yPercentage * graphArea.rect.height
        );
    }

    void InstantiateDataPoint(Vector2 anchoredPosition, GameObject dataPointPrefab)
    {
        GameObject dataPoint = Instantiate(dataPointPrefab, graphArea);
        RectTransform dataPointTransform = dataPoint.GetComponent<RectTransform>();
        dataPointTransform.anchoredPosition = anchoredPosition;
    }

    void DrawLineBetweenPoints(Vector2 startPoint, Vector2 endPoint, Color color)
    {
        GameObject line = new GameObject("Line", typeof(Image));
        line.transform.SetParent(graphArea, false);

        Image lineImage = line.GetComponent<Image>();
        lineImage.color = color;
        lineImage.rectTransform.sizeDelta = new Vector2(Vector2.Distance(startPoint, endPoint), 2f);
        lineImage.rectTransform.pivot = Vector2.zero;
        lineImage.rectTransform.anchorMin = Vector2.zero;
        lineImage.rectTransform.anchorMax = Vector2.zero;
        lineImage.rectTransform.anchoredPosition = startPoint;
        lineImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x) * Mathf.Rad2Deg);
    }


}
