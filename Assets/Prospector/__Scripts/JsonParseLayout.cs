using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//info about the mine layout
[System.Serializable]
public class JsonLayout
{
    public Vector2 multiplier;
    public List<JsonLayoutSlot> slots;
    public JsonLayoutPile drawPile, discardPile;
}

[System.Serializable]
public class JsonLayoutSlot : ISerializationCallbackReceiver
{
    public int id;
    public int x;
    public int y;
    public bool faceUp;
    public string layer;
    public string hiddenByString;

    [System.NonSerialized]
    public List<int> hiddenBy;

    public void OnAfterDeserialize()
    {
        hiddenBy = new List<int>();
        if (hiddenByString.Length == 0) return;

        string[] bits = hiddenByString.Split(',');
        for (int i=0; i<bits.Length; i++)
        {
            hiddenBy.Add(int.Parse(bits[i]));
        }
    }

    public void OnBeforeSerialize() { }
}


[System.Serializable]
public class JsonLayoutPile
{
    public int x, y;
    public string layer;
    public float xStagger; //fans cards to the side for the draw pile
}

public class JsonParseLayout : MonoBehaviour
{
    public static JsonParseLayout S { get; private set; }

    [Header("Inscribed")]
    public TextAsset jsonLayoutFile;

    [Header("Dynamic")]
    public JsonLayout layout;
    // Start is called before the first frame update
    void Awake()
    {
        layout = JsonUtility.FromJson<JsonLayout>(jsonLayoutFile.text);
        S = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
