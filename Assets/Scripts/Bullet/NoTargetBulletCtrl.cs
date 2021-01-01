using System;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Bullet {

    public interface IBullet { }

    public interface INoTargetBullet : IBullet {

        UnityEvent<CharacterCtrl> OnTouch { get; }

    }


    public class NoTargetBulletCtrl : MonoBehaviour, INoTargetBullet {

        [SerializeField]
        private Vector3 _direction;

        private float _time;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private bool _isStartFly = false;

        public UnityEvent<CharacterCtrl> OnTouch => throw new NotImplementedException();

        public void StartFly(IBulletSpawner spawner, Vector3 dir, float secondsLife) {

            var tr = spawner.GetSpawnTransform();

            transform.position = tr.position;

            _time = secondsLife;

            _direction = dir;

            _isStartFly = true;

            Destroy(gameObject, secondsLife);

        }

        private void Update() {

            if (_isStartFly) {
                Debug.Log("asd");
                transform.Translate(_direction * _speed * Time.deltaTime);
            }

        }

    }
}
