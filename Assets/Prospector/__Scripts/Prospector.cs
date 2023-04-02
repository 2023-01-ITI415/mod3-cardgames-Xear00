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

        //set up inital target card
        MoveToTarget(Draw());
        UpdateDrawPile();
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

            //set the sorting layer of all SpriteRenderers on the Card
            cp.SetSpriteSortingLayer(slot.layer);
            mine.Add(cp);
        }
    }

    void MoveToDiscard(CardProspector cp){
        cp.state = eCardState.discard;
        discardPile.Add(cp);
        cp.transform.SetParent(layoutAnchor);


        cp.SetLocalPos(new Vector3( 
            jsonLayout.multiplier.x * jsonLayout.discardPile.x,
            jsonLayout.multiplier.y * jsonLayout.discardPile.y,
            0));

        cp.faceUp = true;

        cp.SetSpriteSortingLayer(jsonLayout.discardPile.layer);
        cp.SetSortingOrder(-200 + (discardPile.Count * 3));

    }

    void MoveToTarget(CardProspector cp) {
        //if there is currently a target card, move it to discardPile
        if (target != null) MoveToDiscard(target);

        MoveToDiscard(cp);

        target = cp; //cp is the new target
        cp.state = eCardState.target;

        cp.SetSpriteSortingLayer("Target");
        cp.SetSortingOrder(0);
    }

    void UpdateDrawPile(){
        CardProspector cp;

        for (int i = 0; i < drawPile.Count; i++){
            cp = drawPile[i];
            cp.transform.SetParent(layoutAnchor);

            Vector3 cpPos = new Vector3();
            cpPos.x = jsonLayout.multiplier.x * jsonLayout.drawPile.x;
            //add the staggering for the drawpile
            cpPos.x += jsonLayout.drawPile.xStagger * i;
            cpPos.y = jsonLayout.multiplier.y * jsonLayout.drawPile.y;
            cpPos.z = 0.1f * i;
            cp.SetLocalPos(cpPos);

            cp.faceUp = false;

            cp.state = eCardState.drawpile;

            cp.SetSpriteSortingLayer(jsonLayout.drawPile.layer);
            cp.SetSortingOrder(-10 * i);
        }
    }

    static public void CARD_CLICKED(CardProspector cp){
        switch (cp.state){
        case eCardState.target:
            break;
        case eCardState.drawpile:

            S.MoveToTarget(S.Draw());
            S.UpdateDrawPile();
            break;
        case eCardState.mine:
            break;

        }
    }


}
