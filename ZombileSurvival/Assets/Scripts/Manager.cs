using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
namespace Dotomchi
{

    public enum GameState
    {
        ready,
        playing,
        gameOver,
    }

    public class Manager : MonoBehaviour
    {

        int time = 0;

        public GameState gameState = GameState.ready;

        public List<GameObject> genPosList = null;
        public GameObject enemyPrefeb = null, damagePrefeb = null;
        public Canvas canvas = null;

        public Image expBar = null;

        public Player player = null;

        public int exp, maxExp, killCountValue = 0;
        public int scoreValue = 0;

        public Text ammoCount = null, ammoMaxCount = null, battleTime = null, levelText = null;
        public Text readyCount = null, killCount = null;
        public Text score = null, resultScore = null;

        public Image reloadGauge = null;

        public GameObject levelUpPanel = null, gameOverPanel = null;

        public GameObject startPos = null;

        public List<Enemy> enemyList = new List<Enemy>();

        public UIUpgradeItem[] upgradeItems = new UIUpgradeItem[3];
        public int[] upgradeItemLevel = new int[(int)UpgradeItemType.max];
       
        

        // Start is called before the first frame update
        void Start()
        {
            exp = 0;
            maxExp = 3;
            StartCoroutine(MobRegenEvent());

            if (expBar)
                expBar.fillAmount = 0.0f;

            gameState = GameState.ready;


            if (levelUpPanel)
                levelUpPanel.SetActive(false);

            if (ammoMaxCount)
                ammoMaxCount.text = string.Format("/{0}", player.maxAmmo);

            InvokeRepeating("OneSecendEvent", 0, 1);



            ObjectPoolManager.instance.ReservePool("Prefabs/Bullet", 100);
            
            ObjectPoolManager.instance.ReservePool("Prefabs/DamageNum", 100, true);

            ObjectPoolManager.instance.ReservePool("Prefabs/soul", 100);
            ObjectPoolManager.instance.ReservePool("Prefabs/zombie", 100);
            ObjectPoolManager.instance.ReservePool("Prefabs/Grenade", 20);

            ObjectPoolManager.instance.ReservePool("Prefabs/girl", 5);
            ObjectPoolManager.instance.ReservePool("Prefabs/hatMan", 5);
            ObjectPoolManager.instance.ReservePool("Prefabs/shirtMan", 5);
            ObjectPoolManager.instance.ReservePool("Prefabs/police", 5);

            StartCoroutine(GameStart());
        }
        private void Update()
        {

            if (gameState != GameState.playing)
                return;


            if (player && player.isAlive == false)
            {
                gameState = GameState.gameOver;
                if (gameOverPanel)
                    gameOverPanel.SetActive(true);



                int bestScoreValue = PlayerPrefs.GetInt("bestScore", 0);
                if (bestScoreValue < scoreValue)
                {
                    PlayerPrefs.SetInt("bestScore", scoreValue);
                    bestScoreValue = scoreValue;
                }

                if (resultScore)
                    resultScore.text = string.Format("현재 스코어: {0}\n\n최고 스코어: {1}", scoreValue, bestScoreValue);


                return;
            }

            if (exp >= maxExp)
            {
                exp = 0;

                player.level++;

                scoreValue += (player.level * 100);

                maxExp = 3 + (player.level * 2);
                LevelUpEvent();
            }

            if (score)
                score.text = string.Format("Score: {0}", scoreValue);

            if (expBar)
                expBar.fillAmount = (float)exp / (float)maxExp;

            if (killCount)
                killCount.text = string.Format("Kill Count : {0}", killCountValue);

            if (player)
            {
                if (ammoCount)
                    ammoCount.text = player.ammo.ToString();
                if (reloadGauge)
                    reloadGauge.fillAmount = player.reloadTime;
                if (levelText)
                    levelText.text = string.Format("Lv. {0}", player.level);
            }
        }
        IEnumerator GameStart()
        {
            int count = 5;

            while (count > 0)
            {
                
                if (readyCount)
                    readyCount.text = count.ToString();

                count--;
                yield return new WaitForSeconds(1.0f);

            }

            if (readyCount)
                readyCount.text = "Battle Start!!";

            yield return new WaitForSeconds(1.0f);
            if (readyCount)
                readyCount.gameObject.SetActive(false);

            gameState = GameState.playing;


        }
        public void GameReStart()
        {

            enemyList.Clear();


            ObjectPoolManager.instance.EnableAllType(false);

            //그동안 올린 업그레이드 레벨의 초기화
            for (int i = 0; i < (int)UpgradeItemType.max; i++)
            {
                upgradeItemLevel[i] = 0;
            }

            if (player)
            {
                player.ResetParam();
                player.transform.position = startPos.transform.position;
            }

            exp = 0;
            maxExp = 3;
            killCountValue = 0;
            scoreValue = 0;

            time = 0;

            if (gameOverPanel)
                gameOverPanel.SetActive(false);

            gameState = GameState.playing;
        }


