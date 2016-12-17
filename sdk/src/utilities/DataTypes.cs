using IBM.Watson.DeveloperCloud.Utilities;

namespace IBM.Watson.DeveloperCloud.DataTypes
{
    /// <summary>
    /// This class holds a AudioClip and maximum sample level.
    /// </summary>
    public class AudioData
    {
        public string Name
        {
            get { return GetName(); }
        }
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AudioData()
        { }

        /// <exclude />
        ~AudioData()
        {
            Clip = null;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clip">The AudioClip.</param>
        /// <param name="maxLevel">The maximum sample level in the audio clip.</param>
        public AudioData(AudioClip clip, float maxLevel)
        {
            Clip = clip;
            MaxLevel = maxLevel;
        }
        /// <summary>
        /// Name of this data type.
        /// </summary>
        /// <returns>The readable name.</returns>
        public string GetName()
        {
            return "Audio";
        }

        /// <summary>
        /// The AudioClip.
        /// </summary>
        public AudioClip Clip { get; set; }
        /// <summary>
        /// The maximum level in the audio clip.
        /// </summary>
        public float MaxLevel { get; set; }
    };
}
