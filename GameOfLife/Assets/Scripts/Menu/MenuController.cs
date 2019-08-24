using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] Text labelPlayPause;
    bool isRunning;

    private void Start()
    {
        isRunning = false;
    }

    public void TogglePlayPause()
    {
        isRunning = !isRunning;
        EventManager.TriggerGameGameStateEvent(isRunning ? GameState.PLAY : GameState.PAUSE);
        labelPlayPause.text = isRunning ? "Pause" : "Play";
    }

    public void Restart()
    {
        isRunning = false;
        labelPlayPause.text = "Play";
        EventManager.TriggerGameGameStateEvent(GameState.RESET);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
