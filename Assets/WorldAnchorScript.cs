using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Sharing;

public class WorldAnchorScript : NetworkBehaviour
{
    public class SyncListWorldAnchor : SyncListStruct<MyStruct>
    {
        protected override MyStruct DeserializeItem(NetworkReader reader)
        {
            MyStruct ret;
            uint sz = reader.ReadPackedUInt32();
            ret.data = reader.ReadBytes((int)sz);
            return ret;
        }

        protected override void SerializeItem(NetworkWriter writer, MyStruct item)
        {
            // Finally gave in and just wrote the data size into the stream (without this the
            // stream to read is somehow packed out with zeros and I couldn't figure out why).
            writer.WritePackedUInt32((uint)item.data.Length);
            writer.Write(item.data, item.data.Length);
            writer.FinishMessage();
        }
    }

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
        SendWorldAnchor(data);
    }

    MemoryStream _stream = new MemoryStream();
    int _offset;

    private void OnExportDataAvailable(byte[] data)
    {
        // buffer up the data..
        _stream.Write(data, 0, data.Length);
        _offset += data.Length;
    }

    public void SendWorldAnchor(byte[] anchorData)
    {
        int numBytes = anchorData.Length;

        int maxBytes = NetworkManager.singleton.connectionConfig.PacketSize;
        int seqId = 0;

        var bytes = new byte[maxBytes];
        int offset = 0;
        _sendingWorldAnchor = true;

        while (numBytes > maxBytes)
        {
            Buffer.BlockCopy(anchorData, offset, bytes, 0, maxBytes);
            offset += maxBytes;
            numBytes -= maxBytes;

            CmdSendData(bytes, seqId);
            seqId++;
        }

        // Do we have any bytes left to send?
        if (numBytes > 0)
        {
            Buffer.BlockCopy(anchorData, offset, bytes, 0, numBytes);

            // Use -1 to signal the end of the data..
            CmdSendData(bytes, -1);
        }
    }

    private void Awake()
    {
        worldAnchor.Callback = WorldAnchorChanged;    
    }

    private void WorldAnchorChanged(SyncList<MyStruct>.Operation op, int itemIndex)
    {
        // If we 
        //if (_sendingWorldAnchor)
        //{
        //    _sendingWorldAnchor = false;
        //    return;
        //}

        var txt = gameObject.transform.Find("Text");
        var tm = txt.GetComponent<TextMesh>();

        int sz = 0;
        foreach (var data in worldAnchor)
        {
            sz += data.data.Length;
        }
        tm.text = sz.ToString();
    }

    [Serializable]
    public struct MyStruct
    {
        public byte[] data;
    }

    MemoryStream ms;
    public SyncListWorldAnchor worldAnchor = new SyncListWorldAnchor();
    private bool _sendingWorldAnchor;

    [Command]
    private void CmdSendData(byte[] bytes, int seqId)
    {
        Debug.Log("Transfer scipt CmdSendData");

        if (ms == null)
            ms = new MemoryStream();

        MyStruct data;
        data.data = bytes;
        worldAnchor.Add(data);
        worldAnchor.Dirty(worldAnchor.Count - 1);

        //if (seqId == -1)
        //{
        //    PrintSyncData(worldAnchor4);
        //}
    }

}
