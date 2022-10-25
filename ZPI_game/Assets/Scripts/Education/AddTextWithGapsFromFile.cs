using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
//using UnityEditor;
using UnityEngine;

public class AddTextWithGapsFromFile : MonoBehaviour
{
    [SerializeField] private string filename;
    void Start()
    {
        string path = Directory.GetCurrentDirectory() + $"\\Assets\\PythonTexts\\{filename}.txt";
        string readText = File.ReadAllText(path);
        var textmesh = gameObject.GetComponent<TextMeshProUGUI>();
        //EditorUtility.SetDirty(textmesh);
        textmesh.text = readText;
    }
}
