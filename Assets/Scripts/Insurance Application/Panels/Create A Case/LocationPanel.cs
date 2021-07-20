using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LocationPanel : MonoBehaviour, IPanel
{
    [SerializeField] Text _caseNumberText;
    [SerializeField] RawImage _mapImage;
    [SerializeField] InputField _mapNotesField;
    [SerializeField] GameObject _warningMessage;
    
    [Header("Map Variables")]
    [SerializeField] string _apiKey;
    [SerializeField] float _xCord, _yCord;
    [SerializeField] int _zoom;
    [SerializeField] int _imageSize;

    private static string _url = "https://maps.googleapis.com/maps/api/staticmap?";

    private IEnumerator Start()
    {
        #region Get Geo Coordinates
        if (Input.location.isEnabledByUser)
        {
            Input.location.Start();

            var maxWaitTime = 20f;

            // Wait until service initializes
            while (Input.location.status == LocationServiceStatus.Initializing && maxWaitTime > 0)
            {
                yield return new WaitForSeconds(1f);
                maxWaitTime--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWaitTime < 1f)
            {
                StartCoroutine(WarningMessage("Location services failed to initialize, please try again."));
            }

            // Connection has failed
            if(Input.location.status == LocationServiceStatus.Failed)
            {
                StartCoroutine(WarningMessage("Connection failed, please check your connection"));
            }
            else
            {
                // Access granted and location value could be retrieved
                _xCord = Input.location.lastData.latitude;
                _yCord = Input.location.lastData.longitude;
            }

            Input.location.Stop();
        }
        else
        {
            StartCoroutine(WarningMessage("Please enable location services."));
            #if UNITY_EDITOR
            _xCord = -23.5266189f;
            _yCord = -46.6098616f;
            #endif
        }

        #endregion

        _caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;

        _url = _url + "center=" + _xCord + "," + _yCord + "&zoom=" + _zoom + "&size=" + _imageSize + "x" + 
               _imageSize + "&markers=color:red%7Clabel:P%7C" + _xCord + "," + _yCord + "&key=" + _apiKey;


        StartCoroutine(GetMapRoutine(_url, (UnityWebRequest req) =>
        {
            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Map error " + req.downloadHandler.text);
            }
            else
            {
                var mapTexture = DownloadHandlerTexture.GetContent(req);
                _mapImage.texture = mapTexture;
            }
        }));

    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(_mapNotesField.text))
        {
            UIManager.Instance.activeCase.locationNotes = _mapNotesField.text;
        }
    }

    IEnumerator GetMapRoutine(string url, Action<UnityWebRequest> callback)
    {
        using var map = UnityWebRequestTexture.GetTexture(url);
        yield return map.SendWebRequest();
        callback(map);
    }

    IEnumerator WarningMessage(string warning)
    {
        yield return new WaitForSeconds(0.5f);
        _warningMessage.SetActive(true);
        var message = _warningMessage.GetComponentInChildren<Text>();
        message.text = warning;
        yield return new WaitForSeconds(2f);
        _warningMessage.SetActive(false);
    }
}
