using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Core.Cards.Actions;
using Core.Tools;

/// <summary>
/// Данный PropertyDrawer применяется ко всем полям типа CardAction (и его наследникам, благодаря параметру true)
/// и отрисовывает дополнительный интерфейс для создания/удаления экземпляра конкретного (неабстрактного) класса.
/// </summary>
[CustomPropertyDrawer(typeof(IAbstractSetup), true)]
public class PolymorphicReferencePropertyDrawer : PropertyDrawer
{
    // Словарь для хранения выбранного индекса в выпадающем списке для каждого свойства (ключ – propertyPath)
    private Dictionary<string, int> selectedIndices = new Dictionary<string, int>();

    // Кэш найденных типов для ускорения работы
    private static Dictionary<Type, Type[]> derivedTypesCache = new Dictionary<Type, Type[]>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Получаем высоту стандартного поля
        float defaultHeight = EditorGUI.GetPropertyHeight(property, label, true);
        // Добавляем высоту для одной строки управления (выпадающий список + кнопки)
        float extraHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        return defaultHeight + extraHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Отрисовываем стандартное свойство (в том числе, если в поле уже установлен экземпляр, его поля будут отображены)
        Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label, true));
        EditorGUI.PropertyField(fieldRect, property, label, true);

        // Рисуем панель управления под стандартным полем
        Rect controlsRect = new Rect(position.x, fieldRect.yMax + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);

        // Определяем базовый тип для данного свойства.
        // Если в поле null, то используется информация из SerializeReference (managedReferenceFieldTypename)
        Type baseType = GetBaseType(property);
        if (baseType == null)
        {
            EditorGUI.LabelField(controlsRect, "Ошибка: не удалось определить базовый тип");
            EditorGUI.EndProperty();
            return;
        }

        // Получаем список всех неабстрактных, [Serializable] типов, наследующих от baseType
        Type[] derivedTypes = GetDerivedTypes(baseType);
        if (derivedTypes.Length == 0)
        {
            EditorGUI.LabelField(controlsRect, "Не найдено ни одного подходящего типа");
            EditorGUI.EndProperty();
            return;
        }

        // Получаем выбранный индекс для данного свойства (если ранее не выбирали, то 0)
        if (!selectedIndices.TryGetValue(property.propertyPath, out int selectedIndex))
        {
            selectedIndex = 0;
        }

        // Формируем массив названий типов для выпадающего списка
        string[] options = derivedTypes.Select(t => t.Name).ToArray();

        // Распределяем по горизонтали: первая половина — выпадающий список, вторая — две кнопки
        float popupWidth = position.width * 0.5f;
        float buttonWidth = (position.width - popupWidth) / 2;
        Rect popupRect = new Rect(controlsRect.x, controlsRect.y, popupWidth, controlsRect.height);
        Rect setButtonRect = new Rect(controlsRect.x + popupWidth, controlsRect.y, buttonWidth, controlsRect.height);
        Rect removeButtonRect = new Rect(controlsRect.x + popupWidth + buttonWidth, controlsRect.y, buttonWidth, controlsRect.height);

        // Рисуем выпадающий список и обновляем выбранный индекс
        selectedIndex = EditorGUI.Popup(popupRect, selectedIndex, options);
        selectedIndices[property.propertyPath] = selectedIndex;

        // Кнопка "Set": если в поле ещё нет значения, создаём экземпляр выбранного типа
        if (GUI.Button(setButtonRect, "Set"))
        {
            if (property.managedReferenceValue == null)
            {
                Type selectedType = derivedTypes[selectedIndex];
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        // Кнопка "Remove": устанавливаем значение поля в null
        if (GUI.Button(removeButtonRect, "Remove"))
        {
            if (property.managedReferenceValue != null)
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }

    /// <summary>
    /// Определяет базовый тип для данного свойства. Если поле ещё null, используется информация из managedReferenceFieldTypename.
    /// </summary>
    private Type GetBaseType(SerializedProperty property)
    {
        // Если поле не содержит значение, в managedReferenceFieldTypename хранится тип, указанный в объявлении
        if (!string.IsNullOrEmpty(property.managedReferenceFieldTypename))
        {
            // Строка имеет вид "AssemblyName TypeName"
            string[] typeSplit = property.managedReferenceFieldTypename.Split(' ');
            if (typeSplit.Length == 2)
            {
                string assemblyName = typeSplit[0];
                string className = typeSplit[1];
                try
                {
                    Assembly assembly = Assembly.Load(assemblyName);
                    if (assembly != null)
                    {
                        Type type = assembly.GetType(className);
                        return type;
                    }
                }
                catch { }
            }
        }
        else if (property.managedReferenceValue != null)
        {
            return property.managedReferenceValue.GetType();
        }
        return null;
    }

    /// <summary>
    /// Ищет во всех сборках все типы, которые:
    /// - наследуются от baseType,
    /// - не являются абстрактными,
    /// - помечены [Serializable].
    /// Результат кэшируется для повышения производительности.
    /// </summary>
    private Type[] GetDerivedTypes(Type baseType)
    {
        if (derivedTypesCache.TryGetValue(baseType, out var types))
        {
            return types;
        }

        types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm =>
            {
                Type[] assemblyTypes;
                try
                {
                    assemblyTypes = asm.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    assemblyTypes = e.Types.Where(t => t != null).ToArray();
                }
                return assemblyTypes;
            })
            .Where(t => t != null && baseType.IsAssignableFrom(t) && !t.IsAbstract && t.IsDefined(typeof(SerializableAttribute), false))
            .ToArray();

        derivedTypesCache[baseType] = types;
        return types;
    }
}
