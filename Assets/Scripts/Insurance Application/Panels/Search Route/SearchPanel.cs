using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{
    [SerializeField] InputField _searchInputField;
    [SerializeField] SelectCasePanel _selectCasePanel;

    public void ProcessInfo()
    {
        AWSManager.Instance.GetS3List(_searchInputField.text, LoadNextPanel);
    }

    private void LoadNextPanel()
    {
        _selectCasePanel.gameObject.SetActive(true);
    }

}
