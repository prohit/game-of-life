using System.Collections;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] float frequency = 1;
    float currentTime;
    bool isRunning;
    bool isCurrentCycle;

    void Start()
    {
        currentTime = 0;
        isRunning = false;
        isCurrentCycle = false;
        EventManager.AddGameStateChangeEvent(OnGameStateChange);
    }

    void OnDestroy()
    {
        EventManager.RemoveGameStateChangeEvent(OnGameStateChange);
    }

    void OnGameStateChange(GameState state)
    {
        switch(state)
        {
            case GameState.PLAY:
                isRunning = true;
                isCurrentCycle = true;
                break;
                
            case GameState.PAUSE:                            
                isRunning = false;
                break;

            case GameState.RESET:
            case GameState.END:
                currentTime = 0;
                isCurrentCycle = false;
                isRunning = false;
                break;           
        }
    }

    private void Update()
    {
        if(isRunning)
        {
            if (isCurrentCycle)
            {
                if (currentTime < frequency)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    isCurrentCycle = false;
                    currentTime = 0;
                    StartCoroutine(UpdateLife());
                }
            }
        }
    }

    IEnumerator UpdateLife()
    {
        EventManager.TriggerGameGameStateEvent(GameState.LIFE_CHANGE);
        yield return new WaitForEndOfFrame();
        EventManager.TriggerGameGameStateEvent(GameState.LIFE_CHANGE_DONE);
        yield return new WaitForEndOfFrame();
        isCurrentCycle = true;
    }
}
