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

    public void Restart()                   //턴 시작할때 쓸거
    {
        actionPoint = 2;
    }

    public void Init(Cards cards)           //무슨 캐릭터들을 가지고 있는지 그리고 그 캐릭터가 가진 카드들도 만들어서 덱에 넣어주기
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
