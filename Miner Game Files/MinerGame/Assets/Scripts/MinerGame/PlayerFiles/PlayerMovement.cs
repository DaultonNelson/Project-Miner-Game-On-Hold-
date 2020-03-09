using Assets.Scripts.MinerGame.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MinerGame.PlayerFiles {
    public class PlayerMovement : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static PlayerMovement Instance { get; private set; }

        /// <summary>
        /// The keys fighting for dominance.
        /// </summary>
        public enum DominantKey { UP, DOWN, LEFT, RIGHT, NONE }
        /// <summary>
        /// The speed at which the player moves.
        /// </summary>
        public float speed = 5;
        /// <summary>
        /// The time in seconds the player is allowed to loiter before points are docked.
        /// </summary>
        public float timeAllowedForLoitering;
        /// <summary>
        /// The body of the player.
        /// </summary>
        public Transform body;
        /// <summary>
        /// The animator attached to the feet parent.
        /// </summary>
        public Animator feetAnimator;
        /// <summary>
        /// Return true if player should be able to move, or false if not.
        /// </summary>
        public bool ableToMove = false;
        /// <summary>
        /// Return true if player is in a No Loitering area, or false if not.
        /// </summary>
        public bool inNoLoiteringArea = true;

        /// <summary>
        /// The dominant key being pressed right now.
        /// </summary>
        private DominantKey dKey = DominantKey.NONE;
        /// <summary>
        /// The character controller attached to the player.
        /// </summary>
        private CharacterController playerControl;
        /// <summary>
        /// The Citizen ranking attached to the player.
        /// </summary>
        private CitizenRanking cr;
        /// <summary>
        /// The value the player accumiliates while standing around.
        /// </summary>
        private float standingValue = 0;
        #endregion

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.LogError("There is more than one " + this + "in the scene!");
                Destroy(gameObject);
            }
        }

        private void Start() {
            playerControl = GetComponent<CharacterController>();
            cr = GetComponent<CitizenRanking>();
            ableToMove = false;
        }

        private void Update() {
            if (ableToMove) {
                FightForDominance();
                MoveCharacter();
                if (inNoLoiteringArea) {
                    standingValue += 1f * Time.deltaTime;
                    if (standingValue >= timeAllowedForLoitering) {
                        DockForLoitering();
                        standingValue = 0;
                    } 
                }
            }
        }

        private void MoveCharacter() {
            switch (dKey) {
                case DominantKey.NONE:
                    break;
                case DominantKey.LEFT:
                    playerControl.Move(Vector3.right * speed * Time.deltaTime);
                    break;
                case DominantKey.RIGHT:
                    playerControl.Move(Vector3.left * speed * Time.deltaTime);
                    break;
                case DominantKey.UP:
                    playerControl.Move(Vector3.back * speed * Time.deltaTime);
                    break;
                case DominantKey.DOWN:
                    playerControl.Move(Vector3.forward * speed * Time.deltaTime);
                    break;
            }
        }

        private void FightForDominance() {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                dKey = DominantKey.LEFT;
                body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 90);
                feetAnimator.SetFloat("Walk", 1);
                standingValue = 0;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                dKey = DominantKey.RIGHT;
                body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, -90);
                feetAnimator.SetFloat("Walk", 1);
                standingValue = 0;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                dKey = DominantKey.UP;
                body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 180);
                feetAnimator.SetFloat("Walk", 1);
                standingValue = 0;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                dKey = DominantKey.DOWN;
                body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 0);
                feetAnimator.SetFloat("Walk", 1);
                standingValue = 0;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow)
                || Input.GetKeyUp(KeyCode.RightArrow)
                || Input.GetKeyUp(KeyCode.UpArrow)
                || Input.GetKeyUp(KeyCode.DownArrow)) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    dKey = DominantKey.UP;
                    body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 180);
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    dKey = DominantKey.DOWN;
                    body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 0);
                } else if (Input.GetKey(KeyCode.LeftArrow)) {
                    dKey = DominantKey.LEFT;
                    body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, 90);
                } else if (Input.GetKey(KeyCode.RightArrow)) {
                    dKey = DominantKey.RIGHT;
                    body.localEulerAngles = new Vector3(body.rotation.x, body.rotation.y, -90);
                }
            }

            if (!Input.GetKey(KeyCode.LeftArrow)
            && !Input.GetKey(KeyCode.RightArrow)
            && !Input.GetKey(KeyCode.UpArrow)
            && !Input.GetKey(KeyCode.DownArrow)) {
                dKey = DominantKey.NONE;
                feetAnimator.SetFloat("Walk", 0);
            }
        }

        private void DockForLoitering () {
            Debug.Log("Player has loitered for too long.");

            int a = 0;

            switch (cr.currentTier) {
                case GlobalConfig.ClassTier.A:
                    break;
                case GlobalConfig.ClassTier.B:
                    a = 1;
                    break;
                case GlobalConfig.ClassTier.C:
                    a = 2;
                    break;
                case GlobalConfig.ClassTier.D:
                    a = 4;
                    break;
                case GlobalConfig.ClassTier.E:
                    a = 8;
                    break;
                case GlobalConfig.ClassTier.F:
                    a = 16;
                    break;
                case GlobalConfig.ClassTier.G:
                    a = 32;
                    break;
            }

            DockAndShow(a, "Loitering");
        }

        private void DockAndShow(int dockingScore, string message) {
            cr.SubtractPoints(dockingScore);
            if (dockingScore != 0) {
                message = "-" + dockingScore + " " + message;
                cr.citizenCardAnimator.SetTrigger("Show");

                Color a = new Color(.263f, 0, 0, 1.0f);

                InteractionSystem.Instance.DisplayNotification(Color.red, a, message);
            }
        }
    }
}