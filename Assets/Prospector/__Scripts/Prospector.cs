using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(JsonParseLayout))]

public class Prospector : MonoBehaviour
{
    private static Prospector S;

    [Header("Dynamic")]
    public List<CardProspector> drawPile;
    public List<CardProspector> discardPile;
    public List<CardProspector> mine;
    public CardProspector target;

    private Transform layoutAnchor;
    private Deck deck;
    private JsonLayout jsonLayout;

    // Start is called before the first frame update
    void Start()
    {
        if (S != null) Debug.LogError("Attempted to set S more than once!");
        S = this;

        jsonLayout = GetComponent<JsonParseLayout>().layout;

        deck = GetComponent<Deck>();

        deck.InitDeck();
        Deck.Shuffle(ref deck.cards);
        drawPile = ConvertCardsToCardProspectors(deck.cards);

        LayoutMine();
    }

    List<CardProspector> ConvertCardsToCardProspectors(List<Card> listCard)
    {
        List<CardProspector> listCP = new List<CardProspector>();
        CardProspector cp;
        foreach (Card card in listCard) {
            cp = card as CardProspector;
            listCP.Add(cp);
        }
        return (listCP);
    }

    CardProspector Draw()
    {
        CardProspector cp = drawPile[0];
        drawPile.RemoveAt(0);
        return (cp);
    }


    void LayoutMine()
    {
        if (layoutAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            layoutAnchor = tGO.transform;
        }

        CardProspector cp;

        foreach (JsonLayoutSlot slot in jsonLayout.slots)
        {
            cp = Draw();
            cp.faceUp = slot.faceUp;
            //make the cardprospector a child of layoutAnchor
            cp.transform.SetParent(layoutAnchor);
            //conver last char of layer string to an int
            int z = int.Parse(slot.layer[slot.layer.Length - 1].ToString());

            cp.SetLocalPos(new Vector3(
                jsonLayout.multiplier.x * slot.x,
                jsonLayout.multiplier.x * slot.y,
                -z));

            cp.layoutID = slot.id;
            cp.layoutSlot = slot;

            cp.state = eCardState.mine;

            mine.Add(cp);
        }
    }


}
