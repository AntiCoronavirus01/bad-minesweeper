using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("GamePlayable",1);
    }
    [SerializeField]TMP_Text hSliderText, vSliderText;
    [SerializeField]Scrollbar hSlider;
    [SerializeField] Scrollbar vSlider;
    public void ReStartGame()
    {
        Camera.main.orthographicSize = 0;
        SceneManager.LoadScene(0);
    }

    public void changeVslider()
    {
        vSliderText.text = ""+ (Mathf.RoundToInt(vSlider.value * 100) + 10);
    }
    public void changeHSlider()
    {
        hSliderText.text = ""+ (Mathf.RoundToInt(hSlider.value * 100) + 10);
    }
    public void SetSize()
    {
        PlayerPrefs.SetInt("x",Mathf.RoundToInt(hSlider.value * 100) + 10);
        PlayerPrefs.SetInt("y", Mathf.RoundToInt(vSlider.value * 100) + 10);
    }
    bool flipFlop = false;
    public void StartStopGame()
    {
        PlayerPrefs.SetInt("GamePlayable", flipFlop ? 0 : 1);
        flipFlop = !flipFlop;
    }
}
