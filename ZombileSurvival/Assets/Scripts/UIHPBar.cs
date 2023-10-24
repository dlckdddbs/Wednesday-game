using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotomchi
{



    public class UIHPBar : MonoBehaviour
    {
        public Player player = null;
        public Image hpBar = null;

        // Update is called once per frame
        void Update()
        {
            if (player)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(player.transform.position) + new Vector3(0, -80, 0);
                transform.position = pos;

                hpBar.fillAmount = (float)player.hp / (float)player.maxHp;
            }
        }
    }
}
