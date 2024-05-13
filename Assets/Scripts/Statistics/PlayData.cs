using System;
using System.Collections.Generic;

[Serializable]
public class DailyPlayData
{
    public string Date;
    public int TodayPlays;
    public int TodayHighScore;
    public float TodayAverageScore;
    public int TodayTotalScore;
    public float TodayTotalWristPercent;
    public float TodayTotalElbowPercent;
    public float TodayAverageWristPercent;
    public float TodayAverageElbowPercent;
}

[Serializable]
public class PlayData
{
    public List<DailyPlayData> DailyCounts = new List<DailyPlayData>();
    public int HighScore;
    public float AverageScore;
}
