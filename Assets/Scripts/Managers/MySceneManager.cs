using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
   
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void LoadScene(int scene)
    {
        if (scene == 1)
            return;

        Time.timeScale = 1;
        SoundManager.Instance.ChangeMusic();
        if (scene > SceneManager.sceneCountInBuildSettings - 1)
            StartCoroutine(loadScene(0));
        else
            StartCoroutine(loadScene(scene));
    }

    IEnumerator loadScene(int scene)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            FindObjectOfType<MainMenu>().GetComponent<Animator>().SetTrigger("StartTransition");
        else
            UIManager.Instance.GoOutSceneTransition();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
        if(scene != 0)
            SceneManager.LoadScene(1,LoadSceneMode.Additive);

    }
}
