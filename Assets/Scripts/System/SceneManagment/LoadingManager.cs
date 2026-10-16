using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string SceneToLoad;

    public static void LoadScene(string sceneName)
    {
        SceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(SceneToLoad))
        {
            StartCoroutine(LoadTargetScene());
        }
    }

    private System.Collections.IEnumerator LoadTargetScene()
    {
        yield return null;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(SceneToLoad);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}