using UnityEngine;
using Mirror;
using System;
using System.IO;
using System.Linq;
using kcp2k;

public class ServerMetricsLogger : NetworkBehaviour
{
    private string logFileName;
    private StreamWriter writer;

    private bool isLoggingInitialized = false;

    private DateTime serverStartTime;

    public override void OnStartServer()
    {
        base.OnStartServer();

        serverStartTime = DateTime.Now;

        if (!isLoggingInitialized)
        {
            InitializeLogging();
            isLoggingInitialized = true;
        }
    }

    void InitializeLogging()
    {
        logFileName = "server_log.csv";
        string logDirectory = Path.Combine(Application.dataPath, "mirror_logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        string path = Path.Combine(logDirectory, logFileName);
        writer = new StreamWriter(path, true);

        // Write CSV header
        writer.WriteLine("Timestamp,Uptime(seconds),PlayerCount,ObjectCount");

        LogServerMetrics();
    }

    void LogServerMetrics()
    {
        InvokeRepeating(nameof(LogMetrics), 0f, 60f); // Log metrics every minute
    }

    void LogMetrics()
    {
        TimeSpan uptime = DateTime.Now - serverStartTime;
        int playerCount = NetworkServer.connections.Count;
        int objectCount = FindObjectsOfType<NetworkIdentity>().Count() - playerCount;

        // Log server metrics to file in CSV format
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string csvLine = $"{timestamp},{uptime.TotalSeconds},{playerCount},{objectCount}";
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
