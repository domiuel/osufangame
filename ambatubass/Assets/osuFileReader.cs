using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class osuFileReader : MonoBehaviour
{
    public string outputDirectory;

    private Animator gameCanvasAnimator;
    public Canvas gameCanvas;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        gameCanvasAnimator = gameCanvas.GetComponent<Animator>();
        audioSource.clip = audioClip;
    }

    public void CheckCircleSize(string osuFolderPath)
    {
        string[] osuFiles = Directory.GetFiles(osuFolderPath, "*.osu", SearchOption.AllDirectories);

        foreach (string osuFile in osuFiles)
        {
            Dictionary<string, string> keyValuePairs = GetOsuFileKeyValuePairs(osuFile);

            if (keyValuePairs.ContainsKey("CircleSize") && keyValuePairs["CircleSize"] == "6")
            {
                ProcessOsuFileSections(osuFile, keyValuePairs);
            }
            else
            {
                Debug.Log("Circle size is not 6 for " + osuFile);
            }
        }
    }

    public Dictionary<string, string> GetOsuFileKeyValuePairs(string filePath)
    {
        string osuFileContent = File.ReadAllText(filePath);
        string[] sections = osuFileContent.Split(new string[] { "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        foreach (string section in sections)
        {
            string[] lines = section.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            // Get the section header (e.g. [General], [Metadata], [Difficulty], etc.)
            string sectionHeader = lines[0];

            // Process each key-value pair in the section
            for (int i = 1; i < lines.Length; i++)
            {
                string[] pair = lines[i].Split(':');
                if (pair.Length > 1)
                {
                    string key = pair[0];
                    string value = pair[1];
                    keyValuePairs[key] = value;
                }
            }
        }

        return keyValuePairs;
    }

    public void ProcessOsuFileSections(string filePath, Dictionary<string, string> keyValuePairs)
    {
        string dictionariesPath = Path.Combine(Application.dataPath, "Dictionaries");
        Directory.CreateDirectory(dictionariesPath);

        string song_dictionaryPath = Path.Combine(dictionariesPath, Path.GetFileNameWithoutExtension(filePath));
        Directory.CreateDirectory(song_dictionaryPath);

        outputDirectory = song_dictionaryPath;

        // Do something with the section (e.g. save to file)
        SaveSectionToFile("Difficulty", keyValuePairs);

        Debug.Log("Processed " + filePath);

        gameCanvasAnimator.SetTrigger("success");
        PlayAudio();
    }

    void SaveSectionToFile(string sectionHeader, Dictionary<string, string> keyValuePairs)
    {
        // Create a filename based on the section header
        string filename = Path.Combine(Application.dataPath, outputDirectory, sectionHeader + ".txt");

        // Write the key-value pairs to the file
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (KeyValuePair<string, string> pair in keyValuePairs)
            {
                writer.WriteLine(pair.Key + ": " + pair.Value);
            }
        }

        Debug.Log("Saved section " + sectionHeader + " to file " + filename);
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}

//28086/67740