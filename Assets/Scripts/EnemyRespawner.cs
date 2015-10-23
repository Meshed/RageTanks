using UnityEngine;
using System.Collections;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject spawnEnemy = null;
    private float respawnTime = 0.0f;

    void OnEnable()
    {
        EnemyControllerScript.enemyDied += scheduleRespawn;
    }

    void OnDisable()
    {
        EnemyControllerScript.enemyDied -= scheduleRespawn;
    }

    // Note: Even though we don't need the enemyScore, we still
    // need to accept it because the event passes it
    void scheduleRespawn(int enemyScore)
    {
        // Get count of enemies. Only spawn new enemies if the count is
        // than the total number of enemies allowed.

        // Randomy decide if we will respawn or not
        if(Random.Range(0, 10) < 5)
            return;

        respawnTime = Time.time + 4.0f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (respawnTime > 0.0f)
	    {
	        if (respawnTime < Time.time)
	        {
	            respawnTime = 0.0f;
	            GameObject newEnemy = Instantiate(spawnEnemy) as GameObject;
	            newEnemy.transform.position = transform.position;
	        }
	    }
	}
}
