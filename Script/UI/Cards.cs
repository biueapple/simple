using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    public Canvas canvas;
    private GameManager gameManager;
    private TileManager tileManager;
    public UIController controller;
    private List<Card> deck = new List<Card>();              //덱
    private List<Card> hand = new List<Card>();              //손
    private List<Card> Abandon = new List<Card>();           //버려진 패
    public Transform t_Deck;
    public Transform t_Abandon;
    public Card dragCard;
    public Card usingCard;
    public bool isDrag;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        tileManager = FindObjectOfType<TileManager>();
        controller = FindObjectOfType<UIController>();
    }

    void Start()
    {
        
    }
    private void Update()
    {
        if(isDrag && dragCard != null)
        {
            dragCard.transform.position = Input.mousePosition;
        }
    }

    public bool CardUse(Tile tile)              //카드를 정말로 사용했을때 쓰는거 usingCard도 비우고 handlist remove도 함
    {
        if(usingCard.Impact(tile))
        {
            hand.Remove(usingCard);
            ToAbandon(usingCard);
            usingCard.gameObject.SetActive(true);
            usingCard = null;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CardDown(Card card)                 //카드를 눌렀음 mouseDown
    {
        card.GetComponent<Image>().raycastTarget = false;

        card.transform.SetParent(canvas.transform);
        dragCard = card;
        isDrag = true;
    }
    public bool CardUp(Image image, Player player)      //카드를 놓았음 mouseUp
    {
        if(dragCard == null)
        {
            return false;
        }
        else
        {
            dragCard.GetComponent<Image>().raycastTarget = true;
            isDrag = false;

            if (image == null)
            {
                //카드 사용 (카드를 사용했다는게 아니라 사용할 카드로 정했다는거 그래서 handlist에 remove는 안함)
                usingCard = dragCard;
                dragCard = null;
                usingCard.gameObject.SetActive(false);
                return true;
            }
            else if(image.name.Equals("Cards"))
            {
                //카드 미사용
                dragCard.transform.SetParent(transform);
            }
            else if(image.name.Equals("Deck"))
            {
                //카드 사용 (카드를 사용했다는게 아니라 사용할 카드로 정했다는거 그래서 handlist에 remove는 안함)
                usingCard = dragCard;
                dragCard = null;
                usingCard.gameObject.SetActive(false);
                return true;
            }
            else if(image.name.Equals("Abandon"))
            {
                //카드 버리기
                player.actionPoint++;
                hand.Remove(dragCard);
                ToAbandon(dragCard);
                dragCard = null;
            }
        }

        return false;
    }
    public void Cancellation()      //카드 안쓰고 캔슬 usingCard다시 손으로 돌려보내고 비우기 (hand리스트에서 remove는 안했으니 parent만 변경하기 위치는 rayoutGroup이 알아서 해줌)
    {
        if(usingCard != null)
        {
            usingCard.transform.SetParent(transform);
            usingCard.gameObject.SetActive(true);
        }
        
        usingCard = null;
        tileManager.ClearTile();
    }

    public void Draw(int count) //count만큼 덱에서 손으로 deck에 있던 리스트도 삭제
    {
        for(int i = 0; i < count; i++)
        {
            if(deck.Count <= 0)
            {
                for(int j = 0; j < Abandon.Count; j++)
                {
                    ToDeck(Abandon[j]);
                }
                Abandon.Clear();
                Shuffle();
            }
            ToHand(deck[0]);
            deck.RemoveAt(0);
        }
    }

    public void Shuffle()       //deck에 있는 카드들 섞기
    {
        for(int i = 0; i < deck.Count; i++)
        {
            controller.ListSwap<Card>(deck, Random.Range(0, deck.Count), Random.Range(0, deck.Count));
        }
    }

    public Card CreateCardToDeck(string str)        //카트를 만들고 덱으로 (str이름과 같은 카드를 만듬)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), t_Deck);
        if(card != null)
        {
            deck.Add(card);
            card.transform.localPosition = Vector3.zero;
        }
        return card;
    }
    public Card CreateCardToDeck(Card card)     //카트를 만들고 덱으로 (card를 복사해서 만듬)
    {
        Card card_ = Instantiate(card, t_Deck);
        if (card != null)
        {
            deck.Add(card_);
            card_.transform.localPosition = Vector3.zero;
        }
        return card_;
    }
    public Card CreateCardToHand(string str)    //카트를 만들고 손으로 (str이름과 같은 카드를 만듬)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), transform);
        if (card != null)
        {
            hand.Add(card);
            card.transform.localPosition = Vector3.zero;
        }
        return card;
    }
    public Card CreateCard(string str)      //카트를 만들고 버린패로 (str이름과 같은 카드를 만듬)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), t_Abandon);
        card.gameObject.SetActive(false);
        if (card != null)
        {
            Abandon.Add(card);
            card.transform.localPosition = Vector3.zero;
        }
        return card;
    }

    private void ToHand(Card card)  //카드를 손으로 (다른 리스트에 있었다면 remove하고 써야함)
    {
        hand.Add(card);
        card.GetComponent<Image>().raycastTarget = true;
        card.transform.SetParent(transform);
        card.SetFront();
    }
    private void ToAbandon(Card card)   //카드를 버린패로 (다른 리스트에 있었다면 remove하고 써야함)
    {
        Abandon.Add(card);
        card.GetComponent<Image>().raycastTarget = false;
        card.transform.SetParent(t_Abandon);
        card.SetBack();
        card.transform.localPosition = Vector3.zero;
    }
    private void ToDeck(Card card)      //카드를 덱으로 (다른 리스트에 있었다면 remove하고 써야함)
    {
        deck.Add(card);
        card.GetComponent<Image>().raycastTarget = false;
        card.transform.SetParent(t_Deck);
        card.SetBack();
        card.transform.localPosition = Vector3.zero;
    }
}
