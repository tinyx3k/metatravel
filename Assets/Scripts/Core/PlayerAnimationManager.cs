using System;
using Core;
using UnityEngine;
using Core.Animation;

[RequireComponent(typeof(Animator), typeof(Player))]
public class PlayerAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private Player _player;
    private const string PlayerIdle = "Idle";
    private const string PlayerMove = "Movement";

    private AnimationManager _animationManager;
    private static readonly int Velocity = Animator.StringToHash( "Velocity");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _animationManager = new AnimationManager(_animator, PlayerIdle);
        _player.OnIdle += AnimateIdle;
        _player.OnMoving += AnimateMove;
    }

    private void AnimateIdle()
    {
        _animationManager.SetState(PlayerIdle);
        _animator.SetFloat(Velocity, 0);
    }

    private void AnimateMove(float velocity)
    {
        _animationManager.SetState(PlayerMove);
        _animator.SetFloat(Velocity, velocity);
    }
}
