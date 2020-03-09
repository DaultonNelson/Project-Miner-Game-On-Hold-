using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.Interactions {
    public class InteractionSystem : MonoBehaviour {

        #region MyRegion
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static InteractionSystem Instance { get; private set; }

        /// <summary>
        /// The action reticle of the player.
        /// </summary>
        public SpriteRenderer actionReticle;
        /// <summary>
        /// The color the reticle will change to when able to interact.
        /// </summary>
        public Color ableToInteractColor = Color.white;
        /// <summary>
        /// The text notification that appears above the player's head.
        /// </summary>
        public GameObject textNotification;
        /// <summary>
        /// The Canvas the text notifications will appear on.
        /// </summary>
        public Canvas notificationCanvas;
        /// <summary>
        /// The text that shows what object is being hovered over.
        /// </summary>
        public Text focusText;
        /// <summary>
        /// Return true if the player is able to interact with things, or false if not.
        /// </summary>
        public bool ableToAction = false;

        /// <summary>
        /// The initial color of the reticle;
        /// </summary>
        private Color initialReticleColor;
        /// <summary>
        /// Return true if reticle is over interactable, or false if not.
        /// </summary>
        private bool overInteractable = false;
        /// <summary>
        /// The interactive object the reticle is over;
        /// </summary>
        private IInteractable interactiveObject;
        #endregion

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("There is more than one " + this + "in the scene!");
                Destroy(gameObject);
            }
        }

        private void Start() {
            initialReticleColor = actionReticle.color;
            ableToAction = false;
        }

        private void Update() {
            notificationCanvas.transform.LookAt(Camera.main.transform);

            if (ableToAction) {
                if (overInteractable) {
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        interactiveObject.InteractionFunction();

                        DisplayNotification(
                            interactiveObject.notificationCharacterColor,
                            interactiveObject.notificationOutlineColor,
                            interactiveObject.notification
                        );

                        if (interactiveObject.nullifyOnInteract) {
                            NullifyInteractionFocus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays a notification above the player's head.
        /// </summary>
        public void DisplayNotification (Color characterColor, Color outlineColor, string notification) {
            Text t = Instantiate(textNotification, notificationCanvas.transform).GetComponent<Text>();
            if (t == null) {
                Debug.LogError("No Text component found on spawned object!");
            }
            t.transform.localPosition = Vector3.zero;

            //Color
            t.color = characterColor;
            Outline to = t.GetComponent<Outline>();
            if (to == null) {
                Debug.LogError("No Outline found on spawned object!");
            }
            to.effectColor = new Color(outlineColor.r, outlineColor.g, outlineColor.b, to.effectColor.a);

            SetNotificationText(notification, t);
            

            Destroy(t.gameObject, 3.1f);
        }

        private void SetNotificationText (string messageToBeDisplayed, Text textComponent) {
            textComponent.text = messageToBeDisplayed;
        }

        /// <summary>
        /// Nullifies th interaction focus.
        /// </summary>
        public void NullifyInteractionFocus() {
            actionReticle.color = initialReticleColor;
            interactiveObject = null;
            overInteractable = false;
            focusText.text = string.Empty;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Interactable") {
                actionReticle.color = ableToInteractColor;
                interactiveObject = other.GetComponent<IInteractable>();
                if (interactiveObject == null) {
                    Debug.LogError("Object does not have IInteractable interface on it!", gameObject);
                }
                focusText.text = other.name;
                overInteractable = true;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.tag == "Interactable") {
                NullifyInteractionFocus();
            }
        }
    }
}