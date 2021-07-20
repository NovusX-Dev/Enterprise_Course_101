using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if(_instance == null )
            {
                Debug.LogError("AWS Manager is Null");
            }
            return _instance;
        }
    }

    private AmazonS3Client _S3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if(_S3Client == null)
            {
                _S3Client = new AmazonS3Client(new CognitoAWSCredentials(
                 "us-east-2:32690323-db25-474c-87de-92cee4c0071f",
                 RegionEndpoint.USEast2
                 ), _S3Region);
            }

            return _S3Client;
        }
    }

    public string S3Region = RegionEndpoint.USEast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

    }

    public void UploadToS3(string path, string caseID)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest objectRequest = new PostObjectRequest()
        { 
            Bucket = "gdvq.enterprise.cases",
            Key = "case#" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        S3Client.PostObjectAsync(objectRequest, (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                UIManager.Instance.StartCoroutine(UIManager.Instance.SuccessRoutine());
            }
            else
            {
                Debug.LogWarning("Error occured during case upload: " + responseObject.Exception);
            }
        });

    }

    public void GetS3List(string caseNumber, Action onComplete = null)
    {
        string target = "case#" + caseNumber;
        
        var request = new ListObjectsRequest()
        {
            BucketName = "gdvq.enterprise.cases"
        };

        S3Client.ListObjectsAsync(request, (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                bool caseFound = responseObject.Response.S3Objects.Any(obj => obj.Key ==  target);
                if(caseFound)
                {
                    //download case
                    DownloadCase(onComplete, target);
                }
                else
                {
                    UIManager.Instance.StartCoroutine(UIManager.Instance.CaseNotFoundRoutine());
                }
            }
            else
            {
                Debug.Log("Listing Object errored from S3: " + responseObject.Exception);
            }
        });
    }

    private void DownloadCase(Action onComplete, string target)
    {
        S3Client.GetObjectAsync("gdvq.enterprise.cases", target, (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                //read data and apply it to case

                //check respose stream
                if (responseObject.Response.ResponseStream != null)
                {
                    //create a byte array
                    byte[] data = null;

                    //use steamreader to read data
                    using (StreamReader reader = new StreamReader(responseObject.Response.ResponseStream))
                    {
                        //access memory stream
                        using (MemoryStream memory = new MemoryStream())
                        {
                            var buffer = new byte[512];
                            var bytesRead = default(int);

                            while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                memory.Write(buffer, 0, bytesRead);
                            }

                            //populate data
                            data = memory.ToArray();
                        }
                    }

                    //convert byte data to a case object
                    using (MemoryStream memory = new MemoryStream(data))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        Case downloadedCase = (Case)bf.Deserialize(memory);
                        UIManager.Instance.activeCase = downloadedCase;

                        if (onComplete != null) onComplete();
                    }
                }
            }
        });
    }

}
