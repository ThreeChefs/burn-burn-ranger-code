using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 타입을 확장하는 클래스 모음
/// </summary>

#region Transform
public static class TransformExtension
{
    public static T FindChild<T>(this Transform t, string name) where T : Component
    {
        T[] children = t.gameObject.GetComponentsInChildren<T>(true);
        foreach (T child in children)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
}
#endregion

#region Array
public static class ArrayExtension
{
    public static T Random<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
            return default;

        return array[Define.Random.Next(0, array.Length)];
    }

    public static T[] Shuffle<T>(this T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int r = Define.Random.Next(i, array.Length);
            (array[i], array[r]) = (array[r], array[i]);
        }
        return array;
    }
}
#endregion

#region ArrayList
public static class ListExtension
{
    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) { return default; }

        return list[Define.Random.Next(0, list.Count)];
    }

    public static List<T> Random<T>(this List<T> list, int count)
    {
        if (list == null)
        {
            throw new System.ArgumentNullException(nameof(list));
        }

        if (list.Count == 0 || count <= 0)
        {
            return new List<T>();
        }

        count = Math.Min(count, list.Count);

        List<T> temp = new(list);

        for (int i = temp.Count - 1; i > 0; i--)
        {
            int j = Define.Random.Next(0, i + 1);
            (temp[i], temp[j]) = (temp[j], temp[i]);
        }

        return temp.GetRange(0, count);
    }
}
#endregion
