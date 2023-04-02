using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum defines a variable as well as named values
public enum eCardState {  drawpile, mine, target, discard}

public class CardProspector : Card
{

    [Header("Dynamic: CardProspector")]
    public eCardState state = eCardState.drawpile;
    public List<CardProspector> hiddenBy = new List<CardProspector>();

    public int layoutID;
    //stores info pulled from JSON_Layout
    public JsonLayoutSlot layoutSlot;

    override public void OnMouseUpAsButton(){
        //uncomment the next line to call the base class version of this method
        //base.OnMouseUpAsButton();

        Prospector.CARD_CLICKED(this);
    }
}
