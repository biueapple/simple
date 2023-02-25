using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum _STATE
{
    NONE = 0,
    CHARACTER_SELECT,   //캐릭터를 고르는 곳
    POSITION_SELECT,    //캐릭터를 다 고르고 시작할 위치 고르는곳
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
    public ACTION action;       //unit이나 character상위에서 직접 변경(함수를 order버튼에 AddListener)

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
                TileMouseOn();                  //타일 반짝이는 효과
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
        //캐릭터리스트 on
        characterList.gameObject.SetActive(true);
        //선택캐릭터 on
        selectingCharacters[0].transform.parent.gameObject.SetActive(true);
        //다음버튼 on
        startButton.gameObject.SetActive(true);
        state = _STATE.CHARACTER_SELECT;
    }
    private void CharacterSelectStep()          //update (state = CHARACTER_SELECT)
    {
        CharacterSelect();
    }
    public void CharacterSelectStepEnd()       //startButton
    {
        //캐릭터리스트 off
        characterList.gameObject.SetActive(false);
        //다음버튼 off
        startButton.gameObject.SetActive(false);
        //기본 5x5타일 만들기
        tileManager.CreateTile(5, 5);
    }


    public void PlayerInitStep()               //startButton
    {
        //player.Init() //카드 만들고 init()하는과정
        for (int i = 0; i < selectingCharacters.Length; i++)
        {
            player.character.Add(Instantiate<Character>(selectingCharacters[i].character));
            player.character[player.character.Count - 1].init(1);
        }
        player.Init(cards);
    }


    public void SelectPositionStepStart()           //startButton
    {
        //선택한 캐릭터 자리를 선택하는 과정
        posiSelect = 0;
        state = _STATE.POSITION_SELECT;
    }
    private void SelectPositionStep()           //update (state = POSITION_SELECT)
    {
        //선택한 캐릭터 자리를 선택하는 과정
        PositionSelect();
    }
    private void SelectPositionStepEnd()           //PositionSelect에서 호출
    {
        //선택한 캐릭터 자리를 선택하는 과정
        //선택된 캐릭터 off
        selectingCharacters[0].transform.parent.gameObject.SetActive(false);
        //턴종료 버튼 on
        trunendButton.SetActive(true);
        //손패 on
        cards.gameObject.SetActive(true);
        //덱 on
        cards.t_Deck.gameObject.SetActive(true);
        //버린패 on
        cards.t_Abandon.gameObject.SetActive(true);
        //기본행동버튼들 on
        orderButtons[0].transform.parent.gameObject.SetActive(true);
        //캐릭터 정보 on
        participantInfo.gameObject.SetActive(true);
        //타일 추가로 깔기
        tileManager.AddTile(5, 0);
        //적을 소환하고 초기화
        CreateEnemy(3, 6);
        //게임 시작
        GameStartStep();
    }


    private void GameStartStep()        //SelectPositionStepEnd
    {
        //덱을 섞고 뽑는 과정
        cards.Shuffle();
        TrunStart();        //카드 2장과 actionpoint 2
        state = _STATE.INGAME;
    }
    private void InGame()               //update (state = INGAME)
    {
        //게임
        
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
                CardRightClick();       //사용중인 카드 취소하기
                break;
        }

        TileLeftClick();        //타일을 클릭해서 유닛의 정보를 볼 수 있음
        TileRightClick();       //오른쪽 클릭으로 선택된 타일이나 액션을 취소할 수 있음
        CardLeftClick();        //카드를 클릭해서 사용할 수 있음

        if(enemys.Count <= 0)
        {
            GameInit();
        }
    }
    private void GameInit()
    {
        //다음 스테이지
        //위치 바꾸고
        PositionSelect(player);
        //다시 적소환
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
    }       //캐릭터를 고르는 함수 uicontroller의 도움을 받음 //CharacterSelectStep()

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
                    //자리 다 고름
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
    }       //캐릭터 자리를 고르는 함수                    //SelectPositionStep()

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
    }       //캐릭터의 자리를 다시 고르는 함수            //GameInit()

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
    }       //적을 소환하는 함수                //CreateEnemyStep() 


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
    }           //타일위에 마우스를 놓으면 반짝이는 효과
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
    }           //타일을 왼클릭했을때 오브젝트의 정보
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
    }       //TileLeftClick에서 자신의 캐릭터를 클릭하면 기본행동들 button에 넣기
    private void TileRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            tileManager.ClearTile();
            action = ACTION.NONE;
        }
    }           //오른클릭해을때 선택한 타일들 없앰
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
    }           //카드를 down해서 이동하고 up해서 놓는데 손패에서 놓았는지 아닌지 판단하고 손패면 되돌아가기 아니면 카드 사용
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
                        //카드 사용 성공
                        action = ACTION.NONE;

                        return;
                    }
                }
            }
            cards.Cancellation();
            action = ACTION.NONE;
        }
    }               //카드의 효과를 받을 대상을 선택하고 발동
    private void CardRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cards.Cancellation();
            action = ACTION.NONE;
        }
    }           //사용중인 카드 되돌리기
    private void TileSpecialLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Special(hit.transform.GetComponent<Tile>()))     //공격 성공
                    {
                        player.actionPoint -= clickParticipant.GetComponent<Character>().skillPoint;
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                    else //공격 실패
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //공격할 수 없는 칸을 클릭함
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //캐릭터 스킬사용
    private void TileAttackLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Attack(hit.transform.GetComponent<Tile>()))     //공격 성공
                    {
                        player.actionPoint--;
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                    else //공격 실패
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //공격할 수 없는 칸을 클릭함
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //캐릭터 공격
    private void TileMoveLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, float.MaxValue, tileMask))
            {
                if (tileManager.selectList.Contains(hit.transform.GetComponent<Tile>()))
                {
                    if (clickParticipant.GetComponent<Character>().Move(hit.transform.GetComponent<Tile>()))     //움직이는데 성공
                    {
                        player.actionPoint--;
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                    else //실패
                    {
                        tileManager.ClearTile();
                        action = ACTION.NONE;
                    }
                }
                else  //움직일 수 없는 칸을 클릭함
                {
                    tileManager.ClearTile();
                    action = ACTION.NONE;
                }
            }
        }
    }       //캐릭터 이동
}
