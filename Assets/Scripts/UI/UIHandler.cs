using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {

        [Inject]
        PlayerInputHandler PlayerInput;

        public void OnFinishedClicked()
        {
            PlayerInput.PlayerInputDone();
        }
    }
}
