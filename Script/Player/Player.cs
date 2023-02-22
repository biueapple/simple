using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Character> character;


    public int actionPoint;


    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void Restart()
    {
        actionPoint = 2;
    }
}
