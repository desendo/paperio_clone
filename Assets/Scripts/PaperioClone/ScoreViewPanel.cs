using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PaperIOClone
{
    public class ScoreViewPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Image back;

        public void SetValues(Color color, string singleScoreText)
        {
            text.text = singleScoreText;
            back.color = color;
        }
    }
}