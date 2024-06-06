using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatEach : MonoBehaviour
{
    public bool isBadStatus = false;
    public Button buttonUp;
    public Button buttonDown;
    public TMP_Text statsTxt;

    public void SetStat(int num)
    {
        statsTxt.text = num.ToString();
    }

    public void ChangeColor(bool isActive)
    {
        if (isActive)
        {
            statsTxt.color = Color.red;
        }
        else
        {
            statsTxt.color = Color.white;
        }
    }
}


