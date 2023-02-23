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

    public override bool Impact(Tile tile)
    {
        if(tile.GetParticipant() != null && character.teamNum == tile.GetParticipant().teamNum)
        {
            tile.GetParticipant().Recovery(10);
            return true;
        }
        return false;
    }
}
