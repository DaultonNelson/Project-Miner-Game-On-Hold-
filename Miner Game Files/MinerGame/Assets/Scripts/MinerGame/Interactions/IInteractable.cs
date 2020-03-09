using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.MinerGame.Interactions {
    public interface IInteractable {
        bool nullifyOnInteract { get; set; }
        string notification { get; set; }
        Color notificationCharacterColor { get; set; }
        Color notificationOutlineColor { get; set; }

        void InteractionFunction();
    }
}