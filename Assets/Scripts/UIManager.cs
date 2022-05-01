using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public static UIManager Instance;

    [Header("Gameplay UI")]
    public TextMeshProUGUI hintText;
    [SerializeField]
    TextMeshProUGUI actualMoves;
    [SerializeField]
    TextMeshProUGUI targetMoves;

    Animator animator;
    [Header("Level finished panel")]
    public GameObject gameEndPanel;

    public GameObject endStar;
    public GameObject movesStar;
    public GameObject coinsStar;
    [Header("Pause panel")]
    public GameObject pausePanel;
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Setup();
        Invoke("playAnim", 0.5f);
        //hintText.text = "";
        //InSceneTransition();
    }
    void playAnim()
    {
        animator.SetTrigger("InTransition");
    }
    void Setup()
    {
        if (GameManager.Instance == null)
            return;

        targetMoves.text = GameManager.Instance.GetTargetMoves().ToString();
        actualMoves.text = "0";
    }
    public void Pause()
    {
        //pausePanel.SetActive(true);
        animator.SetBool("paused", true);
    }
    public void Unpause()
    {
        //pausePanel.SetActive(false);
        animator.SetBool("paused", false);
    }
    public void SetMoveText(int m, MoveType type)
    {
        switch (type)
        {
            case MoveType.underTarget:
                SetMovesTextColor(Color.white);
                break;
            case MoveType.ideal:
                SetMovesTextColor(Color.green);
                break;
            case MoveType.overTarget:
                SetMovesTextColor(Color.red);
                break;
        }
        actualMoves.text = m.ToString();
    }
    void SetMovesTextColor(Color c)
    {
        actualMoves.color = c;
    }
    public void SetWinPanel(bool star1,bool star2,bool star3)
    {
        animator.SetTrigger("Win");
        StartCoroutine(setStars(star1, star2, star3));
    }
    IEnumerator setStars(bool s1, bool s2, bool s3)
    {
        gameEndPanel.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        endStar.SetActive(s1);
        yield return new WaitForSeconds(0.5f);
        movesStar.SetActive(s2);
        yield return new WaitForSeconds(0.5f);
        coinsStar.SetActive(s3);
    }
    public void OnResumeButtonClicked()
    {
        GameManager.Instance.Resume();
    }    
    public void OnNextLevelButtonClicked()
    {
        GameSaver.levelToLoad++;
        if (GameSaver.levelToLoad >= 9)
        {
            MySceneManager.Instance.LoadScene(0);
        }
        else
        {
            MySceneManager.Instance.LoadScene(2);
        }
    }
    public void OnRestartButtonClicked()
    {
        //int thisLvl = GameSaver.levelToLoad;
        MySceneManager.Instance.LoadScene(2);
    }
    public void OnMainMenuButtonClicked()
    {
        MySceneManager.Instance.LoadScene(0);
    }

    public void ShowHint(string hint)
    {
        StartCoroutine(EnumerateOnLetters(hint));
    }
    IEnumerator EnumerateOnLetters(string str)
    {
        string startString = "";
        for(int i = 0; i<str.Length;i++)
        {
            startString += str[i];
            hintText.text = startString;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2.5f);
        hintText.text = "";
    }


    public void GoOutSceneTransition()
    {
        animator.SetTrigger("StartTransition");
    }
}
