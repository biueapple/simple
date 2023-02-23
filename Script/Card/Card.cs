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
    public Sprite front;            //앞면 이미지
    public Sprite back;             //뒷면 이미지
    public string exp;              //카드 설명
    public string output;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SetFront()
    {
        transform.GetComponent<Image>().sprite = front;
    }
    public void SetBack()
    {
        transform.GetComponent<Image>().sprite = back;
    }

    public virtual void ImpactView(TileManager tileManager)     
    {
        tileManager.SelectAttackType((int)character.transform.position.x, (int)character.transform.position.z, size, rangeType);
    }

    public virtual bool Impact(Tile tile)
    {
        return false;
    }

    public void Init(Character character, Sprite back)
    {
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
            output += figure[i];
        }
    }
}
