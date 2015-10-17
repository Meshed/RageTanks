using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class EnemyControllerScript : MonoBehaviour
{
    public float walkingSpeed = 0.45f;
    public TakeDamageFromPlayerBullet bulletColliderListener = null;

    private bool walkingLeft = true;

    void OnEnable()
    {
        // Subscribe to events from the bullet collider
        bulletColliderListener.hitByBullet += hitByPlayerBullet;
    }

    void OnDisable()
    {
        // Unsubscribe from events
        bulletColliderListener.hitByBullet -= hitByPlayerBullet;
    }

    void Start()
    {
        // Randomly default the enemy's direction
        walkingLeft = (Random.Range(0, 2) == 1);
        updateVisualWalkOrientation();
    }

    void Update()
    {
        // Translate the enemy's position based on the direction
        // that they are currently moving.
        if (walkingLeft)
        {
            transform.Translate(new Vector3(walkingSpeed * Time.deltaTime, 0.0f, 0.0f));
        }
        else
        {
            transform.Translate(new Vector3((walkingSpeed * -1.0f) * Time.deltaTime, 0.0f, 0.0f));
        }
    }

    public void switchDirections()
    {
        // Swap the direction to be the opposite of whatever it
        // currently is
        walkingLeft = !walkingLeft;

        // Update the orientation of the Enemy's marterial to match
        // the new walking direction
        updateVisualWalkOrientation();
    }

    void updateVisualWalkOrientation()
    {
        Vector3 localScale = transform.localScale;

        if (walkingLeft)
        {
            if (localScale.x > 0.0f)
            {
                localScale.x = localScale.x*-1.0f;
                transform.localScale = localScale;
            }
        }
        else
        {
            if (localScale.x < 0.0f)
            {
                localScale.x = localScale.x*-1.0f;
                transform.localScale = localScale;
            }
        }
    }

    public void hitByPlayerBullet()
    {
        // Wait a moment to ensure we are clear, then destroy the
        // enemy object.
        Destroy(gameObject, 0.1f);
    }
}
