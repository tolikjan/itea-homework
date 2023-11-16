using UnityEngine;

public class StopMovement : MonoBehaviour
{
    public static bool isStopped;

    // Update is called once per frame
    private void Update()
    {
        if (isStopped == true)
        { 
            MovementIsStopped();
        }
        else
        { 
            MovementIsOn();
        }
    }

    private void MovementIsOn()
    {
        isStopped = false;
        Player.movementSpeed = Player.defaultMovementSpeed;
    }

    private void MovementIsStopped()
    {
        isStopped = true;
        Player.movementSpeed = 0.0f;
    }
}
