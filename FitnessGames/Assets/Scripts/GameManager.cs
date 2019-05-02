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
        Training,
        Counting,
        GameOver
    };
    
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;


    [Tooltip("Game Mode")]
    static public GameMode gameMode = GameMode.ArmRaise;

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

    [Tooltip("OUR CARL!")]
    public GameObject TrainingCarl;

    public GameObject explosion;

    [Tooltip("Sound of Button click (pause/skip)")]
    public AudioSource buttonClickSound;
    
    [Tooltip("Distance check for twist")]
    public float minimumDistance;
    
    [Tooltip("Distance check for twist")]
    public float maxCountingTime = 3f;

    public float trainingTime = 1f;

    public TextMeshPro debug;

    //[Tooltip("Way to continue")]
    //public CommonButton[] continueButtons;

    // Private interaction variables
    //static public GameMode gameMode;
    Spawner spawner;
    ScoreSystem scoreSystem;
    GameState state;
    AudioSource hitSound;
    int speedLevel = 1;
    //bool pauseButtonOnClick = false;
    //bool skipButtonOnClick = false;
    //bool pauseButtonDown = false;
    //bool skipButtonDown = false;
    float targetTime = 0f;

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
        pauseBoard.gameObject.SetActive(false);
    }

    public void MissObject(FlyingObject fo)
    {
        //if (fo.state == FlyingObject.FlyingObjectState.Untouched)
        //{
        if (!fo.collectable)
            ChangeScore(fo.score);
        else
            ChangeHealthPoint(-1);
        //}
        fo.Reclaim();
    }

    public void HitObject(FlyingObject fo)
    {
        // asteroid
        if (fo.collectable)
        {
            fo.Explode();
            GameObject go = GameObject.Instantiate(explosion);
            GameObject.Destroy(go, 3f);
            hitSound.Play();
            ChangeScore(fo.score);
            //rint("Add score" + fo.score.ToString());
        }
        // space ship
        else
        {
            fo.Shine();
            ChangeHealthPoint(-1);
        }
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
        //GameManager.gm = (GameMode)System.Enum.Parse(typeof(GameMode), "Twist");
        spawner = GetComponent<Spawner>();
        scoreSystem = GetComponent<ScoreSystem>();
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        UpdateText();
        //print(gm);
        spawner.state = gameMode;
        spawner.SetHeadPos(headTracker.position);
        state = GameState.Training;
        //state = GameState.Counting;
        targetTime = trainingTime + Time.realtimeSinceStartup;
        TrainingCarl = Instantiate(Resources.Load("Prefabs/Animation/" + gameMode.ToString(), typeof(GameObject))) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        debug.text = state.ToString();
        //distance check
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
     
        if (state == GameState.Counting)
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
        } else if (state == GameState.Training)
        {
            if (targetTime - Time.realtimeSinceStartup <= 0f)
            {
                CountDown();
            }
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

    void TrainingEnd()
    {
        Destroy(TrainingCarl);
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

    void ChangeHealthPoint(int delta)
    {
        scoreSystem.ChangeHealthPoint(delta);
        if (scoreSystem.HealthPoint <= 0)
        {
            GameOver();
        }
    }

    void GameStart()
    {
        spawner.StartSpawn();
    }

    void GameOver()
    {
        pauseBoard.gameObject.SetActive(true);
        pauseBoard.text = "Game Over\n Your score is " + scoreSystem.Score.ToString();
        state = GameState.GameOver;
        targetTime = maxCountingTime + Time.realtimeSinceStartup;
    }

    void UpdateText()
    {
        if (state == GameState.Training)
        {
            if (gameMode == GameMode.ArmRaise)
            {
                textBoard.SetText("Raise and Lower Your Arms\nPress <Trigger> to Skip Training");
            } else if (gameMode == GameMode.Twist)
            {
                textBoard.SetText("Twist your Arms\nPress <Trigger> to Skip Training");
            }
        } else if (state == GameState.Playing)
        {
            textBoard.SetText("Press <Menu Button> to Pause");
        } else if (state == GameState.Pause)
        {
            pauseBoard.fontSize = 7f;
            pauseBoard.SetText("Press <Menu button> to Continue\nPress <trigger> to Exit");
            pauseBoard.gameObject.SetActive(true);
        }
    }
}
