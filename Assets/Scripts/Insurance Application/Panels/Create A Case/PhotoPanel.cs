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

    private string _imagePath;

    private void Start()
    {
        _caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(_photoNotes.text))
        {
            UIManager.Instance.activeCase.photoNotes = _photoNotes.text;
        }

        if (!string.IsNullOrEmpty(_imagePath))
        {
            //convert photo taken to byte array from imagePath
            Texture2D convertedPhoto = NativeCamera.LoadImageAtPath(_imagePath, 512, false);
            byte[] photoData = convertedPhoto.EncodeToPNG();
            UIManager.Instance.activeCase.photoTaken = photoData;
        }
    }

    public void TakePictureButton()
    {
        TakePicture(512);
    }

    private void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize, false );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                _rawPhotoTaken.texture = texture;
                _rawPhotoTaken.gameObject.SetActive(true);
                _imagePath = path;
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }
}
