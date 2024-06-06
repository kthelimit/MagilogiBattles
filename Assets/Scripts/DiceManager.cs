using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour
{
    public Transform Player1;
    public Transform Player1boost;
    public Transform Player2;
    public Transform Player2boost;

    public List<Dice> dices1st;
    public List<Dice> dices2nd;

    public bool is1stConcen = false;
    public bool is2ndConcen = false; // 사실 집중방어는 플롯을 할 수 있는 적만 쓸 수 있다.

    public GameObject DicePrefab;
    public TMP_Text damageText;

    public void Init()
    {
        is1stConcen = false;
        is2ndConcen = false;
        dices1st = new List<Dice>();
        dices2nd = new List<Dice>();
        Player1boost.gameObject.SetActive(false);
        Player2boost.gameObject.SetActive(false);
        if (GameManager.gameManager.orderCount == GameManager.Order.FirstAttack)
        {
            //PL1이 공격턴일때
            int PL1Stat = GameManager.gameManager.player[0].atk;
            for (int i = 0; i < PL1Stat; i++)
            {
                GameObject obj = Instantiate(DicePrefab, Player1);
            }
            int PL2Stat = GameManager.gameManager.player[1].def;
            for (int i = 0; i < PL2Stat; i++)
            {
                GameObject obj = Instantiate(DicePrefab, Player2);
                obj.GetComponent<Dice>().DiceChange(8);
                obj.GetComponent<Button>().enabled = false;
            }
        }
        else if (GameManager.gameManager.orderCount == GameManager.Order.SecondAttack)
        {
            //PL2가 공격턴일때
            int PL1Stat = GameManager.gameManager.player[0].def;
            for (int i = 0; i < PL1Stat; i++)
            {
                GameObject obj = Instantiate(DicePrefab, Player1);
            }
            int PL2Stat = GameManager.gameManager.player[1].atk;
            for (int i = 0; i < PL2Stat; i++)
            {
                GameObject obj = Instantiate(DicePrefab, Player2);
                obj.GetComponent<Dice>().DiceChange(8);
                obj.GetComponent<Button>().enabled = false;
            }
        }
    }

    public void PlotComplete()
    {
        //플레이어1의 주사위
        //입력한 다이스들을 가져옴
        dices1st = Player1.GetComponentsInChildren<Dice>().ToList<Dice>();
        List<int> dicenums1st = new List<int>();
        for (int i = 0; i < dices1st.Count; i++)
        {
            if (dices1st[i].diceNum == 8)
            {
                dices1st[i].RandomDice();
            }
            dicenums1st.Add(dices1st[i].diceNum);
        }
        //다이스들을 정렬함
        dicenums1st.Sort();
        if (dicenums1st[2] == 7)
        {
            is1stConcen = true;
        }
        //다시 배치
        for (int i = 0; i < dicenums1st.Count; i++)
        {
            dices1st[i].DiceChange(dicenums1st[i]);
        }



        //플레이어2의 주사위.
        dices2nd = Player2.GetComponentsInChildren<Dice>().ToList<Dice>();
        List<int> dicenums2nd = new List<int>();
        for (int i = 0; i < dices2nd.Count; i++)
        {
            if (dices2nd[i].diceNum == 8)
            {
                dices2nd[i].RandomDice();
            }
            dicenums2nd.Add(dices2nd[i].diceNum);
        }
        //다이스들을 정렬함
        dicenums2nd.Sort();
        //다시 배치
        for (int i = 0; i < dicenums2nd.Count; i++)
        {
            dices2nd[i].DiceChange(dicenums2nd[i]);
        }
        CheckOverlap();

        //대미지 계산
        int damage = 0;
        if (GameManager.gameManager.orderCount == GameManager.Order.FirstAttack)
        {
            for (int i = 0; i < dices1st.Count; i++)
            {
                if (!dices1st[i].isOverlap)
                {
                    damage++;
                }
            }
        }
        else if (GameManager.gameManager.orderCount == GameManager.Order.SecondAttack)
        {
            for (int i = 0; i < dices2nd.Count; i++)
            {
                if (!dices2nd[i].isOverlap)
                {
                    damage++;
                }
            }
        }
        Debug.Log(damage);
        damageText.text = "대미지 : " + damage.ToString();
    }

    public void CheckOverlap()
    {
        if (is1stConcen)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < dices2nd.Count; k++)
                {
                    if(dices1st[i].diceNum == dices2nd[k].diceNum)
                    {
                        dices1st[i].isOverlap = true;
                        dices1st[i].ChangeColor();
                        dices2nd[k].isOverlap = true;
                        dices2nd[k].ChangeColor();
                    }
                }
            }
        }
        else if (is2ndConcen)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < dices1st.Count; k++)
                {
                    if (dices2nd[i].diceNum == dices1st[k].diceNum)
                    {
                        dices2nd[i].isOverlap = true;
                        dices2nd[i].ChangeColor();
                        dices1st[k].isOverlap = true;
                        dices1st[k].ChangeColor();
                    }
                }
            }
        }
        else
        {
            //일반적인 상황에
            for (int i = 0; i < dices1st.Count; i++)
            {
                for (int k = 0; k < dices2nd.Count; k++)
                {
                    if (dices1st[i].diceNum == dices2nd[k].diceNum && !dices1st[i].isOverlap && !dices2nd[k].isOverlap)
                    {
                        dices1st[i].isOverlap = true;
                        dices1st[i].ChangeColor();
                        dices2nd[k].isOverlap = true;
                        dices2nd[k].ChangeColor();
                        break;

                    }
                }
            }
        }
    }


    public void Concen1st()
    {
        dices1st = Player1.GetComponentsInChildren<Dice>().ToList<Dice>();
        for(int i=2; i<dices1st.Count;i++)
        {
            dices1st[i].DiceChange(7);
        }
    }
}
