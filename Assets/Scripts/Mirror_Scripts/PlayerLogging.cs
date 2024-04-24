using System;
using UnityEngine;
using Mirror;
using System.IO;

public class PlayerLogging : NetworkBehaviour
{
    private string logFileName;
    private StreamWriter writer;

    private bool isLoggingInitialized = false;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (!isLoggingInitialized)
        {
            InitializeLogging();
            isLoggingInitialized = true;
        }
    }

    void InitializeLogging()
    {
        string connectionId = Guid.NewGuid().ToString();
        logFileName = "player_log_" + connectionId + ".csv";
        
        string logDirectory = Path.Combine(Application.dataPath, "mirror_logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        string path = Path.Combine(logDirectory, logFileName);
        writer = new StreamWriter(path, true);

        // Write CSV header
        writer.WriteLine("Timestamp,PlayerPosition_X,PlayerPosition_Y,PlayerPosition_Z,RoundTripDelay_ms");

        LogPlayerData();
    }

    void LogPlayerData()
    {
        InvokeRepeating(nameof(LogPlayerState), 0f, 0.5f);
    }

    void LogPlayerState()
    {
        if (transform == null)
            return;

        Vector3 playerPosition = transform.position;
        double player_rtt = NetworkTime.rtt * 1000;
        string formattedLatency = player_rtt.ToString("F2");
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Log player data to file in CSV format
        string csvLine = $"{timestamp},{playerPosition.x},{playerPosition.y},{playerPosition.z},{formattedLatency}";
        writer.WriteLine(csvLine);

        writer.Flush();
    }

    void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}
