using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum _STATE
{
    NONE = 0,
    CHARACTER_SELECT,   //ĳ���͸� ���� ��
    POSITION_SELECT,    //ĳ���͸� �� ���� ������ ��ġ ���°�
    INGAME,             
    
}
public enum ACTION
{
    NONE = 0,
    MOVE,               //
    ATTACK,
    SPECIAL,
    CARD,
}

public class GameManager : MonoBehaviour
{
    public _STATE state;
    public ACTION action;       //unit�̳� character�������� ���� ����(�Լ��� order��ư�� AddListener)

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

    public Cards cards;
    void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        uIController = FindObjectOfType<UIController>();
        CreateAllCharacter();
        CharacterSelectStepStart();
        

    }


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f));

        switch(state)
        {
            case _STATE.CHARACTER_SELECT:
                CharacterSelectStep();
                break;
            case _STATE.POSITION_SELECT:
                SelectPositionStep();
                TileMouseOn();                  //Ÿ�� ��¦�̴� ȿ��
                break;
            case _STATE.INGAME:
                InGame();
                TileMouseOn();
                
                break;
        }
        
    }

    

    

    

    public void TrunStart()
    {
        player.Restart();
        cards.Draw(2);
    }

    public void _TrunEndButton()
    {
        for(int i = 0; i < enemys.Count; i++)
        {
            enemys[i].FindWay();
        }

        TrunStart();
    }

   

    

    

    

   

    //1

    private void CreateAllCharacter()           //start
    {
        allCharacters = Resources.LoadAll<Character>("Character");
        for(int i = 0; i < allCharacters.Length; i++)
        {
            characterList.AddCharacter(allCharacters[i]);
        }
    }

    private void CharacterSelectStepStart()     //start
    {
        //ĳ���͸���Ʈ on
        characterList.gameObject.SetActive(true);
        //����ĳ���� on
        selectingCharacters[0].transform.parent.gameObject.SetActive(true);
        //������ư on
        startButton.gameObject.SetActive(true);
        state = _STATE.CHARACTER_SELECT;
    }
    private void CharacterSelectStep()          //update (state = CHARACTER_SELECT)
    {
        CharacterSelect();
    }
    public void CharacterSelectStepEnd()       //startButton
    {
        //ĳ���͸���Ʈ off
        characterList.gameObject.SetActive(false);
        //������ư off
        startButton.gameObject.SetActive(false);
        //�⺻ 5x5Ÿ�� �����
        tileManager.CreateTile(5, 5);
    }


    public void PlayerInitStep()               //startButton
    {
        //player.Init() //ī�� ����� init()�ϴ°���
        for (int i = 0; i < selectingCharacters.Length; i++)
        {
            player.character.Add(Instantiate<Character>(selectingCharacters[i].character));
            player.character[player.character.Count - 1].init(1);
        }
        player.Init(cards);
    }


    public void SelectPositionStepStart()           //startButton
    {
        //������ ĳ���� �ڸ��� �����ϴ� ����
        posiSelect = 0;
        state = _STATE.POSITION_SELECT;
    }
    private void SelectPositionStep()           //update (state = POSITION_SELECT)
    {
        //������ ĳ���� �ڸ��� �����ϴ� ����
        PositionSelect();
    }
    private void SelectPositionStepEnd()           //PositionSelect���� ȣ��
    {
        //������ ĳ���� �ڸ��� �����ϴ� ����
        //���õ� ĳ���� off
        selectingCharacters[0].transform.parent.gameObject.SetActive(false);
        //������ ��ư on
        trunendButton.SetActive(true);
        //���� on
        cards.gameObject.SetActive(true);
        //�� on
        cards.t_Deck.gameObject.SetActive(true);
        //������ on
        cards.t_Abandon.gameObject.SetActive(true);
        //�⺻�ൿ��ư�� on
        orderButtons[0].transform.parent.gameObject.SetActive(true);
        //ĳ���� ���� on
        participantInfo.gameObject.SetActive(true);
        //Ÿ�� �߰��� ���
        tileManager.AddTile(5, 0);
        //���� ��ȯ�ϰ� �ʱ�ȭ
        CreateEnemy(3, 6);
        //���� ����
        GameStartStep();
    }


    private void GameStartStep()        //SelectPositionStepEnd
    {
        //���� ���� �̴� ����
        cards.Shuffle();
        TrunStart();        //ī�� 2��� actionpoint 2
        state = _STATE.INGAME;
    }
    private void InGame()               //update (state = INGAME)
    {
        //����
        
        switch (action)
        {
            case ACTION.MOVE:
                TileMoveLeftClick();

                break;
            case ACTION.ATTACK:
                TileAttackLeftClick();

                break;
            case ACTION.SPECIAL:
                TileSpecialLeftClick();

                break;
            case ACTION.CARD:
                CardUse();
                CardRightClick();       //������� ī�� ����ϱ�
                break;
        }

        TileLeftClick();        //Ÿ���� Ŭ���ؼ� ������ ������ �� �� ����
        TileRightClick();       //������ Ŭ������ ���õ� Ÿ���̳� �׼��� ����� �� ����
        CardLeftClick();        //ī�带 Ŭ���ؼ� ����� �� ����

        if(enemys.Count <= 0)
        {
            GameInit();
        }
    }
    private void GameInit()
    {
        //���� ��������
        //��ġ �ٲٰ�
        PositionSelect(player);
        //�ٽ� ����ȯ
        CreateEnemy(3, 6);
    }

    //2

    private void CharacterSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickImage = uIController.GetGraphicRay<CharacterImage>();
            if (clickImage != null)
            {
                isDrag = true;
                clickImage.transform.SetParent(uIController.GetCanvas().transform, false);
                clickImage.GetComponent<Image>().raycastTarget = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;

            SelectingCharacter selectingCharacter = uIController.GetGraphicRay<SelectingCharacter>();
            if (selectingCharacter != null)
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

        if (isDrag)
        {
            if (clickImage != null)
            {
                clickImage.transform.position = Input.mousePosition;
            }
        }
    }       //ĳ���͸� ���� �Լ� uicontroller�� ������ ���� //CharacterSelectStep()

    private void PositionSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                player.character[posiSelect].CreateModel();
                hit.transform.GetComponent<Tile>().Pile(player.character[posiSelect]);
                posiSelect++;

                if (player.character.Count <= posiSelect)
                {
                    //�ڸ� �� ��
                    SelectPositionStepEnd();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (posiSelect > 0)
            {
                posiSelect--;
                player.character[posiSelect].DeleteModel();
                player.character[posiSelect].Move(null);
            }
        }
    }       //ĳ���� �ڸ��� ���� �Լ�                    //SelectPositionStep()

    private void PositionSelect(Player player)
    {
        int j = 0;
        for (int i = 0; i < player.character.Count; i++)
        {
            
            if (player.character[i].Move(tileManager.tiles[(int)(player.character[i].transform.position.x / 2) + j, (int)player.character[i].transform.position.z]))
            {
                j = 0;
            }
            else
            {
                i--;
                j++;
            }
        }
    }       //ĳ������ �ڸ��� �ٽ� ���� �Լ�            //GameInit()

    private void CreateEnemy(int min, int max)
    {
        int count = Random.Range(min, max);
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(5, 10);
            int z = Random.Range(0, 5);

            Enemy enemy = tileManager.CreateEnemy(x, z, "Robot", 2);

            if (enemy != null)
            {
                enemys.Add(enemy);
            }
            else
            {
                i--;
            }
        }
    }       //���� ��ȯ�ϴ� �Լ�                //CreateEnemyStep() 


    //3

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
    }           //Ÿ������ ���콺�� ������ ��¦�̴� ȿ��
    private void TileLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                tileManager.ClearTile();
                clickParticipant = tileManager.AddTile(hit.transform.GetComponent<Tile>(), false, false);
                participantInfo.SetInfo(clickParticipant, player);

                ButtonsSetting(clickParticipant);

            }
        }
    }           //Ÿ���� ��Ŭ�������� ������Ʈ�� ����
    private void ButtonsSetting(Participant participant)
    {
        if(participant != null)
        {
            if (participant.GetComponent<Character>() != null)
            {
                Character unit = participant.GetComponent<Character>();
                if (player.character.Contains(unit))
                {
                    orderButtons[0].onClick.AddListener(unit.ViewMove);
                    orderButtons[1].onClick.AddListener(unit.ViewAttack);
                    orderButtons[2].onClick.AddListener(unit.ViewSpecial);

                    for (int i = 0; i < orderButtons.Length; i++)
                    {
                        orderButtons[i].transform.GetChild(0).GetComponent<Text>().text = unit.actionNames[i];
                        orderButtons[i].interactable = true;
                    }
                    return;
                }
            }
        }
        

        for (int i = 0; i < orderButtons.Length; i++)
        {
            orderButtons[i].onClick.RemoveAllListeners();
            orderButtons[i].transform.GetChild(0).GetComponent<Text>().text = "";
            orderButtons[i].interactable = false;
        }
    }       //TileLeftClick���� �ڽ��� ĳ���͸� Ŭ���ϸ� �⺻�ൿ�� button�� �ֱ�
    private void TileRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            tileManager.ClearTile();
            action = ACTION.NONE;
        }
    }           //����Ŭ�������� ������ Ÿ�ϵ� ����
    private void CardLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Card card = uIController.GetGraphicRay<Card>();
            if (card != null)
            {
                cards.CardDown(card);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Image image = uIController.GetGraphicRay<Image>();

            if (cards.CardUp(image, player))
            {
                action = ACTION.CARD;
                cards.usingCard.ImpactView(tileManager);
            }

        }
    }           //ī�带 down�ؼ� �̵��ϰ� up�ؼ� ���µ� ���п��� ���Ҵ��� �ƴ��� �Ǵ��ϰ� ���и� �ǵ��ư��� �ƴϸ� ī�� ���
    private void CardUse()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (cards.CardUse(hit.transform.GetComponent<Tile>()))
                    {
                        //ī�� ��� ����
                        action = ACTION.NONE;

                        return;
                    }
                }
            }
            cards.Cancellation();
            action = ACTION.NONE;
        }
    }               //ī���� ȿ���� ���� ����� �����ϰ� �ߵ�
    private void CardRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cards.Cancellation();
            action = ACTION.NONE;
        }
    }           //������� ī�� �ǵ�����
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
                        action = ACTION.NONE;
                    }
                    else //���� ����
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //ĳ���� ��ų���
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
                        action = ACTION.NONE;
                    }
                    else //���� ����
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //ĳ���� ����
    private void TileMoveLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Move(hit.transform.GetComponent<Tile>()))     //�����̴µ� ����
                    {
                        player.actionPoint--;
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                    else //����
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //������ �� ���� ĭ�� Ŭ����
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //ĳ���� �̵�
}
