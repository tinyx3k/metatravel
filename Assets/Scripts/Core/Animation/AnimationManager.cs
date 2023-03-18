using System;
using UnityEngine;

namespace Core.Animation
{
    [Serializable]
    public class AnimationManager
    {
        private Animator _animator { get; set; }
        private string _currentState;

        public AnimationManager(Animator animator, string state)
        {
            _animator = animator;
            _currentState = state;
        }

        public void SetState(string newState)
        {
            if (_currentState == newState) return;
            _animator.Play(newState);
            _currentState = newState;
        }
    }
}