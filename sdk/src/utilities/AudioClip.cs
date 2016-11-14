using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBM.Watson.DeveloperCloud.Utilities
{
  /// <summary>
  /// This class holds data and methods for Audio Clips.
  /// </summary>
  public class AudioClip
  {
    public delegate void PCMReaderCallback(float[] data);

    public delegate void PCMSetPositionCallback(int position);

    private event AudioClip.PCMReaderCallback m_PCMReaderCallback
    {
      //[MethodImpl(MethodImplOptions.Synchronized)]
      add
      {
        this.m_PCMReaderCallback = (AudioClip.PCMReaderCallback)Delegate.Combine(this.m_PCMReaderCallback, value);
      }
      //[MethodImpl(MethodImplOptions.Synchronized)]
      remove
      {
        this.m_PCMReaderCallback = (AudioClip.PCMReaderCallback)Delegate.Remove(this.m_PCMReaderCallback, value);
      }
    }

    private event AudioClip.PCMSetPositionCallback m_PCMSetPositionCallback
    {
      //[MethodImpl(MethodImplOptions.Synchronized)]
      add
      {
        this.m_PCMSetPositionCallback = (AudioClip.PCMSetPositionCallback)Delegate.Combine(this.m_PCMSetPositionCallback, value);
      }
      //[MethodImpl(MethodImplOptions.Synchronized)]
      remove
      {
        this.m_PCMSetPositionCallback = (AudioClip.PCMSetPositionCallback)Delegate.Remove(this.m_PCMSetPositionCallback, value);
      }
    }

    /// <summary>
    /// The audio clip's name.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The number of channels in the audio clip.
    /// </summary>
    public int channels { get; set; }
    /// <summary>
    /// The sample frequency of the clip in Hertz.
    /// </summary>
    public int frequency { get; set; }
    /// <summary>
    /// The length of the audio clip in seconds.
    /// </summary>
    public float length { get; set; }
    ///// <summary>
    ///// Corresponding to the "Load In Background" flag in the inspector, when this flag is set, the loading will happen delayed without blocking the main thread.
    ///// </summary>
    //public bool loadInBackground { get; set; }
    /// <summary>
    /// Returns the current load state of the audio data associated with an AudioClip.
    /// </summary>
    public AudioDataLoadState loadState { get; set; }
    /// <summary>
    /// The load type of the clip.
    /// </summary>
    public AudioClipLoadType loadType { get; }
    ///// <summary>
    ///// Preloads audio data of the clip when the clip asset is loaded. When this flag is off, scripts have to call AudioClip.LoadAudioData() to load the data before the clip can be played. Properties like length, channels and format are available before the audio data has been loaded.
    ///// </summary>
    //public bool preloadAudioData { get; set; }
    /// <summary>
    /// The length of the audio clip in samples.
    /// </summary>
    public int samples { get; }

    /// <summary>
    /// Fills an array with sample data from the clip.
    /// The samples are floats ranging from -1.0f to 1.0f. The sample count is determined by the length of the float array.Use the offsetSamples parameter to start the read from a specific position in the clip. If the read length from the offset is longer than the clip length, the read will wrap around and read the remaining samples from the start of the clip.
    /// Note that with compressed audio files, the sample data can only be retrieved when the Load Type is set to Decompress on Load in the audio importer.If this is not the case then the array will be returned with zeroes for all the sample values.
    /// </summary>
    /// <param name="data">Sample data.</param>
    /// <param name="offsetSamples">The offset.</param>
    public void GetData(ref float[] data, int offsetSamples)
    {
      throw new NotImplementedException("AudioClip.GetData() is not implemented!");
    }

    /// <summary>
    /// Set sample data in a clip.
    /// The samples should be floats ranging from -1.0f to 1.0f (exceeding these limits will lead to artifacts and undefined behaviour). The sample count is determined by the length of the float array.Use offsetSamples to write into a random position in the clip. If the length from the offset is longer than the clip length, the write will wrap around and write the remaining samples from the start of the clip.
    /// Note that for compressed audio, the sample data can only be set when the Load Type is set to Decompress on Load in the audio importer.
    /// </summary>
    /// <param name="data">Sample data.</param>
    /// <param name="offsetSamples">The offset.</param>
    public void SetData(ref float[] data, int offsetSamples)
    {
      throw new NotImplementedException("AudioClip.SetData() is not implemented!");
    }

    /// <summary>
    /// Creates a user AudioClip with a name and with the given length in samples, channels and frequency.
    /// Set your own audio data with SetData.Use the PCMReaderCallback and PCMSetPositionCallback delegates to get a callback whenever the clip reads data and changes the position.If stream is true, Unity will on demand read in small chunks of data.If it's false, all the samples will be read during the creation.
    /// </summary>
    /// <param name="name">Name of clip.</param>
    /// <param name="lengthSamples">Number of sample frames.</param>
    /// <param name="channels">Number of channels per frame.</param>
    /// <param name="frequency">Sample frequency of clip.</param>
    /// <param name="stream">True if clip is streamed, that is if the pcmreadercallback generates data on the fly.</param>
    /// <returns></returns>
    public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
    {
      AudioClip clip = new AudioClip();
      clip.name = name;
      clip.length = lengthSamples;
      clip.channels = channels;
      clip.frequency = frequency;
      clip.st
    }
  }

  public enum AudioDataLoadState
  {
    Unloaded,
    Loading,
    Loaded,
    Failed
  }

  public enum AudioClipLoadType
  {
    DecompressOnLoad,
    CompressedInMemory,
    Streaming
  }
}
