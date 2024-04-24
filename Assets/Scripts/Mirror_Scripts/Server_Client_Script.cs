using UnityEngine;
using Mirror;
using kcp2k;

public class NetworkHelper : MonoBehaviour
{
    public NetworkManager networkManager;
    public bool startServerInEditor = false;
    public string serverIP = "localhost";
    public int serverPort = 7777;  // Add server port variable

    void Start()
    {
        if (networkManager == null)
        {
            Debug.LogError("Network Manager is not assigned!");
            return;
        }

        #if UNITY_EDITOR
        if (startServerInEditor)
        {
            networkManager.StartServer();
        }
        else
        {
            networkManager.StartClient();
        }
        #else
        string[] args = System.Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            if (arg == "-server")
            {
                networkManager.StartServer();
                return;
            }
            else if (arg == "-client")
            {
                networkManager.StartClient();
                return;
            }
            else if (arg == "-server_ip")
            {
                int index = System.Array.IndexOf(args, arg);
                if (index < args.Length - 1)
                {
                    serverIP = args[index + 1];
                }
                break;
            }
            else if (arg == "-server_port")  // Handle server port argument
            {
                int index = System.Array.IndexOf(args, arg);
                if (index < args.Length - 1)
                {
                    serverPort = int.Parse(args[index + 1]);
                }
                break;
            }
        }

        // Set the server IP
        networkManager.networkAddress = serverIP;

        // Set the server port if a port transport is available
        if (Transport.active is PortTransport portTransport)
        {
            portTransport.Port = (ushort)serverPort;
        }
        
        networkManager.StartClient();
        #endif
    }
}

