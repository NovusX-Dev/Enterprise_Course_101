using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCasePanel : MonoBehaviour, IPanel
{
    [SerializeField] private Text _infoText;

    private void Start()
    {
        _infoText.text = UIManager.Instance.activeCase.name + "\n" + UIManager.Instance.activeCase.date;
    }

    public void ProcessInfo()
    {
        
    }
}
