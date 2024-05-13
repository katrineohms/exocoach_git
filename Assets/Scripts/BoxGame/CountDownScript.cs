using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownScript : MonoBehaviour
{
    public TextMesh countDownText;
    public GameObject countDownObject;
   
    public void displayCountDown(int cursorTime)
    {
        countDownObject.SetActive(true);
        countDownText.text = cursorTime.ToString();

    }

    public void resetCountdown()
    {
        countDownObject.SetActive(false);
    }
}
