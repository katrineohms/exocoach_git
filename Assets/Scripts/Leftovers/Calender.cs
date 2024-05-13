
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calender : MonoBehaviour
{
    public Text dateText;
    public int day;
    public int month;
    public int year;

    private List<string> loggedDays;

    // Start is called before the first frame update
    void Start()
    {
        day = System.DateTime.Now.Day;
        month = System.DateTime.Now.Month;
        year = System.DateTime.Now.Year;

        Debug.Log(day + month + year);

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current date is already logged
        string currentDate = FormatDate(day, month, year);
        if (!loggedDays.Contains(currentDate))
        {
            loggedDays.Add(currentDate);
            SaveLoggedDays(); // Save the updated list of logged days
        }
    }

    void SaveLoggedDays()
    {
        // Save the list of logged days to PlayerPrefs
        string daysString = string.Join(",", loggedDays.ToArray());
        PlayerPrefs.SetString("LoggedDays", daysString);
        PlayerPrefs.Save();
    }

    void LoadLoggedDays()
    {
        // Load previously logged days from PlayerPrefs
        string daysString = PlayerPrefs.GetString("LoggedDays", "");
        string[] daysArray = daysString.Split(',');
        loggedDays.AddRange(daysArray);
    }

    string FormatDate(int day, int month, int year)
    {
        return day + "/" + month + "/" + year;
    }
}
