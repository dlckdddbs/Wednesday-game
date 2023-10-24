using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotomchi
{

    public class UIDamageNum : MonoBehaviour
    {
        public Text damageNum = null;
        public int life = 1;
        public Vector3 randomPos;
        public GameObject target = null;


        private void Update()
        {
            if (target)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(target.transform.position) + randomPos;
                transform.position = pos;
            }
        }

        // Start is called before the first frame update
        public void ShowDamage(GameObject _target,  int dmg, Color color)
        {
            target = _target;
            randomPos = new Vector3(Random.Range(-20, 20), Random.Range(120, 160), 0);

            if (damageNum)
            {
                damageNum.text = dmg.ToString();
                damageNum.color = color;
            }

            //Destroy(gameObject, life);
            StartCoroutine(RemoveEvent());
        }

        IEnumerator RemoveEvent()
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }


    }
}
