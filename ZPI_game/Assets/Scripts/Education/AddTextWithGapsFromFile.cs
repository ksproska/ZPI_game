using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
// using UnityEditor;
using UnityEngine;

public class AddTextWithGapsFromFile : MonoBehaviour
{
    [SerializeField] private string filename;
    void Start()
    {
        string dir =  Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", "PythonTexts");
        string path =  Path.Combine(dir, $"{filename}.txt");
        string readText = File.ReadAllText(path);
        var textmesh = gameObject.GetComponent<TextMeshProUGUI>();
        // EditorUtility.SetDirty(textmesh);
        textmesh.text = readText;
    }
}
