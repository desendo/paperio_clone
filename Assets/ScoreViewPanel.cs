using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game

{
    public class ScoreViewPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Image back;

        public void SetValues(Color color, string text)
        {
            this.text.text = text;
            back.color = color;
        }
    }

}