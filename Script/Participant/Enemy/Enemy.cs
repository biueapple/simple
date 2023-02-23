using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CreateModel()
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube_0.5"), Vector3.zero, Quaternion.identity, modelParent).transform;
    }

    public override void Destruction()
    {
        if (immortality)
            return;
        if(gameManager.enemys.Contains(this))
        {
            gameManager.enemys.Remove(this);
        }
        Destroy(gameObject);
    }

    public void FindWay()       //가장 가까운 플레이어의 캐릭터를 향해 한칸 전진 대각선으로도 이동 가능
    {
        if(gameManager.player.character.Count > 0)
        {
            int min = tileManager.Difference((int)transform.position.x, (int)transform.position.z,
            (int)gameManager.player.character[0].transform.position.x, (int)gameManager.player.character[0].transform.position.z);
            int index = 0;

            for (int i = 1; i < gameManager.player.character.Count; i++)
            {
                if (tileManager.Difference((int)transform.position.x, (int)transform.position.z,
                        (int)gameManager.player.character[i].transform.position.x, (int)gameManager.player.character[i].transform.position.z) < min)
                {
                    min = tileManager.Difference((int)transform.position.x, (int)transform.position.z,
                        (int)gameManager.player.character[i].transform.position.x, (int)gameManager.player.character[i].transform.position.z);
                    index = i;
                }
            }

            if (tileManager.FindParticipantType(gameManager.player.character[index], (int)transform.position.x, (int)transform.position.z, stat.GetRange(), stat.GetRANGETYPE()))
            {
                Attack(tileManager.tiles[(int)gameManager.player.character[index].transform.position.x, (int)gameManager.player.character[index].transform.position.z]);
            }
            else
            {
                tileManager.AStar((int)transform.position.x, (int)transform.position.z,
                (int)gameManager.player.character[index].transform.position.x, (int)gameManager.player.character[index].transform.position.z, false);    //false면 대각선으로 이동x
                Move(tileManager.GetNextTile());
            }
        }
    }
}
