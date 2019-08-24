using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] Color deadColor;
    [SerializeField] Color aliveColor;

    public int X_Id { get; private set; }
    public int Y_Id { get; private set; }    
    public Action<CellState> neighbourStateChangeEvent;

    protected CellState currentState;
    protected CellState nextState;
    protected int aliveNeighboursCount;
    protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        EventManager.AddGameStateChangeEvent(OnGameStateChange);
        Reset();
    }

    void OnDestroy()
    {
        neighbourStateChangeEvent = null;
        EventManager.RemoveGameStateChangeEvent(OnGameStateChange);        
    }

    void Reset()
    {
        aliveNeighboursCount = 0;
        currentState = CellState.DEAD;
        nextState = CellState.DEAD;        
    }

    public void Init(int x, int y)
    {
        X_Id = x;
        Y_Id = y;
        name = $"{x}_{y}";
    }

    public void RegisterNeighbours(int limitX, int limitY)
    {
        for (int y = Y_Id - 1; y <= Y_Id + 1; y++)
        {
            for (int x = X_Id - 1; x <= X_Id + 1; x++)
            {
                var insideGrid = (x >= 0 && y >= 0 && x < limitX && y < limitY);
                if(insideGrid && !(x == X_Id && y == Y_Id))
                {
                    var neighbour = transform.parent.Find($"{x}_{y}");
                    if(neighbour != null)
                    {
                        var neigbourCell = neighbour.GetComponent<Cell>();
                        neigbourCell.neighbourStateChangeEvent += OnNeigbourStateChange;
                        if(neigbourCell.nextState == CellState.ALIVE)
                        {
                            aliveNeighboursCount++;
                            Debug.Log(name + ":  active nbr:  " + neigbourCell.name);
                        }
                    }
                }
            }
        }
    }

    public void SetInitalState(CellState state)
    {
        currentState = state;
        nextState = state;
        spriteRenderer.color = (state == CellState.DEAD) ? deadColor : aliveColor;
    }

    void OnGameStateChange(GameState state)
    {
        switch(state)
        {
            case GameState.LIFE_CHANGE:
                LifeChange();
                break;

            case GameState.LIFE_CHANGE_DONE:
                LifeChangeDone();
                break;

            case GameState.END:
                Reset();
                break;
        }
    }

    void LifeChange()
    {
        if(currentState == CellState.ALIVE)
        {
            if(aliveNeighboursCount > 3 || aliveNeighboursCount < 2)
            {
                nextState = CellState.DEAD;
                spriteRenderer.color = deadColor;                
            }
        }
        else
        {
            if(aliveNeighboursCount == 3)
            {
                nextState = CellState.ALIVE;
                spriteRenderer.color = aliveColor;
            }
        }
    }

    public void LifeChangeDone()
    {
        if(currentState != nextState)
        {
            currentState = nextState;
            neighbourStateChangeEvent.Invoke(currentState);
        }
    }

    void OnNeigbourStateChange(CellState state)
    {
        if(state == CellState.ALIVE)
        {
            aliveNeighboursCount++;
        }
        else
        {
            if (aliveNeighboursCount > 0)
                aliveNeighboursCount--;
        }
    }
}
