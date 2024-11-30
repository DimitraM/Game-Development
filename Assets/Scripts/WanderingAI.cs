using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float shootingRange = 10.0f;
    public float randomMoveTime = 2.0f;
    public float chaseRange = 5.0f;

    private float timeToMoveRandomly;
    private bool isAlive;
    private bool isShooting = false; // Prevents movement and rotation during shooting

    [SerializeField] GameObject fireballPrefab;
    private GameObject fireball;

    void Start()
    {
        isAlive = true;
        timeToMoveRandomly = randomMoveTime;  // Initialize random move time
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        // If a player exists
        if (PlayerCharacter.instance != null)
        {
            Vector3 direction = PlayerCharacter.instance.transform.position - transform.position;
            float distanceToPlayer = direction.magnitude;

            // Rotate only if not shooting
            if (!isShooting)
            {
                direction.y = 0; // Ensure horizontal rotation only
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);
            }

            // Movement and shooting behavior
            if (distanceToPlayer <= chaseRange && distanceToPlayer > shootingRange)  // Chase if within chase range but not in shooting range
            {
                ChasePlayer(direction);  // Move towards the player
            }
            else if (distanceToPlayer <= shootingRange)  // Stop moving and shoot if in shooting range
            {
                ShootAtPlayer();
            }
            else  // If the player is outside both ranges, move randomly
            {
                MoveRandomly();
            }
        }

        // Handle obstacle avoidance
        if (!isShooting)
        {
            HandleObstacleAvoidance();
        }
    }

    private void HandleObstacleAvoidance()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            if (hit.distance < obstacleRange)
            {
                // Rotate to avoid obstacle
                float angle = Random.Range(-110, 110);
                transform.Rotate(0, angle, 0);
            }
        }
    }

    // Method to move the enemy randomly when not chasing or shooting
    private void MoveRandomly()
    {
        // Only move randomly when the player is far enough and not in shooting range
        timeToMoveRandomly -= Time.deltaTime;

        if (timeToMoveRandomly <= 0)
        {
            // Pick a random direction and rotate the enemy
            float randomAngle = Random.Range(-110, 110);
            transform.Rotate(0, randomAngle, 0);

            // Reset the random movement timer
            timeToMoveRandomly = randomMoveTime;
        }

        // Move the enemy forward in the direction it's facing
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // Method to chase the player
    private void ChasePlayer(Vector3 directionToPlayer)
    {
        // Move towards the player
        directionToPlayer.y = 0;  // Keep movement on the horizontal plane
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);

        // Move forward towards the player
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void ShootAtPlayer()
    {
        // Only shoot if not already shooting
        if (fireball == null && !isShooting)
        {
            isShooting = true; // Prevent rotation or movement during shooting
            fireball = Instantiate(fireballPrefab);
            fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);  // Position the fireball in front of the enemy
            fireball.transform.rotation = transform.rotation;  // Fireball rotation matches the enemy

            // Allow shooting to complete before resetting
            StartCoroutine(ResetShootingState());
        }
    }

    private IEnumerator ResetShootingState()
    {
        yield return new WaitForSeconds(1.0f); // Wait for shooting cooldown
        isShooting = false; // Allow movement and rotation again
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
    }
}
