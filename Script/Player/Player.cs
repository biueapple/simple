using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Character> character;
    public Sprite card_Back;

    public int actionPoint;


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Restart()
    {
        actionPoint = 2;
    }

    public void Init(Cards cards)
    {
        for(int i = 0; i < character.Count; i++)
        {
            for(int j = 0; j < character[i].cardList.Count; j++)
            {
                character[i].cardList[j] =  cards.CreateCardToDeck(character[i].cardList[j]);
                character[i].SetBackSprite(card_Back);
                character[i].cardList[j].Init(character[i], card_Back);
            }
        }
    }
}
