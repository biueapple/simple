using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    public Stat stat;
    public float hp;
    public float mp;
    public float ad;
    public float ap;
    public float adDefence;
    public float apDefence;

    public bool immortality;       //�����ΰ� �ƴѰ�
    public Transform modelParent;
    protected Transform model;
    public int teamNum;
    public Color color;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public virtual void CreateModel()     //0.7�� ũ�ϱ� barricade���� ���� enemy�� character�� override�ؼ� 0.5��
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube/Cube_0.7"), Vector3.zero, Quaternion.identity, modelParent).transform;
        
    }
    public void VisibleModel()          //�� ���̵���
    {
        if (model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(true);
    }
    public void InvisibleModel()        //�� �Ⱥ��̵��� ���̴��� ����ϳ�
    {
        if(model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(false);
    }
    public void DeleteModel()           //�� ���� 
    {
        if (model == null)
        {
            return;
        }
        Destroy(model.gameObject);
    }

    public virtual void Elimination()       //���� immortality�� true���� ���ֱ�
    {
        Destroy(gameObject);
    }
    
    public virtual void Destruction()       //�ı� immortality�� false�϶��� ���ֱ�
    {
        if (immortality)
            return;
        Destroy(gameObject);
    }

    public void SelectObject()          //Ŭ�������� �ƿ����� ���� ����
    {
        if(model != null)
        {
            model.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(0.8f, 0.4f, 0));
        }
    }
    public void DeselectObject()        //Ŭ�� ��� ������ �ƿ����� ���� ����
    {
        if (model != null)
        {
            model.GetComponent<Renderer>().materials[0].SetColor("_Color", Color.black);
        }
    }

    public virtual void init(int team)      //ĳ���͵� ���ݵ� �־���
    {
        hp = stat.GetOriginHp();
        mp = stat.GetOriginMp();
        ad = stat.GetOriginAd();
        ap = stat.GetOriginAp();
        adDefence = stat.GetOriginAdDefence();
        apDefence = stat.OriginApDefence();
        teamNum = team;
    }

    public void Recovery(float f)           //ȸ���Ҷ� �����Լ�
    {
        hp += f;
        if(hp > stat.GetOriginHp())
        {
            hp = stat.GetOriginHp();
        }
    }

    public void GetAdDamage(float f)            //ad����� ������ ���°�
    {
        Debug.Log($"���� �����{f * (100 / (100 + adDefence))}, �ޱ� �� ü�� {hp}, ���� �� ü�� {hp - f * (100 / (100 + adDefence))}");
        hp -= f * (100 / (100 + adDefence));
        if (hp < 0)
            Destruction();
    }
    public void GetApDamage(float f)        //ap
    {
        Debug.Log($"���� �����{f * (100 / (100 + apDefence))}, �ޱ� �� ü�� {hp}, ���� �� ü�� {hp - f * (100 / (100 + apDefence))}");
        hp -= f * (100 / (100 + apDefence));
        if (hp < 0)
            Destruction();
    }
    public void TrueDamage(float f)             //���� ������� ���� ��
    {
        Debug.Log($"���� �����{f}, �ޱ� �� ü�� {hp}, ���� �� ü�� {hp - f}");
        hp -= f;
        if (hp < 0)
            Destruction();
    }
}
