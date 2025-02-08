using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Core;
using Core.Cards;
public class BaseCardEditorWindow : EditorWindow
{
    // Список всех экземпляров BaseCard
    private List<BaseCard> baseCards = new List<BaseCard>();

    // Текущий выбранный или создаваемый BaseCard
    private BaseCard selectedCard = null;

    // Статус режима: создание или редактирование
    private bool isCreating = false;

    // Поля для создания новой карты
    private string newCardName = "NewCard";
    private string newCardPath = "Assets/BaseCards";

    // SerializedObject для редактирования свойств
    private SerializedObject serializedSelectedCard;

    // Состояние скроллбаров
    private Vector2 leftScrollPos;
    private Vector2 rightScrollPos;

    // Кэшируемые типы для работы с абстрактными типами в списках
    private Dictionary<string, List<Type>> derivedTypesCache = new Dictionary<string, List<Type>>();

    // Добавить возможность обновления при активности окна
    [MenuItem("Window/Base Card Editor")]
    public static void ShowWindow()
    {
        GetWindow<BaseCardEditorWindow>("Base Card Editor");
    }

    private void OnEnable()
    {
        RefreshBaseCards();
    }

    private void RefreshBaseCards()
    {
        baseCards = AssetDatabase.FindAssets("t:BaseCard")
                                .Select(asset => AssetDatabase.LoadAssetAtPath<BaseCard>(AssetDatabase.GUIDToAssetPath(asset)))
                                .ToList();
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        // Левая панель
        DrawLeftPanel();

        // Разделительная линия
        GUILayout.Box("", GUILayout.Width(1), GUILayout.Height(position.height));

        // Правая панель
        DrawRightPanel();

        GUILayout.EndHorizontal();
    }

