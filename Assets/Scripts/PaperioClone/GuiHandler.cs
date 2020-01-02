using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PaperIOClone
{
    //to do ALL GUI
    public class GuiHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text kills;
        [SerializeField] private ScoreViewPanel[] panels;
        [SerializeField] private RectTransform scoresContainer;

        public void SetScores(List<ScoresHandler.ScoreData> scoreData)
        {
            ShowScores();

            kills.text = "x 0";
            foreach (var panel in panels) panel.gameObject.SetActive(false);
            
            var currentPanelIndex = 0;
            var isPlayerInFirstThree = false;
            for (var i = 0; i < scoreData.Count; i++)
            {
                var score = scoreData[i];
                var text = i + 1 + " " + score.Name + ".. " + score.AreaNormalized.ToString("p1");
                if (currentPanelIndex < panels.Length - 1)
                {
                    panels[currentPanelIndex].gameObject.SetActive(true);
                    if (score.IsPlayer)
                    {
                        kills.text = "x " + score.Kills;
                        isPlayerInFirstThree = true;
                    }

                    panels[currentPanelIndex].SetValues(score.Color, text);
                    currentPanelIndex++;
                }

                if (!isPlayerInFirstThree && score.IsPlayer)
                {
                    kills.text = "x " + score.Kills;

                    panels[panels.Length - 1].gameObject.SetActive(true);
                    panels[panels.Length - 1].SetValues(score.Color, text);
                }
                else if (isPlayerInFirstThree)
                {
                    panels[panels.Length - 1].gameObject.SetActive(false);
                }
            }
        }
        private void ShowScores()
        {
            scoresContainer.gameObject.SetActive(true);
        }
    }
}