using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleControlScript : MonoBehaviour
{
    public MatchModule MatchClient;

    public void OnClickStartButton()
    {
        //for test 콩
        MatchClient.RequestMatch();

        MatchClient.RequestMatch();
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
