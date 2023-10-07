using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Game");
    }
}
