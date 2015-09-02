using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Schematrix
{
    #region Event Delegate Definitions

    /// <summary>
    /// Represents the method that will handle data available events
    /// </summary>
    public delegate void DataAvailableHandler(object sender, DataAvailableEventArgs e);
    /// <summary>
    /// Represents the method that will handle extraction progress events
    /// </summary>
    public delegate void ExtractionProgressHandler(object sender, ExtractionProgressEventArgs e);
    /// <summary>
    /// Represents the method that will handle missing archive volume events
    /// </summary>
    public delegate void MissingVolumeHandler(object sender, MissingVolumeEventArgs e);
    /// <summary>
    /// Represents the method that will handle new volume events
    /// </summary>
    public delegate void NewVolumeHandler(object sender, NewVolumeEventArgs e);
    /// <summary>
    /// Represents the method that will handle new file notifications
    /// </summary>
    public delegate void NewFileHandler(object sender, NewFileEventArgs e);
    /// <summary>
    /// Represents the method that will handle password required events
    /// </summary>
    public delegate void PasswordRequiredHandler(object sender, PasswordRequiredEventArgs e);

    #endregion

    #region Event Argument Classes

    public class NewVolumeEventArgs
    {
        public string VolumeName;
        public bool ContinueOperation = true;

        public NewVolumeEventArgs(string volumeName)
        {
            this.VolumeName = volumeName;
        }
    }

    public class MissingVolumeEventArgs
    {
        public string VolumeName;
        public bool ContinueOperation = false;

        public MissingVolumeEventArgs(string volumeName)
        {
            this.VolumeName = volumeName;
        }
    }

    public class DataAvailableEventArgs
    {
        public readonly byte[] Data;
        public bool ContinueOperation = true;

        public DataAvailableEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }

    public class PasswordRequiredEventArgs
    {
        public string Password = string.Empty;
        public bool ContinueOperation = true;
    }

    public class NewFileEventArgs
    {
        public RARFileInfo fileInfo;
        public NewFileEventArgs(RARFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }
    }

    public class ExtractionProgressEventArgs
    {
        public string FileName;
        public long FileSize;
        public long BytesExtracted;
        public double PercentComplete;
        public bool ContinueOperation = true;
    }

    public class RARFileInfo
    {
        public string FileName;
        public bool ContinuedFromPrevious = false;
        public bool ContinuedOnNext = false;
        public bool IsDirectory = false;
        public long PackedSize = 0;
        public long UnpackedSize = 0;
        public int HostOS = 0;
        public long FileCRC = 0;
        public DateTime FileTime;
        public int VersionToUnpack = 0;
        public int Method = 0;
        public int FileAttributes = 0;
        public long BytesExtracted = 0;

        public double PercentComplete
        {
            get
            {
                if (this.UnpackedSize != 0)
                    return (((double)this.BytesExtracted / (double)this.UnpackedSize) * (double)100.0);
                else
                    return (double)0;
            }
        }
    }

    #endregion

    public class Unrar
    {

    }

}
