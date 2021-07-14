using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPanel : MonoBehaviour, IPanel
{
    [SerializeField] Text _caseNumberText;
    [SerializeField] Image _takePhoto;
    [SerializeField] RawImage _rawPhotoTaken;
    [SerializeField] InputField _photoNotes;

    private void Start()
    {
        _caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        
    }
}
