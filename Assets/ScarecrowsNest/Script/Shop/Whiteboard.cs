using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public TMP_Text Text;

    public void LoadTextFromItem(ShopItem si) {
        string s = si.name + "\nfor\n";
        foreach (var kvp in si.Cost) {
            if (kvp.Value > 0) {
                s += kvp.Value + " x " + kvp.Key.Name + ", ";
            }   
        }
        s.Remove(s.Length-2); // strip trailing comma
        Text.SetText(s);
    }

    public void LoadTextFromResources(Dictionary<Plant,int> resources)
    {
        string s = "You have:\n";
        foreach (var kvp in resources)
        {
            if (kvp.Value > 0)
            {
                s += kvp.Value + " x " + kvp.Key.name + "\n";
            }
            s.Remove(s.Length-1); // strip trailing newline
            Text.SetText(s);
        }
    }

    public void ClearText() {
        Text.SetText("");
    }

}
