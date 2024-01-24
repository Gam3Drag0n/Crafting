using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CraftItem : MonoBehaviour
{
    [SerializeField] private Image _craftItemIcon;
    [SerializeField] private Transform _craftItemContainer;
    [SerializeField] private GameObject _craftNeedItemPref;
    [SerializeField] private Button _craftBtn;
    [SerializeField] private GameObject _block;
    [SerializeField] private TextMeshProUGUI _blockLevelText;
    private int _ID;
    public int ID => _ID;


    public void SetCraftItemSprite(Sprite icon) => _craftItemIcon.sprite = icon;

    public void CreateNeedCraftItem(Sprite icon, int count)
    {
        GameObject item = Instantiate(_craftNeedItemPref, _craftItemContainer);
        item.GetComponent<Image>().sprite = icon;
        item.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
    }

    public void LockElement(bool isLock) => _block.SetActive(isLock);
    public void LockElementLevel(int level) => _blockLevelText.text = level + " LEVEL";

    public Button GetButton() { return _craftBtn; }

    public void SetID(int id) => _ID = id;
}
