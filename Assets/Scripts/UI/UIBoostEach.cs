using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBoostEach : MonoBehaviour
{
    public GameObject go;
    public TMP_Text text;

    public void SetEffect(int num)
    {
        text.text = num.ToString();
        if(num == 0)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
        }
    }

}