    private void DrawLeftPanel()
    {
        GUILayout.BeginVertical(GUILayout.Width(200), GUILayout.ExpandHeight(true));

        // Скролл для левой панели
        leftScrollPos = GUILayout.BeginScrollView(leftScrollPos);

        // Кнопка Refresh
        GUIStyle refreshButtonStyle = new GUIStyle(GUI.skin.button);
        refreshButtonStyle.normal.textColor = Color.white;
        Color previousColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Refresh", refreshButtonStyle))
        {
            RefreshBaseCards();
        }
        GUI.backgroundColor = previousColor;

        GUILayout.Space(5);

        // Кнопки для каждой карты
        foreach (var card in baseCards)
        {
            if (GUILayout.Button(card.name, GUILayout.Height(25)))
            {
                selectedCard = card;
                isCreating = false;
                serializedSelectedCard = new SerializedObject(selectedCard);
                serializedSelectedCard.Update();
            }
        }

        // Кнопка "+"
        if (GUILayout.Button("+", GUILayout.Height(25)))
        {
            isCreating = true;
            selectedCard = null;
            newCardName = "NewCard";
            newCardPath = "Assets/BaseCards";
            serializedSelectedCard = null;
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawRightPanel()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        rightScrollPos = GUILayout.BeginScrollView(rightScrollPos);

        if (isCreating)
        {
            DrawCreateCardUI();
        }
        else if (selectedCard != null)
        {
            DrawEditCardUI();
        }
        else
        {
            GUILayout.Label("Select a card or create a new one.", EditorStyles.boldLabel);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawCreateCardUI()
    {
        GUILayout.Label("Create New Card", EditorStyles.boldLabel);

        GUILayout.Space(10);

        // Поле для имени карты
        newCardName = EditorGUILayout.TextField("Card Name", newCardName);

        // Поле для пути создания
        newCardPath = EditorGUILayout.TextField("Asset Path", newCardPath);

        GUILayout.Space(20);

        // Кнопка Create
        if (GUILayout.Button("Create"))
        {
            CreateNewCard();
        }
    }

    private void CreateNewCard()
    {
        if (string.IsNullOrEmpty(newCardName))
        {
            EditorUtility.DisplayDialog("Error", "Card name cannot be empty.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(newCardPath))
        {
            EditorUtility.DisplayDialog("Error", "Asset path cannot be empty.", "OK");
            return;
        }

        // Проверка наличия папки
        if (!AssetDatabase.IsValidFolder(newCardPath))
        {
            EditorUtility.DisplayDialog("Error", "Invalid asset path.", "OK");
            return;
        }

        string assetPath = System.IO.Path.Combine(newCardPath, newCardName + ".asset");

        // Проверка существования файла
        if (AssetDatabase.LoadAssetAtPath<BaseCard>(assetPath) != null)
        {
            EditorUtility.DisplayDialog("Error", "A card with this name already exists at the specified path.", "OK");
            return;
        }

        // Создание нового ScriptableObject
        BaseCard newCard = ScriptableObject.CreateInstance<BaseCard>();
        AssetDatabase.CreateAsset(newCard, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Обновление списка
        RefreshBaseCards();

        // Выбор созданной карты для редактирования
        selectedCard = newCard;
        isCreating = false;
        serializedSelectedCard = new SerializedObject(selectedCard);
        serializedSelectedCard.Update();
    }

    private void DrawEditCardUI()
    {
        if (serializedSelectedCard == null)
        {
            serializedSelectedCard = new SerializedObject(selectedCard);
        }

        serializedSelectedCard.Update();

        GUILayout.Label("Edit Card", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // Рисуем все свойства кроме списков с абстрактными типами
        SerializedProperty prop = serializedSelectedCard.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            // Пропускаем Unity внутренние свойства
            if (prop.name == "m_Script") continue;

            // Проверяем, является ли свойство Generic List с абстрактным типом
            if (IsGenericListOfAbstract(prop))
            {
                DrawGenericList(prop);
            }
            else
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        serializedSelectedCard.ApplyModifiedProperties();
    }

    private bool IsGenericListOfAbstract(SerializedProperty prop)
    {
        if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
        {
            // Получаем поле через рефлексию
            var fieldInfo = selectedCard.GetType().GetField(prop.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                Type fieldType = fieldInfo.FieldType;
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type genericArg = fieldType.GetGenericArguments()[0];
                    return genericArg.IsAbstract;
                }
            }
        }
        return false;
    }

    private void DrawGenericList(SerializedProperty prop)
    {
        GUILayout.Label(ObjectNames.NicifyVariableName(prop.name), EditorStyles.boldLabel);

        // Получение типа элементов списка
        var fieldInfo = selectedCard.GetType().GetField(prop.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Type listElementType = null;
        if (fieldInfo != null)
        {
            Type fieldType = fieldInfo.FieldType;
            listElementType = fieldType.GetGenericArguments()[0];
        }

        if (listElementType == null)
        {
            EditorGUILayout.HelpBox("Не удалось определить тип элементов списка.", MessageType.Error);
            return;
        }

        // Получение всех наследников абстрактного типа
        var derivedTypes = GetDerivedTypes(listElementType);

        // Рисуем элементы списка
        for (int i = 0; i < prop.arraySize; i++)
        {
            SerializedProperty elementProp = prop.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            // Определение имени типа
            string typeName = elementProp.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(typeName))
            {
                typeName = "Null";
            }
            else
            {
                var splitted = typeName.Split(".");
                typeName = splitted[splitted.Length - 1];
            }
            //else
            //{
            //    // Извлекаем короткое имя типа из полного имени
            //    Type type = Type.GetType(elementProp.managedReferenceFullTypename);
            //    typeName = type != null ? type.Name : "Unknown";
            //}

            // Кнопка "Edit"
            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                // В данном случае, редактирование осуществляется через отображение свойств ниже
                // Дополнительных действий не требуется
            }

            GUILayout.Label(typeName, GUILayout.Width(150));

            // Кнопка удаления элемента
            if (GUILayout.Button("-", GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Удаление элемента",
                    $"Вы уверены, что хотите удалить этот элемент?",
                    "Да", "Нет"))
                {
                    prop.DeleteArrayElementAtIndex(i);
                    break;
                }
            }

            EditorGUILayout.EndHorizontal();

            // Рисуем свойства элемента, если он не null
            if (!string.IsNullOrEmpty(elementProp.managedReferenceFullTypename))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(elementProp, true);
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.HelpBox("Элемент не установлен.", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(5);

        // Горизонтальный список кнопок типов и кнопка Add
        GUILayout.BeginHorizontal();

        // Кнопки для каждого типа
        foreach (var type in derivedTypes)
        {
            if (GUILayout.Button(type.Name))
            {
                AddElementToList(prop, type);
            }
        }

        // Кнопка Add с выпадающим списком
        //if (GUILayout.Button("Add"))
        //{
        //    GenericMenu menu = new GenericMenu();
        //    foreach (var type in derivedTypes)
        //    {
        //        menu.AddItem(new GUIContent(type.Name), false, () => AddElementToList(prop, type));
        //    }
        //    menu.ShowAsContext();
        //}

        GUILayout.EndHorizontal();
    }

    private void AddElementToList(SerializedProperty prop, Type type)
    {
        // Создаём новый экземпляр требуемого типа
        object newElement = Activator.CreateInstance(type);

        // Установка созданного объекта как managed reference
        prop.arraySize++;
        SerializedProperty newElementProp = prop.GetArrayElementAtIndex(prop.arraySize - 1);
        newElementProp.managedReferenceValue = newElement;

        serializedSelectedCard.ApplyModifiedProperties();
    }

    private List<Type> GetDerivedTypes(Type baseType)
    {
        if (derivedTypesCache.ContainsKey(baseType.FullName))
        {
            return derivedTypesCache[baseType.FullName];
        }

        var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
                        .ToList();

        derivedTypesCache[baseType.FullName] = types; // Сохраняем список типов в кэш

        return types;
    }
}