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

    public void SetInfo(Participant participant, Player player)
    {
        if(participant == null)
        {
            text.text = "";
            return;
        }

        text.text = "이름 : " + participant.stat.GetName_() + "\n" + 
            "체력 : " + (int)participant.stat.GetOriginHp() + "/" + (int)participant.hp + "\n" +
            "마나 : " + (int)participant.stat.GetOriginMp() + "/" + (int)participant.mp + "\n" +
            "공격력 : " + (int)participant.ad + "\n" +
            "주문력 : " + (int)participant.ap + "\n" +
            "방어력 : " + (int)participant.adDefence + "\n" +
            "마법방어력 : " + (int)participant.apDefence + "\n" +
            "남은 행동력 : " + player.actionPoint;
    }    
}
