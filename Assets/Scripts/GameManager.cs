using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum MoveType
{
    underTarget,
    ideal,
    overTarget
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SkyboxSO currSkybox;

    private int movesMade;
    [SerializeField]
    private int targetMoves;

    [SerializeField]
    private LayerMask groundLayers;
    public LayerMask GroundLayers { get => groundLayers;}

    bool paused = false;
    public string hint;


    public GameObject DayVolume;
    public GameObject NightVolume;

    public GameObject hitParticles;

    public float skyboxScrollSpeed;
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
        SetRenderSettings();
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
    public void SetRenderSettings()
    {
        int hq = PlayerPrefs.GetInt("goodGfx");
        bool highQuality = (hq == 1) ? true : false;
        if(highQuality)
        {
            int r = Random.Range(1, 6);

            string name = "HQSkybox" + r;
            string path = "SkyboxSO/HQ/" + name;

            SkyboxSO skybox = Resources.Load<SkyboxSO>(path);

            currSkybox = skybox;

            //if (RenderSettings.skybox != currSkybox.defaultMaterial)
            Debug.Log(currSkybox.defaultMaterial);
            RenderSettings.skybox = currSkybox.defaultMaterial;

            RenderSettings.skybox.SetTexture("_Tex", currSkybox.cubemap);
            //Debug.Log(skybox.material.mainTexture);
            RenderSettings.ambientSkyColor = currSkybox.lightColor;
            RenderSettings.ambientIntensity = currSkybox.lightInensity;
        }
        else
        {
            int r = Random.Range(1, 5);

            string name = "LQSkybox" + r;
            string path = "SkyboxSO/LQ/" + name;

            SkyboxSO skybox = Resources.Load<SkyboxSO>(path);

            currSkybox = skybox;

            //if (RenderSettings.skybox != currSkybox.defaultMaterial)
            Debug.Log(currSkybox.defaultMaterial);
            RenderSettings.skybox = currSkybox.defaultMaterial;

            RenderSettings.skybox.SetTexture("_FrontTex",currSkybox._FrontTex);
            RenderSettings.skybox.SetTexture("_BackTex", currSkybox._BackTex);
            RenderSettings.skybox.SetTexture("_LeftTex", currSkybox._LeftTex);
            RenderSettings.skybox.SetTexture("_RightTex", currSkybox._RightTex);
            RenderSettings.skybox.SetTexture("_UpTex", currSkybox._UpTex);
            RenderSettings.skybox.SetTexture("_DownTex", currSkybox._DownTex);

        }
        UpdatepostProcessingVolume();
    }
    public void UpdatepostProcessingVolume()
    {
        bool volumeEnabled = (PlayerPrefs.GetInt("useVolume") == 1) ? true : false;
        if (volumeEnabled)
        {
            if (currSkybox.atNight)
            {
                NightVolume.SetActive(true);
                DayVolume.SetActive(false);
            }
            else
            {
                NightVolume.SetActive(false);
                DayVolume.SetActive(true);
            }
        }
        else
        {
            NightVolume.SetActive(false);
            DayVolume.SetActive(false);
        }
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
    void ScrollSkybox()
    {
        if (currSkybox != null && !currSkybox.HQ)
        {
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxScrollSpeed);
        }
    }
    private void Update()
    {
        ScrollSkybox();
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
        MoveType t = MoveType.underTarget;
        if (movesMade == GetTargetMoves())
            t = MoveType.ideal;
        else if (movesMade > GetTargetMoves())
            t = MoveType.overTarget;

        UIManager.Instance.SetMoveText(movesMade,t);
        //Debug.Log("moves made: " + movesMade);
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
        //string scene
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
    #region move checks
    public bool ValidFloorCheck(Vector3 moveTarget)
    {
        RaycastHit hit;
        if (Physics.Raycast(moveTarget, Vector3.down, out hit,3f, groundLayers))
        {
            MovingFloor movingFloor = hit.transform.GetComponentInParent<MovingFloor>();
            
            if (movingFloor != null && movingFloor.shouldMove)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    public bool WallInFront(Vector3 pos,Vector3 direction)
    {
        if (Physics.Raycast(pos, direction, 1.5f, groundLayers))
        {
           return true;
        }
        return false;
    }
    #endregion
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
