using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerStateController : MonoBehaviour 
{
    public enum playerStates
    {
        idle = 0,
        left,
        right,
        jump,
        landing,
        falling,
        kill,
        resurrect,
        firingWeapon,
        _stateCount
    }

    public static float[] stateDelayTimer = new float[(int)playerStates._stateCount];

    public delegate void playerStateHandler(PlayerStateController.playerStates newState);
    public static event playerStateHandler onStateChange;

    void LateUpdate()
    {
        // Detect the current input of Horizontal axis, then
        // broadcast a state update for the player as needed.
        // Do this on each frame to make sure the state is always
        // set properly based on the current user input.
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0f)
        {
            if (horizontal < 0f)
            {
                if (onStateChange != null) onStateChange(PlayerStateController.playerStates.left);
            }
            else
            {
                if (onStateChange != null) onStateChange(PlayerStateController.playerStates.right);
            }
        }
        else
        {
            if (onStateChange != null) onStateChange(PlayerStateController.playerStates.idle);
        }

        float jump = Input.GetAxis("Jump");

        if (jump > 0.0f)
        {
            if (onStateChange != null) onStateChange(PlayerStateController.playerStates.jump);
        }
    }
}
