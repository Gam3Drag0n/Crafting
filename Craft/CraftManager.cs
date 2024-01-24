using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ResourcesUI _resourcesUI;
    [SerializeField] private WindowSizeFitter _windowSizeFitter;
    [SerializeField] private ItemsManager _itemsManager;
    [Space]
    [Header("Item prefab")]
    [SerializeField] private CraftItem _itemPref;
    [Space]
    [Header("UI holders")]
    [SerializeField] private Transform _clothesContainer;
    [SerializeField] private Transform _toolsContainer;
    [SerializeField] private Transform _ammoContainer;
    [Space]
    [Header("Craft info")]
    [SerializeField] private Crafting[] _clothesCrafts;
    [SerializeField] private Crafting[] _toolsCrafts;
    [SerializeField] private Crafting[] _ammoCrafts;

    private int _level;

    private Button[] _clothsBtns;
    private Button[] _toolsBtns;
    private Button[] _ammoBtns;

    private Animator[] _clothsBtnsAnim;
    private Animator[] _toolsBtnsAnim;
    private Animator[] _ammoBtnsAnim;



    private void Awake()
    {
        _level = PlayerPrefs.GetInt("Level", 1);
        InitArrays();
        InitUI();
    }

    private void Start() => InitButtons();



    #region Initialisation
    private void InitArrays()
    {
        _clothsBtns = new Button[_clothesCrafts.Length];
        _toolsBtns = new Button[_toolsCrafts.Length];
        _ammoBtns = new Button[_ammoCrafts.Length];

        _clothsBtnsAnim = new Animator[_clothesCrafts.Length];
        _toolsBtnsAnim = new Animator[_toolsCrafts.Length];
        _ammoBtnsAnim = new Animator[_ammoCrafts.Length];
    }

    private void InitUI()
    {
        //creating UI elements of clothes
        for (int i = 0; i < _clothesCrafts.Length; i++)
        {
            CraftItem item = Instantiate(_itemPref, _clothesContainer);

            if (_level < _clothesCrafts[i].openLevel)
            {
                item.LockElement(true);
                item.LockElementLevel(_clothesCrafts[i].openLevel);

            }
            else item.LockElement(false);

            item.SetID(_clothesCrafts[i].ID);
            item.SetCraftItemSprite(_clothesCrafts[i].craftItemIcon);
            _clothsBtns[i] = item.GetButton();
            _clothsBtnsAnim[i] = _clothsBtns[i].GetComponent<Animator>();

            for (int k = 0; k < _clothesCrafts[i].craftNeedItem.Count; k++)
                item.CreateNeedCraftItem(_clothesCrafts[i].craftNeedItem[k].craftNeedItemIcon, _clothesCrafts[i].craftNeedItem[k].craftNeedtItemCount);
        }

        //creating UI elements of tools
        for (int i = 0; i < _toolsCrafts.Length; i++)
        {
            CraftItem item = Instantiate(_itemPref, _toolsContainer);

            if (_level < _toolsCrafts[i].openLevel)
            {
                item.LockElement(true);
                item.LockElementLevel(_toolsCrafts[i].openLevel);
            }
            else item.LockElement(false);

            item.SetID(_toolsCrafts[i].ID);
            item.SetCraftItemSprite(_toolsCrafts[i].craftItemIcon);
            _toolsBtns[i] = item.GetButton();
            _toolsBtnsAnim[i] = _toolsBtns[i].GetComponent<Animator>();

            for (int k = 0; k < _toolsCrafts[i].craftNeedItem.Count; k++)
                item.CreateNeedCraftItem(_toolsCrafts[i].craftNeedItem[k].craftNeedItemIcon, _toolsCrafts[i].craftNeedItem[k].craftNeedtItemCount);
        }

        //creating UI elements of ammo
        for (int i = 0; i < _ammoCrafts.Length; i++)
        {
            CraftItem item = Instantiate(_itemPref, _ammoContainer);

            if (_level < _ammoCrafts[i].openLevel)
            {
                item.LockElement(true);
                item.LockElementLevel(_ammoCrafts[i].openLevel);

            }
            else item.LockElement(false);

            item.SetID(_ammoCrafts[i].ID);
            item.SetCraftItemSprite(_ammoCrafts[i].craftItemIcon);
            _ammoBtns[i] = item.GetButton();
            _ammoBtnsAnim[i] = _ammoBtns[i].GetComponent<Animator>();

            for (int k = 0; k < _ammoCrafts[i].craftNeedItem.Count; k++)
                item.CreateNeedCraftItem(_ammoCrafts[i].craftNeedItem[k].craftNeedItemIcon, _ammoCrafts[i].craftNeedItem[k].craftNeedtItemCount);
        }
    }

    private void InitButtons()
    {
        for (int i = 0; i < _clothsBtns.Length; i++)
        {
            Button button = _clothsBtns[i];
            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CraftClothes(index));
        }

        for (int i = 0; i < _toolsBtns.Length; i++)
        {
            Button button = _toolsBtns[i];
            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CraftTools(index));
        }

        for (int i = 0; i < _ammoBtns.Length; i++)
        {
            Button button = _ammoBtns[i];
            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CraftAmmo(index));
        }
    }
    #endregion

    #region Craft
    public void CraftClothes(int id)
    {
        _clothsBtnsAnim[id].SetTrigger("Pressed");

        //load resources data
        ResourcesData resourcesData = ResourcesSave.LoadData();

        TypeManager.ClothesItemType clothesItemType = _clothesCrafts[id].clothesType;
        int clothesID = _clothesCrafts[id].ID;

        int itemsCount = _clothesCrafts[id].craftNeedItem.Count;
        int[] resNeedCount = new int[itemsCount];
        int[] resCurrentCount = resourcesData.Count;
        TypeManager.ResourcesType[] resTypes = new TypeManager.ResourcesType[itemsCount];
        bool[] isResourcesEnough = new bool[itemsCount];

        //check resources
        for (int i = 0; i < itemsCount; i++)
        {
            resNeedCount[i] = _clothesCrafts[id].craftNeedItem[i].craftNeedtItemCount;
            resTypes[i] = _clothesCrafts[id].craftNeedItem[i].craftNeedtItem;

            if (resCurrentCount[(int)_clothesCrafts[id].craftNeedItem[i].craftNeedtItem] >= resNeedCount[i])
                isResourcesEnough[i] = true;
        }

        //craft item
        if (IsResourcesEnough(isResourcesEnough))
        {
            for (int i = 0; i < _clothesCrafts[id].count; i++)
            {
                //load clothes data
                ClothesData data = ClothesSave.LoadData();

                //add and save clothes data
                if (data != null)
                {
                    int clothesLastID = 0;

                    if (data.ClothesID.Count > 0)
                    {
                        clothesLastID = data.ClothesID[data.ClothesID.Count - 1];
                        clothesLastID++;
                    }

                    data.ClothesType.Add(clothesItemType);
                    data.ID.Add(clothesID);
                    data.ClothesID.Add(clothesLastID);
                    data.IsPlaced.Add(false);
                    ClothesSave.SaveData(data);

                    //create item in inventory
                    _itemsManager.InstantiateClothesItem(clothesItemType, clothesID, data.ClothesType.Count - 1, clothesLastID);
                }
                else
                {
                    ClothesData newData = new ClothesData();
                    newData.ClothesType = new List<TypeManager.ClothesItemType> { clothesItemType };
                    newData.ID = new List<int> { clothesID };
                    newData.ClothesID = new List<int> { 0 };
                    newData.IsPlaced = new List<bool> { false };
                    ClothesSave.SaveData(newData);

                    _itemsManager.InstantiateClothesItem(clothesItemType, clothesID, newData.ClothesType.Count - 1, 0);
                }
            }

            for (int i = 0; i < resNeedCount.Length; i++)
                resCurrentCount[(int)_clothesCrafts[id].craftNeedItem[i].craftNeedtItem] -= resNeedCount[i];

            //save data
            ResourcesSave.SaveData(resCurrentCount);
            _resourcesUI.UpdateResourcesCount(resCurrentCount);
            _windowSizeFitter.ChangeInventorySize();

            Debug.Log("Created " + _clothesCrafts[id].count + " " + clothesItemType + " " + clothesID);
        }
        else Debug.Log("Not enough materials!");
    }

    public void CraftTools(int id)
    {
        _toolsBtnsAnim[id].SetTrigger("Pressed");

        //load resources data
        ResourcesData resourcesData = ResourcesSave.LoadData();

        TypeManager.WeaponKitItemType toolType = _toolsCrafts[id].toolType;

        int itemsCount = _toolsCrafts[id].craftNeedItem.Count;
        int[] resNeedCount = new int[itemsCount];
        int[] resCurrentCount = resourcesData.Count;
        TypeManager.ResourcesType[] resTypes = new TypeManager.ResourcesType[itemsCount];
        bool[] isResourcesEnough = new bool[itemsCount];

        for (int i = 0; i < itemsCount; i++)
        {
            resNeedCount[i] = _toolsCrafts[id].craftNeedItem[i].craftNeedtItemCount;
            resTypes[i] = _toolsCrafts[id].craftNeedItem[i].craftNeedtItem;

            if (resCurrentCount[(int)_toolsCrafts[id].craftNeedItem[i].craftNeedtItem] >= resNeedCount[i])
                isResourcesEnough[i] = true;
        }

        //craft item
        if (IsResourcesEnough(isResourcesEnough))
        {
            for (int i = 0; i < _toolsCrafts[id].count; i++)
            {
                //load kit data
                WeaponKitData data = WeaponKitSave.LoadData();

                //add and save kit data
                if (data != null)
                {
                    data.KitType.Add(toolType);
                    data.ID.Add(_toolsCrafts[id].ID);
                    data.IsPlaced.Add(false);
                    data.KitID.Add(data.KitID.Count);
                    WeaponKitSave.SaveData(data);

                    //create item in inventory
                    _itemsManager.InstantiateWeaponKitItem(toolType, _toolsCrafts[id].ID, data.KitType.Count - 1);
                }
                else
                {
                    WeaponKitData newData = new WeaponKitData();

                    newData.KitType.Add(toolType);
                    newData.ID.Add(_toolsCrafts[id].ID);
                    newData.IsPlaced.Add(false);
                    newData.KitID.Add(0);

                    WeaponKitSave.SaveData(newData);

                    _itemsManager.InstantiateWeaponKitItem(toolType, _toolsCrafts[id].ID, 0);
                }
            }

            for (int i = 0; i < resNeedCount.Length; i++)
                resCurrentCount[(int)_toolsCrafts[id].craftNeedItem[i].craftNeedtItem] -= resNeedCount[i];

            //save data
            ResourcesSave.SaveData(resCurrentCount);
            _resourcesUI.UpdateResourcesCount(resCurrentCount);
            _windowSizeFitter.ChangeInventorySize();
            
            Debug.Log("Created " + _toolsCrafts[id].count + " " + toolType + " " + _toolsCrafts[id].ID);
        }
        else Debug.Log("Not enough materials!");
    }

    public void CraftAmmo(int id)
    {
        _ammoBtnsAnim[id].SetTrigger("Pressed");
        
        //load resources data
        ResourcesData resourcesData = ResourcesSave.LoadData();

        TypeManager.AmmoType ammoType = _ammoCrafts[id].ammoType;

        int itemsCount = _ammoCrafts[id].craftNeedItem.Count;
        int[] resNeedCount = new int[itemsCount];
        int[] resCurrentCount = resourcesData.Count;
        TypeManager.ResourcesType[] resTypes = new TypeManager.ResourcesType[itemsCount];
        bool[] isResourcesEnough = new bool[itemsCount];

        for (int i = 0; i < itemsCount; i++)
        {
            resNeedCount[i] = _ammoCrafts[id].craftNeedItem[i].craftNeedtItemCount;
            resTypes[i] = _ammoCrafts[id].craftNeedItem[i].craftNeedtItem;

            if (resCurrentCount[(int)_ammoCrafts[id].craftNeedItem[i].craftNeedtItem] >= resNeedCount[i])
                isResourcesEnough[i] = true;
        }
        
        //craft item
        if (IsResourcesEnough(isResourcesEnough))
        {
            for (int i = 0; i < _ammoCrafts[id].count; i++)
            {
                //load ammo data
                AmmoData data = AmmoSave.LoadData();

                //add and save ammo data
                if (data != null)
                {
                    data.Count[(int)ammoType]++;
                    AmmoSave.SaveData(data.Count);
                    _resourcesUI.UpdateAmmoCount(data.Count);
                }
                else
                {
                    int[] ammoCount = new int[System.Enum.GetValues(typeof(TypeManager.AmmoType)).Length];
                    ammoCount[(int)ammoType]++;
                    AmmoSave.SaveData(ammoCount);
                    _resourcesUI.UpdateAmmoCount(ammoCount);
                }
            }

            for (int i = 0; i < resNeedCount.Length; i++)
                resCurrentCount[(int)_ammoCrafts[id].craftNeedItem[i].craftNeedtItem] -= resNeedCount[i];
            
            //save data
            ResourcesSave.SaveData(resCurrentCount);
            _resourcesUI.UpdateResourcesCount(resCurrentCount);

            Debug.Log("Created " + _ammoCrafts[id].count  + " " + ammoType + " " + _toolsCrafts[id].ID);
        }
        else Debug.Log("Not enough materials!");
    }
    #endregion


    private bool IsResourcesEnough(bool[] isResourcesEnough)
    {
        for (int i = 0; i < isResourcesEnough.Length; ++i)
        {
            if (!isResourcesEnough[i])
                return false;
        }

        return true;
    }
}
