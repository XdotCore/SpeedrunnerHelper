using UnityEngine;

namespace SpeedrunnerHelper {

    public class Clickable : MonoBehaviour {
        private AudioSource activateSound;
        private AudioSource deactivateSound;

        protected virtual void Awake() {
#if IS_MOD
            activateSound = GameOptions.Instance.transform.Find("ActivateSound").GetComponent<AudioSource>();
            deactivateSound = GameOptions.Instance.transform.Find("DeactivateSound").GetComponent<AudioSource>();
#endif
        }

        public void PlayClickedSound() {
            activateSound.Play();
        }

        public void PlayClickedSound2() {
            deactivateSound.Play();
        }
    }

}
