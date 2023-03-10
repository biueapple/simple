using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsiA : Character
{



    void Start()
    {
        
    }


    void Update()
    {
        
    }

   
    public override bool Special(Tile tile)
    {
        if (tile.GetParticipant() != null)
        {
            if (!tile.Immortality())
            {
                tile.GetParticipant().GetAdDamage(ad + 3);
                return true;
            }
        }

        return false;
    }
    public override void ViewSpecial()
    {
        if (player.actionPoint < skillPoint)
        {
            return;
        }
        tileManager.SelectAttackType((int)transform.position.x, (int)transform.position.z, 2, RANGETYPE.RECT);
        gameManager.action = ACTION.SPECIAL;
    }
}
