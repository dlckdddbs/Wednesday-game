using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{

    public class Soul : MonoBehaviour
    {
        public GameObject target = null;
        public Manager manager = null;
        public ParticleSystem particle = null;
        public GameObject explosion = null;

        public int exp = 0;
        public bool isAlive = true;
        // Start is called before the first frame update
        void Start()
        {
        }

        private void OnEnable()
        {
            //�ʱ�ȭ.
            target = null;
            isAlive = true;
            if (explosion)
                explosion.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (isAlive == false)
                return;

            if(target != null) //target == �÷��̾�
            {
                Vector3 dist = target.transform.position - transform.position;
                if (dist.sqrMagnitude < 0.1f)
                {

                    if (manager)
                    {
                        manager.exp += exp;
                        manager.scoreValue += 3;
                    }

                    isAlive = false;
                    if (explosion)
                        explosion.SetActive(true);

                    StartCoroutine(RemoveEvent());
                    return;
                }
                //�Ÿ��� ������������ ����ؼ� Ÿ�ٿ��� �̵��Ѵ�.
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 5.0f);
                
            }
        }

        IEnumerator RemoveEvent()
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
           

            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }


    }
}