        void LevelUpEvent()
        {
            Time.timeScale = 0.0f;
            if (levelUpPanel)
                levelUpPanel.SetActive(true);


            List<UpgradeItemType> currentUpgradeTypes = new List<UpgradeItemType>();

            while (currentUpgradeTypes.Count < 3)
            {
                UpgradeItemType addType = (UpgradeItemType)Random.Range(0, (int)UpgradeItemType.max);
                if(currentUpgradeTypes.Contains(addType) == false)
                    currentUpgradeTypes.Add(addType);
            }

            for (int i = 0; i < upgradeItems.Length; i++)
            {
                upgradeItems[i].thisButton.interactable = false;
                int index = (int)currentUpgradeTypes[i];
                upgradeItems[i].SetUpgradeInfo(currentUpgradeTypes[i], upgradeItemLevel[index] + 1);
            }

            StartCoroutine(ActiveButtons());

        }
        IEnumerator ActiveButtons()
        {
            yield return new WaitForSecondsRealtime(1.0f);
            for (int i = 0; i < upgradeItems.Length; i++)
            {
                upgradeItems[i].thisButton.interactable = true;
            }
        }


        public int GetUpgradeLevel(UpgradeItemType type)
        {
            return upgradeItemLevel[(int)type];
        }

        public void SelectUpgrade(UpgradeItemType uType)
        {
            Time.timeScale = 1.0f;
            if (levelUpPanel)
                levelUpPanel.SetActive(false);

            upgradeItemLevel[(int)uType]++;

            int currentLevel = upgradeItemLevel[(int)uType];

            switch (uType)
            {
                case UpgradeItemType.recoveryHp:
                    {
                        if (player)
                        {
                            player.maxHp = 20 + (currentLevel * 5);
                            player.hp = player.maxHp;
                        }

                    }
                    break;
                case UpgradeItemType.attackUp:
                    {
                        if (player)
                        {
                            player.attackPoint = 1+ currentLevel;
                        }

                    }
                    break;
                case UpgradeItemType.ammoUp:
                    {
                        if (player)
                        {
                            player.maxAmmo = 30 + (currentLevel * 5);
                        }
                        if (ammoMaxCount)
                            ammoMaxCount.text = string.Format("/{0}", player.maxAmmo);
                    }
                    break;
                case UpgradeItemType.grabSoul:
                    {
                        if (player)
                        {
                            player.grabSoulRange = 1.0f + currentLevel;
                        }
                    }
                    break;
                case UpgradeItemType.partner_girl:
                case UpgradeItemType.partner_hatMan:
                case UpgradeItemType.partner_shirtMan:
                case UpgradeItemType.partner_police:
                    {
                        SummonPartner(uType);
                    }
                    break;
            }
        }

        public void SummonPartner(UpgradeItemType type)
        {
            string summonUnitName = "";
            switch (type)
            {
                case UpgradeItemType.partner_girl: summonUnitName = "Prefabs/girl"; break;
                case UpgradeItemType.partner_hatMan: summonUnitName = "Prefabs/hatMan"; break;
                case UpgradeItemType.partner_shirtMan: summonUnitName = "Prefabs/shirtMan"; break;
                case UpgradeItemType.partner_police: summonUnitName = "Prefabs/police"; break;
            }
            GameObject summonPartner = ObjectPoolManager.instance.GetObject(summonUnitName);

            Vector3 genPos = Player.GetDegreesDistVector3(player.transform.position, Random.Range(0, 360), 5.0f);
            summonPartner.transform.position = genPos;

            Partner script = summonPartner.GetComponent<Partner>();
            if (script)
            {
                script.player = player;
                script.manager = this;
            }


        }

        void OneSecendEvent()
        {

            if (gameState != GameState.playing)
                return;

            time++;
            if (battleTime)
                battleTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time / 3600, (time % 3600) / 60, time % 60);



        }

        public void ShowDamagePrefeb(GameObject obj, int dmg, Color color)
        {
            //GameObject damage = (GameObject)Instantiate(damagePrefeb);
            //damage.transform.SetParent(canvas.transform);


            

            GameObject damage = ObjectPoolManager.instance.GetObject("Prefabs/DamageNum", true, true);

            UIDamageNum script = damage.GetComponent<UIDamageNum>();
            if (script)
            {
                script.ShowDamage(obj, dmg, color);
            }
        }

        IEnumerator MobRegenEvent()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);



                if (gameState != GameState.playing)
                    continue;

                if (enemyList.Count >= 100)
                    continue;


                Vector3 genPos = Player.GetDegreesDistVector3(player.transform.position, Random.Range(0, 360), 20);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(genPos, out hit, 2.0f, NavMesh.AllAreas))
                {
                    //int randomValue = Random.Range((int)EnemyType.zombie, (int)EnemyType.max);
                    //string genName = string.Format("Prefabs/{0}", ((EnemyType)randomValue).ToString());
                    GameObject enemyGen = ObjectPoolManager.instance.GetObject("Prefabs/zombie"); // (GameObject)Instantiate(enemyPrefeb);
                    

                    enemyGen.transform.position = hit.position;

                    Enemy enemyScript = enemyGen.GetComponent<Enemy>();
                    if (enemyScript)
                    {
                        enemyScript.manager = this;
                        enemyScript.player = player;
                        enemyScript.SetLevel(player.level);

                        enemyList.Add(enemyScript);
                    }
                }
                
            }
        }

    }
}
