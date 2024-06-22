using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class TimesPlayed : MonoBehaviour
{
    public PlayData playData = new PlayData();
    public ArmDataObj armDataObj = new ArmDataObj();
    private string filePath0;
    private string filePath1;
    private string filePath2;
    public RightLeftHand rightleftData;

    void Start()
    {
        filePath0 = Path.Combine(Application.persistentDataPath, "playData.json");
        string timedate = DateTime.Now.ToString("yyyy-MM-dd");
        string fileName = "Armdata" + timedate + ".json";
        filePath1 = Path.Combine(Application.persistentDataPath, fileName);
        LoadData();
    }

    public void LoadData()
    {
        if (File.Exists(filePath0))
        {
            Debug.Log("Loading data");
            string json = File.ReadAllText(filePath0);
            playData = JsonUtility.FromJson<PlayData>(json);
        }
        else
        {
            Debug.Log("No play data, creating new data");
            // If file doesn't exist, create a new PlayData object
            playData = new PlayData();

            // Create a new DailyPlayData object for today's date and add it to the DailyCounts list
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            playData.DailyCounts.Add(new DailyPlayData { Date = today, TodayPlays = 0, TodayHighScore = 0, TodayAverageScore = 0 });

            // Save the new data
            SaveData();
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playData, true);
        File.WriteAllText(filePath0, json);



        armDataObj.master = rightleftData.armDataMasterList;
        armDataObj.secondary = rightleftData.armDataSecondaryList;
        Debug.Log("sec len : " + armDataObj.secondary.Count);
        string jsonArmData = JsonUtility.ToJson(armDataObj, true);
        Debug.Log("json : " + jsonArmData);

        File.WriteAllText(filePath1, jsonArmData);


    }

    public void DisplayCount(Text countText)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DailyPlayData todayData = playData.DailyCounts.Find(x => x.Date == today);

        // Check if data for today exists
        if (todayData != null)
        {
            int count = todayData.TodayPlays;
            int highScore = todayData.TodayHighScore;
            float averageScore = todayData.TodayAverageScore;

            // Set text to display count, high score, and average score for today
            countText.text = $"Times played today: {count}\nHighscore: {highScore}\nAverage score: {averageScore:F2}";
        }
        else
        {
            // If no data for today, display default message
            countText.text = "No data available for today";
        }
    }

    public void DisplayPercentage(Text percentageText)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DailyPlayData todayData = playData.DailyCounts.Find(x => x.Date == today);

        // Check if data for today exists
        if (todayData != null)
        {
            float wristControl = todayData.TodayAverageWristPercent;
            float elbowControl = todayData.TodayAverageElbowPercent;

            // Set text to display count, high score, and average score for today
            percentageText.text = $"Wrist control: {wristControl}% \n \n \n \n \n Elbow control: {elbowControl}%";
        }
        else
        {
            // If no data for today, display default message
            percentageText.text = "No data available for today";
        }
    }

    public void IncrementCount(int score)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        DailyPlayData todayData = playData.DailyCounts.Find(x => x.Date == today);
        (float wristPercent, float elbowPercent) = rightleftData.GetDifference();

        if (todayData != null)
        {
            // Increment count for today
            todayData.TodayPlays++;
            todayData.TodayTotalScore += score;

            todayData.TodayTotalWristPercent += wristPercent;
            todayData.TodayAverageWristPercent = todayData.TodayTotalWristPercent / todayData.TodayPlays;

            todayData.TodayTotalElbowPercent += elbowPercent;
            todayData.TodayAverageElbowPercent = todayData.TodayTotalElbowPercent / todayData.TodayPlays;

            Debug.Log("Wrist percent:" + todayData.TodayTotalWristPercent + "Elbow percent:" + todayData.TodayTotalElbowPercent);
            Debug.Log("Average Elbow: " + todayData.TodayAverageElbowPercent + "%  Average Wrist: " + todayData.TodayAverageWristPercent + "%");
            rightleftData.Reset();

            if (score > todayData.TodayHighScore)
            {
                todayData.TodayHighScore = score;
            }
        }
        else
        {
            // Create new entry for today
            todayData = new DailyPlayData
            {
                Date = today,
                TodayPlays = 1,
                TodayTotalScore = score,
                TodayHighScore = score,
                TodayAverageElbowPercent = elbowPercent,
                TodayAverageWristPercent = wristPercent,
                TodayTotalElbowPercent = elbowPercent,
                TodayTotalWristPercent = wristPercent,
            };
            playData.DailyCounts.Add(todayData); // Add the new entry to the list
        }

        todayData.TodayAverageScore = (float)todayData.TodayTotalScore / todayData.TodayPlays;
        SaveData();
        Debug.Log($"Action performed. Current plays for today ({today}): {todayData.TodayPlays}. High Score for today: {todayData.TodayHighScore}. Average Score for today: {todayData.TodayAverageScore}");
    }

    public void ResetData()
    {
        filePath0 = Path.Combine(Application.persistentDataPath, "playData.json");
        if (File.Exists(filePath0))
        {
            File.Delete(filePath0);
            Debug.Log("Play data file deleted.");
        }
        else
        {
            Debug.Log("Play data file not found.");
        }
    }

    public void PrintPercentage(Text countText)
    {
        string elbowText = "Elbow percentage \n";
        string wristText = "Wrist percentage \n";
        string dataText = "";

        for (int i = 0; i < 14; i++)
        {
            DateTime currentDate = DateTime.Now.AddDays(-i);
            string dateToCheck = currentDate.ToString("yyyy-MM-dd");

            DailyPlayData dailyData = playData.DailyCounts.Find(x => x.Date == dateToCheck);

            if (dailyData != null)
            {
                elbowText = "Date: " + dateToCheck + ", Elbow: " + Math.Round(dailyData.TodayAverageElbowPercent, 2);
                wristText = "Wrist: " + Math.Round(dailyData.TodayAverageWristPercent, 2);
            }
            else
            {
                elbowText = "Date: " + dateToCheck + ", Elbow: No data";
                wristText = "Wrist: No data";
            }
            dataText += elbowText + "      " + wristText + "\n";
        }

        countText.text = dataText;
    }
}
