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
    public int teamNum;
    public Color color;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public virtual void CreateModel()     //0.7은 크니까 barricade에서 쓰고 enemy랑 character는 override해서 0.5로
    {
        model = Instantiate(Resources.Load<GameObject>("OutlineCube/Cube/Cube_0.7"), Vector3.zero, Quaternion.identity, modelParent).transform;
        
    }
    public void VisibleModel()          //모델 보이도록
    {
        if (model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(true);
    }
    public void InvisibleModel()        //모델 안보이도록 쉐이더를 써야하나
    {
        if(model == null)
        {
            CreateModel();
        }
        model.gameObject.SetActive(false);
    }
    public void DeleteModel()           //모델 삭제 
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

    public void SelectObject()          //클릭했을때 아웃라인 색깔 변경
    {
        if(model != null)
        {
            model.GetComponent<Renderer>().materials[0].SetColor("_Color", new Color(0.8f, 0.4f, 0));
        }
    }
    public void DeselectObject()        //클릭 취소 했을때 아웃라인 색깔 변경
    {
        if (model != null)
        {
            model.GetComponent<Renderer>().materials[0].SetColor("_Color", Color.black);
        }
    }

    public virtual void init(int team)      //캐릭터들 스텟들 넣어줌
    {
        hp = stat.GetOriginHp();
        mp = stat.GetOriginMp();
        ad = stat.GetOriginAd();
        ap = stat.GetOriginAp();
        adDefence = stat.GetOriginAdDefence();
        apDefence = stat.OriginApDefence();
        teamNum = team;
    }

    public void Recovery(float f)           //회복할때 쓰는함수
    {
        hp += f;
        if(hp > stat.GetOriginHp())
        {
            hp = stat.GetOriginHp();
        }
    }

    public void GetAdDamage(float f)            //ad대미지 받을때 쓰는거
    {
        Debug.Log($"받은 대미지{f * (100 / (100 + adDefence))}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f * (100 / (100 + adDefence))}");
        hp -= f * (100 / (100 + adDefence));
        if (hp < 0)
            Destruction();
    }
    public void GetApDamage(float f)        //ap
    {
        Debug.Log($"받은 대미지{f * (100 / (100 + apDefence))}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f * (100 / (100 + apDefence))}");
        hp -= f * (100 / (100 + apDefence));
        if (hp < 0)
            Destruction();
    }
    public void TrueDamage(float f)             //방어력 상관없이 전부 들어감
    {
        Debug.Log($"받은 대미지{f}, 받기 전 체력 {hp}, 받은 후 체력 {hp - f}");
        hp -= f;
        if (hp < 0)
            Destruction();
    }
}
