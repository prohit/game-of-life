using System.Collections;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;

    [SerializeField] int gridthWidth = 6;
    [SerializeField] int gridHeight = 6;
    [SerializeField] int cellSize = 1;

    [Header("To use custome seed set this value greater 0")]
    [SerializeField] int seed = -1;

    int currentSeed;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddGameStateChangeEvent(OnGameStateChange);
        var startPos = new Vector2(-(gridthWidth - cellSize) / 2.0f, -(gridHeight - cellSize) / 2.0f);
        for(int i=0; i < gridHeight; i++)
        {
            for(int j=0; j<gridthWidth; j++)
            {
                var cell = Instantiate(cellPrefab, transform).GetComponent<Cell>();
                cell.transform.position = startPos + new Vector2(cellSize * i, cellSize * j);
                cell.Init(j , i);
            }
        }
        StartCoroutine(InitGame());
    }

    void OnDestroy()
    {
        EventManager.RemoveGameStateChangeEvent(OnGameStateChange);
    }

    IEnumerator InitGame(bool reset = false)
    {
        yield return new WaitForEndOfFrame();
        InitCells(reset);
        yield return new WaitForEndOfFrame();
        RegisterNeighbours(reset);
    }

    void InitCells(bool reset = false)
    {
        if(!reset)
        {
            if (seed > 0)
            {
                currentSeed = seed;
            }
            else
            {
                currentSeed = System.DateTime.Now.Millisecond;
            }
        }
        Random.InitState(currentSeed);

        foreach (Transform item in transform)
        {
            int randState = Random.Range(0, 2);
            item.GetComponent<Cell>().SetInitalState((CellState)randState);
        }
    }

    void RegisterNeighbours(bool reset = false)
    {
        foreach (Transform item in transform)
        {
            item.GetComponent<Cell>().RegisterNeighbours(gridthWidth, gridHeight, reset);
        }
    }

    void OnGameStateChange(GameState state)
    {
        switch(state)
        {
            case GameState.RESET:
                StartCoroutine(InitGame(true));
                break;
        }
    }
}
