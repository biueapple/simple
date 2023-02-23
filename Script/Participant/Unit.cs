using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Participant
{
    public string[] actionNames = new string[3];
    protected Player player;
    protected TileManager tileManager;
    protected GameManager gameManager;
    public int skillPoint;

    public virtual void ViewMove()
    {
        if(player.actionPoint > 0)
        {
            tileManager.SelectMoveType((int)transform.position.x, (int)transform.position.z, 1, RANGETYPE.RECT);
            gameManager.action = ACTION.MOVE;
        }
    }
    public bool Move(Tile tile)
    {
        if(tile == null)
        {
            tileManager.tiles[(int)transform.position.x, (int)transform.position.z].Pile(null);
            return true;
        }
        else if(tile.CanMove())
        {
            tileManager.tiles[(int)transform.position.x, (int)transform.position.z].Pile(null);
            tile.Pile(this);
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void ViewAttack()
    {
        if (player.actionPoint > 0)
        {
            tileManager.SelectAttackType((int)transform.position.x, (int)transform.position.z, 1, stat.GetRANGETYPE());
            gameManager.action = ACTION.ATTACK;
        }
            
    }
    public bool Attack(Tile tile)
    {
        if(tile.GetParticipant() != null)
        {
            if(!tile.Immortality())
            {
                tile.GetParticipant().GetAdDamage(ad);
                return true;
            }
        }

        return false;
    }
    public virtual void ViewSpecial()
    {
        if(player.actionPoint < skillPoint)
        {
            Debug.Log("포인트 부족");
            return;
        }
    }
    public virtual bool Special(Tile tile)
    {
        return false;
    }

    public override void Elimination()       //삭제 immortality가 true여도 없애기
    {
        if(GetComponent<Character>() != null)
        {
            if (player != null)
                if (player.character.Contains(GetComponent<Character>()))
                    player.character.Remove(GetComponent<Character>());
        }
        
        Destroy(gameObject);
    }

    public override void Destruction()
    {
        if (GetComponent<Character>() != null)
        {
            if (player != null)
                if (player.character.Contains(GetComponent<Character>()))
                    player.character.Remove(GetComponent<Character>());
        }
        if (immortality)
            return;
        Destroy(gameObject);
    }

    public override void init(int team)
    {
        base.init(team);
        tileManager = FindObjectOfType<TileManager>();
        gameManager = FindObjectOfType<GameManager>();  
        player = FindObjectOfType<Player>();
        actionNames[0] = "Move";
        actionNames[1] = "Attack";
        actionNames[2] = "Special";
    }
}
