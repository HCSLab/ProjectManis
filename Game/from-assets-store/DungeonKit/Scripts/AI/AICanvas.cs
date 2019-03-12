using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonKIT
{
    public class AICanvas : MonoBehaviour
    {
        AIStats aIStats;

        [Header("Elements")]
        public Image hpBar;

        private void Start()
        {
            aIStats = GetComponentInParent<AIStats>();
            UpdateUI();
        }

        //Method for updating the UI
        public void UpdateUI()
        {
            hpBar.fillAmount = aIStats.HP.current / aIStats.HP.max;
        }

    }
}
