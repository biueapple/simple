using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Character character;
    public RANGETYPE rangeType; //����Ÿ��
    public int size;    //����
    public int[] figure;      //��ġ
    public Sprite front;            //�ո� �̹���
    public Sprite back;             //�޸� �̹���
    public string exp;              //ī�� ����
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
