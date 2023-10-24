using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dotomchi
{

    public enum EnemyType
    {
        zombie,
        girl,
        hatMan,
        shirtMan,
        police,
        max,
    }

    public class Enemy : MonoBehaviour
    {
        public EnemyType enemyType = EnemyType.zombie;

        public Renderer[] render;

        public Animator ani = null;
        public NavMeshAgent agent = null;
        public Collider col = null;

        public Manager manager = null;
        public Player player = null;

        public int hp, maxHp, attackPoint;
        public bool isAlive = true;
        public float dissolveEffectValue = 0.0f;

        public AniStateType aniStateType = AniStateType.idle;
        public AnimatorStateInfo stateInfo;

        public Coroutine attackCoroutine = null;
        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
            col = GetComponent<Collider>();
            agent = GetComponent<NavMeshAgent>();

             isAlive = true;
            dissolveEffectValue = 0.0f;
            SetDissovleEffectValue(dissolveEffectValue);


        }

        private void OnEnable()
        {
            if (col)
                col.enabled = true;

            isAlive = true;
            dissolveEffectValue = 0.0f;
            SetDissovleEffectValue(dissolveEffectValue);

        }
        private void OnDisable()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            if (manager && manager.gameState != GameState.playing)
                return;

            if (ani)
            {
                stateInfo = ani.GetCurrentAnimatorStateInfo(0);
                for (AniStateType type = AniStateType.idle; type < AniStateType.max; type++)
                {
                    if (stateInfo.IsName(type.ToString())) 
                        aniStateType = type; 
                }
            }

            if (isAlive)//네브매쉬 사용
            {
                if (player)
                {
                    Vector3 look = player.transform.position - transform.position;
                    look.y = transform.position.y;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look), Time.deltaTime * 2.0f);

                    if (ani)
                        ani.SetFloat("moveSpeed", (agent.velocity.magnitude / agent.speed));

                    if (agent)
                    {

                        if (look.sqrMagnitude > 1.0f)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                    }

                    if (look.sqrMagnitude < 3.0f && aniStateType != AniStateType.attack)
                    {
                        if (ani)
                            ani.Play("attack");

                        if (attackCoroutine != null)
                            StopCoroutine(attackCoroutine);

                        attackCoroutine = StartCoroutine(AttackImact());

                    }

                }
                if (hp <= 0)
                {
                    isAlive = false;
                    hp = 0;
                    if (ani)
                        ani.Play("death");
                    if (agent)
                        agent.isStopped = true;

                    if (manager)
                    {
                        manager.killCountValue++;
                        manager.scoreValue += 5;
                        manager.enemyList.Remove(this);
                    }

                    if (col)
                        col.enabled = false;

                    StartCoroutine(DissolveEvent());
                }
            }
        }

        IEnumerator AttackImact()
        {
            yield return new WaitForSeconds(0.3f);
            if (player)
            {
                player.SetDamage(attackPoint, null);
            }
        }


        public void SetLevel(int level)
        {
            hp = maxHp = 5 + level;
            attackPoint = 1 + (int)((float)level * 0.25f);
        }

        public void SetDamage(int damage, Transform form)
        {
            if (isAlive)
            {
                if (manager)
                {
                    manager.ShowDamagePrefeb(gameObject, damage, Color.white);
                    manager.scoreValue++;
                }
                if (form != null)
                {
                    Vector3 look = form.position - transform.position;
                    look.y = transform.position.y;

                    transform.rotation = Quaternion.LookRotation(look);
                }
                hp -= damage;
            }
        }


        public void SetDissovleEffectValue(float value)
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.SetFloat("_SliceAmount", value);
            }
        }


        IEnumerator DissolveEvent()
        {
            yield return new WaitForSeconds(3.0f);


            GameObject soul = ObjectPoolManager.instance.GetObject("Prefabs/soul");
            soul.transform.position = transform.position;


            Soul soulScript = soul.GetComponent<Soul>();
            if (soulScript)
            {
                soulScript.manager = manager;
                soulScript.exp = 1;
            }

            dissolveEffectValue = 0.0f;
            while (dissolveEffectValue < 1.0f)
            {
                dissolveEffectValue += 0.01f;
                SetDissovleEffectValue(dissolveEffectValue);
                yield return new WaitForSeconds(0.01f);
            }


            gameObject.SetActive(false);

        }
    }
}
