using UnityEngine;
using System;

namespace NinjaFruit.UI
{
    /// <summary>
    /// Manages game settings persistence using PlayerPrefs
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        // PlayerPrefs keys
        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SOUND_FX_KEY = "SoundEffectsEnabled";
        private const string MUSIC_KEY = "MusicEnabled";
        
        // Default values
        private const float DEFAULT_MASTER_VOLUME = 0.8f;
        private const bool DEFAULT_SOUND_FX = true;
        private const bool DEFAULT_MUSIC = true;
        
        // Properties
        public float MasterVolume { get; private set; }
        public bool SoundEffectsEnabled { get; private set; }
        public bool MusicEnabled { get; private set; }
        
        // Events
        public event Action<float> OnMasterVolumeChanged;
        public event Action<bool> OnSoundEffectsToggled;
        public event Action<bool> OnMusicToggled;
        
        /// <summary>
        /// Load settings from PlayerPrefs
        /// </summary>
        public void LoadSettings()
        {
            // TODO: Implement PlayerPrefs loading with defaults
            MasterVolume = DEFAULT_MASTER_VOLUME;
            SoundEffectsEnabled = DEFAULT_SOUND_FX;
            MusicEnabled = DEFAULT_MUSIC;
        }
        
        /// <summary>
        /// Save all settings to PlayerPrefs
        /// </summary>
        public void SaveSettings()
        {
            // TODO: Implement PlayerPrefs save logic
        }
        
        /// <summary>
        /// Set master volume (0.0 - 1.0) and trigger event
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            // TODO: Implement volume setting with clamping
            MasterVolume = Mathf.Clamp01(volume);
            OnMasterVolumeChanged?.Invoke(MasterVolume);
        }
        
        /// <summary>
        /// Toggle sound effects on/off
        /// </summary>
        public void SetSoundEffects(bool enabled)
        {
            // TODO: Implement sound effects toggle
            SoundEffectsEnabled = enabled;
            OnSoundEffectsToggled?.Invoke(SoundEffectsEnabled);
        }
        
        /// <summary>
        /// Toggle music on/off
        /// </summary>
        public void SetMusic(bool enabled)
        {
            // TODO: Implement music toggle
            MusicEnabled = enabled;
            OnMusicToggled?.Invoke(MusicEnabled);
        }
    }
}
