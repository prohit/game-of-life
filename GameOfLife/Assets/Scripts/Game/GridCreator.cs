using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;

    [SerializeField] int gridthWidth = 6;
    [SerializeField] int gridHeight = 6;
    [SerializeField] int cellSize = 1;

    // Start is called before the first frame update
    void Start()
    {
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

    IEnumerator InitGame()
    {
        yield return new WaitForEndOfFrame();
        InitCells();
        yield return new WaitForEndOfFrame();
        RegisterNeighbours();
        yield return new WaitForEndOfFrame();
        StartGame();
    }

    void InitCells()
    {
        foreach(Transform item in transform)
        {
            int randState = Random.Range(0, 2);
            item.GetComponent<Cell>().SetInitalState((CellState)randState);
        }
    }

    void RegisterNeighbours()
    {
        foreach (Transform item in transform)
        {
            item.GetComponent<Cell>().RegisterNeighbours(gridthWidth, gridHeight);
        }
    }

    void StartGame()
    {
        //EventManager.TriggerGameGameStateEvent(GameState.START);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            EventManager.TriggerGameGameStateEvent(GameState.START);
        }
    }
}
