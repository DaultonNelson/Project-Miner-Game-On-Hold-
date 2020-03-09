using Assets.Scripts.MinerGame.Interactions;
using Assets.Scripts.MinerGame.PlayerFiles;
using Assets.Scripts.MinerGame.TimeSensitive;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MinerGame.UIFiles {
    public class PauseMenu : MonoBehaviour {

        #region Variables
        /// <summary>
        /// The instance of this class.
        /// </summary>
        public static PauseMenu Instance { get; private set; }

        /// <summary>
        /// The Color of the Citizen Status Menu.
        /// </summary>
        public Color citizenStatusColor = Color.white;
        /// <summary>
        /// The Color of the Inventory Menu.
        /// </summary>
        public Color inventoryColor = Color.white;
        /// <summary>
        /// The Color of the Health Menu.
        /// </summary>
        public Color healthColor = Color.white;
        /// <summary>
        /// The Image representing the Citizen Status Button.
        /// </summary>
        public Image citizenStatusButtonImage;
        /// <summary>
        /// The Image representing the Inventory Button.
        /// </summary>
        public Image inventoryButtonImage;
        /// <summary>
        /// The Image representing the Health.
        /// </summary>
        public Image healthButtonImage;
        /// <summary>
        /// The GameObject holding all of the Citizen Status UI items.
        /// </summary>
        public GameObject citizenStatusItems;
        /// <summary>
        /// The GameObject holding all of the Inventory UI items.
        /// </summary>
        public GameObject inventoryItems;
        /// <summary>
        /// The GameObject holding all of the Health UI items.
        /// </summary>
        public GameObject healthItems;
        /// <summary>
        /// The PauseGroup containing the Citizen Status information.
        /// </summary>
        public PauseGroup_CitizenStatus ps_CitizenStatus;
        /// <summary>
        /// The PauseGroup containing the Inventory information.
        /// </summary>
        public PauseGroup_Inventory ps_Inventory;
        /// <summary>
        /// The PauseGroup containing the Health information.
        /// </summary>
        public PauseGroup_Health ps_Health;

        /// <summary>
        /// Return True if game is paused, or false if not.
        /// </summary>
        private bool paused = false;
        /// <summary>
        /// The object that holds the pause menu data.
        /// </summary>
        private GameObject pauseMenuObject;
        /// <summary>
        /// The Background Image of the pause menu.
        /// </summary>
        private Image pauseMenuBackground;
        /// <summary>
        /// The initial color of the Citizen Status Button.
        /// </summary>
        private Color initialCitizenStatusButtonColor;
        /// <summary>
        /// The initial color of the Inventory Button.
        /// </summary>
        private Color initialInventoryButtonColor;
        /// <summary>
        /// The initial color of the Health Button.
        /// </summary>
        private Color initialHealthButtonColor;
        /// <summary>
        /// A List of the Citizen in the scene.
        /// </summary>
        private List<CitizenObject> citizens = new List<CitizenObject>();

        /// <summary>
        /// Return true if the player is able to pause, or false if not.
        /// </summary>
        public bool ableToPause { get; set; }
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
            GetAndSetInitialData();
            ReturnToGame();
        }

        private void GetAndSetInitialData() {
            pauseMenuObject = transform.GetChild(0).gameObject;
            pauseMenuBackground = pauseMenuObject.GetComponent<Image>();
            initialCitizenStatusButtonColor = citizenStatusButtonImage.color;
            initialInventoryButtonColor = inventoryButtonImage.color;
            initialHealthButtonColor = healthButtonImage.color;
            citizens = FindObjectsOfType<CitizenObject>().ToList();
            ableToPause = false;
        }

        private void Update() {
            if (ableToPause) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    TogglePauseMenu();
                } 
            }
        }

        private void PauseGame() {
            TurnEverythingOff();
            pauseMenuObject.SetActive(true);
            ChangeToCitizenStatusTab();
            DisableInventoryTab();
            DisableHealthTab();
        }

        private void ReturnToGame() {
            pauseMenuObject.SetActive(false);
            TurnEverythingBackOn();
            ChangeToCitizenStatusTab();
            DisableInventoryTab();
            DisableHealthTab();
        }

        private void ChangeToCitizenStatusTab() {
            pauseMenuBackground.color = citizenStatusColor;
            citizenStatusButtonImage.color = initialCitizenStatusButtonColor;
            citizenStatusItems.SetActive(true);
            ps_CitizenStatus.LoadCitizenStatusData();
        }

        private void ChangeToInventoryTab() {
            pauseMenuBackground.color = inventoryColor;
            inventoryButtonImage.color = initialInventoryButtonColor;
            inventoryItems.SetActive(true);
            ps_Inventory.LoadInventoryData();
        }

        private void ChangeToHealthTab() {
            pauseMenuBackground.color = healthColor;
            healthButtonImage.color = initialHealthButtonColor;
            healthItems.SetActive(true);
            ps_Health.LoadHealthData();
        }

        private void DisableCitizenStatusTab() {
            citizenStatusButtonImage.color = initialCitizenStatusButtonColor * 0.5f;
            citizenStatusItems.SetActive(false);
        }

        private void DisableInventoryTab() {
            inventoryButtonImage.color = initialInventoryButtonColor * 0.5f;
            inventoryItems.SetActive(false);
        }

        private void DisableHealthTab() {
            healthButtonImage.color = initialHealthButtonColor * 0.5f;
            healthItems.SetActive(false);
        }

        /// <summary>
        /// Toggles the Pause Menu.
        /// </summary>
        public void TogglePauseMenu() {
            paused = !paused;
            if (paused) {
                PauseGame();
            }
            else {
                ReturnToGame();
            }
        }

        /// <summary>
        /// Changes the type of menu.
        /// </summary>
        public void ChangeMenu(int menuTabNumber) {
            Color newColor = Color.white;

            switch(menuTabNumber) {
                case 1:
                    //Citizen Status
                    newColor = citizenStatusColor;
                    ChangeToCitizenStatusTab();
                    DisableInventoryTab();
                    DisableHealthTab();
                    break;
                case 2:
                    //Inventory
                    newColor = inventoryColor;
                    DisableCitizenStatusTab();
                    ChangeToInventoryTab();
                    DisableHealthTab();
                    break;
                case 3:
                    //Heath
                    newColor = healthColor;
                    DisableCitizenStatusTab();
                    DisableInventoryTab();
                    ChangeToHealthTab();
                    break;
                default:
                    Debug.LogError("The int given to the button doesn't match any menu tab numbers.");
                    break;
            }

            pauseMenuBackground.color = newColor;
        }

        /// <summary>
        /// Turns everything that should have been disabled back on.
        /// </summary>
        public void TurnEverythingBackOn() {
            TimeSystem.Instance.timePaused = false;
            InteractionSystem.Instance.ableToAction = true;
            if (citizens.Count > 0) {
                foreach (CitizenObject c in citizens) {
                    if (c.ableToMove == false) {
                        c.ableToMove = true;
                    }
                }
            } else {
                Debug.LogWarning("No Citizens found in scene.");
            }
            PlayerMovement.Instance.ableToMove = true;
            ableToPause = true;
        }

        /// <summary>
        /// Turns every off that needs to be turned off.
        /// </summary>
        public void TurnEverythingOff() {
            TimeSystem.Instance.timePaused = true;
            PlayerMovement.Instance.ableToMove = false;
            InteractionSystem.Instance.ableToAction = false;
            if (citizens.Count > 0) {
                foreach (CitizenObject c in citizens) {
                    if (c.ableToMove == true) {
                        c.ableToMove = false;
                    }
                }
            }
            else {
                Debug.LogWarning("No Citizens found in scene.");
            }
        }
    }
}