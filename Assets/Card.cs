using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Card
{
    public Sprite image;

    public String[] VerbalHint, WordHint, ObjectHint, PantomimeHint;

    public Card()
    { 
        
    }

    public string getHintForAssociation(AssociationTypes type)
    {
        switch (type)
        {
            case AssociationTypes.Word:
                if (WordHint.Length > 0)
                {
                    return Reverse(WordHint[UnityEngine.Random.Range(0, WordHint.Length)]);
                }
                return "";
            case AssociationTypes.Pantomime:
                if (PantomimeHint.Length > 0)
                {
                    return Reverse(PantomimeHint[UnityEngine.Random.Range(0, PantomimeHint.Length)]);
                }
                return "";
            case AssociationTypes.Sound:
                if (VerbalHint.Length > 0)
                {
                    return Reverse(VerbalHint[UnityEngine.Random.Range(0, VerbalHint.Length)]);
                }
                
                return "";
            case AssociationTypes.Object:
                if (ObjectHint.Length > 0)
                {
                    return Reverse(ObjectHint[UnityEngine.Random.Range(0, ObjectHint.Length)]);
                }
                return "";
            default:
                return "";
        }
    }

    public string Reverse(string text)
    {
        char[] cArray = text.ToCharArray();
        string reverse = String.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }

}
