using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum MoveType
{
    UNDER_TARGET,
    IDEAL,
    OVER_TARGET
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int movesMade;
    private int targetMoves;

    [SerializeField]
    private LayerMask groundLayers;
    public LayerMask GroundLayers { get => groundLayers;}

    private bool paused = false;

    private string hint;


    [SerializeField]
    private GameObject hitParticles;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        InitializeLevel();
        HideCursor();
        UIManager.Instance.ShowHint(hint); 
    }
    void InitializeLevel()
    {
        string name = "Level" + GameSaver.levelToLoad;
        string path = "Levels/" + name;
        SceneToLoad level = Resources.Load<SceneToLoad>(path);

        hint = level.hint;
        targetMoves = level.targetMoves;
        Instantiate(level.mapPrefab, Vector3.zero, Quaternion.identity);
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Pause()
    {
        ShowCursor();
        FindObjectOfType<PlayerMove>().enabled = false;
        paused = true;
        Time.timeScale = 0;
        UIManager.Instance.Pause();
    }
    public void Resume()
    {
        HideCursor();
        FindObjectOfType<PlayerMove>().enabled = true;
        paused = false;
        UIManager.Instance.Unpause();
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Resume();

            }
            else
            {
                Pause();
            }
        }
    }
    public void AddMoveCount()
    {
        movesMade++;
        MoveType t = MoveType.UNDER_TARGET;
        if (movesMade == GetTargetMoves())
            t = MoveType.IDEAL;
        else if (movesMade > GetTargetMoves())
            t = MoveType.OVER_TARGET;

        UIManager.Instance.SetMoveText(movesMade,t);
    }
    public void CollectCoin()
    {
        //colectedCoins++;
    }
    public int GetTargetMoves()
    {
        return targetMoves;
    }
    public void FinishGame()
    { 
        Coin[] coins = FindObjectsOfType<Coin>();
        bool coinStar = false;
        if (coins.Length == 0)
            coinStar = true;


        bool movesStar = false;
        if (movesMade == targetMoves)
            movesStar = true;

        FindObjectOfType<PlayerMove>().enabled = false;
        Save(true, movesStar, coinStar);
        ShowCursor();
        UIManager.Instance.SetWinPanel(true, movesStar, coinStar);
        enabled = false;
    }

    void Save(bool s1,bool s2,bool s3)
    {

        //new save sytem
        int currLevel = GameSaver.levelToLoad;
        string toSave = currLevel + "levelState";
        int points = 0;
        if (s1)
            points += 1;
        if (s2)
            points += 2;
        if (s3)
            points += 4;

        PlayerPrefs.SetInt(toSave, points);

        string nextLvl = (currLevel+1) + "levelState";
        if(!PlayerPrefs.HasKey(nextLvl))
            PlayerPrefs.SetInt(nextLvl, 0);
    }

    public void HitWall(Vector3 pos, Vector3 moveVector)
    {
        SoundManager.Instance.PlaySound("Hit");
        RaycastHit hit;
        if (Physics.Raycast(pos, moveVector, out hit, 1.5f, groundLayers))
        {
            Wall w = hit.transform.GetComponent<Wall>();
            if (w != null)
            {
                Quaternion rotation = Quaternion.LookRotation(hit.normal);
                Instantiate(hitParticles, hit.point, rotation);
                w.Hit();
                return;
            }
            MovableBlock b = hit.transform.GetComponent<MovableBlock>();
            if (b != null)
            {
                b.Move(moveVector);
                return;
            }
            
            Lever l = hit.transform.GetComponent<Lever>();
            if(l != null)
            {
                l.PullLever();
                return;
            }
        }
    }
}
