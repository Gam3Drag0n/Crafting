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
        _crafing.itemType = (TypeManager.ItemType)EditorGUILayout.EnumPopup("��� ������", _crafing.itemType);

        switch (_crafing.itemType)
        {
            case TypeManager.ItemType.ClothesItemType:
                _crafing.clothesType = (TypeManager.ClothesItemType)EditorGUILayout.EnumPopup("��� ������", _crafing.clothesType);
                break;

            case TypeManager.ItemType.WeaponKitItemType:
                _crafing.toolType = (TypeManager.WeaponKitItemType)EditorGUILayout.EnumPopup("��� ������", _crafing.toolType);
                break;

            case TypeManager.ItemType.AmmoType:
                _crafing.ammoType = (TypeManager.AmmoType)EditorGUILayout.EnumPopup("��� ��������", _crafing.ammoType);
                break;
        }

        EditorGUILayout.Space(10);
        _crafing.craftItemIcon = (Sprite)EditorGUILayout.ObjectField("������ ������", _crafing.craftItemIcon, typeof(Sprite), false);
        EditorGUILayout.Space(10);
        _crafing.openLevel = EditorGUILayout.IntField("������� �������� ������", _crafing.openLevel);
        EditorGUILayout.Space(10);
        _crafing.ID = EditorGUILayout.IntField("ID ����", _crafing.ID);
        _crafing.count = EditorGUILayout.IntField("����������", _crafing.count);

        if (_crafing.craftNeedItem.Count > 0)
        {
            foreach (CraftNeedItem item in _crafing.craftNeedItem.ToList())
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                item.craftNeedItemIcon = (Sprite)EditorGUILayout.ObjectField("������ ���������", item.craftNeedItemIcon, typeof(Sprite), false);
                item.craftNeedtItem = (TypeManager.ResourcesType)EditorGUILayout.EnumPopup("��� ���������", item.craftNeedtItem);
                item.craftNeedtItemCount = EditorGUILayout.IntField("���������� ���������", item.craftNeedtItemCount);
                EditorGUILayout.Space(5);
                if (GUILayout.Button("������� �������� ������")) _crafing.craftNeedItem.Remove(_crafing.craftNeedItem[_crafing.craftNeedItem.Count - 1]);
                EditorGUILayout.EndVertical();
            }
        }
        else EditorGUILayout.LabelField("��� ��������� � ������");

        EditorGUILayout.Space(10);
        if (GUILayout.Button("�������� �������� ������")) _crafing.craftNeedItem.Add(new CraftNeedItem());

        EditorUtility.SetDirty(target);
        EditorSceneManager.MarkAllScenesDirty();
    }
}
