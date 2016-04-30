using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.UI;

public class CardPreview : MonoBehaviour
{
    public RawImage Image;
    public Text TitleLabel;
    public Text DescriptionLabel;
    public CardTokenVisualgroup CardTokens;
    public TokenComponent HealthModifier;
    public TokenComponent SanityModiifier;

    private static CardPreview instance = null;

    void Awake()
    {
        instance = this;
        SetEnabled(false);
    }

    public static void SetEnabled(bool enabled)
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(enabled);
        }
        else
        {
            Debug.LogWarning("no CardPreview in scene");
        }
    }

    public static void ApplyCard(BaseCard card)
    {
        if (instance != null)
        {
            instance.ApplyCardInstance(card);
        }
        else
        {
            Debug.LogWarning("no CardPreview in scene");
        }
        SetEnabled(true);
    }

    private void ApplyCardInstance(BaseCard card)
    {
        if (card != null)
        {
            Image.texture = card.Image;
            TitleLabel.text = card.Title;
            DescriptionLabel.text = card.Description;
            CardTokens.Card = card;
            HealthModifier.Card = card;
            SanityModiifier.Card = card;
        }
    }
}
