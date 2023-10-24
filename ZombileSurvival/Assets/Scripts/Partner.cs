using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dotomchi
{

    public class Partner : Enemy
    {
        public Enemy targetEnemy = null;
        public GameObject muzzleEffect = null;


        // Start is called before the first frame update
        void Start()
        {
            attackPoint = 1;

            ani = GetComponent<Animator>();
            col = GetComponent<Collider>();
            agent = GetComponent<NavMeshAgent>();

            isAlive = true;
            dissolveEffectValue = 0.0f;
            SetDissovleEffectValue(dissolveEffectValue);

            if (muzzleEffect)
                muzzleEffect.SetActive(false);
        }


        private void OnEnable()
        {
            attackPoint = 1;

            isAlive = true;
            dissolveEffectValue = 0.0f;
            SetDissovleEffectValue(dissolveEffectValue);

            if (muzzleEffect)
                muzzleEffect.SetActive(false);
        }



        // Update is called once per frame
        void Update()
        {
            if (ani)
            {
                stateInfo = ani.GetCurrentAnimatorStateInfo(0);
                for (AniStateType type = AniStateType.idle; type < AniStateType.max; type++)
                {
                    if (stateInfo.IsName(type.ToString()))
                        aniStateType = type;
                }
            }

            Vector3 look = player.transform.position - transform.position;
            if (ani)
                ani.SetFloat("moveSpeed", (agent.velocity.magnitude / agent.speed));

            if (look.sqrMagnitude <= 20.0f)
            {

                if (targetEnemy == null)
                {
                    if (agent)
                        agent.isStopped = true;


                    float minDist = Mathf.Infinity;
                    Enemy minEnemy = null;

                    Collider[] colList = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("Enemy"));
                    for (int i = 0; i < colList.Length; i++)
                    {
                        Enemy findEnemy = colList[i].GetComponent<Enemy>();
                        if (findEnemy && findEnemy.isAlive)
                        {

                            Vector3 findDist = findEnemy.transform.position - transform.position;
                            if (minDist > findDist.sqrMagnitude)
                            {
                                minDist = findDist.sqrMagnitude;
                                minEnemy = findEnemy;
                            }

                        }
                    }
                    if (minEnemy != null)
                    {
                        targetEnemy = minEnemy;
                    }
                }
                else//targetEnemy 있음
                {
                    if (agent)
                        agent.isStopped = false;

                    if (targetEnemy.isAlive == false)
                    {
                        targetEnemy = null;
                        return;
                    }

                    Vector3 look2 = targetEnemy.transform.position - transform.position;
                    look2.y = transform.position.y;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look2), Time.deltaTime * 2.0f);

                    if (look2.sqrMagnitude > 1.0f)//거리구하는거
                    {
                        agent.SetDestination(targetEnemy.transform.position);
                    }

                    if (look2.sqrMagnitude < 3.0f && aniStateType != AniStateType.attack)
                    {
                        if (ani)
                            ani.Play("attack");

                        if (attackCoroutine != null)
                            StopCoroutine(attackCoroutine);

                        attackCoroutine = StartCoroutine(AttackEnemyImact());
                    }

                }

                //todo:: 적에게 공격
            }
            else// if (look.sqrMagnitude > 20.0f)
            {
                look.y = transform.position.y;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 2.0f);


                if (agent)
                {
                    agent.isStopped = false;
                    if (look.sqrMagnitude > 1.0f)//거리구하는거
                    {
                        agent.SetDestination(player.transform.position);
                    }
                }

            }

        }

        IEnumerator AttackEnemyImact()
        {
            yield return new WaitForSeconds(0.3f);
            if (targetEnemy)
            {

                if (enemyType == EnemyType.girl)
                {
                    attackPoint = targetEnemy.maxHp + 1;
                    targetEnemy.SetDamage(attackPoint, transform);
                    targetEnemy = null;
                }


                else if (enemyType == EnemyType.hatMan)
                {
                    Collider[] colList = Physics.OverlapSphere(transform.forward + transform.position, 1.0f, LayerMask.GetMask("Enemy"));
                    for (int i = 0; i < colList.Length; i++)
                    {
                        Enemy findEnemy = colList[i].GetComponent<Enemy>();
                        if (findEnemy && findEnemy.isAlive)
                        {
                            findEnemy.SetDamage(attackPoint, transform);
                        }
                    }
                    targetEnemy = null;
                }
                else if (enemyType == EnemyType.police)
                {
                    GameObject bullet = ObjectPoolManager.instance.GetObject("Prefabs/Bullet"); //Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));

                    if (bullet)
                    {
                        bullet.transform.position = muzzleEffect.transform.position;
                        Bullet bulletScript = bullet.GetComponent<Bullet>();
                        if (bulletScript)
                            bulletScript.MoveStart(transform, 1);

                    }

                    if (muzzleEffect)
                        muzzleEffect.SetActive(true);

                    yield return new WaitForSeconds(0.1f);
                    if (muzzleEffect)
                        muzzleEffect.SetActive(false);

                }
                else if(enemyType == EnemyType.shirtMan)
                {
                    //targetEnemy.SetDamage(attackPoint, transform);
                    //targetEnemy = null;

                    GameObject grenade = Instantiate(Resources.Load<GameObject> ("Prefabs/Grenade"));
                    if (grenade)
                    {
                        grenade.transform.position = transform.position + new Vector3(0, 1, 0);
                        Grenade g = grenade.GetComponent<Grenade>();
                        if (g)
                        {
                            g.ThrowStart(transform);
                        }
                    }

                }

            }
        }

    }
}
