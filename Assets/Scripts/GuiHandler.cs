using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game
{
    public class GuiHandler : MonoBehaviour, IReceive<SignalZoneBorderPass>
    {
        [SerializeField] TMP_Text exit;
        [SerializeField] TMP_Text enter;

        private void OnEnable()
        {
            SignalsController.Default.Add(this);
        }
        private void OnDisable()
        {
            SignalsController.Default.Remove(this);
        }


        public void HandleSignal(SignalZoneBorderPass arg)
        {


            if (arg.isExiting)
            {
                exit.text = "exit point " + arg.nearestBorderPointIndex.ToString();
                enter.text = "enter point ";
            }
            if (!arg.isExiting)
            {
                enter.text = "enter point " + arg.nearestBorderPointIndex.ToString();
            }
        }
    }
}
