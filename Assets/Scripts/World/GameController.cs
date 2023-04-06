using System.Collections;
using UnityEngine;

// Controls the flow of the game, as well as the score
public class GameController : MonoBehaviour
{
    public static GameController Instance;                                      // A singleton
    public int points;                                                          // Player's points

    void Awake()
    {
        Time.timeScale = 0;
        StartCoroutine(StartGame());
    }

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        EventManager.Instance.OnPlayerHit.AddListener(EndGame);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnPlayerHit.RemoveListener(EndGame);
    }

    // Stops the game, hides all meteors and starts the countdown to a new game
    void EndGame()
    {
        Time.timeScale = 0;
        MeteorPool.Instance.SetAllInactive();
        StartCoroutine(Restart());
    }

    // Restarts the game
    IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(5);
        points = 0;
        Time.timeScale = 1;
        EventManager.Instance.OnRestartGame.Invoke();
    }

    // Starts the game
    IEnumerator StartGame()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        Time.timeScale = 1;
        EventManager.Instance.OnRestartGame.Invoke();
    }
}
