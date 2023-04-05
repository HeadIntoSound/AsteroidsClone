using UnityEngine;
using UnityEngine.Events;

// A singleton that manages all the events that used to trigger things across the game
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // Used when a meteor is struck by a projectile
    public class MeteorHit : UnityEvent<MeteorController> { }
    public MeteorHit OnMeteorHit = new MeteorHit();

    // Used when a meteor spawns
    public class MeteorSpawn : UnityEvent<MeteorController> { }
    public MeteorSpawn OnMeteorSpawn = new MeteorSpawn();

    // Used to spawn small meteors when a big one is destroyed
    public class MeteorFragment : UnityEvent<Vector2, int> { }
    public MeteorFragment OnMeteorFragment = new MeteorFragment();

    // Used when the player is struck by a meteor
    public class PlayerHit : UnityEvent { }
    public PlayerHit OnPlayerHit = new PlayerHit();

    // Used when the game is restarted after a game over
    public class RestartGame : UnityEvent { }
    public RestartGame OnRestartGame = new RestartGame();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
