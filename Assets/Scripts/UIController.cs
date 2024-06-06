using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Transform UIBattle;
  
    public void OpenBattleUI()
    {
        UIBattle.gameObject.SetActive(true);
        UIBattle.GetComponent<DiceManager>().Init();
    }
}
