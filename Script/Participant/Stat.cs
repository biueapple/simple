using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unitData", menuName = "ScriptableObj/CreateUnitStat", order = int.MaxValue)]
public class Stat : ScriptableObject
{
    [SerializeField]
    private string name_;
    public string GetName_()
    {
        return name_;
    }

    [SerializeField]
    private float originHp;
    public float GetOriginHp()
    {
        return originHp;
    }

    [SerializeField]
    private float originMp;
    public float GetOriginMp()
    {
        return originMp;
    }

    [SerializeField]
    private float originAd;
    public float GetOriginAd()
    {
        return originAd;
    }

    [SerializeField]
    private float originAp;
    public float GetOriginAp()
    {
        return originAp;
    }

    [SerializeField]
    private int range;
    public int GetRange()
    {
        return range;
    }
    [SerializeField]
    private RANGETYPE rangeType;
    public RANGETYPE GetRANGETYPE()
    {
        return rangeType;
    }

    [SerializeField]
    private float originAdDefence;
    public float GetOriginAdDefence()
    {
        return originAdDefence;
    }

    [SerializeField]
    private float originApDefence;
    public float OriginApDefence()
    {
        return originApDefence;
    }

    [SerializeField]
    private Sprite[] skillSprites;
    public Sprite[] GetSprites()
    {
        return skillSprites;
    }

    [SerializeField]
    private Sprite sprite2D;
    public Sprite GetSprite2D()
    {
        return sprite2D;
    }
}


