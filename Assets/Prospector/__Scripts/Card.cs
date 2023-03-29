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

        AddDecorators();
    }

    public virtual void SetLocalPos(Vector3 v){
        transform.localPosition = v;
    }

    //private variables that will be reused
    private Sprite _tSprite = null;
    private GameObject _tGO = null;
    private SpriteRenderer _tSRend = null;

    //An Euler rotation of 180 degrees around the Z-axis will flip the sprites upside down
    private Quaternion _flipRot = Quaternion.Euler(0, 0, 180);


    private void AddDecorators(){
        foreach (JsonPip pip in JsonParseDeck.DECORATORS){
            if (pip.type == "suit"){
                _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB,transform);
                //get the SpriteRenderer component
                _tSRend = _tGO.GetComponent<SpriteRenderer>();
                //get the suit Sprite from the CardSpritesSO.SUIT static field
                _tSRend.sprite = CardSpritesSO.SUITS[suit];
            } else {
                _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
                _tSRend = _tGO.GetComponent<SpriteRenderer>();
                _tSRend.sprite = CardSpritesSO.RANKS[rank];
                _tSRend.color = color;
            }

        _tSRend.sortingOrder = 1;
        _tGO.transform.localPosition = pip.loc;
        //flip the decorator if needed
        if (pip.flip) _tGO.transform.rotation = _flipRot;
        //set the scale to keep decorators from being too bug
        if (pip.scale != 1 ){
            _tGO.transform.localScale = Vector3.one * pip.scale;

        }

        _tGO.name = pip.type;

        decoGOs.Add(_tGO);

        }
    }




}
