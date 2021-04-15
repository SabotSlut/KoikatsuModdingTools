using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public static class NullCheck
{
    public static bool IsDefault<T>(this T value) where T : struct
    {
        return value.Equals(default(T));
    }

    public static bool IsNull<T, TU>(this KeyValuePair<T, TU> pair)
    {
        return pair.Equals(new KeyValuePair<T, TU>());
    }

    public static T GetCache<T>(this object _, ref T ret, Func<T> get)
    {
        return (object)ret != null ? ret : (ret = get());
    }

    public static T GetCacheObject<T>(this object _, ref T ret, Func<T> get) where T : Object
    {
        return (Object)ret != (Object)null ? ret : (ret = get());
    }

    public static T GetComponentCache<T>(this Component component, ref T ret) where T : Component
    {
        return component.GetCacheObject(ref ret, () => component.GetComponent<T>());
    }

    public static T GetComponentCache<T>(this GameObject gameObject, ref T ret) where T : Component
    {
        return gameObject.GetCacheObject(ref ret, gameObject.GetComponent<T>);
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        return (Object)component == (Object)null ? null : component.gameObject.GetOrAddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        T obj = gameObject.GetComponent<T>();
        if (obj == null)
        {
            obj = gameObject.AddComponent<T>();
        }

        return obj;
    }

    public static bool IsNullOrWhiteSpace(this string self)
    {
        return self == null || self.Trim() == string.Empty;
    }

    public static bool IsNullOrEmpty(this string self)
    {
        return string.IsNullOrEmpty(self);
    }

    public static bool IsNullOrEmpty(this string[] args, int index)
    {
        bool ret = false;
        args.SafeGet(index).SafeProc<string>((Action<string>)(s => ret = !s.IsNullOrEmpty()));
        return !ret;
    }

    public static bool IsNullOrEmpty(this List<string> args, int index)
    {
        bool ret = false;
        args.SafeGet(index).SafeProc<string>((Action<string>)(s => ret = !s.IsNullOrEmpty()));
        return !ret;
    }

    public static bool IsNullOrEmpty<T>(this IList<T> self)
    {
        return self == null || self.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this List<T> self)
    {
        return self == null || self.Count == 0;
    }

    public static bool IsNullOrEmpty(this MulticastDelegate self)
    {
        return (object)self == null || self.GetInvocationList() == null || self.GetInvocationList().Length == 0;
    }

    public static bool IsNullOrEmpty(this UnityEvent self)
    {
        return self == null || self.GetPersistentEventCount() == 0;
    }

    public static bool IsNullOrEmpty(this UnityEvent self, int target)
    {
        return self.IsNullOrEmpty() || self.GetPersistentTarget(target) == null || self.GetPersistentMethodName(target).IsNullOrEmpty();
    }

    public static T SafeGet<T>(this T[] array, int index)
    {
        if (array == null)
        {
            return default(T);
        }

        return (long)(uint)index < (long)array.Length ? array[index] : default(T);
    }

    public static bool SafeProc<T>(this T[] array, int index, Action<T> act)
    {
        return array.SafeGet(index).SafeProc(act);
    }

    public static T SafeGet<T>(this List<T> list, int index)
    {
        if (list == null)
        {
            return default(T);
        }

        return (long)(uint)index < (long)list.Count ? list[index] : default(T);
    }

    public static bool SafeProc<T>(this List<T> list, int index, Action<T> act)
    {
        return list.SafeGet(index).SafeProc(act);
    }

    public static bool SafeProc(this string[] args, int index, Action<string> act)
    {
        if (args.IsNullOrEmpty(index))
        {
            return false;
        }

        act.Call(args[index]);
        return true;
    }

    public static bool SafeProc(this List<string> args, int index, Action<string> act)
    {
        if (args.IsNullOrEmpty(index))
        {
            return false;
        }

        act.Call(args[index]);
        return true;
    }

    public static bool SafeProc<T>(this T self, Action<T> act)
    {
        bool flag = self != null;
        if (flag)
        {
            act.Call(self);
        }

        return flag;
    }

    public static bool SafeProcObject<T>(this T self, Action<T> act) where T : Object
    {
        bool flag = self != null;
        if (flag)
        {
            act.Call(self);
        }

        return flag;
    }

    public static void Call(this Action action)
    {
        if (action == null)
        {
            return;
        }

        action();
    }

    public static void Call<T>(this Action<T> action, T arg)
    {
        if (action == null)
        {
            return;
        }

        action(arg);
    }

    public static void Call<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
        if (action == null)
        {
            return;
        }

        action(arg1, arg2);
    }

    public static void Call<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action == null)
        {
            return;
        }

        action(arg1, arg2, arg3);
    }

    public static TResult Call<TResult>(this Func<TResult> func, TResult result = default(TResult))
    {
        return func != null ? func() : result;
    }

    public static TResult Call<T, TResult>(this Func<T, TResult> func, T arg, TResult result = default(TResult))
    {
        return func != null ? func(arg) : result;
    }

    public static TResult Call<T1, T2, TResult>(
        this Func<T1, T2, TResult> func,
        T1 arg1,
        T2 arg2,
        TResult result = default(TResult))
    {
        return func != null ? func(arg1, arg2) : result;
    }

    public static TResult Call<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, TResult> func,
        T1 arg1,
        T2 arg2,
        T3 arg3,
        TResult result = default(TResult))
    {
        return func != null ? func(arg1, arg2, arg3) : result;
    }

    public static bool Proc<T>(this T self, Func<T, bool> conditional, Action<T> act)
    {
        bool flag = conditional(self);
        if (flag)
        {
            act.Call(self);
        }

        return flag;
    }

    public static bool Proc<T>(
        this T self,
        Func<T, bool> conditional,
        Action<T> actTrue,
        Action<T> actFalse)
    {
        bool flag = conditional(self);
        self.Proc(_ => true, !flag ? actFalse : actTrue);
        return flag;
    }
}
