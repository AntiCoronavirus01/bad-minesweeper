using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public enum CheckmarkStatus
{
    None = 0,
    Risky = 1,
    Unknown = 2
}
public class CheckerTile : MonoBehaviour
{
    
    public bool isMine = false;
    bool isClicking = false;
    public bool isWorked = true;
    public int mineInBetween = 0;
    public CheckmarkStatus checkmarkStatus = CheckmarkStatus.None;
    [HideInInspector]
    public UnityEvent onDeselect,onClick;

    [SerializeField] Sprite HidedPressed, Hided, Risky, Unknown, UnknownPressed,Mine,MinePressed;
    public Vector2Int pos;
    [SerializeField]
    Sprite[] numbers;
    SpriteRenderer sr;
    public Board board;

    private void Get()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        print("anan");
        
        if (checkmarkStatus != CheckmarkStatus.Risky && !isWorked && PlayerPrefs.GetInt("GamePlayable",1) == 1)
        {
            onClick.Invoke();
            isClicking = true;
            onClick.Invoke();
        }
        Render();
    }
    private void OnMouseUp()
    {
        if (isClicking)
        {
            print("Baban");
            if (checkmarkStatus != CheckmarkStatus.Risky)
            {
                onDeselect.Invoke();
                board.Click(pos);
                if (isMine) return;
            }
            isClicking = false;
            
        }
        Render();
    }
    private void OnMouseExit()
    {
        if (isClicking && !isWorked)
        {
            onDeselect.Invoke();
            isClicking = false;
            print("yarra");
        }
        Render();
    }

    public void Render()
    {
        if (sr == null) Get();
        if (!isWorked)
        {
            if (isClicking)
            {
                switch (checkmarkStatus)
                {
                    case CheckmarkStatus.None:
                        sr.sprite = HidedPressed;
                        break;
                    case CheckmarkStatus.Unknown:
                        sr.sprite = UnknownPressed;
                        break;
                }
            }
            else
            {
                switch (checkmarkStatus)
                {
                    case CheckmarkStatus.None:
                        sr.sprite = Hided; 
                        break;
                    case CheckmarkStatus.Risky:
                        sr.sprite = Risky;
                        break;
                    case CheckmarkStatus.Unknown:
                        sr.sprite = Unknown;
                        break;
                    default:
                        sr.sprite = Hided;
                        break;
                }
            }
        }
        else
        {
            if (isMine)
            {
                if (isClicking)
                {
                    sr.sprite = MinePressed;
                }
                else
                {
                    sr.sprite = Mine;
                }
            }
            else
            {
                if (mineInBetween == -1)
                {
                    sr.sprite = HidedPressed;
                }
                else
                {
                    sr.sprite = numbers[mineInBetween];
                }
            }
        }    
    }

}
