using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{

    public class Bullet : MonoBehaviour
    {
        public GameObject bulletObj = null, explosionEff = null;
        public TrailRenderer trail = null;


        public float speed = 10.0f;
        public int damage = 0;

        public bool isMoving = false;
        public int life = 10;
        // Start is called before the first frame update
        void Start()
        {
            
        }



        public void MoveStart(Transform master, int attackPoint)
        {
            if (trail)
                trail.Clear();

            StopAllCoroutines();

            
            transform.rotation = master.rotation;

            isMoving = true;
            damage = attackPoint;

            if (bulletObj)
                bulletObj.SetActive(true);
            if (explosionEff)
                explosionEff.SetActive(false);



            life = 10;
            StartCoroutine(OneSecendEvent());

        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        IEnumerator OneSecendEvent()
        {
            while (isMoving)
            {
                yield return new WaitForSeconds(1.0f);
                //--life;
                if (--life < 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        IEnumerator RemoveEvent()
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (bulletObj)
                bulletObj.SetActive(false);
            if (explosionEff)
                explosionEff.SetActive(true);

            Collider[] colList = Physics.OverlapSphere(transform.position, 1.0f, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < colList.Length; i++)
            {
                Enemy enemy = colList[i].GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.SetDamage(damage, transform);
                    break;
                }
            }
            isMoving = false;

            StartCoroutine(RemoveEvent());
        }
    }
}
