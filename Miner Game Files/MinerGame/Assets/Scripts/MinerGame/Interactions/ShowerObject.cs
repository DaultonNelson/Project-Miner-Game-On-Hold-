using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.UIFiles;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public class ShowerObject : MonoBehaviour, IInteractable {

        #region Variables
        #region Inherited From Interface
        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get { return "Showering"; }

            set { }
        }

        public Color notificationCharacterColor {
            get { return Color.green; }

            set { }
        }

        public Color notificationOutlineColor {
            get { return new Color(0, .263f, 0, 1); }

            set { }
        }
        #endregion

        /// <summary>
        /// The Particle System that represents the shower.
        /// </summary>
        public ParticleSystem showerSystem;
        /// <summary>
        /// The Animator attached to the player's feet.
        /// </summary>
        public Animator footAnimator;
        /// <summary>
        /// The time it takes the player to take a shower.
        /// </summary>
        public float showerTime;
        /// <summary>
        /// The speed at which the player will move to the shower position.
        /// </summary>
        public float movementSpeed;

        /// <summary>
        /// Return true to move the player, or false if not.
        /// </summary>
        private bool movePlayerToShower = false;
        /// <summary>
        /// Return true to move the player back to their entered position, or false if not.
        /// </summary>
        private bool movePlayerBack = false;
        /// <summary>
        /// The Transform of the Protagonist character.
        /// </summary>
        private Transform protagonist;
        /// <summary>
        /// The position the player entered the shower
        /// </summary>
        private Vector3 enteredPosition;
        #endregion

        private void Start() {
            protagonist = GameObject.FindGameObjectWithTag("Player").transform;
            if (protagonist == null) {
                Debug.LogError("Protagonist was not found!");
            }
        }

        private void Update() {
            if (movePlayerToShower) {
                MovePlayerToPosition();
            }
            if (movePlayerBack) {
                MovePlayerBackToEnter();
            }
        }

        private void MovePlayerToPosition() {
            float distance = Vector3.Distance(protagonist.position, transform.position);
            if (distance == 0) {
                showerSystem.Play();
                footAnimator.SetFloat("Walk", 0);
                StartCoroutine(TurnOffShower());
                movePlayerToShower = false;
            }
            protagonist.position = Vector3.MoveTowards(protagonist.position, transform.position, movementSpeed * Time.deltaTime);
        }

        private void MovePlayerBackToEnter() {
            float distance = Vector3.Distance(protagonist.position, enteredPosition);
            if (distance == 0) {
                PlayerHealth ph = PlayerHealth.Instance;
                ph.SetLastTimePlayerShowered();
                GlobalConfig.SaveShoweringData(ph);
                PlayerMovement.Instance.ableToMove = true;
                InteractionSystem.Instance.ableToAction = true;
                PauseMenu.Instance.ableToPause = true;
                movePlayerBack = false;
            }
            protagonist.position = Vector3.MoveTowards(protagonist.position, enteredPosition, movementSpeed * Time.deltaTime);
        }

        public void InteractionFunction() {
            PauseMenu.Instance.ableToPause = false;
            PlayerMovement.Instance.ableToMove = false;
            InteractionSystem.Instance.ableToAction = false;
            enteredPosition = protagonist.position;
            footAnimator.SetFloat("Walk", 1);
            movePlayerToShower = true;
        }

        IEnumerator TurnOffShower () {
            yield return new WaitForSeconds(showerTime);
            showerSystem.Stop();
            footAnimator.SetFloat("Walk", 1);
            movePlayerBack = true;
            //PlayerMovement.Instance.ableToMove = true;
            //InteractionSystem.Instance.ableToAction = true;
            StopCoroutine(TurnOffShower());
        }
    }
}