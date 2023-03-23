using System;
using System.Collections.Generic;
using Core.Map.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Manager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds;
        [SerializeField] private InputActionReference triggerAction;
        private bool _isEnable;
        private void Awake()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
            }
        }

        private void OnEnable()
        {
            triggerAction.action.performed += _ => TriggerAudio();
            triggerAction.action.Enable();
        }

        private void OnDestroy()
        {
            triggerAction.action.Disable();
        }

        private void TriggerAudio()
        {
            if (_isEnable) PlayAll();
            else StopAll();
            _isEnable = !_isEnable;
        }

        public void Play(string soundName)
        {
            var sound = sounds.Find(sound => sound.name == soundName);
            sound.source.Play();
        }

        public void PlayAll()
        {
            foreach (var sound in sounds)
            {
                sound.source.Play();
            }
        }

        public void Stop(string soundName)
        {
            var sound = sounds.Find(sound => sound.name == soundName);
            sound.source.Stop();
        }

        public void StopAll()
        {
            foreach (var sound in sounds)
            {
                sound.source.Stop();
            }
        }
    }
}