using UnityEngine;
using TMPro;

public class FilePathManager : MonoBehaviour
{
    private OszExtractor oszExtractor;
    public GameObject oszManager;

    private string previousFilePath = " ";
    public string oszFilePath;

    private void Start()
    {
        //Sets or Resets the oszFilePath to the value to previousFilePath (" ")
        previousFilePath = oszFilePath;

        oszExtractor = oszManager.GetComponent<OszExtractor>();
    }

    private void Update()
    {

        if (oszFilePath != previousFilePath)
        {
            oszExtractor.ExtractOSZFile(oszFilePath);
            previousFilePath = oszFilePath;
            
        }
    }

}
