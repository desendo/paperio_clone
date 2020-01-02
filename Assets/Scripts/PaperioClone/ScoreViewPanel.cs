using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PaperIOClone
{
    public class ScoreViewPanel : MonoBehaviour
    {
        [SerializeField] private Image back;
        [SerializeField] private TMP_Text text;

        public void SetValues(Color color, string singleScoreText)
        {
            text.text = singleScoreText;
            back.color = color;
        }
    }
}