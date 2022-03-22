using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ChangeTo(string sceneName)
    {
        LoadScene.Instance.Loading(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
