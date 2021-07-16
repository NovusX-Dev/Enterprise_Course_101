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
        if (!string.IsNullOrEmpty(_photoNotes.text))
        {
            UIManager.Instance.activeCase.photoNotes = _photoNotes.text;
        }

        if (_rawPhotoTaken.texture != null)
        {
            UIManager.Instance.activeCase.photoTaken.texture = _rawPhotoTaken.texture;
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
                Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                _rawPhotoTaken.texture = texture;
                _rawPhotoTaken.gameObject.SetActive(true);
                //_takePhoto.gameObject.SetActive(false);
            }
        }, maxSize );

        Debug.Log( "Permission result: " + permission );
    }
}
