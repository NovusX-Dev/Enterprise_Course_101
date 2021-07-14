using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewPanel : MonoBehaviour, IPanel
{
    [SerializeField] Text _caseNumberText;
    [SerializeField] Text _nameText, _dateText, _locationText, _locationNotesText;
    [SerializeField] RawImage _photoTaken;
    [SerializeField] Text _photoNotesText;

    public void ProcessInfo()
    {
        
    }
}
