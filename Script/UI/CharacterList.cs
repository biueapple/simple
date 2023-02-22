using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    public CharacterImage image;
    public Transform content;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void AddCharacter(Character character)
    {
        CharacterImage im = Instantiate(image, Vector3.zero, Quaternion.identity, content);
        im.character = character;
        im.name = character.stat.GetName_();
        im.transform.GetComponent<Image>().sprite = character.stat.GetSprite2D();
    }
}
