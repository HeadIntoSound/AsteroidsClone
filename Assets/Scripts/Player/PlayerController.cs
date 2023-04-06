using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls player's movement and interactions
public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(1, 5)] float moveSpeed = 1;                              // Ship's movement speed
    [SerializeField][Range(1, 500)] float rotationSpeed = 1;                        // Ship's rotation speed
    [SerializeField][Range(1, 500)] float projSpeed = 1;                            // Projectile's speed
    [SerializeField] Rigidbody2D rb;                                                // Rigidbody of the ship's gameobject
    [SerializeField] Transform front;                                               // An empty gameobject that's always at the tip of the ship
    [SerializeField] Vector3 dir;                                                   // The direction in which the ship is facing
    [SerializeField] List<Rigidbody2D> projectiles = new List<Rigidbody2D>();       // A list of projectiles. To avoid performance issues, the ammo is
                                                                                    // limited to a set amount of projectiles that will be reused.
                                                                                    // This amount can be changed in the editor
    bool paused = false;                                                            // Is the game paused?

    void Start()
    {
        EventManager.Instance.OnRestartGame.AddListener(Restart);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnRestartGame.RemoveListener(Restart);
    }

    // Reset's the ship position and rotation
    void Restart()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // Inputs, movement and shooting
    void Update()
    {
        // Moves forward
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Move(1);
        }
        // Moves backwards
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Move(-1);
        }
        // Rotates to the right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Rotate(-1);
        }
        // Rotates to the left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate(1);
        }
        // Shoots the projectiles
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    // Moves the ship, facing is either 1 for forward or -1 for backwards
    void Move(int facing)
    {
        dir = front.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + facing * dir, Time.deltaTime * moveSpeed);
    }

    // The ship only moves forward or backwards and rotates when trying to turn left or right, facing tells the direction in which the ship rotates
    void Rotate(int facing)
    {
        transform.Rotate(0, 0, facing * Time.deltaTime * rotationSpeed, Space.Self);
    }

    // Shoots the projectiles
    void Fire()
    {
        if (Time.timeScale == 0)
            return;
        // Brings the first unused projectile, sets its position and rotation
        var proj = projectiles[0];
        proj.velocity = Vector2.zero;
        proj.transform.rotation = transform.rotation;
        proj.transform.position = front.position;
        proj.gameObject.SetActive(true);

        // Fires the projectile
        proj.AddForce((proj.transform.position - transform.position) * projSpeed, ForceMode2D.Impulse);

        // Moves the fired projectile to the end of the list, so the next projectile is one that's inactive
        projectiles.RemoveAt(0);
        projectiles.Add(proj);
    }

    // Closes the game
    void Quit()
    {
        Application.Quit();
    }

    // Pauses the game
    void Pause()
    {
        // If not enough time has passed since the start of the game, you can't pause
        if (Time.time <= 0.01f)
            return;
        
        Time.timeScale = paused ? 1 : 0;
        paused = !paused;
        EventManager.Instance.OnPauseGame.Invoke(paused);
    }

    // Makes the player warparound when leaving the screen
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

    // Game over :(
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag.Contains(Tags.METEOR))
        {
            EventManager.Instance.OnPlayerHit.Invoke();
            // Hides all projectiles
            projectiles.ForEach(proj => { proj.gameObject.SetActive(false); });
        }
    }
}
