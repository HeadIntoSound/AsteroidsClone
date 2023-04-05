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

    void Start()
    {
        EventManager.Instance.OnPlayerHit.AddListener(GameOver);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnPlayerHit.RemoveListener(GameOver);
    }

    // Starts the countdown
    void GameOver()
    {
        StartCoroutine(RestartCountdown());
    }

    // Hides unnecessary info, shows the panel, makes the count down and reverts everything once finished
    IEnumerator RestartCountdown()
    {
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
        points.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    // Updates the points and time
    void Update()
    {
        points.text = GameController.Instance.points.ToString();
        
        var t = TimeSpan.FromSeconds((double)Time.time);
        time.text = string.Format("{0:00}:{1:00}", t.Minutes, t.Seconds);
    }
}
