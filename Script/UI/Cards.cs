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
    private List<Card> deck = new List<Card>();              //��
    private List<Card> hand = new List<Card>();              //��
    private List<Card> Abandon = new List<Card>();           //������ ��
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

    public bool CardUse(Tile tile)              //ī�带 ������ ��������� ���°� usingCard�� ���� handlist remove�� ��
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
    public void CardDown(Card card)                 //ī�带 ������ mouseDown
    {
        card.GetComponent<Image>().raycastTarget = false;

        card.transform.SetParent(canvas.transform);
        dragCard = card;
        isDrag = true;
    }
    public bool CardUp(Image image, Player player)      //ī�带 ������ mouseUp
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
                //ī�� ��� (ī�带 ����ߴٴ°� �ƴ϶� ����� ī��� ���ߴٴ°� �׷��� handlist�� remove�� ����)
                usingCard = dragCard;
                dragCard = null;
                usingCard.gameObject.SetActive(false);
                return true;
            }
            else if(image.name.Equals("Cards"))
            {
                //ī�� �̻��
                dragCard.transform.SetParent(transform);
            }
            else if(image.name.Equals("Deck"))
            {
                //ī�� ��� (ī�带 ����ߴٴ°� �ƴ϶� ����� ī��� ���ߴٴ°� �׷��� handlist�� remove�� ����)
                usingCard = dragCard;
                dragCard = null;
                usingCard.gameObject.SetActive(false);
                return true;
            }
            else if(image.name.Equals("Abandon"))
            {
                //ī�� ������
                player.actionPoint++;
                hand.Remove(dragCard);
                ToAbandon(dragCard);
                dragCard = null;
            }
        }

        return false;
    }
    public void Cancellation()      //ī�� �Ⱦ��� ĵ�� usingCard�ٽ� ������ ���������� ���� (hand����Ʈ���� remove�� �������� parent�� �����ϱ� ��ġ�� rayoutGroup�� �˾Ƽ� ����)
    {
        if(usingCard != null)
        {
            usingCard.transform.SetParent(transform);
            usingCard.gameObject.SetActive(true);
        }
        
        usingCard = null;
        tileManager.ClearTile();
    }

    public void Draw(int count) //count��ŭ ������ ������ deck�� �ִ� ����Ʈ�� ����
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

    public void Shuffle()       //deck�� �ִ� ī��� ����
    {
        for(int i = 0; i < deck.Count; i++)
        {
            controller.ListSwap<Card>(deck, Random.Range(0, deck.Count), Random.Range(0, deck.Count));
        }
    }

    public Card CreateCardToDeck(string str)        //īƮ�� ����� ������ (str�̸��� ���� ī�带 ����)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), t_Deck);
        if(card != null)
        {
            deck.Add(card);
            card.transform.localPosition = Vector3.zero;
        }
        return card;
    }
    public Card CreateCardToDeck(Card card)     //īƮ�� ����� ������ (card�� �����ؼ� ����)
    {
        Card card_ = Instantiate(card, t_Deck);
        if (card != null)
        {
            deck.Add(card_);
            card_.transform.localPosition = Vector3.zero;
        }
        return card_;
    }
    public Card CreateCardToHand(string str)    //īƮ�� ����� ������ (str�̸��� ���� ī�带 ����)
    {
        Card card = Instantiate(Resources.Load<Card>("Card/" + str), transform);
        if (card != null)
        {
            hand.Add(card);
            card.transform.localPosition = Vector3.zero;
        }
        return card;
    }
    public Card CreateCard(string str)      //īƮ�� ����� �����з� (str�̸��� ���� ī�带 ����)
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

    private void ToHand(Card card)  //ī�带 ������ (�ٸ� ����Ʈ�� �־��ٸ� remove�ϰ� �����)
    {
        hand.Add(card);
        card.GetComponent<Image>().raycastTarget = true;
        card.transform.SetParent(transform);
        card.SetFront();
    }
    private void ToAbandon(Card card)   //ī�带 �����з� (�ٸ� ����Ʈ�� �־��ٸ� remove�ϰ� �����)
    {
        Abandon.Add(card);
        card.GetComponent<Image>().raycastTarget = false;
        card.transform.SetParent(t_Abandon);
        card.SetBack();
        card.transform.localPosition = Vector3.zero;
    }
    private void ToDeck(Card card)      //ī�带 ������ (�ٸ� ����Ʈ�� �־��ٸ� remove�ϰ� �����)
    {
        deck.Add(card);
        card.GetComponent<Image>().raycastTarget = false;
        card.transform.SetParent(t_Deck);
        card.SetBack();
        card.transform.localPosition = Vector3.zero;
    }
}
