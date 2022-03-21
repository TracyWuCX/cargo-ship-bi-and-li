using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public GameObject prevScreen;
    public GameObject loadingScreen;
    public Image loadingProgress;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    // Start is called before the first frame update
    void Start()
    {
        prevScreen.SetActive(false);
        loadingScreen.SetActive(true);
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Start"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync("OceanFloor", LoadSceneMode.Additive));
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                loadingProgress.fillAmount = totalProgress / scenesToLoad.Count;
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
