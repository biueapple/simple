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

    public bool immortality;       //무적인가 아닌가
    public Transform modelParent;
    protected Transform model;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public virtual void CreateModel()     //true unit, false barricade
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube/Cube_0.7"), Vector3.zero, Quaternion.identity, modelParent).transform;
    }
    public void VisibleModel()
    {
        if (model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(true);
    }
    public void InvisibleModel()
    {
        if(model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(false);
    }
    public void DeleteModel()
    {
        if (model == null)
        {
            return;
        }
        Destroy(model.gameObject);
    }

    public virtual void Elimination()       //삭제 immortality가 true여도 없애기
    {
        Destroy(gameObject);
    }
    
    public virtual void Destruction()       //파괴 immortality가 false일때만 없애기
    {
        if (immortality)
            return;
        Destroy(gameObject);
    }

    public void SelectObject()
    {
        if(model != null)
        {
            model.GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(0.8f, 0.4f, 0));
            model.GetComponent<Renderer>().materials[0].SetFloat("_Float", 2);
        }
    }
    public void DeselectObject()
    {
        if (model != null)
        {
            model.GetComponent<Renderer>().materials[1].SetColor("_Color", Color.black);
            model.GetComponent<Renderer>().materials[0].SetFloat("_Float", 1);
        }
    }

    public virtual void init()
    {
        hp = stat.GetOriginHp();
        mp = stat.GetOriginMp();
        ad = stat.GetOriginAd();
        ap = stat.GetOriginAp();
        adDefence = stat.GetOriginAdDefence();
        apDefence = stat.OriginApDefence();
    }

    public void GetAdDamage(float f)
    {
        Debug.Log($"받은 대미지{f * (100 / (100 + adDefence))}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f * (100 / (100 + adDefence))}");
        hp -= f * (100 / (100 + adDefence));
        if (hp < 0)
            Destruction();
    }
    public void GetApDamage(float f)
    {
        Debug.Log($"받은 대미지{f * (100 / (100 + apDefence))}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f * (100 / (100 + apDefence))}");
        hp -= f * (100 / (100 + apDefence));
        if (hp < 0)
            Destruction();
    }
    public void TrueDamage(float f)
    {
        Debug.Log($"받은 대미지{f}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f}");
        hp -= f;
        if (hp < 0)
            Destruction();
    }
}
