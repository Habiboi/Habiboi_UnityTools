using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public class HexToMaterial : EditorWindow
{
    private string path;
    private Shader matShader;
    private string saveFolder = "Materials";
    private Color[] colors;

    [MenuItem("Window/Habiboi Tools/Color Palette Generator")]
    public static void ShowWindow()
    {
        GetWindow<HexToMaterial>("Color Palette Generator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("RESET"))
        {
            Close();
            ShowWindow();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Select Hex File"))
        {
            path = EditorUtility.OpenFilePanel("Select hex file", "", "hex");

            if (path.Length != 0)
            {
                string[] hex = HexToString(path);
                colors = StringToColor(hex);
            }
        }

        if (colors != null && colors.Length != 0)
        {
            GUILayout.Label("Path: " + path);

            GUILayout.Space(10);

            matShader = Shader.Find("Universal Render Pipeline/Lit");
            matShader = EditorGUILayout.ObjectField(matShader, typeof(Shader), true) as Shader;
            saveFolder = EditorGUILayout.TextField("Save: ", saveFolder);
            if (GUILayout.Button("Save As Materials"))
            {
                AssetDatabase.CreateFolder("Assets", saveFolder);
                for (int i = 0; i < colors.Length; i++)
                {
                    Material mat = new Material(matShader);
                    mat.color = colors[i];
                    string matName = "Material " + i;
                    string matPath = "Assets/" + saveFolder + "/" + matName + ".mat";
                    AssetDatabase.CreateAsset(mat, matPath);
                }
            }

            GUILayout.Space(10);

            GUILayout.Label("Colors: ");

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = EditorGUILayout.ColorField(colors[i]);
            }
        }
    }

    private string[] HexToString(string fileName)
    {
        List<string> hex = new List<string>();

        FileStream fSTR = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);

        byte[] bytes = new byte[8];
        long fAddress = 0;
        int count;

        while ((count = fSTR.Read(bytes, 0, 8)) > 0)
        {
            string line = "#";
            for (int i = 0; i < 8; i++)
            {
                char ch = (i < count) ? Convert.ToChar(bytes[i]) : ' ';
                line += Char.IsControl(ch) ? "" : ch.ToString();
            }
            hex.Add(line);

            fAddress += 8;
        }

        fSTR.Close();

        string[] result = hex.ToArray();
        return result;
    }

    private Color[] StringToColor(string[] hex)
    {
        Color[] result = new Color[hex.Length];

        for (int i = 0; i < result.Length; i++)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hex[i], out color))
            {
                result[i] = color;
            }
        }

        return result;
    }
}
