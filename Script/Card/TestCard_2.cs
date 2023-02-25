using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard_2 : Card      //��� �Ʊ����� ��� ����
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ImpactView(TileManager tileManager)        //������ ���������� �Ÿ� ������̷� �ٲ�
    {
        tileManager.SelectTeamType(character.teamNum);
    }

    public override bool Impact(Tile tile)          //���������� ȸ�� figure[0]��ŭ
    {
        if (tile.GetParticipant() != null && character.teamNum == tile.GetParticipant().teamNum)
        {
            tile.GetParticipant().Recovery(figure[0]);
            return true;
        }
        return false;
    }

    public override void Init(Character character, Sprite back)
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
            output += figure[i] + " (+ %" + coefficient[0] + "AP )";
        }
        SetBack();
    }
}
