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

    public virtual void ViewMove()      //이동 범위 표시
    {
        if(player.actionPoint > 0)
        {
            tileManager.SelectMoveType((int)transform.position.x, (int)transform.position.z, 1, RANGETYPE.RECT);
            gameManager.action = ACTION.MOVE;
        }
    }
    public bool Move(Tile tile)         //이동
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
    public virtual void ViewAttack()        //공격 범위 표시
    {
        if (player.actionPoint > 0)
        {
            tileManager.SelectAttackType((int)transform.position.x, (int)transform.position.z, 1, stat.GetRANGETYPE());
            gameManager.action = ACTION.ATTACK;
        }
            
    }
    public bool Attack(Tile tile)           //공격 효과
    {
        if(tile.GetParticipant() != null)
        {
            if(!tile.Immortality() && tile.GetParticipant().teamNum != teamNum)
            {
                tile.GetParticipant().GetAdDamage(ad);
                return true;
            }
        }

        return false;
    }
    public virtual void ViewSpecial()           //스킬 범위 override 반드시 해야함 base.ViewSpecial()해도 조건 리턴이 안됌 그냥 복붙하고 내용 써야함
    {
        if(player.actionPoint < skillPoint)
        {
            return;
        }
    }
    public virtual bool Special(Tile tile)      //스킬 효과 override 반드시 해야함
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

    public override void Destruction()      //불멸(immortality)이 아니라면 삭제 보통 이걸 사용 (Enemy는 override해서 gameManager에 enemys List에 remove 해야함)
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
