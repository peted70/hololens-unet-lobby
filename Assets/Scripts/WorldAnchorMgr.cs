using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA.Sharing;

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using System;
using System.IO;
#endif

public class WorldAnchorMgr : Singleton<WorldAnchorMgr>
{
    public GameObject rootSharedObject;
    public static WorldAnchorStore WorldAnchorStore;

    private static void WorldAnchorStoreReady(WorldAnchorStore store)
    {
        WorldAnchorStore = store;
    }

    private void Start()
    {
        // Store the world anchor to the local world anchor store...
#if !DEBUG
        WorldAnchorStore.GetAsync(WorldAnchorStoreReady);
#endif
    }

    internal void UploadWorldAnchor(WorldAnchor wa, WorldAnchorTransferBatch.SerializationDataAvailableDelegate OnExportDataAvailable,
        WorldAnchorTransferBatch.SerializationCompleteDelegate OnExportComplete)
    {
        // Serialise the world anchor and then send the byte array 
        // to the server..
        WorldAnchorTransferBatch transferBatch = new WorldAnchorTransferBatch();
        transferBatch.AddWorldAnchor("GameRootAnchor", wa);
        WorldAnchorTransferBatch.ExportAsync(transferBatch, OnExportDataAvailable, OnExportComplete);
    }

    internal void ImportAsync(byte[] anchorData)
    {
        WorldAnchorTransferBatch.ImportAsync(anchorData, OnImportComplete);
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch wat)
    {
        if (completionReason == SerializationCompletionReason.Succeeded)
        {
            Logger.Log("World Anchor Import Complete (succeeded)");

            if (wat.GetAllIds().Length > 0)
            {
                string first = wat.GetAllIds()[0];
                Logger.Log("Anchor name: " + first);

                WorldAnchor anchor = wat.LockObject(first, rootSharedObject);
                rootSharedObject.transform.position += anchor.transform.position;
                
                Logger.Log("Game Object " + gameObject.name + " locked with world anchor " + anchor != null ? anchor.name : "NULL");

                if (WorldAnchorStore != null)
                    WorldAnchorStore.Save(first, anchor);
            }
        }
        else
        {
            Logger.Log("World Anchor Import Failed - reason " + completionReason);
        }
    }

#if WINDOWS_UWP
    bool _import;
    byte[] _anchorData;

    public async Task ImportWorldAnchorFromDisk()
    {
        Debug.Log("Reading World Anchor from file...");

        _anchorData = await LoadDataAsync();
        Debug.Log("Read World Anchor size - " + _anchorData.Length);

        Debug.Log("Importing World Anchor...");

        _import = true;
    }

    private void Update()
    {
        if (_import)
        {
            // Import the world anchor...
            WorldAnchorTransferBatch.ImportAsync(_anchorData, OnImportComplete);
            _import = false;
        }
    }

    const string AnchorFilename = "anchors.dat";

    public async Task SaveData(byte[] data)
    {
        StorageFolder folder = KnownFolders.CameraRoll;
        StorageFile file = await folder.CreateFileAsync(AnchorFilename, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteBytesAsync(file, data);
        Debug.Log("Saved world anchor data to camera roll - " + AnchorFilename);
    }

    static async Task<byte[]> LoadDataAsync()
    {
        StorageFolder folder = KnownFolders.CameraRoll;
        byte[] bytes = null;
        var file = await folder.GetFileAsync("anchors.dat");
        using (var stream = await file.OpenStreamForReadAsync())
        {
            bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
        }
        return bytes;
    }
#endif

}
