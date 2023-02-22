using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Participant onSide;      //자신위에 무엇이 있는가

    public void SelectTile()
    {
        //GetComponent<Renderer>().materials[1].SetFloat("_Boolean", 1);
        GetComponent<Renderer>().materials[1].SetColor("_Color", new Color(0.8f,0.4f,0));
        GetComponent<Renderer>().materials[0].SetFloat("_Float", 2);
        if(onSide != null)
        {
            onSide.SelectObject();
        }
    }
    public void DeselectTile()
    {
        //GetComponent<Renderer>().materials[1].SetFloat("_Boolean", 0);
        GetComponent<Renderer>().materials[1].SetColor("_Color", Color.black);
        GetComponent<Renderer>().materials[0].SetFloat("_Float", 1);
        if (onSide != null)
        {
            onSide.DeselectObject();
        }
    }

    public void Pile(Participant participant)
    {
        onSide = participant;
        if (onSide != null)
        {
            onSide.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            onSide.modelParent.GetChild(0).transform.localPosition = Vector3.zero;
        }
            
    }

    public bool CanMove()
    {
        if (onSide != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool Immortality()
    {
        if(onSide == null)
        {
            return false;
        }
        else
        {
            return onSide.immortality;
        }
    }
    public Participant GetParticipant()
    {
        return onSide;
    }
}
