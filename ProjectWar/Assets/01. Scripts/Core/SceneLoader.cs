using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static SceneLoader Instance = null;

    private float syncDelayTick = 0.5f;

    public void LoadSceneAsync(string sceneName, Action onCompleted = null)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        GameManager.Instance.StartCoroutine(AsyncLoadCoroutine(asyncOper, onCompleted));
    }

    private IEnumerator AsyncLoadCoroutine(AsyncOperation asyncOper, Action onCompleted)
    {
        YieldInstruction delay = new WaitForSeconds(syncDelayTick);

        while (true)
        {
            if (asyncOper.isDone)
                break;

            yield return delay;
        }

        onCompleted?.Invoke();
    }
}
