using System.IO.Compression;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class OszExtractor : MonoBehaviour
{
    public GameObject oszFileReader;
    private osuFileReader s_osuFileReader;

    public string outputDirectory = "Songs";

    // so osuFileReader it can be easier to also get the file location

    private void Start()
    {
        s_osuFileReader = oszFileReader.GetComponent<osuFileReader>();
    }

    public void ExtractOSZFile(string filePath)
    {
        string songsFolder = Path.Combine(Application.dataPath, outputDirectory);
        Directory.CreateDirectory(songsFolder);

        string outputFolder = Path.Combine(songsFolder, Path.GetFileNameWithoutExtension(filePath));
        Directory.CreateDirectory(outputFolder);


        // Open the OSZ file as a ZIP archive and extract its contents to the output directory
        using (ZipArchive archive = ZipFile.OpenRead(filePath))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string outputPath = Path.Combine(outputFolder, entry.Name);

                // Checking if file doesn't exist before extracting it
                if (!File.Exists(outputPath))
                {
                    entry.ExtractToFile(outputPath, true);
                }
            }
        }

        s_osuFileReader.CheckCircleSize(outputFolder);


    }


}
//51886/61250