using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public TextMeshProUGUI _titleText;
    public TextMeshProUGUI _descriptionText;
    public TextMeshProUGUI _animalsExhibit;
    public Image _animalImage;
    
    public CardModelSO[] cards;


    public void PopulateSelectedCard(int cardIndex)
    {
        _titleText.text = cards[cardIndex].titleText;
        _descriptionText.text = cards[cardIndex].descriptionText;
        _animalsExhibit.text = cards[cardIndex].animalsExhibit;
        _animalImage.sprite = cards[cardIndex].animalImage;
    }
}
