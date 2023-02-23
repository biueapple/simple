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
    public List<Card> deck = new List<Card>();              //덱
    public List<Card> hand = new List<Card>();              //손
    public List<Card> Abandon = new List<Card>();           //버려진 패
    public Transform t_Deck;
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

    public bool CardUse(Tile tile)
    {
        if(usingCard.Impact(tile))
        {
            hand.Remove(usingCard);
            ToAbandon(usingCard);
            usingCard = null;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CardDown(Card card)
    {
        card.GetComponent<Image>().raycastTarget = false;

        card.transform.SetParent(canvas.transform);
        dragCard = card;
        isDrag = true;
    }
    public bool CardUp(Cards cards)
    {
        if(dragCard != null)
            dragCard.GetComponent<Image>().raycastTarget = true;

        isDrag = false;
        if(cards != null && dragCard != null)
        {
            dragCard.transform.SetParent(transform);
            Debug.Log("카드미사용");
        }
        else if(dragCard != null)
        {
            //카드 사용
            Debug.Log("카드사용");
            usingCard = dragCard;
            dragCard = null;
            usingCard.gameObject.SetActive(false);
            return true;
        }

        return false;
    }
    public void Cancellation()
    {
        if(usingCard != null)
        {
            usingCard.transform.SetParent(transform);
            usingCard.gameObject.SetActive(true);
        }
        
        usingCard = null;
        tileManager.ClearTile();
    }

    public void Draw(int count)
    {
        for(int i = 0; i < count; i++)
        {
            if(deck.Count <= 0)
            {
                for(int j = 0; j < Abandon.Count; j++)
                {
                    deck.Add(Abandon[j]);
                }
                Abandon.Clear();
                Shuffle();
                break;
            }
            ToHand(deck[0]);
            deck.RemoveAt(0);
        }
    }

    public void Shuffle()
    {
        for(int i = 0; i < deck.Count; i++)
        {
            controller.ListSwap<Card>(deck, Random.Range(0, deck.Count), Random.Range(0, deck.Count));
        }
    }

    public Card CreateCardToDeck(string str)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), t_Deck);
        if(card != null)
        {
            deck.Add(card);
            card.gameObject.SetActive(false);      //나중에 바꿔야할듯
        }
        return card;
    }
    public Card CreateCardToDeck(Card card)
    {
        Card card_ = Instantiate(card, t_Deck);
        if (card != null)
        {
            deck.Add(card_);
            card_.gameObject.SetActive(false);      //나중에 바꿔야할듯
        }
        return card_;
    }
    public Card CreateCardToHand(string str)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), transform);
        if (card != null)
        {
            hand.Add(card);
        }
        return card;
    }
    public Card CreateCard(string str)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str));
        card.gameObject.SetActive(false);
        if (card != null)
        {
            Abandon.Add(card);
        }
        return card;
    }

    private void ToHand(Card card)
    {
        hand.Add(card);
        card.transform.SetParent(transform);
        card.gameObject.SetActive(true);
    }
    private void ToAbandon(Card card)
    {
        Abandon.Add(card);
        card.transform.SetParent(null);
        card.gameObject.SetActive(false);
    }
}
