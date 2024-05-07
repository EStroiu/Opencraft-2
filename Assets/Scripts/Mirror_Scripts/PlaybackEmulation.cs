using UnityEngine;
using PolkaDOTS.Emulation;

public class EmulationStarter : MonoBehaviour
{
    void Start()
    {
        // Check if the command line arguments contain emulation flags
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-emulationType" && i < args.Length - 1 && args[i + 1] == "Playback")
            {
                // Check if the next argument is the emulation file path
                if (i + 2 < args.Length && args[i + 2] == "-emulationFile" && i + 3 < args.Length)
                {
                    string inputTraceFile = args[i + 3];
                    StartReplay(inputTraceFile);
                    return;
                }
                else
                {
                    Debug.LogError("Missing emulation file path argument.");
                    return;
                }
            }
        }
    }

    void StartReplay(string inputTraceFile)
    {
        InputRecorder inputRecorder = GetComponent<InputRecorder>();
        if (inputRecorder != null)
        {
            inputRecorder.LoadCaptureFromFile(inputTraceFile);
            inputRecorder.StartReplay();
        }
        else
        {
            Debug.LogError("InputRecorder component not found on player.");
        }
    }
}
