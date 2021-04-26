using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

/*
 *  The following class controls the gameobjects in the game.
 * 
 */

public class GameController : MonoBehaviour
{
    public enum Direction
    {
        LEFT, UP, DOWN, RIGHT
    }

    public Direction moveDirection;
    public float delayStep;
    public float step;

    public Transform head;
    public Transform food;
    public List<Transform> tail;
    public GameObject tailPrefab;

    private Vector3 lastPos;

    public int cols = 29;
    public int rows = 15;

    public Text txtScore;
    public Text txtRecord;
    public int score;
    public int record;

    public AudioSource eat;
    public AudioSource die;
    public AudioSource buttonPress;

    public GameObject panelGameOver;
    public GameObject panelTitle;


    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    void Start()
    {
        StartCoroutine(MoveSnake());
        SetFood();
        record = PlayerPrefs.GetInt("Record");
        txtRecord.text = "Record: " + record.ToString();
        Time.timeScale = 0;
    }

    void Update()
    {
        MyoInput();
        KeyboardInput();
    }

    // Movement controls using keyboard.
    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (moveDirection != Direction.DOWN)
            {
                moveDirection = Direction.UP;
            }

        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveDirection != Direction.RIGHT)
            {
                moveDirection = Direction.LEFT;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (moveDirection != Direction.UP)
            {
                moveDirection = Direction.DOWN;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveDirection != Direction.LEFT)
            {
                moveDirection = Direction.RIGHT;
            }

        }
    }

    // Rotating Snake head depending on direction.
    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(delayStep);

        Vector3 nexPos = Vector3.zero;
        switch (moveDirection)
        {
            case Direction.DOWN:
                nexPos = Vector3.down;
                head.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.LEFT:
                nexPos = Vector3.left;
                head.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.RIGHT:
                nexPos = Vector3.right;
                head.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.UP:
                nexPos = Vector3.up;
                head.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }

        nexPos *= step;
        lastPos = head.position;
        head.position += nexPos;

        foreach (Transform t in tail)
        {
            Vector3 temp = t.position;
            t.position = lastPos;
            lastPos = temp;
            t.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        StartCoroutine(MoveSnake());
    }

    // Myo Armband Movement Controls.
    private void MyoInput()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Fist = down
        // Fingers Spread = up
        // Wave Out = right
        // Wave In = left

        // Check if the pose has changed since last update.
        // The ThalmicMyo component of a Myo game object has a pose property that is set to the
        // currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
        // detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
        // is not on a user's arm, pose will be set to Pose.Unknown.
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            // Vibrate the Myo armband when a fingers spread.
            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                thalmicMyo.Vibrate(VibrationType.Medium);
                if (moveDirection != Direction.DOWN)
                {
                    moveDirection = Direction.UP;
                }
                ExtendUnlockAndNotifyUserAction(thalmicMyo);

            }
            else if (thalmicMyo.pose == Pose.Fist)
            {
                
                if (moveDirection != Direction.UP)
                {
                    moveDirection = Direction.DOWN;
                }

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveIn)
            {
                if (moveDirection != Direction.RIGHT)
                {
                    moveDirection = Direction.LEFT;
                }

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                if (moveDirection != Direction.LEFT)
                {
                    moveDirection = Direction.RIGHT;
                }

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
    }

    // Adding body piece to the snake once food is eaten and spawning more food.
    public void Eat()
    {
        Vector3 tailPosition = head.position;

        eat.Play();

        if (tail.Count > 0)
        {
            tailPosition = tail[tail.Count - 1].position;
        }

        GameObject temp = Instantiate(tailPrefab, tailPosition, transform.localRotation);
        tail.Add(temp.transform);
        score += 10;
        txtScore.text = "Score: " + score.ToString();
        SetFood();
    }

    // Spawn Food in random position.
    void SetFood()
    {
        int x = UnityEngine.Random.Range((cols - 1) / 2 * -1, (cols - 1) / 2);
        int y = UnityEngine.Random.Range((rows - 1) / 2 * -1, (rows - 1) / 2);

        food.position = new Vector2(x * step, y * step);
    }

    // Game over management.
    public void GameOver()
    {
        Time.timeScale = 0;
        die.Play();
        panelGameOver.SetActive(true);
        if(score > record)
        {
            PlayerPrefs.SetInt("Record", score);
            txtRecord.text = "New Record: " + score.ToString();
        }
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }

    // Setting variables for the start of the game.
    public void Play()
    {
        buttonPress.Play();
        head.position = Vector3.zero;
        moveDirection = Direction.LEFT;

        foreach(Transform t in tail)
        {
            Destroy(t.gameObject);
        }

        tail.Clear();
        head.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        SetFood();
        score = 0;
        txtScore.text = "Score: 0";
        record = PlayerPrefs.GetInt("Record");
        txtRecord.text = "Record: " + record.ToString();
        panelGameOver.SetActive(false);
        panelTitle.SetActive(false);
        Time.timeScale = 1;
    }
}
