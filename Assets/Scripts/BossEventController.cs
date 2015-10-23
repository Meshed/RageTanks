using System.Collections.Generic;
using UnityEngine;

public class BossEventController : MonoBehaviour
{
    public delegate void bossEventHandler(int scoreMod);
    public static event bossEventHandler bossDied;

    public GameObject inActiveNode = null;
    public GameObject dropToStartNode = null;
    public GameObject dropFXSpawnPoint = null;
    public List<GameObject> DropNodeList = new List<GameObject>();
    public GameObject bossDeathFX = null;
    public GameObject bossDropFX = null;
    public TakeDamageFromPlayerBullet bullletColliderListener = null;
    public float moveSpeed = 0.1f;
    public float eventWaitDelay = 3f;

    // Amount of time to wait between each event
    public int enemiesToStartBattle = 10;

    public enum bossEvents
    {
        inactive = 0,
        fallingToNode,
        waitingToJump,
        waitingToFall,
        jumpingOffPlatform
    }

    // Current event to cycle on each Update() pass
    public bossEvents currentEvent = bossEvents.inactive;
    // The node object the boss will be falling towards
    private GameObject targetNode = null;
    // Amount of time to wait until jumping or falling again
    private float timeForNextEvent = 0.0f;
    // Target position used for when jumping off a platform.
    private Vector3 targetPosition = Vector3.zero;

    // Current health of the boss
    public int health = 20;

    // Health to start the boss at whenever the battle begins
    private int startHealth = 20;
    // Used to determine if the boss has been defeated
    private bool isDead = false;
    // How many enemies left to kill before the boss is spawned
    private int enemiesLeftToKill = 0;

    void OnEnable()
    {
        bullletColliderListener.hitByBullet += hitByPlayerBullet;
        EnemyControllerScript.enemyDied += enemyDied;
    }

    void OnDisable()
    {
        bullletColliderListener.hitByBullet -= hitByPlayerBullet;
        EnemyControllerScript.enemyDied -= enemyDied;
    }

	// Use this for initialization
	void Start ()
	{
	    transform.position = inActiveNode.transform.position;
	    enemiesLeftToKill = enemiesToStartBattle;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    switch (currentEvent)
	    {
            case bossEvents.inactive:
                // Not doing anything, so nothing to do
                break;
            case bossEvents.fallingToNode:
	            if (transform.position.y > targetNode.transform.position.y)
	            {
	                // Movespeed here is negative
                    // so the object moves downwards
                    transform.Translate(new Vector3(0f, -moveSpeed * Time.deltaTime, 0f));

	                if (transform.position.y < targetNode.transform.position.y)
	                {
	                    Vector3 targetPos = targetNode.transform.position;
	                    transform.position = targetPos;
	                }
	            }
	            else
	            {
	                // Create the 'Hit Ground' smoke FX
	                createDropFX();

	                timeForNextEvent = 0.0f;
	                currentEvent = bossEvents.waitingToJump;
	            }
                break;
            case bossEvents.waitingToFall:
                // Boss is waiting to fall to another node
	            if (timeForNextEvent == 0.0f)
	            {
	                timeForNextEvent = Time.time + eventWaitDelay;
	            }
                else if (timeForNextEvent < Time.time)
                {
                    // Need to find a new node!
                    targetNode = DropNodeList[Random.Range(0, DropNodeList.Count)];

                    // Set the boss position to the sky position of this node
                    transform.position = getSkyPositionOfNode(targetNode);

                    // Set the event state
                    currentEvent = bossEvents.fallingToNode;
                    timeForNextEvent = 0.0f;
                }
                break;
            case bossEvents.waitingToJump:
                // Boss is on a platform and is just waiting to
                // jump off of it
	            if (timeForNextEvent == 0.0f)
	            {
	                timeForNextEvent = Time.time + eventWaitDelay;
	            }
                else if (timeForNextEvent < Time.time)
                {
                    // Build the target position based on the
                    // current node
                    targetPosition = getSkyPositionOfNode(targetNode);

                    // Set our event state
                    currentEvent = bossEvents.jumpingOffPlatform;
                    timeForNextEvent = 0.0f;

                    // Also set the target node to null so we know
                    // to find a random one when it's time to fall
                    // to one again
                    targetNode = null;
                }
                break;
            case bossEvents.jumpingOffPlatform:
	            if (transform.position.y < targetPosition.y)
	            {
	                // moveSpeed is positive here, causing
                    // the object to move upwards
                    transform.Translate(new Vector3(0f, moveSpeed * Time.deltaTime, 0f));

	                if (transform.position.y > targetPosition.y)
	                    transform.position = targetPosition;
	            }
	            else
	            {
	                timeForNextEvent = 0.0f;
	                currentEvent = bossEvents.waitingToFall;
	            }
                break;
	    }
	}

    public void beginBossBattle()
    {
        // Set the first falling node and have the boss
        // fall towards it
        targetNode = dropToStartNode;
        currentEvent = bossEvents.fallingToNode;

        // Reset various control variables used to track
        // the boss battle
        timeForNextEvent = 0.0f;
        health = startHealth;
        isDead = false;
    }

    Vector3 getSkyPositionOfNode(GameObject node)
    {
        Vector3 targetPosition = targetNode.transform.position;

        targetPosition.y += 9f;

        return targetPosition;
    }

    void hitByPlayerBullet()
    {
        health -= 1;
        
        // If the boss is out of health - kill 'em!
        if (health <= 0)
            killBoss();
    }

    void createDropFX()
    {
        //GameObject dropFxParticle = (GameObject) Instantiate(bossDropFX);

        //dropFxParticle.transform.position = dropFXSpawnPoint.transform.position;
    }

    void killBoss()
    {
        if(isDead)
            return;

        isDead = true;

        //GameObject deadFxParticle = (GameObject) Instantiate(bossDeathFX);

        // Reposition the particle emitter at the same
        // position as dropFXSpawnPoint
        //deadFxParticle.transform.position = dropFXSpawnPoint.transform.position;

        // Call the bossDied event and give it a score of 1000
        if (bossDied != null)
            bossDied(1000);

        transform.position = inActiveNode.transform.position;

        currentEvent = bossEvents.inactive;
        timeForNextEvent = 0.0f;
        enemiesLeftToKill = enemiesToStartBattle;
    }

    void enemyDied(int enemyScore)
    {
        if (currentEvent == bossEvents.inactive)
        {
            enemiesLeftToKill -= 1;
            Debug.Log("--- Enemies left to start boss battle: " + enemiesLeftToKill);

            if(enemiesLeftToKill <= 0)
                beginBossBattle();
        }
    }
}
