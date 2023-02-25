using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Character character;
    public RANGETYPE rangeType; //범위타입
    public int size;    //범위
    public int[] figure;      //수치
    public float[] coefficient;
    public Sprite front;            //앞면 이미지 카드마다 다름
    protected Sprite back;             //뒷면 이미지 시작할때 player가 정해줌
    public string exp;              //카드 설명
    protected string output;           //카드설명에 스킬 계수나 다른것들 처리가 된거

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SetFront()      //카드 앞면 보여주기
    {
        transform.GetComponent<Image>().sprite = front;
    }
    public void SetBack()       //카드 뒷면 보여주기
    {
        transform.GetComponent<Image>().sprite = back;
    }

    public virtual void ImpactView(TileManager tileManager)     //카드 범위 보여주기 override 해야함 (반드시는 아님)
    {
        tileManager.SelectAttackType((int)character.transform.position.x, (int)character.transform.position.z, size, rangeType);
    }

    public virtual bool Impact(Tile tile)           //카드 효과 override 반드시 해야함
    {
        return false;
    }

    public virtual void Init(Character character, Sprite back)      //카드 주인이 누구인지와 설명을 써줌 (뒷면도 설정해주는데 플레이어마다 뒷면을 다른걸 사용할 수 있도록)
    {
        //스킬 계수와 ap인지 ad인지를 써야해서 override를 해야함 아래는 그냥 예시
        this.character = character;
        this.back = back;
        string[] str = exp.Split("|f|");
        output = "";
        for (int i = 0; i < str.Length; i++)
        {
            output += str[i];
            if (i >= figure.Length)
            {
                break;
            }
            output += figure[i] + " (+%" + coefficient[0] + "AP )";
        }
        SetBack();
    }
}
