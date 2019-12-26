using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game
{
    public class GuiHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text exit;
        [SerializeField] TMP_Text enter;
        [SerializeField] TMP_Text kills;
        [SerializeField] TMP_Text area;

        private void OnEnable()
        {
            SignalsController.Default.Add(this);
        }
        private void OnDisable()
        {
            SignalsController.Default.Remove(this);
        }


    }
}
