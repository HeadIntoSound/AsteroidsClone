using UnityEngine;

// Controls the spawn of the meteors
public class MeteorSpawnerController : MonoBehaviour
{
    [SerializeField] Transform playArea;                                    // A reference to the playable area
    public float spawnInterval = 2;                                         // Time between meteors spawn
    [SerializeField] float meteorSpeed = 5;                                 // Speed at which the meteors will be launched
    float lastSpawn;                                                        // A reference to the last time a meteor was spawned

    void Start()
    {
        EventManager.Instance.OnMeteorFragment.AddListener(FragmentMeteor);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnMeteorFragment.RemoveListener(FragmentMeteor);
    }

    void Update()
    {
        // Spawns the meteors in random places around the edges of the screen
        if (Time.time <= 0 || Time.time > lastSpawn + spawnInterval)
        {
            Vector2 spawnPoint = Vector2.zero;

            // Searches an inactive meteor in the pool, if there isn't one available, skips the spawn until there's one
            var meteor = MeteorPool.Instance.PopBigInactive();
            if (meteor == null)
                return;

            // Chooses in which margin the meteor will spawn
            float chance = Random.Range(0, 3);
            // Spawns anywhere along the top margin
            if (chance == 0)
            {
                float spawnX = Random.Range(-playArea.lossyScale.x / 2, playArea.lossyScale.x / 2);
                spawnPoint = new Vector3(spawnX, playArea.lossyScale.y / 2, 0);
            }
            // Spawns anywhere along the bottom margin
            if (chance == 1)
            {
                float spawnX = Random.Range(-playArea.lossyScale.x / 2, playArea.lossyScale.x / 2);
                spawnPoint = new Vector3(spawnX, -playArea.lossyScale.y / 2, 0);
            }
            // Spawns anywhere along the right margin
            if (chance == 2)
            {
                float spawnY = Random.Range(-playArea.lossyScale.y / 2, playArea.lossyScale.y / 2);
                spawnPoint = new Vector3(playArea.lossyScale.x / 2, spawnY, 0);
            }
            // Spawns anywhere along the left margin
            if (chance == 3)
            {
                float spawnY = Random.Range(-playArea.lossyScale.y / 2, playArea.lossyScale.y / 2);
                spawnPoint = new Vector3(-playArea.lossyScale.x / 2, spawnY, 0);
            }

            // Sets position and resets velocity to avoid launching it too fast or too slow
            meteor.transform.position = spawnPoint;
            meteor.rb.velocity = Vector2.zero;

            // Launches the meteor in a random direction (the direction is normalied to launch all meteors equally)
            meteor.rb.AddForce((RandomPoint(3) - spawnPoint).normalized * meteorSpeed, ForceMode2D.Impulse);

            lastSpawn = Time.time;
        }
    }

    // Spawn fragments of meteors when a big meteor is destroyed by the player, using the same mechanism as above
    void FragmentMeteor(Vector2 initialPoint, int amount = 3)
    {
        for (int i = 0; i < amount; i++)
        {
            // Searches an inactive meteor in the pool, if there isn't one available, it doesn't spawn more
            var meteor = MeteorPool.Instance.PopSmallInactive();
            if (meteor == null)
                return;


            meteor.transform.position = initialPoint;
            meteor.rb.AddForce((RandomPoint() - initialPoint).normalized * meteorSpeed * 1.75f, ForceMode2D.Impulse);
        }
    }

    // Returns a random point in the playable area, as size increases the point will land closer to the center
    Vector2 RandomPoint(float size = 1)
    {
        return new Vector2(Random.Range(0, playArea.lossyScale.x / size), Random.Range(0, playArea.lossyScale.y / size));
    }
}
