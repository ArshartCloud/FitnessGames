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

    [Tooltip("Score System to calculate score")]
    public ScoreSystem scoreSystem;

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
    //[Tooltip("Way to continue")]
    //public CommonButton[] continueButtons;

    // Private interaction variables
    GameState state;
    bool pauseButtonOnClick = false;
    bool skipButtonOnClick = false;
    bool buttonDown = false;

    //Awake is always called before any Start functions
    void Awake()
    {
        state = GameState.Training;
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
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
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
        }
        // space ship
        else
        {
            fo.Shine();
        }
        ChangeScore(fo.score);
    }

    // Use this for initialization
    void Start () {
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
                buttonDown = true;
            }
        }
        if (!buttonPress && buttonDown)
        {
            pauseButtonOnClick = true;
            buttonDown = false;
        }
        buttonPress = false;
        //TODO test skip+pause
        foreach (CommonButton button in skipButtons)
        {
            if (button.GetPressDown())
            {
                buttonPress = true;
                buttonDown = true;
            }
        }
        if (!buttonPress && buttonDown)
        {
            skipButtonOnClick = true;
            buttonDown = false;
        }
        if (pauseButtonOnClick)
        {
         //   print("click");
            pauseButtonOnClick = false;
            if (state == GameState.Playing)
            {
                state = GameState.OnMenu;
                GamePause();
            } else if (state == GameState.OnMenu)
            {
                state = GameState.Playing;
                GameContinue();
            }
        } else if (skipButtonOnClick)
        {
            skipButtonOnClick = false;
            if (state == GameState.Training)
            {
                state = GameState.Playing;
                TrainingEnd();
            } else if (state == GameState.OnMenu)
            {
                ReturnToMenu();
            }
        }

        if (gameMode == GameMode.Twist)
        {
            float distance = Vector3.Distance(leftHand.transform.position, rightHand.transform.position);
            if (distance < minimumDistance)
            {
                textBoard.SetText("Please open your arms");
                textBoard.gameObject.SetActive(true);
                if (state == GameState.Playing)
                    GamePause();
            }
            else
            {
                textBoard.gameObject.SetActive(false);
            }
        }
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void TrainingEnd()
    {
        textBoard.gameObject.SetActive(false);
        Destroy(TrainingCarl);
    }

    void ChangeScore(int delta)
    {
        scoreSystem.ChangeScore(delta);
    }
}
