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

    public override void CreateModel()          //0.7cube�� �ʹ� Ŀ�� 0.5�� override
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube_0.5"), Vector3.zero, Quaternion.identity, modelParent).transform;
        model.GetComponent<Renderer>().materials[1].color = color;
    }

    public override void init(int team)     //ī��� �ʱ�ȭ
    {
        base.init(team);    
        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].Init(this, back);
        }
    }

    public void SetBackSprite(Sprite sprite)    //�޸�鵵 �����ֱ�(��� ĳ���Ͱ� ���� �޸��� �����)
    {
        back = sprite;
    }
}
