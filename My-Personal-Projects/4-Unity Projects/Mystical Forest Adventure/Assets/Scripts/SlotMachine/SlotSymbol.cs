using UnityEngine;

public enum SymbolType 
{ 
    Low,
    High,
    Wild,
    Scatter,
    Bonus
}

[System.Serializable]
public class SlotSymbol
{
    public string name;
    public SymbolType type;
    public Sprite sprite;
    public float[] payouts;

    public SlotSymbol(string name, SymbolType type, Sprite sprite, float[] payouts)
    {
        this.name = name;
        this.type = type;
        this.sprite = sprite;
        this.payouts = payouts;
    }
}
