using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Sharing;

public class WorldAnchorScript : NetworkBehaviour
{
    [Serializable]
    public struct MyStruct
    {
        public byte[] data;
        public int seqId;
    }

    public class SyncListWorldAnchor : SyncListStruct<MyStruct>
    {
        protected override MyStruct DeserializeItem(NetworkReader reader)
        {
            MyStruct ret;
            uint sz = reader.ReadPackedUInt32();
            ret.data = reader.ReadBytes((int)sz);
            ret.seqId = reader.ReadInt32();
            return ret;
        }

        protected override void SerializeItem(NetworkWriter writer, MyStruct item)
        {
            // Finally gave in and just wrote the data size into the stream (without this the
            // stream to read is somehow packed out with zeros and I couldn't figure out why).
            writer.WritePackedUInt32((uint)item.data.Length);
            writer.Write(item.data, item.data.Length);
            writer.Write(item.seqId);
        }
    }

    private byte[] _anchorData;
    public SyncListWorldAnchor _anchor = new SyncListWorldAnchor();
    bool IsSender;

    internal void UploadWorldAnchor(WorldAnchor wa)
    {
        WorldAnchorMgr.Instance.UploadWorldAnchor(wa, OnExportDataAvailable, OnExportComplete);
    }

    public void OnExportComplete(SerializationCompletionReason completionReason)
    {
        if (completionReason == SerializationCompletionReason.Succeeded)
        {
            Logger.Log("Export Complete (succeeded)");

            var data = _stream.ToArray();

#if WINDOWS_UWP
            WorldAnchorMgr.Instance.SaveData(data);
#endif
            IsSender = true;
            SendAnchor(data);
        }
        else
        {
            Logger.Log("Export Complete (failure) reason " + completionReason.ToString());
        }
    }

    MemoryStream _stream = new MemoryStream();
    int _offset;

    private void OnExportDataAvailable(byte[] data)
    {
        // buffer up the data..
        _stream.Write(data, _offset, data.Length);
        _offset += data.Length;
    }

    // Using NetworkManager.singleton.connectionConfig.PacketSize currently
    const int BLOCKSIZE = 128;

    private void SendAnchor(byte[] bytes)
    {
        int numBytes = bytes.Length;
        int seqId = 0, offset = 0;
        int maxBytes = NetworkManager.singleton.connectionConfig.PacketSize;
        var buffer = new byte[maxBytes];

        while (numBytes > maxBytes)
        {
            Buffer.BlockCopy(bytes, offset, buffer, 0, BLOCKSIZE);
            offset += BLOCKSIZE;
            numBytes -= BLOCKSIZE;
            CmdSendAnchorBlock(bytes, seqId);
            seqId++;
        }
        if (numBytes > 0)
        {
            Buffer.BlockCopy(bytes, offset, buffer, 0, numBytes);
            // Use -1 to signal the end of the data..
            CmdSendAnchorBlock(bytes, -1);
        }
    }

    [Command]
    private void CmdSendAnchorBlock(byte[] bytes, int seqId)
    {
        MyStruct data;
        data.data = bytes;
        data.seqId = seqId;
        _anchor.Add(data);
        Debug.Log("added block " + _anchor.Count + " " + netId);
    }

    private void Awake()
    {
        _anchor.Callback = AnchorChanged;
    }

    private void AnchorChanged(SyncList<MyStruct>.Operation op, int itemIndex)
    {
        Debug.Log("received block " + itemIndex.ToString() + " " + netId);

        // Reconstruct the anchor data... - detect when the whole thing has been sent..
        UpdateAnchor();
    }

    private void UpdateAnchor()
    {
        if (_anchor[_anchor.Count - 1].seqId == -1)
        {
            // If we sent the anchor, we don't need to import it as we define the coord
            // system..
            //
            if (IsSender == true)
                return;

            // put all of the array elements back into one blob..
            Debug.Log("detected last block");

            int sz = _anchor.Sum(b => b.data.Length);
            if (sz <= 0)
                return;

            // Recontruct the anchor data..
            byte[] anchorData = new byte[sz];
            int offset = 0;
            foreach (var block in _anchor)
            {
                Buffer.BlockCopy(block.data, 0, anchorData, offset, block.data.Length);
                offset += block.data.Length;
            }

            WorldAnchorMgr.Instance.ImportAsync(anchorData);
        }
    }
}
