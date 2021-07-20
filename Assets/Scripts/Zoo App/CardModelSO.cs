using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zoo Card", menuName = "New Animal Card")]
public class CardModelSO : ScriptableObject
{
    public string titleText;
    public string descriptionText;
    public string animalsExhibit;
    public Sprite animalImage;
}
