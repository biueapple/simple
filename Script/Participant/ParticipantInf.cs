using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantInf : MonoBehaviour
{
    public Text text;
    public Text actionPoint;
    private Player player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            actionPoint.text = player.actionPoint.ToString();
        }
    }

    public void SetInfo(Participant participant, Player player)     //선택한 캐릭터 정보들
    {
        if(participant == null)
        {
            text.text = "";
            return;
        }

        this.player = player;

        text.text = "이름 : " + participant.stat.GetName_() + "\n" +
            "체력 : " + (int)participant.stat.GetOriginHp() + "/" + (int)participant.hp + "\n" +
            "마나 : " + (int)participant.stat.GetOriginMp() + "/" + (int)participant.mp + "\n" +
            "공격력 : " + (int)participant.ad + "\n" +
            "주문력 : " + (int)participant.ap + "\n" +
            "방어력 : " + (int)participant.adDefence + "\n" +
            "마법방어력 : " + (int)participant.apDefence + "\n";
    }    
}
