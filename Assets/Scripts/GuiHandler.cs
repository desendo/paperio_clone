using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game
{

    //to do ALL GUI
    public class GuiHandler : MonoBehaviour
    {

        [SerializeField]
        ScoreViewPanel[] panels;
        [SerializeField]
        RectTransform scoresContainer;
        [SerializeField]
        TMP_Text kills;

        public void SetScores(List<ScoresHandler.ScoreData> scoreData)
        {
            ShowScores();

            kills.text = "x 0";

            foreach (var panel in panels)
            {
                panel.gameObject.SetActive(false);
            }
            int k = 0;
            bool playerinFirstThree = false;
            for (int i = 0; i < scoreData.Count; i++)
            {
                
                ScoresHandler.ScoreData score = scoreData[i];
                string text = (i+1).ToString() + " " + score.name + ".. " + (score.areaNormalized ).ToString("p1");
                if (k < panels.Length-1)
                {
                    panels[k].gameObject.SetActive(true);
                    if (score.isPlayer)
                    {
                        kills.text = "x "+(score.kills).ToString();
                        playerinFirstThree = true;
                    }
                    panels[k].SetValues(score.color, text);
                    k++;
                }
                if (!playerinFirstThree && score.isPlayer)
                {
                    kills.text = "x " + (score.kills).ToString();

                    panels[panels.Length - 1].gameObject.SetActive(true);
                    panels[panels.Length - 1].SetValues(score.color, text);
                }
                else if(playerinFirstThree)
                    panels[panels.Length - 1].gameObject.SetActive(false);
            }



        }
        void HideScores()
        {
            scoresContainer.gameObject.SetActive(false);
        }
        void ShowScores()
        {
            scoresContainer.gameObject.SetActive(true);
        }
    }
}
