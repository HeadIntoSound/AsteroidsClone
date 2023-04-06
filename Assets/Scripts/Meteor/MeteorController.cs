using UnityEngine;

// Controls meteor's properties
public class MeteorController : MonoBehaviour
{
    public bool isBig = false;                                              // Used to know if it should spawn other meteors when destroyed
    public Rigidbody2D rb;                                                  // Meteor's rigidbody
    [SerializeField] int meteorsToSpawn = 3;                                // Amount of meteors to spawn, if any
    [SerializeField] int value = 5;                                         // Amount of points awarded to the player when destroyed

    // Controls behaviour when destroyed
    void OnHit()
    {
        // Awards points
        GameController.Instance.points += value;

        rb.velocity = Vector2.zero;

        // Triggers an event that will set inactive this gameobject
        EventManager.Instance.OnMeteorHit.Invoke(this);

        // Tells the spawner to spawn smaller meteors where this meteor was
        if (isBig)
        {
            EventManager.Instance.OnMeteorFragment.Invoke(transform.position, meteorsToSpawn);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If being hit by a projectile
        if (other.tag.Contains(Tags.PROJECTILE))
        {
            OnHit();
        }
    }

    // Makes the meteor warparound when leaving the screen
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Contains(Tags.PLAYAREA))
        {
            if (Mathf.Abs(transform.position.x) > other.transform.lossyScale.x / 2)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
            }
            if (Mathf.Abs(transform.position.y) > other.transform.lossyScale.y / 2)
            {
                transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
            }
        }
    }
}
