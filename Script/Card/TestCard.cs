using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : Card        //범위 안 적군만 대상 가능
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override bool Impact(Tile tile)      //테스트 카드 범위 안에 아군만 힐 figure[0]만큼;
    {
        if(tile.GetParticipant() != null && character.teamNum == tile.GetParticipant().teamNum)
        {
            tile.GetParticipant().Recovery(figure[0] * (character.ap * coefficient[0]));
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
