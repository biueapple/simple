using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard_2 : Card      //모든 아군에게 사용 가능
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ImpactView(TileManager tileManager)
    {
        tileManager.SelectTeamType(character.teamNum);
    }

    public override bool Impact(Tile tile)
    {
        if (tile.GetParticipant() != null && character.teamNum == tile.GetParticipant().teamNum)
        {
            tile.GetParticipant().Recovery(10);
            return true;
        }
        return false;
    }
}
