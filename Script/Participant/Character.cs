using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    public List<Card> cardList = new List<Card>();
    private Sprite back;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void init(int team)
    {
        base.init(team);    
        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].Init(this, back);
        }
    }

    public void SetBackSprite(Sprite sprite)
    {
        back = sprite;
    }
}
