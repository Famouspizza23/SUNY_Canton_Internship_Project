using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextLevelName;

    public Animator anim;

    public void StartButton()
    {
        anim.SetTrigger("Start");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Anim_ChangeLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}
