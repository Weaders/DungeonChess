using UnityEngine;

namespace Assets.Scripts.Effects {

    [RequireComponent(typeof(EffectObj))]
    public class ParticleEffectRunner : MonoBehaviour {

        public ParticleSystem[] particleSystems;

        public EffectObj effectObj;

        private void Awake() {
            effectObj.onStart.AddListener(Run);
        }

        [ContextMenu("Start")]
        public void Run() {
            foreach (var particle in particleSystems)
                particle.Play();
        }

    }
}
