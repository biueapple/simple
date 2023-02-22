using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum _STATE
{
    NONE = 0,
    MOVE,
    ATTACK,
    SPECIAL,
    SELECT,
    POSITIONSELECT,

}

public class GameManager : MonoBehaviour
{
    public _STATE state;

    TileManager tileManager;
    Ray ray;
    RaycastHit hit;
    public LayerMask tileMask;
    private Participant clickParticipant;
    public ParticipantInf participantInfo;

    public Button[] orderButtons;       //0 move, 1 attack, 2 special
    
    public Player player;

    public Character[] allCharacters;
    public CharacterList characterList;

    private UIController uIController;
    private CharacterImage clickImage;
    public SelectingCharacter[] selectingCharacters;
    public GameObject startButton;
    public GameObject trunendButton;
    public List<Enemy> enemys = new List<Enemy>();

    private bool isDrag;
    private int posiSelect = 0;

    void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        uIController = FindObjectOfType<UIController>();
        CreateAllCharacter();
    }


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));

        switch(state)
        {
            case _STATE.NONE:
                TileMouseOn();          //Ÿ�� ���� ���콺�� �÷������� ����select
                TileLeftClick();        //Ŭ���ϸ� select
                TileRightClick();       //��Ŭ���ϸ� select ������� state->none
                break;
            case _STATE.MOVE:
                TileMoveLeftClick();    //Ŭ���� ĭ�� selectĭ�̸� �ű�� ������
                TileRightClick();       //��Ŭ���ϸ� select ������� state->none
                break;
            case _STATE.ATTACK:
                TileAttackLeftClick();  //Ŭ���� ĭ�� selectĭ�̸� �ű�� ������
                TileRightClick();       //��Ŭ���ϸ� select ������� state->none
                break;
            case _STATE.SPECIAL:        //��ų
                TileSpecialLeftClick();
                TileRightClick();       //��Ŭ���ϸ� select ������� state->none
                break;
            case _STATE.SELECT:         //ĳ���� ����â uicontrol
                CharacterSelect();

                break;
            case _STATE.POSITIONSELECT: //ĳ���� �ʱ��ڸ� ��´ܰ�
                TileMouseOn();          //Ÿ�� ���� ���콺�� �÷������� ����select
                PositionSelect();       //Ŭ���Ѱ��� poil ��Ŭ���ϸ� �ϳ� ������ ���ư�

                break;
        }
        
    }

    public void PositionSelect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                player.character[posiSelect].CreateModel();
                hit.transform.GetComponent<Tile>().Pile(player.character[posiSelect]);
                posiSelect++;

                if(player.character.Count <= posiSelect)
                {
                    selectingCharacters[0].transform.parent.gameObject.SetActive(false);
                    trunendButton.SetActive(true);
                    tileManager.AddTile(5, 0);
                    state = _STATE.NONE;
                    posiSelect = 0;

                    int count = Random.Range(3, 6);
                    for(int i = 0; i < count; i++)
                    {
                        int x = Random.Range(5, 10);
                        int z = Random.Range(0, 5);

                        Enemy enemy = tileManager.CreateEnemy(x, z, "Robot");

                        if (enemy != null)
                        {
                            enemys.Add(enemy);
                            Debug.Log("��������");
                        }
                        else
                        {
                            i--;
                        }
                    }

                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if(posiSelect > 0)
            {
                posiSelect--;
                player.character[posiSelect].DeleteModel();
                player.character[posiSelect].Move(null);
            }
        }
    }

    public void CharacterSelect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clickImage = uIController.GetGraphicRay<CharacterImage>();
            if (clickImage != null)
            {
                isDrag = true;
                clickImage.transform.SetParent(uIController.GetCanvas().transform, false);
                clickImage.GetComponent<Image>().raycastTarget = false;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDrag = false;

            SelectingCharacter selectingCharacter = uIController.GetGraphicRay<SelectingCharacter>();
            if(selectingCharacter != null)
            {
                selectingCharacter.GetComponent<Image>().sprite = clickImage.GetComponent<Image>().sprite;
                selectingCharacter.character = clickImage.character;
            }

            if (clickImage != null)
            {
                clickImage.transform.SetParent(characterList.content, false);
                clickImage.transform.localPosition = Vector3.zero;
                clickImage.GetComponent<Image>().raycastTarget = true;
            }

            clickImage = null;
        }

        if(isDrag)
        {
            if(clickImage != null)
            {
                clickImage.transform.position = Input.mousePosition;
            }
        }
    }

    public void TrunStart()
    {
        player.actionPoint = 2;
    }

    public void _TrunEndButton()
    {
        for(int i = 0; i < enemys.Count; i++)
        {
            enemys[i].FindWay();
        }
    }

    public void _StartButton()
    {
        for(int i = 0; i < selectingCharacters.Length; i++)
        {
            if (selectingCharacters[i].GetComponent<Image>().sprite == null)
            {
                return;
            }
        }


        
        characterList.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        orderButtons[0].transform.parent.gameObject.SetActive(true);
        participantInfo.gameObject.SetActive(true);

        tileManager.CreateTile(5, 5);

        state = _STATE.POSITIONSELECT;

        for(int i = 0; i < selectingCharacters.Length; i++)
        {
            player.character.Add(Instantiate<Character>(selectingCharacters[i].character));
            player.character[i].init();
        }

        //tileManager.SelectRect(endx, endz, size);
        //tileManager.SelectRhombus(endx, endz, size);
        //if(tileManager.CreateParticipant(endx, endz, "Stone") != null)
        //{
        //    Debug.Log("��������");
        //}
        //else
        //{
        //    Debug.Log("��������");
        //}
        //if(tileManager.CreateCharacter(endx + 1, endz + 1, "AsiA") != null)
        //{
        //    Debug.Log("��������");
        //}
        //else
        //{
        //    Debug.Log("��������");
        //}
    }
    private void TileSpecialLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Special(hit.transform.GetComponent<Tile>()))     //���� ����
                    {
                        player.actionPoint -= clickParticipant.GetComponent<Character>().skillPoint;
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                    else //���� ����
                    {
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    state = _STATE.NONE;
                }
            }
        }
    }

    private void TileAttackLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Attack(hit.transform.GetComponent<Tile>()))     //���� ����
                    {
                        player.actionPoint--;
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                    else //���� ����
                    {
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    state = _STATE.NONE;
                }
            }
        }
    }

    private void TileMoveLeftClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if(tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if(clickParticipant.GetComponent<Character>().Move(hit.transform.GetComponent<Tile>()))     //�����̴µ� ����
                    {
                        player.actionPoint--;
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                    else //����
                    {
                        tileManager.ClearTile();
                        state = _STATE.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    state = _STATE.NONE;
                }
            }
        }
    }

    private void TileMouseOn()
    {
        if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
        {
            tileManager.OneTileOn(hit.transform.GetComponent<Tile>());
        }
        else
        {
            tileManager.OneTileDelete();
        }
        
    }

    private void TileLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                tileManager.ClearTile();
                clickParticipant = tileManager.AddTile(hit.transform.GetComponent<Tile>(), false, false);
                participantInfo.SetInfo(clickParticipant);

                ButtonsSetting(clickParticipant);

            }
        } 
    }

    private void ButtonsSetting(Participant participant)
    {
        if(participant == null)
        {
            for(int i = 0; i < orderButtons.Length; i++)
            {
                orderButtons[i].onClick.RemoveAllListeners();
                orderButtons[i].transform.GetChild(0).GetComponent<Text>().text = "";
                orderButtons[i].interactable = false;
            }
        }
        else
        {
            if(participant.GetComponent<Unit>() != null)
            {
                Unit unit = participant.GetComponent<Unit>();

                orderButtons[0].onClick.AddListener(unit.ViewMove);
                orderButtons[1].onClick.AddListener(unit.ViewAttack);
                orderButtons[2].onClick.AddListener(unit.ViewSpecial);

                for (int i = 0; i < orderButtons.Length; i++)
                {
                    orderButtons[i].transform.GetChild(0).GetComponent<Text>().text = unit.actionNames[i];
                    orderButtons[i].interactable = true;
                }
            }
            else
            {
                for (int i = 0; i < orderButtons.Length; i++)
                {
                    orderButtons[i].onClick.RemoveAllListeners();
                    orderButtons[i].transform.GetChild(0).GetComponent<Text>().text = "";
                    orderButtons[i].interactable = false;
                }
            }
        }
    }

    private void TileRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            tileManager.ClearTile();
            state = _STATE.NONE;
        }
    }


    private void CreateAllCharacter()
    {
        allCharacters = Resources.LoadAll<Character>("Character");
        for(int i = 0; i < allCharacters.Length; i++)
        {
            characterList.AddCharacter(allCharacters[i]);
        }
    }
}
