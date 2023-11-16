using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    // Boundaries of the room
    private float _roomBoundariesPositiveX = 1.3f;
    private float _roomBoundariesNegativeX = -5.3f;
    private float _roomBoundariesY = 2.7f; // Height of a Player
    private float _roomBoundariesPositiveZ = -1.15f;
    private float _roomBoundariesNegativeZ = -6.0f;

    // Speed of movement
    public static float movementSpeed;
    public static float defaultMovementSpeed = 1.0f;

    // Jump
    public float jumpHeight = 0.7f;
    public float jumpDuration = 0.4f;

    // Movement directions
    private bool _moveRight;
    private bool _moveLeft;
    private bool _moveForward;
    private bool _moveBackward;
    private bool _jump;

    public static bool illegalKeyPressed = false; // Gamer pressed the movement key that can not be executed

    public static List<string> listOfActions; // Actions logs

    private void Awake()
    {
        // Add 1st and 2nd elements to the 'list of actions'
        listOfActions = new List<string>();
        listOfActions.Add("start");
        listOfActions.Add("stop");
    }

    void Start()
    {
        // Put the player to starting position (-1.75f, 2.7f, -6.0f)
        transform.position = new Vector3(-1.75f, 2.7f, -6.0f);

        StopMovement.isStopped = true;

        movementSpeed = 0.0f;
    }

    private void Update()
    {
        defaultMovementSpeed = Random.Range(2.0f, 6.0f); // Simulate the human movement
        CalculateMovement(); // Calculate movement for Player
    }

    private void CalculateMovement()
    {
        // Keyboard Input (WASD)
        Vector3 move = new Vector3(0, 0, 0);

        // If game just started, the movementSpeed remains the same as default 
        if (listOfActions.Last() == "stop" && listOfActions[listOfActions.Count - 2] == "start")
        {
            movementSpeed = defaultMovementSpeed; // Default movement speed of a player
        }

        // Movements management:
        // Checking whether the previous movement and movement before the stop not what you trying to do,
        // and execute it according to the direction of key pressed
        //
        // W ==> Forward
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (listOfActions.Last() != "backward" && listOfActions.Last() != "forward")
            {
                if (listOfActions.Last() == "stop" && listOfActions[listOfActions.Count - 2] == "forward")
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = false;
                }
                else
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = !_moveForward;
                    _moveBackward = false;
                    listOfActions.Add("forward");
                    StopMovement.isStopped = false;

                    illegalKeyPressed = false;
                }
            }
            else
            {
                listOfActions.Add("stop");
                StopMovement.isStopped = true;
            }
        }

        // S ==> Backward
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (listOfActions.Last() != "forward" && listOfActions.Last() != "backward")
            {
                if (listOfActions.Last() == "stop" && listOfActions[listOfActions.Count - 2] == "backward")
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = false;
                }
                else
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = !_moveBackward;
                    listOfActions.Add("backward");
                    StopMovement.isStopped = false;

                    illegalKeyPressed = false;
                }
            }
            else
            {
                listOfActions.Add("stop");
                StopMovement.isStopped = true;
            }
        }

        // A ==> LEFT
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (listOfActions.Last() != "left" && listOfActions.Last() != "right")
            {
                if (listOfActions.Last() == "stop" && listOfActions[listOfActions.Count - 2] == "right")
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = false;
                }
                else
                {
                    _moveRight = false;
                    _moveLeft = !_moveLeft;
                    _moveForward = false;
                    _moveBackward = false;
                    listOfActions.Add("left");
                    StopMovement.isStopped = false;

                    illegalKeyPressed = false;
                }
            }
            else
            {
                listOfActions.Add("stop");
                StopMovement.isStopped = true;
            }
        }

        // D ==> RIGHT on X axis
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (listOfActions.Last() != "right" && listOfActions.Last() != "left")
            {
                if (listOfActions.Last() == "stop" && listOfActions[listOfActions.Count - 2] == "left")
                {
                    _moveRight = false;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = false;
                }
                else
                {
                    _moveRight = !_moveRight;
                    _moveLeft = false;
                    _moveForward = false;
                    _moveBackward = false;
                    listOfActions.Add("right");
                    StopMovement.isStopped = false;

                    illegalKeyPressed = false;
                }
            }
            else
            {
                listOfActions.Add("stop");
                StopMovement.isStopped = true;
            }
        }

        // Stop moving)
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (listOfActions.Last() != "stop")
            {
                _moveRight = false;
                _moveLeft = false;
                _moveForward = false;
                _moveBackward = false;
                listOfActions.Add("stop");
                StopMovement.isStopped = true;

                illegalKeyPressed = false;
            }
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _moveRight = false;
            _moveLeft = false;
            _moveForward = false;
            _moveBackward = false;
            StartCoroutine(Jump());
            listOfActions.Add("jump");
            StopMovement.isStopped = false;

            illegalKeyPressed = false;
        }

        // Handle cases when player was paused / stopped
        if (_moveRight)
        {
            if (movementSpeed != 0)
            {
                move.x += movementSpeed;
                MovingIn(move);
            }
        }
        if (_moveLeft) 
        {
            if (movementSpeed != 0)
            {
                move.x -= movementSpeed;
                MovingIn(move);
            }
        }
        if (_moveForward)
        {
            if (movementSpeed != 0)
            {
                move.z += movementSpeed;
                MovingIn(move);
            }
        }
        if (_moveBackward)
        {
            if (movementSpeed != 0)
            {
                move.z -= movementSpeed;
                MovingIn(move);
            }
        }

        // Moving to direction but not more than boundaries of the world
        void MovingIn(Vector3 moving)
        {
            transform.Translate(moving * Time.deltaTime);
        }

        if (transform.position.x > _roomBoundariesPositiveX)
        {
            transform.position = new Vector3(_roomBoundariesPositiveX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < _roomBoundariesNegativeX)
        {
            transform.position = new Vector3(_roomBoundariesNegativeX, transform.position.y, transform.position.z);
        }

        if (transform.position.y > _roomBoundariesY)
        {
            transform.position = new Vector3(transform.position.x, _roomBoundariesY, transform.position.z);
        }
        else if (transform.position.y < _roomBoundariesY)
        {
            transform.position = new Vector3(transform.position.x, _roomBoundariesY, transform.position.z);
        }

        if (transform.position.z > _roomBoundariesPositiveZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _roomBoundariesPositiveZ);
        }
        else if (transform.position.z < _roomBoundariesNegativeZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _roomBoundariesNegativeZ);
        }
    }

    public IEnumerator Jump()
    {
        _jump = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + jumpHeight, startPosition.z);

        // Jump up
        float elapsedTime = 0;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / (jumpDuration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Finish Jump
        elapsedTime = 0;
        while (elapsedTime < jumpDuration / 2)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, (elapsedTime / (jumpDuration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition; // Ensure the player is exactly at the starting position
        _jump = false;
    }
}
