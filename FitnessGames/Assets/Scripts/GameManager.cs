using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Twist,
    ArmRaise,
    Squat,
    Mixed
}
public class GameManager : MonoBehaviour {
    // Enumerate states of virtual hand interactions
    public enum GameState
    {
        Pause,
        Playing,
        Instruction,
        Training,
        Counting,
        GameOver
    };
    
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;


    [Tooltip("Game Mode")]
    static public GameMode gameMode = GameMode.Mixed;

    [Tooltip("Head of user")]
    public Transform headTracker;

    [Tooltip("Left controller (for twist)")]
    public GameObject leftHand;

    [Tooltip("Right controller (for twist)")]
    public GameObject rightHand;

    //[Tooltip("Way to pause")]
    //public CommonButton[] pauseButtons;

    //[Tooltip("button to skip training and return to menu")]
    //public CommonButton[] skipButtons;

    [Tooltip("TextMeshPro that show information")]
    public TextMeshPro textBoard;

    [Tooltip("TextMeshPro that show pause information")]
    public TextMeshPro pauseBoard;

    [Tooltip("TextMeshPro that show game information")]
    public TextMeshPro instructionBoard;

    [Tooltip("OUR CARL!")]
    public GameObject TrainingCarl;

    [Tooltip("Sound of Button click (pause/skip)")]
    public AudioSource buttonClickSound;
    
    [Tooltip("Distance check for twist")]
    public float minimumDistance;
    
    [Tooltip("Distance check for twist")]
    public float maxCountingTime = 3f;

    public float instructionTime = 5f;
    public float trainingTime = 1f;

    public float warnTime = 3f;

    public TextMeshPro debug;

    //[Tooltip("Way to continue")]
    //public CommonButton[] continueButtons;

