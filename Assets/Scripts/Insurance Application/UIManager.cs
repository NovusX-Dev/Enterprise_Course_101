using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is Null");
            }

            return _instance;
        }
    }

    [SerializeField] GameObject _successMessage;
    [SerializeField] GameObject _caseNotFoundText;

    public Case activeCase;

    private void Awake()
    {
        _instance = this;
    }

    public void CreateNewCase()
    {
        activeCase = new Case();
        var iD = Random.Range(0, 1000);
        activeCase.caseID = iD.ToString("000");
    }

    public void SubmitNewCase()
    {
        //open a file/data stream to turn the case into a file

        Case awsCase = new Case();
        awsCase.caseID = activeCase.caseID;
        awsCase.name = activeCase.name;
        awsCase.date = activeCase.date;
        awsCase.location = activeCase.location;
        awsCase.locationNotes = activeCase.locationNotes;
        awsCase.photoTaken = activeCase.photoTaken;
        awsCase.photoNotes = activeCase.photoNotes;

        string filePath = Application.persistentDataPath + "/case#" + awsCase.caseID + ".dat";
        BinaryFormatter binaryF = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        binaryF.Serialize(file, awsCase);
        file.Close();

        //submit to AWS

        
        AWSManager.Instance.UploadToS3(filePath, awsCase.caseID);

    }

    public IEnumerator CaseNotFoundRoutine()
    {
        _caseNotFoundText.SetActive(true);
        yield return new WaitForSeconds(3f);
        _caseNotFoundText.SetActive(false);
    }

    public IEnumerator SuccessRoutine()
    {
        _successMessage.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
