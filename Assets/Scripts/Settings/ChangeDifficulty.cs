using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDifficulty : MonoBehaviour
{
    [SerializeField] public string defaultDifficulty;

    [SerializeField] public string PlayerPrefClass; //I.e. "CurrentSkinTexture"

    private void Start()
    {

        string savedDifficulty = PlayerPrefs.GetString(PlayerPrefClass, defaultDifficulty);
        Debug.Log("Start. Difficulty is: " + savedDifficulty);

    }

    public void AChangeDifficulty(string playerPrefKey)
    {
        PlayerPrefs.SetString(PlayerPrefClass, playerPrefKey);
        PlayerPrefs.Save();
        Debug.Log("Changed difficulty to" + playerPrefKey);
    }




}