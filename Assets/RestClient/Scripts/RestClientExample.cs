using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RestClient.Core;
using RestClient.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestClientExample : MonoBehaviour
{
    [SerializeField]
    private string baseUrl = "localhost:3000/home";

    [SerializeField]
    Text Test1;

    [SerializeField]
    Text Test2;

    void Start()
    {
        // setup the request header
        RequestHeader clientSecurityHeader = new RequestHeader {
            Key = "Authorization",
            Value = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjVmZTU3ZTUwMWU1N2M0MTk5Yzc2MzI2NyIsImlhdCI6MTYwODg3ODAxOSwiZXhwIjoxNjA4OTY0NDE5fQ.donZf-mdD7OSBUuFHQswULdPJ0or2qotEV0ZGCHsf8g"
        };

        // setup the request header
        RequestHeader contentTypeHeader = new RequestHeader {
            Key = "Content-Type",
            Value = "application/json"
        };
        
        StartCoroutine(RestWebClient.Instance.HttpGet(baseUrl, (r) => OnRequestComplete(r), new List<RequestHeader> 
        {
            clientSecurityHeader,
            contentTypeHeader
        }));
    }

    void OnRequestComplete(Response response)
    {
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"Data: {response.Data}");
        

        Test1.text = response.Data;
        
        if(string.IsNullOrEmpty(response.Error) && !string.IsNullOrEmpty(response.Data))
        {
            //Debug.Log($"Error: {response.Error}");
        }
    }
}
