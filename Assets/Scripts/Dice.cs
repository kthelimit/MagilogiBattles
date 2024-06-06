using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    Image DiceImage;
    [SerializeField]
    Sprite[] diceSprites;
    [SerializeField]
    public int diceNum = 1;
    Button ButtonDice;
    [SerializeField]
    public bool isOverlap = false; //중복여부를 체크해주기 위한 변수

    private void Awake()
    {
        DiceImage = this.GetComponent<Image>();
        diceSprites = Resources.LoadAll<Sprite>("Dice");
        ButtonDice = GetComponent<Button>();
        ButtonDice.onClick.AddListener((DiceButton));
    }
    //외부에서 주사위의 숫자와 이미지를 변경하기 위한 함수
    public void DiceChange(int num)
    {
        diceNum = num;
        DiceImage.sprite = diceSprites[diceNum - 1];

    }

    //주사위의 숫자를 랜덤하게 바꾸는 함수
    public void RandomDice()
    {
        diceNum = Random.Range(1, 7);
    }

    public void DiceButton()
    {
        //현재 조작할 수 없는 주사위인 경우 리턴
        if (diceNum == 9)
            return;
        //현재 조작가능한 주사위인 경우 숫자 증가
        diceNum++;
        //8이상이 되는 경우 다시 1로 복귀
        if (diceNum > 8)
        {
            diceNum = 1;
        }
        DiceChange(diceNum);
    }

    public void ChangeColor()
    {
        DiceImage.color = Color.white;
        if (isOverlap)
            DiceImage.color = Color.gray;
    }

}
