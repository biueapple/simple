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

        text.text = "�̸� : " + participant.stat.GetName_() + "\n" + 
            "ü�� : " + participant.stat.GetOriginHp() + "/" + participant.hp + "\n" +
            "���� : " + participant.stat.GetOriginMp() + "/" + participant.mp + "\n" +
            "���ݷ� : " + participant.ad + "\n" +
            "�ֹ��� : " + participant.ap + "\n" +
            "���� : " + participant.adDefence + "\n" +
            "�������� : " + participant.apDefence;
    }    
}
