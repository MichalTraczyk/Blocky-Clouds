using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI sceneNumber;
    public Image img;
    public GameObject s1GO;
    public GameObject s2GO;
    public GameObject s3GO;
    public Button button;
    public void Setup(int scene,bool unlocked, bool s1, bool s2,bool s3)
    {
        sceneNumber.text = (scene+1).ToString();
        if(!unlocked)
        {
            img.color = Color.black;
            s1GO.SetActive(false);
            s2GO.SetActive(false);
            s3GO.SetActive(false);

            button.interactable = false;
            return;
        }

        //Debug.Log(button);
        button.onClick.AddListener(delegate 
        {
            int i = scene;
            //SoundManager.Instance.ChangeMusic();
            GameSaver.levelToLoad = i;
            MySceneManager.Instance.LoadScene(2);       
        });
        s1GO.SetActive(s1);
        s2GO.SetActive(s2);
        s3GO.SetActive(s3);
        
    }


}
