using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    AsyncOperation asyncLoad;
    bool bLoadDone;
    public enum Scenes
    {
        MainMenu = 0,
        GameScene = 1,
        DeathScene = 3
    }
    public IEnumerator LoadAsyncScene(Scenes scene)
    {
        bLoadDone = false;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //scene has loaded as much as possible,
            // the last 10% can't be multi-threaded
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        bLoadDone = asyncLoad.isDone;
    }
}
