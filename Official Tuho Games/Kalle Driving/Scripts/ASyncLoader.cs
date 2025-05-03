using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    public GameObject loadingObject, mainMenuObject;

    public Image theLoadImage;

    public TMP_Text theTip;
    public string[] tips;

    public void LoadLeveltn(string leee)
    {
        mainMenuObject.SetActive(false);
        loadingObject.SetActive(true);
        int i = Random.Range(0,tips.Length);
        theTip.text = tips[i];


        StartCoroutine(LoadLevelASync(leee));
    }

    IEnumerator LoadLevelASync(string leee)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(leee);
        
        while(!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / .9f);
            theLoadImage.fillAmount = progress;
            yield return null;
        }
    }
}
