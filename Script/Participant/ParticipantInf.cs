using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantInf : MonoBehaviour
{
    public Text text;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(Participant participant)
    {
        if(participant == null)
        {
            text.text = "";
            return;
        }

        text.text = "이름 : " + participant.stat.GetName_() + "\n" + 
            "체력 : " + participant.stat.GetOriginHp() + "/" + participant.hp + "\n" +
            "마나 : " + participant.stat.GetOriginMp() + "/" + participant.mp + "\n" +
            "공격력 : " + participant.ad + "\n" +
            "주문력 : " + participant.ap + "\n" +
            "방어력 : " + participant.adDefence + "\n" +
            "마법방어력 : " + participant.apDefence;
    }    
}
