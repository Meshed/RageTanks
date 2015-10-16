using UnityEngine;
using System.Collections;

public class PlayerBulletController : MonoBehaviour
{
    // Will be populated automatically when the bullet is created in PlayerStateListener
    public GameObject playerObject = null;
    public float bulletSpeed = 15.0f;
    private float selfDestructTimer = 0.0f;

    public void lauchBullet()
    {
        // The local scale of the player object tells us which
        // diretion the player is looking. Rather than programming in extra
        // variables to store where the player is looking, just check what
        // already knows that information... the object scale!
        float mainXScale = playerObject.transform.localScale.x;
        Vector2 bulletForce;

        if (mainXScale < 0.0f)
        {
            // Fire bullet left
            bulletForce = new Vector2(bulletSpeed * -1.0f, 0.0f);
        }
        else
        {
            // Fire bullet right
            bulletForce = new Vector2(bulletSpeed, 0.0f);
        }

        GetComponent<Rigidbody2D>().velocity = bulletForce;

        selfDestructTimer = Time.time + 1.0f;
    }

    void Update()
    {
        if (selfDestructTimer > 0.0f)
        {
            if (selfDestructTimer < Time.time)
            {
                Destroy(gameObject);
            }
        }
    }
}
