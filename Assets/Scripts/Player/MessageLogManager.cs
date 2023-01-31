using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.IO;

public class MessageLogManager : NetworkBehaviour
{
    public readonly SyncList<string> messages = new SyncList<string>();

    [SerializeField]
    public TextMeshProUGUI log;

    //public int currentFirst = -1;

    // Update is called once per frame
    void Update()
    {
        //if (currentFirst == messages.Count - 1)
        //    currentFirst = messages.Count;
        log.text = "";
        for (int i = 0; i < 10; i++)
        {
            if (i >= 0 && i < messages.Count)
                log.text = log.text + "\n" + messages[i];
        }
    }

    [Command(requiresAuthority = false)]
    public void AppendMessage(string message)
    {
        messages.Add(message);
        if (messages.Count >= 11)
            messages.Remove(messages[0]);
        //RpcSetFirst();
    }

    //[ClientRpc]
    //public void RpcSetFirst()
    //{
    //    if (currentFirst == messages.Count - 2)
    //        currentFirst = messages.Count - 1;
    //}
}
