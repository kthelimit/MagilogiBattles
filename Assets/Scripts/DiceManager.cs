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
    public Transform Option1;
    public Transform Option2;
    public Transform buttonPlot;
    public Transform buttonBoost;
    public Transform buttonContract;
    public Transform textNotice;

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
        Option1.gameObject.SetActive(true);
        Option2.gameObject.SetActive(true);
        buttonPlot.gameObject.SetActive(true);
        buttonBoost.gameObject.SetActive(false);
        buttonContract.gameObject.SetActive(false);
        textNotice.gameObject.SetActive(false);
        //기존의 주사위들을 제거하기
        Dice[] gameObjects = Player1.GetComponentsInChildren<Dice>();
        foreach (Dice dice in gameObjects)
        {
            Destroy(dice.gameObject);
        }
        gameObjects = Player2.GetComponentsInChildren<Dice>();
        foreach (Dice dice in gameObjects)
        {
            Destroy(dice.gameObject);
        }

        //주사위 새로 생성하기
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
        //다이스를 오름차순으로 정렬함
        dicenums1st.Sort();
        //집중방어인지 체크. 2개만 숫자이고 나머지가 X라면 집중방어로 인식한다.
        if (dicenums1st[2] == 7)
        {
            is1stConcen = true;
        }
        //만일 집중방어가 아닌데 X를 사용했을 경우, 랜덤처리하기
        if (!is1stConcen)
        {
            for (int i = 0; i < dices1st.Count; i++)
            {
                if (dicenums1st[i] == 7)
                {
                    dicenums1st[i] = Random.Range(1, 7);
                }
            }
            textNotice.gameObject.SetActive(true);
            textNotice.GetComponent<TMP_Text>().text = "집중 방어 조건을 만족하지 못해 \r\nX를 전부 랜덤주사위로 변경합니다.";

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

        //플롯을 완료한 후에는 집중방어나 적의 집중 방어 허용 옵션이 떠서는 안됨.
        Option1.gameObject.SetActive(false);
        Option2.gameObject.SetActive(false);

        //플롯을 완료한 후에는 다시 재플롯할 수 없게한다. 또한 부스트와 계약을 사용할 수 있게 된다.
        buttonPlot.gameObject.SetActive(false);
        buttonBoost.gameObject.SetActive(true);
        buttonContract.gameObject.SetActive(true);
    }

    public void CheckOverlap()
    {
        //플레이어가 집중방어를 사용했을때
        if (is1stConcen)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < dices2nd.Count; k++)
                {
                    if (dices1st[i].diceNum == dices2nd[k].diceNum)
                    {
                        dices1st[i].isOverlap = true;
                        dices1st[i].ChangeColor();
                        dices2nd[k].isOverlap = true;
                        dices2nd[k].ChangeColor();
                    }
                }
            }
        }
        //플레이어2, 즉 에너미가 집중방어를 사용했을때
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
            //아무도 집중방어를 사용하지 않는 일반적인 상황에
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

    //플레이어가 집중방어를 사용하겠다는 토글을 체크하면 맨 위의 2개를 제외한 나머지를 X로 변경.
    public void Concen1st()
    {
        if (Option1.GetComponent<Toggle>().isOn)
        {
            //집중방어에 대한 설명을 출력해준다.
            textNotice.gameObject.SetActive(true);
            textNotice.GetComponent<TMP_Text>().text = "집중방어를 하는 경우,\r\n2종류의 주사위를 놓고\r\n거기에 해당하는 주사위를\r\n얼마든지 지울 수 있습니다.";
            dices1st = Player1.GetComponentsInChildren<Dice>().ToList<Dice>();
            for (int i = 2; i < dices1st.Count; i++)
            {
                dices1st[i].DiceChange(7);
            }
        }
        else
        {
            //설명 제거
            textNotice.gameObject.SetActive(false);
        }
    }
}
