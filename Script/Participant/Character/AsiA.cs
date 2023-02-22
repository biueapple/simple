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

    public override void CreateModel()
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube_0.5"), Vector3.zero, Quaternion.identity, modelParent).transform;
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
        base.ViewSpecial();
        tileManager.SelectAttackType((int)transform.position.x, (int)transform.position.z, 2, RANGETYPE.RECT);
        gameManager.state = _STATE.SPECIAL;
    }
}
