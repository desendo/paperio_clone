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
            var k = 0;
            var playerinFirstThree = false;
            for (var i = 0; i < scoreData.Count; i++)
            {
                var score = scoreData[i];
                var text = i + 1 + " " + score.Name + ".. " + score.AreaNormalized.ToString("p1");
                if (k < panels.Length - 1)
                {
                    panels[k].gameObject.SetActive(true);
                    if (score.IsPlayer)
                    {
                        kills.text = "x " + score.Kills;
                        playerinFirstThree = true;
                    }

                    panels[k].SetValues(score.Color, text);
                    k++;
                }

                if (!playerinFirstThree && score.IsPlayer)
                {
                    kills.text = "x " + score.Kills;

                    panels[panels.Length - 1].gameObject.SetActive(true);
                    panels[panels.Length - 1].SetValues(score.Color, text);
                }
                else if (playerinFirstThree)
                {
                    panels[panels.Length - 1].gameObject.SetActive(false);
                }
            }
        }

        private void HideScores()
        {
            scoresContainer.gameObject.SetActive(false);
        }

        private void ShowScores()
        {
            scoresContainer.gameObject.SetActive(true);
        }
    }
}