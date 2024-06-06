using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalEffect : MonoBehaviour
{
    Image image;
    Text effectTxt;
    public bool is1st = false;
    public int whateffect = 0;
    int effecCount=0;

    private void Awake()
    {
        image = GetComponent<Image>();
        effectTxt = GetComponentInChildren<Text>();
    }
    private void Start()
    {
        //switch (whateffect)
        //{
        //    case 1:
        //        effecCount = GameManager.gameManager.LoadAdd(is1st);
        //        break;
        //    case 2:
        //        effecCount = GameManager.gameManager.LoadBlock(is1st);
        //        break;
        //    case 3:
        //        effecCount = GameManager.gameManager.LoadBoost(is1st);
        //        break;
        //}
        //ChangeCount(effecCount);
    }

    void ChangeCount(int num)
    {
        effecCount = num;
        effectTxt.text = effecCount.ToString();
        if(effecCount==0)
        {
            image.transform.gameObject.SetActive(false);
        }
        else
        {
            image.transform.gameObject.SetActive(true);
        }

    }
}
