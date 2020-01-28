using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MetricsManager : ScriptableObject
{

    public bool saveMetrics = false;
    private List<CsvLine> csvText;

    public void Reset()
    {
        csvText = null;
    }

    public void InitCSVText()
    {
        if(csvText==null)
        {
            csvText = new List<CsvLine>();
        }
        CsvLine  firstLine = new CsvLine(ConvertToListLine("LEVEL NAME","PLAYER TIME"));
        csvText.Add(firstLine);
    }

    private string GetPath(string archiveName)
    {
        return Application.dataPath + "/" + archiveName + ".csv";
    }

    public IEnumerator CreateArchiveCSV(string archiveName)
    {
        string rootFile = GetPath(archiveName);

        if (File.Exists(rootFile))
        {
            File.Delete(rootFile);
        }

        var sr = File.CreateText(rootFile);
        string finalText = "";
        foreach(CsvLine line in csvText)
        {
            finalText += line.GetCSVLine();
        }
        sr.WriteLine(finalText);

        FileInfo fInfo = new FileInfo(rootFile);
        fInfo.IsReadOnly = true;

        sr.Close();

        yield return new WaitForSeconds(0.5f);

        Application.OpenURL(rootFile);
    }

    public void AddLine(string levelName, string playerTime)
    {
        CsvLine csvLine = new CsvLine(ConvertToListLine(levelName,playerTime));
        csvText.Add(csvLine);
    }

    public void AddInfoToLine(string levelName, string newInfo)
    {
        foreach(CsvLine line in csvText)
        {
            if(line.GetField(0)==levelName)
            {
                line.AddField(newInfo);
            }
        }
    }

    public List<string> ConvertToListLine(string field1, string field2)
    {
        List<string> toReturn = new List<string>();
        toReturn.Add(field1);
        toReturn.Add(field2);
        return toReturn;
    }

}

public class CsvLine
{

    private static char lineSeperater = '\n'; // It defines line seperate character
    private static char fieldSeperator = ','; // It defines field seperate chracter

    private List<string> fields;

    public CsvLine(List<string> newLine)
    {
        fields = newLine;
        CheckPlayerTimeFormat();
    }

    public void CheckPlayerTimeFormat()
    {
        foreach(string field in fields)
        {
            char[] c = field.ToCharArray();
            for(int i = 0; i < c.Length;i++)
            {
                if(c[i]==',')
                {
                    c[i] = '.';
                }
            }
        }
    }

    public string GetField(int index)
    {
        if(index>fields.Count)
        {
            return "";
        }
        return fields[index];
    }

    public void AddField(string field)
    {
        fields.Add(field);
    }

    public string GetCSVLine()
    {
        string toReturn = "";
        for(int i =0; i < fields.Count; i++)
        {
            if(i==fields.Count-1)
            {
                toReturn += fields[i];
            }
            else
            {
                toReturn += fields[i] + fieldSeperator;
            }
        }
        toReturn += lineSeperater;
        return toReturn;
    }
}
