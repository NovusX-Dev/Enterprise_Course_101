using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamePanel : MonoBehaviour, IPanel
{
    [SerializeField] Text _caseNumberText;
    [SerializeField] InputField _firstNameField, _LastNameField;
    [SerializeField] GameObject _warningText;
    [SerializeField] LocationPanel _locationPanel;

    private void Start()
    {
        _caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(_firstNameField.text) && !string.IsNullOrEmpty(_LastNameField.text))
        {
            UIManager.Instance.activeCase.name = _firstNameField.text + " " + _LastNameField.text;
            _locationPanel.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(WarningRoutine());
        }
    }

    IEnumerator WarningRoutine()
    {
        _warningText.SetActive(true);
        yield return new WaitForSeconds(3f);
        _warningText.SetActive(false);
    }
}
