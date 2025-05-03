using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] bool startsTimeAtOne = true;
    public bool locksMouse = false;

    private void Start()
    {
        UpdateTheTimeIfNeeded(startsTimeAtOne);
        UpdateMouseState(locksMouse);
    }
    public void UpdateMouseState(bool unlockMouse)
    {
        Cursor.lockState = unlockMouse ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = unlockMouse;
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
