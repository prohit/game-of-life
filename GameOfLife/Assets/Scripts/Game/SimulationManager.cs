using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    [SerializeField] float frequency = 1;
    float currentTime;
    bool isRunning;
    bool isCurrentCycle;

    void Start()
    {
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
            case GameState.START:
                currentTime = 0;
                isRunning = true;
                isCurrentCycle = true;
                break;

            case GameState.END:
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