    // Private interaction variables
    //static public GameMode gameMode;
    Spawner spawner;
    ScoreSystem scoreSystem;
    GameState state;
    AudioSource hitSound;
    AudioSource warnSound;
    int speedLevel = 1;
    //bool pauseButtonOnClick = false;
    //bool skipButtonOnClick = false;
    //bool pauseButtonDown = false;
    //bool skipButtonDown = false;
    float targetTime = 0f;
    float warnStartTime;
    Color originalStart;
    Color originalEnd;
    Color warnStart = Color.red;
    Color warnEnd = Color.red;
    bool disableHandle = false;
    GameObject carl;
    bool armPause = false;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            print("\n\nWarning! Multiple GameManager!\n\n");
        //textBoard.gameObject.SetActive(false);
        
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        state = GameState.Pause;
	    UpdateText();
    }

    public void PauseEnd()
    {
        state = GameState.Counting;
        CountDown();
    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        state = GameState.Playing;
    }

    public void MissObject(FlyingObject fo)
    {
        // do nothing
        //if (fo.state == FlyingObject.FlyingObjectState.Untouched)
        //{
        //if (!fo.collectable)
        //    ChangeScore(fo.score);
        //else
        //    ChangeHealthPoint(-1);
        //}
        fo.Reclaim();
    }

    public void HitObject(FlyingObject fo)
    {
        // asteroid
        if (fo.collectable)
        {
            if (!disableHandle)
            {
                fo.Explode();
                hitSound.Play();
                ChangeScore(fo.score);
            }
            //rint("Add score" + fo.score.ToString());
        }
        // space ship
        else
        {
            warnSound.Play();
            Warning();
            //GameObject go = GameObject.Instantiate(explosion);
            //GameObject.Destroy(go, 3f);
            //go.transform.position = fo.transform.position;
            //fo.Shine();
            //ChangeHealthPoint(-1);
        }
    }

    public void Warning()
    {
        warnStartTime = Time.time;
        LineRenderer lr = leftHand.GetComponentInChildren<LineRenderer>();
        lr.startColor = warnStart;
        lr.endColor = warnEnd;
        lr = rightHand.GetComponentInChildren<LineRenderer>();
        lr.startColor = warnStart;
        lr.endColor = warnEnd;
        disableHandle = true;
    }

    public void ResetLine()
    {
        LineRenderer lr = leftHand.GetComponentInChildren<LineRenderer>();
        lr.startColor = originalStart;
        lr.endColor = originalEnd;
        lr = rightHand.GetComponentInChildren<LineRenderer>();
        lr.startColor = originalStart;
        lr.endColor = originalEnd;
        disableHandle = false;
    }

    public void TriggerOnClick()
    {
        buttonClickSound.Play();
        if (state == GameState.Training)
        {
            TrainingEnd();
        }
        else if (state == GameState.Pause)
        {
            ReturnToMenu();
        }
    }

    public void MenuOnClick()
    {
        buttonClickSound.Play();
        if (state == GameState.Playing)
        {
            GamePause();
        }
        else if (state == GameState.Pause)
        {
            //GameContinue();
            PauseEnd();
        } 
    }

    // Use this for initialization
    void Start ()
    {
        spawner = GetComponent<Spawner>();
        if (gameMode != GameMode.Mixed)
        {
            spawner.state = gameMode;
        }
        else
        {
            // TEST ONLY 
            // TODO
            gameMode = spawner.state;
        }

        scoreSystem = GetComponent<ScoreSystem>();
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        warnSound = GameObject.Find("WarnSound").GetComponent<AudioSource>();
        spawner.SetHeadPos(headTracker.position);
        state = GameState.Instruction;
        targetTime = instructionTime + Time.realtimeSinceStartup;

        LineRenderer lr = leftHand.GetComponentInChildren<LineRenderer>();
        originalStart = lr.startColor;
        originalEnd = lr.endColor;
        UpdateText();
        //Warning();
    }

    // Update is called once per frame
    void Update()
    {
        debug.text = state.ToString();
        //distance check

        if (Time.time - warnStartTime > warnTime)
        {
            warnStartTime = Time.time + 999;
            ResetLine();
        }

        if (gameMode == GameMode.Twist)
        {
            float distance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            if (distance < minimumDistance && state == GameState.Playing)
            {
                ArmPause();
            }
            else if(distance > minimumDistance && state == GameState.Pause)
            {
                UpdateText();
            }
        }
        
        if (state == GameState.Instruction)
        {
            if (targetTime - Time.realtimeSinceStartup <= 0f)
            {
                TrainingBegin();
            }
        }
        else if (state == GameState.Training)
        {
            if (targetTime - Time.realtimeSinceStartup <= 0f)
            {
                CountDown();
            }
        }
        else if (state == GameState.Counting)
        {
            if (targetTime - Time.realtimeSinceStartup > 0)
                pauseBoard.text = Mathf.RoundToInt(targetTime - Time.realtimeSinceStartup).ToString();
            if (targetTime - Time.realtimeSinceStartup <= 0f)
            {
                pauseBoard.text = "GO!";
                if (targetTime - Time.realtimeSinceStartup <= -.5f)
                {
                    spawner.StartSpawn();
                    TrainingEnd();
                    GameContinue();
                }
            }
        }
        else if (state == GameState.Playing)
        {
            scoreSystem.timer -= Time.deltaTime;
            scoreSystem.TextUpdate();
            if (scoreSystem.timer <= 0) GameOver();
        }
        else if (state == GameState.GameOver)
        {
            if (targetTime - Time.realtimeSinceStartup <= 0f)
            {
                ReturnToMenu();
            }
        }
    }



    void ArmPause()
    {
        GamePause();
        textBoard.SetText("Please open your arms and press <Menu Button> to continue");
    }

    void ReturnToMenu()
    {
	   GameContinue();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void CountDown()
    {
        pauseBoard.gameObject.SetActive(true);
        targetTime = maxCountingTime + Time.realtimeSinceStartup;
        state = GameState.Counting;
        pauseBoard.fontSize = 15f;
    }

    void TrainingBegin()
    {
        state = GameState.Training;
        string objPath = "Prefabs/Animation/" + gameMode.ToString();
        carl = Instantiate(Resources.Load(objPath, typeof(GameObject))) as GameObject;
        carl.transform.position = TrainingCarl.transform.position;
        carl.transform.parent = TrainingCarl.transform.parent;
        targetTime = trainingTime + Time.realtimeSinceStartup;
    }

    void TrainingEnd()
    {
        Destroy(carl);
        state = GameState.Playing;
        UpdateText();
    }

    void ChangeScore(int delta)
    {
        scoreSystem.ChangeScore(delta);
        if (scoreSystem.Score >= 12 && state == GameState.Training)
        {
            TrainingEnd();
        }
        if (scoreSystem.Score > speedLevel * 100) {
            spawner.objectSpeed += 1;
            speedLevel++;
        }
    }

    //void ChangeHealthPoint(int delta)
    //{
    //    scoreSystem.ChangeHealthPoint(delta);
    //    if (scoreSystem.HealthPoint <= 0)
    //    {
    //        GameOver();
    //    }
    //}

    void GameStart()
    {
        spawner.StartSpawn();
    }

    void GameOver()
    {
        state = GameState.GameOver;
        targetTime = maxCountingTime + Time.realtimeSinceStartup;
    }

    void UpdateText()
    {
        if (state == GameState.Instruction)
        {
            instructionBoard.gameObject.SetActive(true);
            pauseBoard.gameObject.SetActive(false);
            textBoard.gameObject.SetActive(false);
            if (gameMode == GameMode.ArmRaise)
            {
                instructionBoard.SetText("Raise and Lower Your Arms to collect Energy Stars\n\nPress <Menu Button> to pause the game");
            }
            else if (gameMode == GameMode.Twist)
            {
                instructionBoard.SetText("Spread your arms to collect Energy Stars\n\nTwist your arms to avoid Asteroids\n\nDon't bring your arms too close\n\nPress <Menu Button> to pause the game");
            }
            else if (gameMode == GameMode.Squat)
            {
                instructionBoard.SetText("Squat to advoid Asteroids\n\nRaise your arms to collect Energy Stars\n\nPress <Menu Button> to pause the game");
            }
        }
        if (state == GameState.Training)
        {
            instructionBoard.gameObject.SetActive(false);
            textBoard.gameObject.SetActive(true);
            pauseBoard.gameObject.SetActive(false);
            if (gameMode == GameMode.ArmRaise)
            {
                textBoard.SetText("Raise and Lower Your Arms\nPress <Trigger> to Skip Training");
            }
            else if (gameMode == GameMode.Twist)
            {
                textBoard.SetText("Spread your arms to collect energy stars\nTwist your arms to avoid comets\nDon't bring your arms too close\nPress <Trigger> to Skip Training");
            }
            else if (gameMode == GameMode.Squat)
            {
                textBoard.SetText("Squat to advoid asteroid\nPress <Trigger> to Skip Training");
            }
        }
        else if (state == GameState.Counting)
        {
            pauseBoard.fontSize = 15f;
        }
        else if (state == GameState.Playing)
        {
            textBoard.gameObject.SetActive(true);
            pauseBoard.gameObject.SetActive(false);
            textBoard.SetText("Press <Menu Button> to Pause");
        } else if (state == GameState.Pause)
        {
            textBoard.gameObject.SetActive(false);
            pauseBoard.gameObject.SetActive(true);
            pauseBoard.fontSize = 7f;
            pauseBoard.SetText("Press <Menu button> to Continue\nPress <trigger> to Exit");
        }
        else if (state == GameState.GameOver)
        {
            pauseBoard.fontSize = 7f;
            textBoard.gameObject.SetActive(false);
            pauseBoard.gameObject.SetActive(true);
            pauseBoard.gameObject.SetActive(true);
            pauseBoard.text = "Game Over\n Your score is " + scoreSystem.Score.ToString();
        }
    }
}
