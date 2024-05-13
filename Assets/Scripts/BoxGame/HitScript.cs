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
    public SoundEffect SoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        SoundEffect = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<SoundEffect>(); 
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
            cursorInside = true;
            StartCoroutine(CheckCursorStay());
            countDown.displayCountDown(cursorTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cursor"))
        {
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
        }

        if (cursorInside)
        {
            // The cursor has stayed inside the hitbox for 3 seconds
            score.addScore();
            Destroy(gameObject);

            countDown.resetCountdown();
            cursorTime = countDownTime;
            SoundEffect.playSound();
        }
    }

}