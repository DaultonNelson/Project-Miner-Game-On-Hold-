using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Fungus;
using Assets.Scripts.MinerGame.TimeSensitive;
using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.UIFiles;
//NOTE: Perhaps have citizen run away if protag has stink/weapon
namespace Assets.Scripts.MinerGame.Interactions {
    class CitizenObject : MonoBehaviour, IInteractable {
        #region Variables
        public bool nullifyOnInteract {
            get { return true; }

            set { }
        }

        public string notification {
            get { return "Talking..."; }

            set { }
        }

        public Color notificationCharacterColor {
            get { return Color.grey; }

            set { }
        }

        public Color notificationOutlineColor {
            get { return new Color(0.263f, 0.263f, 0.263f, 1); }

            set { }
        }

        /// <summary>
        /// Return true if the citizen is able to move, or false if not.
        /// </summary>
        public bool ableToMove { get; set; }

        /// <summary>
        /// The dialogue box this citizen will communicate to the player with.
        /// </summary>
        public string dialogueBox = "";
        /// <summary>
        /// The Distance the citizen will walk from their standing position.
        /// </summary>
        public float walkingDistance = 1f;
        /// <summary>
        /// The time in seconds the citizen will take between movements.
        /// </summary>
        public float timeBetweenMovement = 3f;
        /// <summary>
        /// The speed at which this citizen walks.
        /// </summary>
        public float walkingSpeed = 1f;

        /// <summary>
        /// The animator attached to this Citizen's feet.
        /// </summary>
        private Animator feetAnim;
        /// <summary>
        /// The flowchart found within the scene.
        /// </summary>
        private Flowchart flo;
        /// <summary>
        /// The Transform attached to the protagonist.
        /// </summary>
        private Transform protagonist;
        /// <summary>
        /// The initial position of this citizen.
        /// </summary>
        private Vector3 initialPosition;
        /// <summary>
        /// The destination the citizen will move to.
        /// </summary>
        private Vector3 destination = Vector3.zero;
        /// <summary>
        /// Return true if the citizen should move to a point, or false if not.
        /// </summary>
        private bool moveToPoint = false;
        #endregion

        private void Start() {
            ableToMove = true;
            initialPosition = transform.position;
            flo = FindObjectOfType<Flowchart>();
            if (flo == null) {
                Debug.LogError("A Flowhcart object was not found!");
            }
            protagonist = GameObject.FindGameObjectWithTag("Player").transform;
            if (protagonist == null) {
                Debug.LogError("The Protagonist's transform was not found.");
            }
            if (timeBetweenMovement <= 0) {
                Debug.LogError("Time Between Movement is less than or equal to zero!");
                return;
            }
            feetAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
            if (feetAnim == null) {
                Debug.LogError("The Feet Animator was not set!");
            }
            StartCoroutine(CitizenWalk());
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(initialPosition, walkingDistance);
        }

        private void Update() {
            if (ableToMove) {
                if (moveToPoint) {
                    MoveTowardsDestination();
                }
            } else {
                feetAnim.SetFloat("Walk", 0);
            }
        }

        private void MoveTowardsDestination() {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance == 0) {
                feetAnim.SetFloat("Walk", 0);
                destination = Vector3.zero;
                moveToPoint = false;
                StartCoroutine(CitizenWalk());
            }
            if (distance > 0) {
                feetAnim.SetFloat("Walk", 1);
                transform.LookAt(destination);
                transform.position = Vector3.MoveTowards(transform.position, destination, walkingSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Calculates the new position this Citizen will walk to.
        /// </summary>
        /// <returns>
        /// The coordinate the citizen will walk to.
        /// </returns>
        private Vector3 CalculateNewPosition () {
            Vector3 newLoc = Vector3.zero;
            Vector2 ran = Random.insideUnitCircle * walkingDistance;
            newLoc = new Vector3(ran.x + initialPosition.x, 0, ran.y + initialPosition.z);
            //Debug.Log(newLoc);
            return newLoc;
        }

        public void InteractionFunction() {
            transform.LookAt(protagonist);
            flo.SetGameObjectVariable("GO", gameObject);
            ableToMove = false;
            TimeSystem.Instance.timePaused = true;
            InteractionSystem.Instance.ableToAction = false;
            PlayerMovement.Instance.ableToMove = false;
            PauseMenu.Instance.ableToPause = false;

            flo.ExecuteBlock(dialogueBox);
        }

        IEnumerator CitizenWalk () {
            yield return new WaitForSeconds(timeBetweenMovement);
            destination = CalculateNewPosition();
            moveToPoint = true;
            StopCoroutine(CitizenWalk());
        }
    }
}