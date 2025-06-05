// 코루틴 관련 라이브러리 사용
using System.Collections;
// 컬렉션 관련 라이브러리 사용
using System.Collections.Generic;
// 문자열 처리를 위한 StringBuilder 사용
using System.Text;
// Unity 기본 라이브러리 사용
using UnityEngine;
// Unity UI 컴포넌트 사용
using UnityEngine.UI;

// 장착된 아이템 슬롯 클래스 - MonoBehaviour를 상속받음
public class EquippedItemSlot : MonoBehaviour
{
    public Image AddIcon;
    public Image EquippedItemGradeBg;
    public Image EquippedItemIcon;

    private UserItemData m_EquippedItemData;

    public void SetItem(UserItemData userItemData)
    {
        m_EquippedItemData = userItemData;

        AddIcon.gameObject.SetActive(false);
        EquippedItemGradeBg.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(true);

        var itemGrade = (ItemGrade)((m_EquippedItemData.ItemId / 1000) % 10);
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        if (gradeBgTexture != null)
        {
            EquippedItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        StringBuilder sb = new StringBuilder(m_EquippedItemData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if (itemIconTexture != null)
        {
            EquippedItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }

    public void ClearItem()
    {
        m_EquippedItemData = null;

        AddIcon.gameObject.SetActive(true);
        EquippedItemGradeBg.gameObject.SetActive(false);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    public void OnClickEquippedItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        uiData.IsEquipped = true;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}