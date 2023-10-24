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
                            title.text = string.Format("{0} Lv. {1}", "ü�� ȸ��", level);
                        if (explain)
                            explain.text = "���� ü���� ��� ȸ���ϰ� �ִ� ü���� +10��ŭ ���� ��ŵ�ϴ�.";
                    }
                    break;
                case UpgradeItemType.attackUp:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_07");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "���ݷ� ��ȭ", level);
                        if (explain)
                            explain.text = "�Ѿ� ���ݷ��� +1 ����մϴ�.";
                    }
                    break;
                case UpgradeItemType.ammoUp:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_06");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "źâ �뷮 Ȯ��", level);
                        if (explain)
                            explain.text = "źâ�� ���� �Ѿ��� �뷮�� +5 �����մϴ�.";
                    }
                    break;
                case UpgradeItemType.fastReload:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_09");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "������ �ð� ����", level);
                        if (explain)
                            explain.text = "�Ѿ��� ������ �ϴ� �ð��� ����˴ϴ�.";
                    }
                    break;
                case UpgradeItemType.grabSoul:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_08");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "����ġ ȹ�� Ȯ��", level);
                        if (explain)
                            explain.text = "����ġ�� ȹ���� �� �ִ� �ҿ��� ȹ�� �Ÿ��� Ȯ��˴ϴ�.";
                    }
                    break;
                case UpgradeItemType.partner_girl:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_02");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "��Į�� ����", level);
                        if (explain)
                            explain.text = "�տ� �� ��Į�� ���� �ѹ濡 ���� �� �ִ� ���ڸ� �����մϴ�.";
                    }
                    break;
                case UpgradeItemType.partner_hatMan:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_01");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "�߱� ����� ����", level);
                        if (explain)
                            explain.text = "���� �߱����� ���� ���ڸ� �����մϴ�. \n�տ� �� ����̷� �ټ��� ���񿡰� �����մϴ�.";
                    }
                    break;
                case UpgradeItemType.partner_shirtMan:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_03");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "������ ��ź��", level);
                        if (explain)
                            explain.text = "����ź�� ������ �ٴϴ� ���ڸ� �����մϴ�.";
                    }
                    break;
                case UpgradeItemType.partner_police:
                    {
                        if (icon)
                            icon.sprite = Resources.Load<Sprite>("Sprites/skill_04");

                        if (title)
                            title.text = string.Format("{0} Lv. {1}", "����� ������", level);
                        if (explain)
                            explain.text = "���� ���� ������������ ũ�� ������ ���� �ʽ��ϴ�. ��Ʈ�ʷ� �����մϴ�.";
                    }
                    break;
            }
        }

    }
}
