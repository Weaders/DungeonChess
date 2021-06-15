using UnityEngine;

namespace Assets.Scripts.Effects {

    [RequireComponent(typeof(Effect))]
    public class ParticleEffectRunner : MonoBehaviour {

        public ParticleSystem[] particleSystems;

        public Effect effectObj;

        private void Awake() {

            if (particleSystems == null || particleSystems.Length == 0)
                particleSystems = GetComponents<ParticleSystem>();

            effectObj.onStart.AddListener(Run);
        }

        [ContextMenu("Start")]
        public void Run() {
            foreach (var particle in particleSystems)
                particle.Play();
        }

    }
}
