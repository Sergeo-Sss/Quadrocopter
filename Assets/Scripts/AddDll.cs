using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;

public class AddDll : MonoBehaviour
{
    private byte[] rez;

    private void Start()
    {
        StartCoroutine(GetData());
    }

    //private void Tester()
    //{
    //    Assembly assembly = Assembly.LoadFile(@"C:\Users\chief\source\repos\TestDll\TestDll\bin\Release\TestDll.dll");
    //    var type = assembly.GetType("TestDll.TestClass");
    //    gameObject.AddComponent(type);
    //}

    private void Tester2()
    {
        Assembly assembly = Assembly.Load(rez);
        var type = assembly.GetType("TestDll.TestClass");
        gameObject.AddComponent(type);
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://my-files.su/Save/du2clt/TestDll.dll");
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            rez = results;
            Tester2();
        }
    }
}