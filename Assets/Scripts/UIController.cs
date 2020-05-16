using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void LoadGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
    }

    public void BackToMenu () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
    }

    public void ExitGame () {
        Application.Quit ();
    }
}
