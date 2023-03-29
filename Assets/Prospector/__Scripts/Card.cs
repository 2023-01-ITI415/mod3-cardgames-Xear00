using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Dynamic")]
    public char suit;
    public int rank;
    public Color color = Color.black;
    public string colS = "Black";
    public GameObject back;
    public JsonCard def;

    public List<GameObject> decoGOs = new List<GameObject>();

    public List<GameObject> pipGOs = new List<GameObject>();
    // Start is called before the first frame update
    public void Init(char eSuit, int eRank, bool startFaceUp=true){

        gameObject.name = name = eSuit.ToString() + eRank;
        suit = eSuit;
        rank = eRank;

        if (suit == 'D' || suit == 'H') {
            colS = "Red";
            color = Color.red;
        }

        def = JsonParseDeck.GET_CARD_DEF(rank);
    }

    public virtual void SetLocalPos(Vector3 v){
        transform.localPosition = v;
    }
}
