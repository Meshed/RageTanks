using UnityEngine;
using System.Collections;

public class BossCrushTrigger : MonoBehaviour
{
    public BossEventController bossController;

    void OnTriggerEnter2D(Collider2D collidedObject)
    {
        Debug.Log("Boss crush trigger entered.");
        if (bossController.currentEvent != BossEventController.bossEvents.fallingToNode)
            return;

        if (collidedObject.tag == "Player")
            collidedObject.SendMessage("hitByCrusher");
    }
}
