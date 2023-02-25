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

    public void SetInfo(Participant participant, Player player)     //������ ĳ���� ������
    {
        if(participant == null)
        {
            text.text = "";
            return;
        }

        this.player = player;

        text.text = "�̸� : " + participant.stat.GetName_() + "\n" +
            "ü�� : " + (int)participant.stat.GetOriginHp() + "/" + (int)participant.hp + "\n" +
            "���� : " + (int)participant.stat.GetOriginMp() + "/" + (int)participant.mp + "\n" +
            "���ݷ� : " + (int)participant.ad + "\n" +
            "�ֹ��� : " + (int)participant.ap + "\n" +
            "���� : " + (int)participant.adDefence + "\n" +
            "�������� : " + (int)participant.apDefence + "\n";
    }    
}
