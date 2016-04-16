using UnityEngine;
using UnityEditor;
using System.Collections;
using Assets.Scripts;

public class DeckEditor : MonoBehaviour
{
    [MenuItem("CARDS/Create New Card Set!")]
    public static void CreateNewDeck()
    {
        DeckSettings deck = ScriptableObject.CreateInstance<DeckSettings>();
        AssetDatabase.CreateAsset(deck, "Assets/Resources/Decks/NewDeck.asset");
    }
}
