using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    public ScoreScript score;
    public CountDownScript countDown;
    public bool cursorInside = false;
    public int countDownTime = 3;
    public int cursorTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreScript>();
        countDown = GameObject.FindGameObjectWithTag("CountDownManager").GetComponent<CountDownScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cursor"))
        {
            Debug.Log("Cursor entered collider");
            cursorInside = true;
            StartCoroutine(CheckCursorStay());
            countDown.displayCountDown(cursorTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cursor"))
        {
            Debug.Log("Cursor exited collider");
            cursorInside = false;
            StopAllCoroutines();

            countDown.resetCountdown();
            cursorTime = countDownTime;
        }
    }

    private IEnumerator CheckCursorStay()
    {
        while (cursorTime >= 1)
        {
            yield return new WaitForSeconds(1); // Wait for 1 second
            cursorTime = cursorTime - 1;
            countDown.displayCountDown(cursorTime);
            Debug.Log("Time remaining: " + cursorTime);
        }

        if (cursorInside)
        {
            // The cursor has stayed inside the hitbox for 3 seconds
            Debug.Log("Cursor stayed inside hitbox for 3 seconds.");
            score.addScore();
            Destroy(gameObject);

            countDown.resetCountdown();
            cursorTime = countDownTime;
        }
    }

}