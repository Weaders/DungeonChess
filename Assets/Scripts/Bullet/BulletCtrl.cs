using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Bullet {

    public interface IBulletSpawner {
        Transform GetSpawnTransform();
    }

    public interface IBulletTarget {
        Transform GetTargetTransform();
    }

    public class BulletCtrl : MonoBehaviour, IBullet {

        public UnityEvent onCome = new UnityEvent();

        [SerializeField]
        private GameObject onComeExplosion;

        [SerializeField]
        private Transform currentTarget;

        [SerializeField]
        private float timeBeforeDestroy = 1f;

        public float speed = .5f;

        public Vector3 up;

        private bool isForByDirection = false;

        private Vector3 _direction;

        private float _time;

        public void StartFly(IBulletSpawner bulletSpawner, IBulletTarget bulletTarget) {

            var tr = bulletSpawner.GetSpawnTransform();

            transform.position = tr.position;

            currentTarget = bulletTarget.GetTargetTransform();

            isForByDirection = false;

        }

        public void StartFly(IBulletSpawner spawner, Vector3 direction, float time) {

            var tr = spawner.GetSpawnTransform();

            transform.position = tr.position;

            isForByDirection = true;

            _direction = direction;





        }

        public void StopFly() {
            currentTarget = null;
        }

        private void Update() {

            if (currentTarget != null) {

                var delta = (currentTarget.position - transform.position);

                if (delta.magnitude > 0.1f) {

                    transform.Translate(delta.normalized * speed * Time.deltaTime, Space.World);
                    transform.LookAt(currentTarget.position);

                } else {

                    onCome.Invoke();
                    currentTarget = null;

                    if (onComeExplosion != null) {

                        var obj = Instantiate(onComeExplosion, transform);
                        obj.transform.localPosition = Vector3.zero;
                        Destroy(gameObject, timeBeforeDestroy);

                    } else {
                        Destroy(gameObject);
                    }

                }

            }

        }

    }
}
