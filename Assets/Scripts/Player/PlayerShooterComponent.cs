using BulletSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Player
{
        public class PlayerShooterComponent : ShooterComponent
        {
            private InputHandler _inputHandler;

            [Inject]
            public void Construct(InputHandler inputHandler)
            {
                _inputHandler = inputHandler;
            }
            

            private void OnEnable()
            {
                _inputHandler.OnFirePressed += ShootBullet;
            }

            private void OnDisable()
            {
                _inputHandler.OnFirePressed -= ShootBullet;
            }
        }
}