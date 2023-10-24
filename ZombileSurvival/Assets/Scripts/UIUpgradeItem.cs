using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotomchi
{
    public enum UpgradeItemType
    {
        recoveryHp, 
        attackUp,
        ammoUp,
        fastReload,
        grabSoul,
        partner_girl,
        partner_hatMan,
        partner_shirtMan,
        partner_police,
        max
    }



    public class UIUpgradeItem : MonoBehaviour
    {
        public Button thisButton = null;
        public Manager manager = null;
        public UpgradeItemType upradeItemType = UpgradeItemType.recoveryHp;
        public Image icon = null;
        public Text title = null, explain = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpgradeItemAction()
        {
            if (manager)
                manager.SelectUpgrade(upradeItemType);
        }


        public void SetUpgradeInfo(UpgradeItemType uType, int level)
        {
            upradeItemType = uType;

            switch (upradeItemType)
            {
                case UpgradeItemType.recoveryHp:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_05");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "체력 회복", level);
                        if (explain)
                            explain.text = "현재 체력을 즉시 회복하고 최대 체력을 +10만큼 증가 시킵니다.";
                    }
                    break;
                case UpgradeItemType.attackUp:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_07");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "공격력 강화", level);
                        if (explain)
                            explain.text = "총알 공격력이 +1 상승합니다.";
                    }
                    break;
                case UpgradeItemType.ammoUp:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_06");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "탄창 용량 확장", level);
                        if (explain)
                            explain.text = "탄창에 들어가는 총알의 용량이 +5 증가합니다.";
                    }
                    break;
                case UpgradeItemType.fastReload:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_09");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "재장전 시간 단축", level);
                        if (explain)
                            explain.text = "총알을 재장전 하는 시간이 단축됩니다.";
                    }
                    break;
                case UpgradeItemType.grabSoul:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_08");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "경험치 획득 확장", level);
                        if (explain)
                            explain.text = "경험치를 획득할 수 있는 소울의 획득 거리가 확장됩니다.";
                    }
                    break;
                case UpgradeItemType.partner_girl:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_02");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "식칼의 여자", level);
                        if (explain)
                            explain.text = "손에 든 식칼로 좀비를 한방에 죽일 수 있는 여자를 영입합니다.";
                    }
                    break;
                case UpgradeItemType.partner_hatMan:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_01");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "야구 방망이 남자", level);
                        if (explain)
                            explain.text = "전직 야구선수 였던 남자를 영입합니다. \n손에 든 방망이로 다수의 좀비에게 공격합니다.";
                    }
                    break;
                case UpgradeItemType.partner_shirtMan:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_03");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "무서운 폭탄마", level);
                        if (explain)
                            explain.text = "수류탄을 던지고 다니는 남자를 영입합니다.";
                    }
                    break;
                case UpgradeItemType.partner_police:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_04");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "어리버리 경찰관", level);
                        if (explain)
                            explain.text = "총을 가진 경찰관이지만 크게 도움은 되지 않습니다. 파트너로 영입합니다.";
                    }
                    break;
            }
        }

    }
}
