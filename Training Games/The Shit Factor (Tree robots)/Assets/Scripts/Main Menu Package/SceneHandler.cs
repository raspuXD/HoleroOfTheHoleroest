using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    //This Script is for General Scene handling. very useful

    [SerializeField] bool startsTimeAtOne = true;
    [SerializeField] bool locksMouse = false;
    private void Start()
    {
        UpdateMouseIfNeeded(locksMouse);
        UpdateTheTimeIfNeeded(startsTimeAtOne);
    }

    public void UpdateMouseIfNeeded(bool whatToDo)
    {
        Cursor.lockState = whatToDo ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = whatToDo ? false : true;
    }

    public void UpdateTheTimeIfNeeded(bool whatToDoWithTime)
    {
        Time.timeScale = whatToDoWithTime ? 1f : 0f;
    }

    public void LoadSceneNamed(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAgain()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void CloseTheGame()
    {
        Application.Quit();
    }
}
