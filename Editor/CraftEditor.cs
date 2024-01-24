using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(Crafting))]
public class CraftEditor : Editor
{
    private Crafting _crafing;

    private void OnEnable() => _crafing = (Crafting)target;

    public override void OnInspectorGUI()
    {
        _crafing.itemType = (TypeManager.ItemType)EditorGUILayout.EnumPopup("Тип крафта", _crafing.itemType);

        switch (_crafing.itemType)
        {
            case TypeManager.ItemType.ClothesItemType:
                _crafing.clothesType = (TypeManager.ClothesItemType)EditorGUILayout.EnumPopup("Тип одежды", _crafing.clothesType);
                break;

            case TypeManager.ItemType.WeaponKitItemType:
                _crafing.toolType = (TypeManager.WeaponKitItemType)EditorGUILayout.EnumPopup("Тип обвеса", _crafing.toolType);
                break;

            case TypeManager.ItemType.AmmoType:
                _crafing.ammoType = (TypeManager.AmmoType)EditorGUILayout.EnumPopup("Тип патронов", _crafing.ammoType);
                break;
        }

        EditorGUILayout.Space(10);
        _crafing.craftItemIcon = (Sprite)EditorGUILayout.ObjectField("Иконка крафта", _crafing.craftItemIcon, typeof(Sprite), false);
        EditorGUILayout.Space(10);
        _crafing.openLevel = EditorGUILayout.IntField("Уровень открытия крафта", _crafing.openLevel);
        EditorGUILayout.Space(10);
        _crafing.ID = EditorGUILayout.IntField("ID вещи", _crafing.ID);
        _crafing.count = EditorGUILayout.IntField("Количество", _crafing.count);

        if (_crafing.craftNeedItem.Count > 0)
        {
            foreach (CraftNeedItem item in _crafing.craftNeedItem.ToList())
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                item.craftNeedItemIcon = (Sprite)EditorGUILayout.ObjectField("Иконка материала", item.craftNeedItemIcon, typeof(Sprite), false);
                item.craftNeedtItem = (TypeManager.ResourcesType)EditorGUILayout.EnumPopup("Тип материала", item.craftNeedtItem);
                item.craftNeedtItemCount = EditorGUILayout.IntField("Количество материала", item.craftNeedtItemCount);
                EditorGUILayout.Space(5);
                if (GUILayout.Button("Удалить материал крафта")) _crafing.craftNeedItem.Remove(_crafing.craftNeedItem[_crafing.craftNeedItem.Count - 1]);
                EditorGUILayout.EndVertical();
            }
        }
        else EditorGUILayout.LabelField("Нет материала в списке");

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Добавить материал крафта")) _crafing.craftNeedItem.Add(new CraftNeedItem());

        EditorUtility.SetDirty(target);
        EditorSceneManager.MarkAllScenesDirty();
    }
}
