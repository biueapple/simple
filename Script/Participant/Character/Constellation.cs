using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : Character
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
