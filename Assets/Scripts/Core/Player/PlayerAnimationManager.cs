using UnityEngine;
using Core.Animation;

namespace Core.Player
{
    [RequireComponent(typeof(Animator), typeof(Player))]

    public class PlayerAnimationManager : MonoBehaviour
    {
        private Animator _animator;
        private Player _player;
        private const string PlayerIdle = "Idle";
        private const string PlayerMove = "Movement";

        private AnimationManager _animationManager;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int TurnOffset = Animator.StringToHash("TurnOffset");
        private static readonly int FrontBackDirection = Animator.StringToHash("FrontBackDirection");
        private static readonly int SideDirection = Animator.StringToHash("SideDirection");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponent<Player>();
            _animationManager = new AnimationManager(_animator, PlayerIdle);
            _player.OnIdle += AnimateIdle;
            _player.OnMoving += AnimateMove;
            _player.OnTurning += AnimateTurning;
        }

        private void AnimateIdle()
        {
            // _animationManager.SetState(PlayerIdle);
            _animator.SetFloat(Velocity, 0);
            _animator.SetInteger(FrontBackDirection, 0);
        }

        private void AnimateMove(float velocity, Vector2 direction)
        {
            // _animationManager.SetState(PlayerMove);
            _animator.SetFloat(Velocity, velocity);
            _animator.SetInteger(FrontBackDirection, (int)direction.y);
            _animator.SetInteger(SideDirection, (int)direction.x);
        }

        private void AnimateTurning(float turnOffset, int side) // -x to x (left to right)
        {
            // _animator.SetFloat(TurnOffset, turnOffset);
            _animator.SetInteger(SideDirection, side);
        }
    }
}
