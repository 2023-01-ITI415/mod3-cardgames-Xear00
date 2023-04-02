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

        //build the cards from sprites
        AddDecorators();
        AddPips();
        AddFace();
        AddBack();
        faceUp = startFaceUp;
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

    private void AddPips(){
        int pipNum = 0;
        foreach (JsonPip pip in def.pips){
            //Instantiate a GameObject from deck
            _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB,transform);
            _tGO.transform.localPosition = pip.loc;
            //flip it if necessary
            if (pip.flip) _tGO.transform.rotation = _flipRot;
            //set the scale if necessary
            if (pip.scale != 1 ){
            _tGO.transform.localScale = Vector3.one * pip.scale;
                 }
        }
        _tGO.name = "pip_"+pipNum++;
        _tSRend = _tGO.GetComponent<SpriteRenderer>();
        _tSRend.sprite = CardSpritesSO.SUITS[suit];
        _tSRend.sortingOrder = 1;
        pipGOs.Add(_tGO);
    }

    private void AddFace(){
        if (def.face == "")
            return;
    
        string faceName = def.face + suit;
        _tSprite = CardSpritesSO.GET_FACE(faceName);
        if (_tSprite == null){
            Debug.LogError("Face sprite" + faceName + " not found.");
            return;
        }

    _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB,transform);
    _tSRend = _tGO.GetComponent<SpriteRenderer>();
    _tSRend.sprite = _tSprite;
    _tSRend.sortingOrder = 1;
    _tGO.transform.localPosition = Vector3.zero;
    _tGO.name = faceName;
    }

    public bool faceUp
    {
        get { return (!back.activeSelf); }
        set { back.SetActive(!value); }
    }

    private void AddBack()
    {
        _tGO = Instantiate<GameObject>(Deck.SPRITE_PREFAB, transform);
        _tSRend = _tGO.GetComponent<SpriteRenderer>();
        _tSRend.sprite = CardSpritesSO.BACK;
        _tGO.transform.localPosition = Vector3.zero;
        _tSRend.sortingOrder = 2;
        _tGO.name = "back";
        back = _tGO;
    }

    private SpriteRenderer[] spriteRenderers;

    void PopulateSpriteRenderers(){
        if (spriteRenderers != null) return;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }


    public void SetSpriteSortingLayer(string layerName){
        PopulateSpriteRenderers();

        foreach (SpriteRenderer srend in spriteRenderers){
            srend.sortingLayerName = layerName;
        }
    }

    public void SetSortingOrder(int s0rd){
        PopulateSpriteRenderers();

        foreach (SpriteRenderer srend in spriteRenderers){
            if (srend.gameObject == this.gameObject){
                srend.sortingOrder = s0rd;
            } else if (srend.gameObject.name == "back"){
                //if it in the back, set it to the highest layer
                srend.sortingOrder = s0rd + 2;
            } else{
                srend.sortingOrder = s0rd + 1;
            }
        }
    }
    // virtual methods can be overridden by subclasses with same name
    virtual public void OnMouseUpAsButton(){
        print(name);
    }

    

}
