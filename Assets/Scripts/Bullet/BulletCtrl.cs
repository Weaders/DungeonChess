using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Bullet {

    public interface IBulletSpawner {
        Transform GetSpawnTransform();
    }

    public interface IBulletTarget {
        Transform GetTargetTransform();
    }

    public class BulletCtrl : MonoBehaviour {

        public UnityEvent onCome = new UnityEvent();

        [SerializeField]
        private GameObject onComeExplosion;

        [SerializeField]
        private Transform currentTarget;

        [SerializeField]
        private float timeBeforeDestroy = 1f;

        public float speed = .5f;

        public Vector3 up;

        public void StartFly(IBulletSpawner bulletSpawner, IBulletTarget bulletTarget) {

            var tr = bulletSpawner.GetSpawnTransform();

            transform.position = tr.position;

            currentTarget = bulletTarget.GetTargetTransform();

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
