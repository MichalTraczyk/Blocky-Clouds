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
    [SerializeField]
    private TextMeshProUGUI hintText;

    [SerializeField]
    private TextMeshProUGUI actualMoves;

    [SerializeField]
    private TextMeshProUGUI targetMoves;

    [Header("Level finished panel")]
    [SerializeField] private GameObject gameEndPanel;

    [SerializeField] private GameObject endStar;
    [SerializeField] private GameObject movesStar;
    [SerializeField] private GameObject coinsStar;


    [Header("Pause panel")]
    [SerializeField]

    private Animator animator;
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
        Invoke("playEntryAnim", 0.5f);
    }

    void Setup()
    {
        if (GameManager.Instance == null)
            return;

        targetMoves.text = GameManager.Instance.GetTargetMoves().ToString();
        actualMoves.text = "0";
    }

    void playEntryAnim()
    {
        animator.SetTrigger("InTransition");
    }

    #region Game pausing
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
    public void OnResumeButtonClicked()
    {
        GameManager.Instance.Resume();
    }
    #endregion


    #region Target moves text
    public void SetMoveText(int m, MoveType type)
    {
        switch (type)
        {
            case MoveType.UNDER_TARGET:
                SetMovesTextColor(Color.white);
                break;
            case MoveType.IDEAL:
                SetMovesTextColor(Color.green);
                break;
            case MoveType.OVER_TARGET:
                SetMovesTextColor(Color.red);
                break;
        }
        actualMoves.text = m.ToString();
    }
    void SetMovesTextColor(Color c)
    {
        actualMoves.color = c;
    }
    #endregion
    #region Showing hint
    public void ShowHint(string hint)
    {
        StartCoroutine(EnumerateOnLetters(hint));
    }
    IEnumerator EnumerateOnLetters(string str)
    {
        string startString = "";
        for (int i = 0; i < str.Length; i++)
        {
            startString += str[i];
            hintText.text = startString;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2.5f);
        hintText.text = "";
    }
    #endregion

    #region Level finishing
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
    #endregion

    public void GoOutSceneTransition()
    {
        animator.SetTrigger("StartTransition");
    }
}
