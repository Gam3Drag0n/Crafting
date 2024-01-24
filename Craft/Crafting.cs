using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft Item")]
public class Crafting : ScriptableObject
{
    public TypeManager.ItemType itemType;
    [Space]
    public TypeManager.ClothesItemType clothesType;
    public TypeManager.WeaponKitItemType toolType;
    public TypeManager.AmmoType ammoType;
    [Space]
    public Sprite craftItemIcon;
    public List<CraftNeedItem> craftNeedItem;
    [Space]
    public int openLevel;
    [Space]
    public int ID;
    public int count;
}

[System.Serializable]
public class CraftNeedItem
{
    [Space]
    public Sprite craftNeedItemIcon;
    public TypeManager.ResourcesType craftNeedtItem;
    public int craftNeedtItemCount;
}
