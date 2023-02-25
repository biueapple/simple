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

    public override void CreateModel()          //0.7cube는 너무 커서 0.5로 override
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube_0.5"), Vector3.zero, Quaternion.identity, modelParent).transform;
        model.GetComponent<Renderer>().materials[1].color = color;
    }

    public override void init(int team)     //카드들 초기화
    {
        base.init(team);    
        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].Init(this, back);
        }
    }

    public void SetBackSprite(Sprite sprite)    //뒷면들도 정해주기(모든 캐릭터가 같은 뒷면을 사용함)
    {
        back = sprite;
    }
}
