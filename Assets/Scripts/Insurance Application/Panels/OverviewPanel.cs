using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewPanel : MonoBehaviour, IPanel
{
    [SerializeField] Text _caseNumberText;
    [SerializeField] Text _nameText, _dateText, _locationText, _locationNotesText;
    [SerializeField] RawImage _photoTaken;
    [SerializeField] Text _photoNotesText;


    private void Start()
    {
        _caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        _nameText.text = "NAME: " + UIManager.Instance.activeCase.name;
        _dateText.text = "DATE: " + DateTime.Today.ToString("G");
        _locationText.text = "Location: \n" + UIManager.Instance.activeCase.location;
        _locationNotesText.text = "LOCATION NOTES: \n" + UIManager.Instance.activeCase.locationNotes;

        //rebuild photo
        Texture2D reconstructedPhoto = new Texture2D(1, 1);
        reconstructedPhoto.LoadImage(UIManager.Instance.activeCase.photoTaken);
        //Texture photo = (Texture)reconstructedPhoto;
        _photoTaken.texture = (Texture)reconstructedPhoto;

        _photoNotesText.text = "PHOTO NOTES: \n" + UIManager.Instance.activeCase.photoNotes;
    }

    public void ProcessInfo()
    {
        
    }
}
