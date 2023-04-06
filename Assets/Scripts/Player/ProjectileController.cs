using UnityEngine;

// Controls projectile's properties
public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;                            // A reference to its rigidbody

    void OnTriggerEnter2D(Collider2D other)
    {
        // When it hits a meteor
        if (other.tag.Contains(Tags.METEOR))
        {
            TurnOff();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When it leaves the playable area
        if (other.tag.Contains(Tags.PLAYAREA))
        {
            TurnOff();
        }
    }

    // Stops and makes its gameobject inactive
    void TurnOff()
    {
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

}
