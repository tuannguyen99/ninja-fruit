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
            MasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
            SoundEffectsEnabled = PlayerPrefs.GetInt(SOUND_FX_KEY, DEFAULT_SOUND_FX ? 1 : 0) == 1;
            MusicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, DEFAULT_MUSIC ? 1 : 0) == 1;
        }
        
        /// <summary>
        /// Save all settings to PlayerPrefs
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, MasterVolume);
            PlayerPrefs.SetInt(SOUND_FX_KEY, SoundEffectsEnabled ? 1 : 0);
            PlayerPrefs.SetInt(MUSIC_KEY, MusicEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Set master volume (0.0 - 1.0) and trigger event
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            OnMasterVolumeChanged?.Invoke(MasterVolume);
        }
        
        /// <summary>
        /// Toggle sound effects on/off
        /// </summary>
        public void SetSoundEffects(bool enabled)
        {
            SoundEffectsEnabled = enabled;
            OnSoundEffectsToggled?.Invoke(SoundEffectsEnabled);
        }
        
        /// <summary>
        /// Toggle music on/off
        /// </summary>
        public void SetMusic(bool enabled)
        {
            MusicEnabled = enabled;
            OnMusicToggled?.Invoke(MusicEnabled);
        }
    }
}
