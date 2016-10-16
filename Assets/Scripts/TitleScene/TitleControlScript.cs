using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleControlScript : MonoBehaviour
{
    public MatchModule MatchClient;
    public UIButton StartButton;
    public UIButton ExitButton;

    public void OnClickStartButton()
    {
        StartButton.enabled = false;
        ExitButton.enabled = false;
        MatchClient.RequestMatch();
    }

    public void OnClickExitButton()
    {
        StartButton.enabled = false;
        ExitButton.enabled = false;
        Application.Quit();
    }
}
