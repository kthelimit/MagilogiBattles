using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager; // 싱글턴

    //배열화하면 더 편할테니까 배열로 바꾸자.
    public List<PlayerInfo> player;
    public List<UIStat> stat;
    public List<UIStatus> status;
    public List<UIBoost> boost;

    //플레이어1   
    public UIStat StatPl1;
    public UIStatus StatusPl1;
    public UIBoost BoostPl1;
    public List<Dice> player1Dices;

    //플레이어2 
    public UIStat StatPl2;
    public UIStatus StatusPl2;
    public UIBoost BoostPl2;
    public List<Dice> player2Dices;

    //공통
    public enum Order{ FirstSummon, SecondSummon, FirstAttack, SecondAttack}
    public Order orderCount = Order.FirstSummon; 
    public int RoundCount = 1;


    private void Awake()
    {
        if (gameManager == null) //게임매니저가 없는 경우
        {
            gameManager = this;
        }
        else //게임매니저가 있는 경우
        {
            Debug.LogWarning("씬에 두 개 이상의 게임매니저가 존재합니다.");
            Destroy(gameObject);
        }

        player = new List<PlayerInfo>();
        stat = new List<UIStat>();
        status = new List<UIStatus>();
        boost = new List<UIBoost>();
        player1Dices = new List<Dice>();
        player2Dices = new List<Dice>();
    }
    void Start()
    {
        //플레이어 스펙은 추후 시작 화면에서 설정할 수 있게 하는게 좋을지도

        PlayerInfo player1 = new PlayerInfo("플레이어1", 1, 0, 3, 3, 3);
        PlayerInfo player2 = new PlayerInfo("플레이어2", 1, 0, 3, 3, 3);
        player.Add(player1);
        player.Add(player2);
        stat.Add(StatPl1);
        stat.Add(StatPl2);
        status.Add(StatusPl1);
        status.Add(StatusPl2);
        boost.Add(BoostPl1); 
        boost.Add(BoostPl2);

        Init();

    }

    //각 버튼을 세팅해주는 함수
    private void Init()
    {

        for (int i = 0; i < 2; i++)
        {
            UIStat tempstat = stat[i];
            UIStatus tempstatus= status[i];
            PlayerInfo tempplayer = player[i];
            UIBoost tempboost = boost[i];

            //초기화
            tempstat.statAtk.SetStat(tempplayer.atk);
            tempstat.statDef.SetStat(tempplayer.def);
            tempstat.statBas.SetStat(tempplayer.bas);
            tempstatus.chadan.ChangeStatus(tempplayer.debuffChadan);
            tempstatus.byungma.ChangeStatus(tempplayer.debuffByungma);
            tempstatus.heoyak.ChangeStatus(tempplayer.debuffHeoyak);
            tempstatus.tajim.ChangeStatus(tempplayer.debuffTajim);
            tempboost.AddAtk.SetEffect(0);
            tempboost.Block.SetEffect(0);
            tempboost.Boost.SetEffect(0);

            //스탯의 버튼업 세팅
            tempstat.statAtk.buttonUp.onClick.AddListener(() =>
            {
                tempplayer.atk++;
                tempstat.statAtk.SetStat(tempplayer.atk);
            });
            tempstat.statDef.buttonUp.onClick.AddListener(() =>
            {
                tempplayer.def++;
                tempstat.statDef.SetStat(tempplayer.def);
            });
            tempstat.statBas.buttonUp.onClick.AddListener(() =>
            {
                tempplayer.bas++;
                tempstat.statBas.SetStat(tempplayer.bas);
            });


            //스탯의 버튼다운 세팅
            tempstat.statAtk.buttonDown.onClick.AddListener(() =>
            {
                tempplayer.atk--;
                tempstat.statAtk.SetStat(tempplayer.atk);
            });
            tempstat.statDef.buttonDown.onClick.AddListener(() =>
            {
                tempplayer.def--;
                tempstat.statDef.SetStat(tempplayer.def);
            });
            tempstat.statBas.buttonDown.onClick.AddListener(() =>
            {
                tempplayer.bas--;
                tempstat.statBas.SetStat(tempplayer.bas);
            });

            //상태이상 버튼 세팅
            tempstatus.chadan.btn.onClick.AddListener(() =>
            {
                tempplayer.debuffChadan = !(tempplayer.debuffChadan);
                tempstatus.chadan.ChangeStatus(tempplayer.debuffChadan);
                tempstat.statBas.ChangeColor(tempplayer.debuffChadan);
                if (tempplayer.debuffChadan)
                {
                    tempplayer.bas--;
                }
                else
                {
                    tempplayer.bas++;
                }
                tempstat.statBas.SetStat(tempplayer.bas);
            });
            tempstatus.byungma.btn.onClick.AddListener(() =>
            {
                tempplayer.debuffByungma = !(tempplayer.debuffByungma);
                tempstatus.byungma.ChangeStatus(tempplayer.debuffByungma);
                tempstat.statDef.ChangeColor(tempplayer.debuffByungma);
                if (tempplayer.debuffByungma)
                {
                    tempplayer.def--;
                }
                else
                {
                    tempplayer.def++;
                }
                tempstat.statDef.SetStat(tempplayer.def);
            });
            tempstatus.heoyak.btn.onClick.AddListener(() =>
            {
                tempplayer.debuffHeoyak = !(tempplayer.debuffHeoyak);
                tempstatus.heoyak.ChangeStatus(tempplayer.debuffHeoyak);
                tempstat.statAtk.ChangeColor(tempplayer.debuffHeoyak);
                if (tempplayer.debuffHeoyak)
                {
                    tempplayer.atk--;
                }
                else
                {
                    tempplayer.atk++;
                }
                tempstat.statAtk.SetStat(tempplayer.atk);
            });
            tempstatus.tajim.btn.onClick.AddListener(() =>
            {
                tempplayer.debuffTajim = !(tempplayer.debuffTajim);
                tempstatus.tajim.ChangeStatus(tempplayer.debuffTajim);
            });


        }

    }


    //다이스 사이의 중복을 체크해주는 함수
    public void CheckOverlap()
    {
        int z = 0;
        if (player1Dices.Count == 0)
        {
            return;
        }
        else if (player2Dices.Count == 0)
        {
            return;
        }

        for (int i = 0; i < player1Dices.Count; i++)
        {
            for (int k = z; k < player2Dices.Count; k++)
            {
                player1Dices[i].isOverlap = false;
                player2Dices[k].isOverlap = false;
                if (player2Dices[k].diceNum == player1Dices[i].diceNum)
                {
                    player1Dices[i].isOverlap = true;
                    player2Dices[k].isOverlap = true;
                    z = k + 1;
                    Debug.Log($"{i},{k}.{z}");
                    break;
                }
            }
        }
        foreach (Dice dice in player1Dices)
        {
            dice.ChangeColor();
        }
        foreach (Dice dice in player2Dices)
        {
            dice.ChangeColor();
        }
    }
}
