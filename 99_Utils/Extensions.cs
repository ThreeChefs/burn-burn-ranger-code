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
        return ShuffleRangeAndTake<T>(list, 0, list.Count, count);
    }

    /// <summary>
    /// [start, end)까지 섞고, [0, count)까지 뽑아줍니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static List<T> ShuffleRangeAndTake<T>(this List<T> list, int start, int end, int count)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count == 0 || count <= 0)
        {
            return new List<T>();
        }

        if (start < 0 || start >= end || end > list.Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        count = Math.Min(count, list.Count);

        List<T> temp = new(list);

        for (int i = end - 1; i > start; i--)
        {
            int j = Define.Random.Next(start, i + 1);
            (temp[i], temp[j]) = (temp[j], temp[i]);
        }

        return temp.GetRange(0, count);
    }
}
#endregion
