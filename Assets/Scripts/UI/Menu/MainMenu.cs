using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public enum MainMenuState
{
    MainMenu,
    Options,
    Levels
}
public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button[] buttons;
    public Vector3[] buttonPositions;
    [Header("Navigation")]
    public Vector2 backButtonPosition;
    public Vector3 titlePosition;
    public RectTransform LevelsPanelGO;
    public RectTransform OptionsPanelGO;
    //public RectTransform MainMenuGO;
    public RectTransform backButton;
    [Header("Level generator variables")]
    public LevelButton[] levelsGO;

    //public int howManylevelsAreIngame = 6;
    //Private variables
    //Animator animator;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        buttonPositions = new Vector3[buttons.Length];
        for(int i = 0;i<buttons.Length;i++)
        {
            buttonPositions[i] = buttons[i].GetComponent<RectTransform>().anchoredPosition;
        }
    }
    #region Navigation animations

    void SetTitle(RectTransform tr)
    {
        tr.DOAnchorPos(titlePosition,1);
    }
    void SetPanel(RectTransform tr)
    {
        tr.DOAnchorPos(Vector3.zero, 1);
        backButton.DOAnchorPos(backButtonPosition, 1);
    }
    void GoOffScreen(RectTransform tr,Vector3 direction,float howFar=2000)
    {
        tr.DOAnchorPos(direction * howFar, 1);
    }
    void ReturnToStartState()
    {
        EnableInput();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttonPositions[i], 1);
        }
        GoOffScreen(LevelsPanelGO, Vector2.right);
        GoOffScreen(OptionsPanelGO, Vector2.left);
        //MainMenuGO.DOAnchorPos(Vector2.zero, 1);
    }

    #endregion
    public void EnableInput()
    {
        foreach(Button b in buttons)
        {
            b.interactable = true;
            //b.GetComponent<Animator>().enabled = true;
        }
    }
    public void DisableInput()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
            //b.GetComponent<Animator>().enabled = false;
        }
    }
    private void Start()
    {
        if(!PlayerPrefs.HasKey("Started"))
        {
            FirstStartSetup();
        }
        Setuplevels();
    }
    private void OnLevelWasLoaded(int level)
    {
        Setuplevels();
    }
    void FirstStartSetup()
    {
        string save = 0 + "levelState";
        PlayerPrefs.SetInt(save, 0);
        PlayerPrefs.SetInt("Started", 1);
    }
    public void ResetProgress()
    {
        for(int i = 0;i< levelsGO.Length;i++)
        {
            string save = i + "levelState";
            PlayerPrefs.DeleteKey(save);
        }
        FirstStartSetup();
        Setuplevels();
        MySceneManager.Instance.LoadScene(0);
        //FirstStartSetup();
    }
    #region Navigation inputs
    public void StartGameButtonClicked()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        foreach(Button b in buttons)
        {
            if (b.gameObject.name == "PlayButton")
            {
                SetTitle(b.GetComponent<RectTransform>());
                LevelsPanelGO.gameObject.SetActive(true);
                SetPanel(LevelsPanelGO);
            }
            else
                GoOffScreen(b.GetComponent<RectTransform>(), Vector3.up,900);
        }
    }
    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        ReturnToStartState();
        GoOffScreen(backButton, Vector2.left);
    }
    public void OnOptionsButtonClicked()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        foreach (Button b in buttons)
        {
            if (b.gameObject.name == "OptionsButton")
            {
                SetTitle(b.GetComponent<RectTransform>());
                OptionsPanelGO.gameObject.SetActive(true);
                SetPanel(OptionsPanelGO);
            }
            else
                GoOffScreen(b.GetComponent<RectTransform>(), Vector3.up,900);
        }
    }
    public void OnExitButtonClicked()
    {
        SoundManager.Instance.PlaySound("ButtonClick");
        Application.Quit(0);
    }
    #endregion

    void Setuplevels()
    {
        int sceneCount = levelsGO.Length;
        for (int i = 0; i<sceneCount;i++)
        {
            string save = i + "levelState";
            bool s1 = false;
            bool s2 = false;
            bool s3 = false;
            bool c1 = false;
            if (PlayerPrefs.HasKey(save))
            {
                c1 = true;
                int points = PlayerPrefs.GetInt(save);
                if (points >= 4)
                {
                    s3 = true;
                    points -= 4;
                }
                if (points >= 2)
                {
                    s2 = true;
                    points -= 2;
                }
                if (points >= 1)
                {
                    s1 = true;
                }
            }
            levelsGO[i].Setup(i, c1, s1, s2, s3);
        }
    }
}
