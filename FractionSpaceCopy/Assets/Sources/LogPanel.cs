using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogPanel : MonoBehaviour
{
    protected static LogPanel current;

    public TMP_Text logLabel;

    void Awake()
    {
        current = this;
    }

    public static void Write(string message)
    {
        current.logLabel.text = message;
    }
}
