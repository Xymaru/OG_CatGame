using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;
using System.Threading;
using System.Net;

public class Client : MonoBehaviour
{
    static bool downloading = false;
    static object downloadLock = new object();

    Thread dtask = new Thread(DownloadTask);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool dloading = false;

        lock (downloadLock)
        {
            dloading = downloading;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!dloading)
            {
                Debug.Log("Empesando la descarga");

                downloading = true;

                dtask.Start();
            }
            else
            {
                Debug.Log("Ya se esta descargando");
            }
        }
    }

    static void DownloadTask()
    {
        DownloadFile("https://speed.hetzner.de/100MB.bin", "100MB.bin");

        lock (downloadLock)
        {
            downloading = false;
        }

        Debug.Log("Se ha acabado de descargar ekisde");
    }

    public static void DownloadFile(string url, string localfile)
    {
        var webclient = new WebClient();

        webclient.DownloadFile(url, localfile);
    }

    private void OnDestroy()
    {
        if (dtask.IsAlive)
        {
            dtask.Abort();
        }
    }
}
