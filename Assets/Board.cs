using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
    public GameObject Tile;
    CheckerTile[,] _Board;
    CheckerTile _tile;

    bool isFirstMove = true;
    bool isLastMove = false;

    [SerializeField] Sprite onSelect, onDeselect,onDie,onWin;
    [SerializeField] Image button;
    public void Start()
    {
        _Board = new CheckerTile[PlayerPrefs.GetInt("x", 10), PlayerPrefs.GetInt("y", 10)];
        for (int i = 0; i < _Board.GetLength(0); i++)
        {
            for (int y = 0; y < _Board.GetLength(1); y++)
            {
                GameObject instance = Instantiate(Tile, new Vector2(i, y), Quaternion.identity);
                instance.TryGetComponent<CheckerTile>(out CheckerTile component);

                component.onClick.AddListener(() => { button.sprite = onSelect; });
                component.onDeselect.AddListener(() => { button.sprite = onDeselect; });
                component.Render();
                component.isMine = Random.Range(0, 100) < 20;
                component.board = this;
                component.isWorked = false;
                component.pos = new Vector2Int(i, y);
                _Board[i, y] = component;
            }
        }
        MakeHeatMap();
        SetCamera();
    }

    void SetCamera()
    {
        Camera main = Camera.main;
        main.orthographicSize = _Board.GetLength(1) / 2;
        main.transform.position = new Vector2(_Board.GetLength(0)/2,_Board.GetLength(1) / 2);
        main.transform.position -= new Vector3(0.5f,0.5f,10);
    }

    void MakeHeatMap()
    {
        for (int x = 0; x < _Board.GetLength(0); x++)
        {
            for (int y = 0; y < _Board.GetLength(1); y++)
            {
                int mineCounter = 0;
                if (isMine(new Vector2Int(x - 1, y))) mineCounter++;
                if (isMine(new Vector2Int(x - 1, y + 1))) mineCounter++;
                if (isMine(new Vector2Int(x - 1, y - 1))) mineCounter++;
                if (isMine(new Vector2Int(x + 1, y))) mineCounter++;
                if (isMine(new Vector2Int(x + 1, y + 1))) mineCounter++;
                if (isMine(new Vector2Int(x + 1, y - 1))) mineCounter++;
                if (isMine(new Vector2Int(x, y - 1))) mineCounter++;
                if (isMine(new Vector2Int(x, y + 1))) mineCounter++;

                _tile = _Board[x, y];
                _tile.mineInBetween = mineCounter - 1;
                _tile.Render();
            }
        }
    }

    bool isMine(Vector2Int pos)
    {
        if (!inBoard(pos)) return false;

        _tile = _Board[pos.x, pos.y];
        return _tile.isMine;
    }

    bool inBoard(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x > _Board.GetLength(0) - 1) return false;
        if (pos.y < 0 || pos.y > _Board.GetLength(1) - 1) return false;
        return true;
    }

    public void Click(Vector2Int pos)
    {
        if (isLastMove) return;
        if (!inBoard(pos)) return;

        _tile = _Board[pos.x, pos.y];
        if (_tile.isWorked) return;

        _tile.isWorked = true;

        if (!_tile.isMine || isFirstMove)
        {
            isFirstMove = false;
            if (isFirstMove && _tile.isMine)
            {
                _tile.isMine = false;
                _Board[pos.x + 1, pos.y + 1].isMine = true;
                MakeHeatMap();
            }
            _tile.Render();

            if (_tile.mineInBetween == -1)
            {
                Click(new Vector2Int(pos.x + 1, pos.y));
                Click(new Vector2Int(pos.x - 1, pos.y));
                Click(new Vector2Int(pos.x, pos.y + 1));
                Click(new Vector2Int(pos.x, pos.y - 1));
            }
        }
        else
        {
            button.sprite = onDie;
            unlockAllMines();
            isLastMove = true;
        }
    }

    void unlockAllMines()
    {
        for (int x = 0; x < _Board.GetLength(0); x++)
        {
            for (int y = 0; y < _Board.GetLength(1); y++)
            {
                _tile = _Board[x, y];
                if (_tile.isMine)
                {
                    _tile.isWorked = true;
                    _tile.Render();
                }
            }
        }
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        Collider2D raycast = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.one,0.1f).collider;
        if (raycast == null || !raycast.TryGetComponent<CheckerTile>(out CheckerTile component)) return;
        component.checkmarkStatus = (CheckmarkStatus)Mathf.Repeat((int)component.checkmarkStatus +1,3);
        component.Render();
    }
}
