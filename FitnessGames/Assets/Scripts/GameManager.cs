using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    // Enumerate states of virtual hand interactions
    public enum GameState
    {
        Training,
        Playing,
        OnMenu,
    };

    public enum GameMode
    {
        Twist,
        ArmRaise
    }

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    

    [Tooltip("Way to pause")]
    public CommonButton[] pauseButtons;

    [Tooltip("button to skip training and return to menu")]
    public CommonButton[] skipButtons;

    [Tooltip("Left controller (for twist)")]
    public GameObject leftHand;
    
    [Tooltip("Right controller (for twist)")]
    public GameObject rightHand;

    [Tooltip("Distance check for twist")]
    public float minimumDistance;

    [Tooltip("TextMeshPro that show information")]
    public TextMeshPro textBoard;

    [Tooltip("OUR CARL!")]
    public GameObject TrainingCarl;

    [Tooltip("Game Mode")]
    public GameMode gameMode;

    [Tooltip("Sound of Button click (pause/skip)")]
    public AudioSource buttonClickSound;

    //[Tooltip("Way to continue")]
    //public CommonButton[] continueButtons;

    // Private interaction variables
    Spawner spawner;
    ScoreSystem scoreSystem;
    GameState state;
    AudioSource hitSound;
    int speedLevel = 1;
    bool pauseButtonOnClick = false;
    bool skipButtonOnClick = false;
    bool pauseButtonDown = false;
    bool skipButtonDown = false;

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
        state = GameState.OnMenu;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
        state = GameState.Playing;
    }

    public void MissObject(FlyingObject fo)
    {
        if (fo.state == FlyingObject.FlyingObjectState.Untouched)
        {
            if (fo.breakable == false)
                GameManager.instance.ChangeScore(fo.score);
        }
        fo.Reclaim();
    }

    public void HitObject(FlyingObject fo)
    {
        // asteroid
        if (fo.breakable)
        {
            fo.Explode();
            hitSound.Play();
            ChangeScore(fo.score);
            //rint("Add score" + fo.score.ToString());
        }
        // space ship
        else
        {
            fo.Shine();
        }
    }

    // Use this for initialization
    void Start () {
        spawner = GetComponent<Spawner>();
        scoreSystem = GetComponent<ScoreSystem>();
        state = GameState.Training;
        hitSound = GameObject.Find("HitSound").GetComponent<AudioSource>();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        bool buttonPress = false;
        foreach (CommonButton button in pauseButtons)
        {
            if (button.GetPressDown())
            {
                buttonPress = true;
                pauseButtonDown = true;
            }
        }
        if (!buttonPress && pauseButtonDown)
        {
            pauseButtonOnClick = true;
            pauseButtonDown = false;
        }
        buttonPress = false;
        //TODO test skip+pause
        foreach (CommonButton button in skipButtons)
        {
            if (button.GetPressDown())
            {
                buttonPress = true;
                skipButtonDown = true;
            }
        }
        if (!buttonPress && skipButtonDown)
        {
            skipButtonOnClick = true;
            skipButtonDown = false;
        }
        if (pauseButtonOnClick)
        {
            //   print("click");
            buttonClickSound.Play();
            pauseButtonOnClick = false;
            if (state == GameState.Playing)
            {
                GamePause();
            } else if (state == GameState.OnMenu)
            {
                GameContinue();
            }
        } else if (skipButtonOnClick)
        {
            buttonClickSound.Play();
            skipButtonOnClick = false;
            if (state == GameState.Training)
            {
                TrainingEnd();
            } else if (state == GameState.OnMenu)
            {
                ReturnToMenu();
            }
        }

        if (gameMode == GameMode.Twist)
        {
            float distance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            if (distance < minimumDistance && state == GameState.Playing)
            {
                ArmPause();
            }
            else
            {
                //UpdateText();
            }
        }
    }

    void ArmPause()
    {
        textBoard.SetText("Please open your arms");
        GamePause();
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
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
        if (scoreSystem.score >= 12 && state == GameState.Training)
        {
            TrainingEnd();
        }
        if (scoreSystem.score > speedLevel * 100) spawner.objectSpeed += 1;
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
        } else if (state == GameState.OnMenu)
        {
            textBoard.SetText("Press <Menu button> to Continue\nPress <trigger> to Exit");
        }
    }
}
