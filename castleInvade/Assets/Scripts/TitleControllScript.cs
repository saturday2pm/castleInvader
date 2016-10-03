using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleControllScript : MonoBehaviour {

    public void OnClickStartButton()
    {
        Debug.Log("start button clicked!");
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickExitButton()
    {
        Debug.Log("exit button clicked!");
        Application.Quit();
    }
}
