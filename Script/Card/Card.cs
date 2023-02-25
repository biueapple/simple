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
    public float[] coefficient;
    public Sprite front;            //�ո� �̹��� ī�帶�� �ٸ�
    protected Sprite back;             //�޸� �̹��� �����Ҷ� player�� ������
    public string exp;              //ī�� ����
    protected string output;           //ī�弳�� ��ų ����� �ٸ��͵� ó���� �Ȱ�

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SetFront()      //ī�� �ո� �����ֱ�
    {
        transform.GetComponent<Image>().sprite = front;
    }
    public void SetBack()       //ī�� �޸� �����ֱ�
    {
        transform.GetComponent<Image>().sprite = back;
    }

    public virtual void ImpactView(TileManager tileManager)     //ī�� ���� �����ֱ� override �ؾ��� (�ݵ�ô� �ƴ�)
    {
        tileManager.SelectAttackType((int)character.transform.position.x, (int)character.transform.position.z, size, rangeType);
    }

    public virtual bool Impact(Tile tile)           //ī�� ȿ�� override �ݵ�� �ؾ���
    {
        return false;
    }

    public virtual void Init(Character character, Sprite back)      //ī�� ������ ���������� ������ ���� (�޸鵵 �������ִµ� �÷��̾�� �޸��� �ٸ��� ����� �� �ֵ���)
    {
        //��ų ����� ap���� ad������ ����ؼ� override�� �ؾ��� �Ʒ��� �׳� ����
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
