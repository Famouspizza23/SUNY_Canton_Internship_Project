using UnityEngine;
using UnityEngine.SceneManagement;

public class Introduction : MonoBehaviour
{
    public string nextLevelName;

    [Header("Animation")]
    public Animator anim;

    [Header("Audio")]
    public AudioSource aSource;
    public AudioClip eSliding;
    public AudioClip eOpen;
    public AudioClip letterRead;

    public void ContinueButton()
    {
        anim.SetTrigger("Continue");
    }

    public void Anim_ChangeScene()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    #region Audio Cues

    public void Audio_EnvelopeSliding()
    {
        aSource.PlayOneShot(eSliding);
    }

    public void Audio_EnvelopeOpen()
    {
        aSource.PlayOneShot(eOpen);
    }

    public void Audio_LetterRead()
    {
        aSource.PlayOneShot(letterRead);
    }

    #endregion
}