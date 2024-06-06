using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusEach : MonoBehaviour
{
    public Image img1;
    public Image img2;
    public Button btn;

    public void ChangeStatus(bool isActive)
    {
        if (isActive)
        {
            img1.color = Color.white;
            img2.color = Color.white;
        }
        else
        {
            img1.color = Color.grey;
            img2.color = Color.grey;
        }
    }
}
