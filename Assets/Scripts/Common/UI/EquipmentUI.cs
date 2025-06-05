using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ��� UI�� ������ �����͸� ��� Ŭ����
public class EquipmentUIData : BaseUIData
{
    // �������� ���� �ø��� ��ȣ
    public long SerialNumber;
    // ������ ID
    public int ItemId;
    // ���� ���� ����
    public bool IsEquipped;
}

// ��� ����/���� UI�� ����ϴ� Ŭ����
public class EquipmentUI : BaseUI
{
    // ������ ��� ��� �̹���
    public Image ItemGradeBg;
    // ������ ������ �̹���
    public Image ItemIcon;
    // ������ ��� �ؽ�Ʈ
    public TextMeshProUGUI ItemGradeTxt;
    // ������ �̸� �ؽ�Ʈ
    public TextMeshProUGUI ItemNameTxt;
    // ���ݷ� ��ġ �ؽ�Ʈ
    public TextMeshProUGUI AttackPowerAmountTxt;
    // ���� ��ġ �ؽ�Ʈ
    public TextMeshProUGUI DefenseAmountTxt;
    // ����/���� ��ư �ؽ�Ʈ
    public TextMeshProUGUI EquipBtnTxt;

    // ��� UI �����͸� �����ϴ� ��� ����
    private EquipmentUIData m_EquipmentUIData;

    // UI ������ �����ϴ� �޼���
    public override void SetInfo(BaseUIData uiData)
    {
        // �θ� Ŭ������ SetInfo ȣ��
        base.SetInfo(uiData);

        // ���޹��� �����͸� EquipmentUIData�� ĳ����
        m_EquipmentUIData = uiData as EquipmentUIData;
        // ĳ������ ������ ��� ���� �α� ��� �� ����
        if (m_EquipmentUIData == null)
        {
            Logger.LogError("m_EquipmentUIData is invalid");
            return;
        }

        // ������ ID�� ������ ������ ��������
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);
        // ������ �����Ͱ� ������ ���� �α� ��� �� ����
        if (itemData == null)
        {
            Logger.LogError($"Item data is invalid. ItemId:{m_EquipmentUIData.ItemId}");
            return;
        }

        // ������ ID���� ��� ���� ���� (õ�� �ڸ� ����)
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        // ��޿� �ش��ϴ� ��� �ؽ�ó �ε�
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        // �ؽ�ó�� �����ϸ� ��������Ʈ�� ��ȯ�Ͽ� ��濡 ����
        if (gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        // ��� �ؽ�Ʈ ����
        ItemGradeTxt.text = itemGrade.ToString();
        // ��޺� ���� �ڵ带 ������ ����
        var hexColor = string.Empty;
        // ��޿� ���� ���� ����
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "#EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }

        // ���� ��ȯ�� ����
        Color color;
        // 16���� ���� �ڵ带 Color�� ��ȯ�Ͽ� ����
        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        // ������ ID�� ���ڿ��� ��ȯ�Ͽ� StringBuilder�� ����
        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        // �� ��° �ڸ� ���ڸ� '1'�� ���� (������ �̸� ��Ģ)
        sb[1] = '1';
        // ����� ���ڿ��� ������ �̸����� ���
        var itemIconName = sb.ToString();

        // ������ �ؽ�ó �ε�
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        // �ؽ�ó�� �����ϸ� ��������Ʈ�� ��ȯ�Ͽ� �����ܿ� ����
        if (itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        // ������ �̸� ����
        ItemNameTxt.text = itemData.ItemName;
        // ���ݷ� ��ġ ���� (+ ��ȣ�� �Բ� ǥ��)
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}";
        // ���� ��ġ ���� (+ ��ȣ�� �Բ� ǥ��)
        DefenseAmountTxt.text = $"+{itemData.Defense}";
        // ���� ���¿� ���� ��ư �ؽ�Ʈ ����
        EquipBtnTxt.text = m_EquipmentUIData.IsEquipped ? "Unequip" : "Equip";
    }

    // ����/���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnClickEquipBtn()
    {
        // ����� �κ��丮 ������ ��������
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        // �κ��丮 �����Ͱ� ������ �α� ��� �� ����
        if (userInventoryData == null)
        {
            Logger.Log("UserInventoryData does not exist.");
            return;
        }

        // ���� ���� ���¿� ���� ����/���� ó��
        if (m_EquipmentUIData.IsEquipped)
        {
            // ������ ����
            userInventoryData.UnequipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }
        else
        {
            // ������ ����
            userInventoryData.EquipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }

        // ����� ������ ����
        userInventoryData.SaveData();

        // Ȱ��ȭ�� �κ��丮 UI ��������
        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;
        // �κ��丮 UI�� �����ϸ� ���� ������Ʈ
        if (inventoryUI != null)
        {
            // ���� ���¿� ���� �κ��丮 UI ������Ʈ
            if (m_EquipmentUIData.IsEquipped)
            {
                // ������ ������ UI ������Ʈ
                inventoryUI.OnUnequipItem(m_EquipmentUIData.ItemId);
            }
            else
            {
                // ������ ������ UI ������Ʈ
                inventoryUI.OnEquipItem(m_EquipmentUIData.ItemId);
            }
        }

        // ���� UI �ݱ�
        CloseUI();
    }
}