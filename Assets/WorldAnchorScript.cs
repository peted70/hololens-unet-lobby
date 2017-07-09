using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Sharing;

public class WorldAnchorScript : NetworkBehaviour
{
    private byte[] _anchorData;

    internal void UploadWorldAnchor(WorldAnchor wa)
    {
        // Serialise the world anchor and then send the byte array 
        // to the server..
        WorldAnchorTransferBatch transferBatch = new WorldAnchorTransferBatch();
        transferBatch.AddWorldAnchor("GameRootAnchor", wa);
        WorldAnchorTransferBatch.ExportAsync(transferBatch, OnExportDataAvailable, OnExportComplete);
    }

    public void OnExportComplete(SerializationCompletionReason completionReason)
    {
        var data = _stream.ToArray();
        CmdSendWorldAnchor(data);
    }

    MemoryStream _stream = new MemoryStream();
    int _offset;

    private void OnExportDataAvailable(byte[] data)
    {
        // buffer up the data..
        _stream.Write(data, 0, data.Length);
        _offset += data.Length;
    }

    [Command]
    public void CmdSendWorldAnchor(byte[] anchorData)
    {
        _anchorData = new byte[anchorData.Length];
        Buffer.BlockCopy(anchorData, 0, _anchorData, 0, anchorData.Length * sizeof(byte));
    }
}
