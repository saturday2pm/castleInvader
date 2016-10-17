using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleControlScript : MonoBehaviour
{
    public MatchModule MatchClient;
    public UIButton SingleButton;
    public UIButton MultiButton;
    public UIButton ExitButton;

    void Start()
    {
        MatchClient.OnConnected += OnMatchClientConnected;
        MultiButton.isEnabled = false;
    }

    public void OnClickSingleButton()
    {
        SingleButton.isEnabled = false;
        MultiButton.isEnabled = false;
        ExitButton.isEnabled = false;

        MatchModule.LastSuccessMatch = null;
        MatchClient.Close();
        SceneManager.LoadScene("GameScene");
        //DO SOMETHING FOR SOLO GAME
    }

    public void OnClickMultiButton()
    {
        SingleButton.isEnabled = false;
        MultiButton.isEnabled = false;
        ExitButton.isEnabled = false;

        MatchClient.RequestMatch();
    }

    public void OnClickExitButton()
    {
        SingleButton.isEnabled = false;
        MultiButton.isEnabled = false;
        ExitButton.isEnabled = false;

        Application.Quit();
    }

    void OnMatchClientConnected()
    {
        MultiButton.isEnabled = true;
    }
}
