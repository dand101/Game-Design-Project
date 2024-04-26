using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;


public class LvlCompleteDisplay : MonoBehaviour
{
    public int displayTime = 2;
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }
    
    public void DisplayLevelComplete()
    {
        _text.text = "Level Complete";
        Invoke(nameof(ClearText), displayTime);
    }

    private void ClearText()
    {
        _text.text = "";
    }
}