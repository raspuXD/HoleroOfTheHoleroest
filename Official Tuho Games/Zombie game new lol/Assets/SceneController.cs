using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool LocksMouse = true;
    public bool UnlocksMouse = true;

    private void Start()
    {
        if (LocksMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (UnlocksMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1.0f;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
