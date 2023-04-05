using System.Collections;
using UnityEngine;
using TMPro;
using System;

// Controls everything in-game UI related
public class HUDController : MonoBehaviour
{
    [SerializeField] TMP_Text points;                                   // Score count text
    [SerializeField] TMP_Text time;                                     // Clock text
    [SerializeField] GameObject gameOverPanel;                          // The panel that appears when the game is over
    [SerializeField] TMP_Text countdown;                                // The countdown text when the game is over
    [SerializeField] TMP_Text gameOverPoints;                           // Score count text in the game over panel
    [SerializeField] TMP_Text gameOverTime;                             // Clock text in the game over panel
    float stopTime;                                                     // The moment of the last game over

    void Start()
    {
        EventManager.Instance.OnPlayerHit.AddListener(GameOver);
        EventManager.Instance.OnMeteorHit.AddListener(UpdateScore);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnPlayerHit.RemoveListener(GameOver);
        EventManager.Instance.OnMeteorHit.RemoveListener(UpdateScore);
    }

    // Starts the countdown
    void GameOver()
    {
        StartCoroutine(RestartCountdown());
    }

    // Hides unnecessary info, shows the panel, makes the count down and reverts everything once finished
    IEnumerator RestartCountdown()
    {
        stopTime = Time.time;
        gameOverPanel.SetActive(true);
        gameOverPoints.text = points.text;
        gameOverTime.text = time.text;
        points.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
        float t = 5;
        while (t > 0)
        {
            countdown.text = t.ToString("0");
            yield return new WaitForSecondsRealtime(1);
            t--;
        }
        points.text = "000000";
        points.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    // Sets the score
    void UpdateScore(MeteorController m)
    {
        points.text = GameController.Instance.points.ToString("000000");
    }

    // Sets the time
    void Timer()
    {
        var t = TimeSpan.FromSeconds((double)Time.time - stopTime);
        time.text = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);
    }

    // Updates the points and time
    void Update()
    {
        Timer();
    }
}
