using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Conatains all custom classes including save and load functions for serializable classes
/// </summary>
public static partial class UsefulExtensions
{
    /// <summary>
    /// Simplifies loading a saved class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultReturn"></param>
    /// <returns></returns>
    internal static T GetClass<T>(string key, T defaultReturn)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultReturn;
        }
        string saveString = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(saveString))
        {
            return defaultReturn;
        }
        T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(saveString);
        return obj;

    }
    /// <summary>
    /// Simplifies saving a class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultReturn"></param>
    /// <returns></returns>
    internal static void SaveClass<T>(string key, T classToSave)
    {
        var saveString = Newtonsoft.Json.JsonConvert.SerializeObject(classToSave);
        PlayerPrefs.SetString(key, saveString);
        PlayerPrefs.Save();
    }

}

/// <summary>
/// Wrapper class used to save Vector 3 types
/// </summary>
[Serializable]
public class SerializableVector3
{
    public float X;
    public float Y;
    public float Z;
    public Vector3 Get()
    {
        return new Vector3(X, Y, Z);
    }
    public SerializableVector3(Vector3 vec)
    {
        X = vec.x;
        Y = vec.y;
        Z = vec.z;
    }
}

public class SavedInt : SavedObject<int>
{
    public SavedInt(string saveString, int defaultValue, bool saveOnChange, Action onChange)
        : base(saveString, defaultValue, saveOnChange, onChange)
    {
    }

    public override void Save()
    {
        PlayerPrefs.SetInt(_saveString, _currentValue);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static SavedInt operator +(SavedInt first, int v)
    {
        first.Value += v;
        return first;
    }

    public static SavedInt operator -(SavedInt first, int v)
    {
        first.Value -= v;
        return first;
    }

    public static SavedInt operator /(SavedInt first, int v)
    {
        first.Value /= v;
        return first;
    }

    public static SavedInt operator *(SavedInt first, int v)
    {
        first.Value *= v;
        return first;
    }

    public static implicit operator int(SavedInt v)
    {
        return v.Value;
    }

    public bool Equals(SavedInt other)
    {
        if (other == null)
            return false;
        return this.Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() == typeof(int))
        {
            return this.Value == (int)obj;
        }

        SavedInt other = obj as SavedInt;

        if (other == null)
            return false;

        return Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Get().GetHashCode();
    }

    public static bool operator ==(SavedInt first, SavedInt second)
    {
        if (object.ReferenceEquals(first, second))
            return true;
        if (object.ReferenceEquals(null, first))
            return false;
        if (object.ReferenceEquals(null, second))
            return false;
        return first.Equals(second);
    }

    public static bool operator !=(SavedInt first, SavedInt second)
    {
        if (object.ReferenceEquals(first, second))
            return false;
        if (object.ReferenceEquals(null, first))
            return true;
        if (object.ReferenceEquals(null, second))
            return true;
        return !first.Equals(second);
    }

    public static bool operator ==(SavedInt first, int second)
    {
        if (object.ReferenceEquals(first, second))
            return true;
        if (object.ReferenceEquals(null, first))
            return false;
        if (object.ReferenceEquals(null, second))
            return false;
        return first.Equals(second);
    }

    public static bool operator !=(SavedInt first, int second)
    {
        if (object.ReferenceEquals(first, second))
            return false;
        if (object.ReferenceEquals(null, first))
            return true;
        if (object.ReferenceEquals(null, second))
            return true;
        return !first.Equals(second);
    }

   
    public static bool operator ==(float first, SavedInt second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }
    public static bool operator !=(float first, SavedInt second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }
} 

///// <summary>
///// Allows for simple creation of persistent ints
///// </summary>
//    public class SavedInt
//{
//    private string _saveString;
//    private int _currentValue = int.MinValue;
//    private int _defaultValue;
//    private bool _saveOnChange = false;
//    /// <summary>
//    /// Callback to track changes to the value
//    /// </summary>
//    Action _onChanged = null;
//    /// <summary>
//    /// Saved Bool creator
//    /// </summary>
//    /// <param name="saveString"></param>
//    /// Key used by player prefs
//    /// <param name="defaultValue"></param>
//    /// Default value set on create
//    /// <param name="saveOnChange"></param>
//    /// Option to set the value using player prefs whenever the value is changed. If set to false use the Save function to record the value
//    /// <param name="onChange"></param>
//    /// Callback that is sent when the value is changed
//    public SavedInt(string saveString, int defaultValue, bool saveOnChange, Action onChange)
//    {
//        _saveString = saveString;
//        _defaultValue = defaultValue;
//        _onChanged = onChange;
//        _saveOnChange = saveOnChange;
//    }

//    public void Save()
//    {
//        PlayerPrefs.SetInt(_saveString, _currentValue);
//    }
//    /// <summary>
//    /// Public value used to retrive the saved data
//    /// </summary>
//    public int Value
//    {
//        get
//        {
//            if (_currentValue == int.MinValue)
//            {
//                Value = PlayerPrefs.GetInt(_saveString, _defaultValue);
//            }
//            return _currentValue;
//        }
//        set
//        {
//            _currentValue = value;
//            if (_saveOnChange)
//                Save();
//            _onChanged?.Invoke();
//        }
//    }
//    public override string ToString()
//    {
//        return Value.ToString();
//    }

   
//    public static implicit operator int(SavedInt v)
//    {
//        return v.Value;
//    }
   
//}
/// <summary>
/// Allows for simple creation of persistent booleans
/// </summary>
public class SavedBool : SavedObject<bool>
{
    public SavedBool(string saveString, bool defaultValue, bool saveOnChange, Action onChange) : base(saveString, defaultValue, saveOnChange, onChange)
    {
    }
    public override void Save()
    {
        PlayerPrefs.SetInt(_saveString, _currentValue ? 1 : 0);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator bool(SavedBool v)
    {
        return v.Value;
    }

    public bool Equals(SavedBool other)
    {
        if (other == null)
            return false;
        return this.Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj.GetType() == typeof(bool))
        {
            return this.Value == (bool)obj;
        }

        SavedBool other = obj as SavedBool;

        if (other == null) return false;

        return Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Get().GetHashCode();
    }

    public static bool operator ==(SavedBool first, SavedBool second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(SavedBool first, SavedBool second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);
    }
}




public class SavedString : SavedObject<string>
{
    public SavedString(string saveString, string defaultValue, bool saveOnChange, Action onChange) : base(saveString, defaultValue, saveOnChange, onChange)
    {
    }
    public override void Save()
    {
        PlayerPrefs.SetString(_saveString, _currentValue);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static SavedString operator +(SavedString first, string v)
    {
        first.Value += v;
        return first;
    }


    public static implicit operator string(SavedString v)
    {
        return v.Value;
    }

    public bool Equals(SavedString other)
    {
        if (other == null)
            return false;
        return this.Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj.GetType() == typeof(string))
        {
            return this.Value == (string)obj;
        }

        SavedString other = obj as SavedString;

        if (other == null) return false;

        return Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Get().GetHashCode();
    }

    public static bool operator ==(SavedString first, SavedString second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(SavedString first, SavedString second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }

    public static bool operator ==(SavedString first, string second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(SavedString first, string second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }

    public static bool operator ==(string first, SavedString second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(string first, SavedString second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }
}
/// <summary>
/// Allows for simple creation of persistent floats
/// </summary>
public class SavedFloat:SavedObject<float>
{
    public SavedFloat(string saveString, float defaultValue, bool saveOnChange, Action onChange):base(saveString, defaultValue, saveOnChange,onChange)
    {
    }

    public override void Save()
    {
        PlayerPrefs.SetFloat(_saveString, _currentValue);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static SavedFloat operator +(SavedFloat first,float v)
    {
        first.Value += v;
        return first;
    }

    public static SavedFloat operator -(SavedFloat first, float v)
    {
        first.Value -= v;
        return first;
    }

    public static SavedFloat operator /(SavedFloat first, float v)
    {
        first.Value /= v;
        return first;
    }

    public static SavedFloat operator *(SavedFloat first, float v)
    {
        first.Value /= v;
        return first;
    }



    public static implicit operator float(SavedFloat v)
    {
        return v.Value;
    }
    public bool Equals(SavedFloat other)
    {
        if (other == null)
            return false;
        return this.Value == other.Value;
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if(obj.GetType() == typeof(float))
        {
            return this.Value == (float)obj;
        }

        SavedFloat other = obj as SavedFloat;

        if (other == null) return false;

        return Equals(other);
    }
    public override int GetHashCode()
    {
        return this.Get().GetHashCode();
    }
    public static bool operator ==(SavedFloat first, SavedFloat second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }
    public static bool operator !=(SavedFloat first, SavedFloat second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }
    public static bool operator ==(SavedFloat first, float second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }
    public static bool operator !=(SavedFloat first, float second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }
    public static bool operator ==(float first, SavedFloat second)
    {
        if (object.ReferenceEquals(first, second)) return true;
        if (object.ReferenceEquals(null, first)) return false;
        if (object.ReferenceEquals(null, second)) return false;
        return first.Equals(second);
    }
    public static bool operator !=(float first, SavedFloat second)
    {
        if (object.ReferenceEquals(first, second)) return false;
        if (object.ReferenceEquals(null, first)) return true;
        if (object.ReferenceEquals(null, second)) return true;
        return !first.Equals(second);

    }

}

public class SavedObject<T>
{
    protected string _saveString;

    protected T _currentValue;
    bool IsInitialised = false;
    protected T _defaultValue;
    protected bool _saveOnChange = false;
    Action _onChanged = null;



    public SavedObject(string saveString, T defaultValue, bool saveOnChange, Action onChange)
    {
        _saveOnChange = saveOnChange;
        _saveString = saveString;
        Value = defaultValue;
        _onChanged = onChange;
     
    }

public T Value
    {
        
        get
        {
                if (IsInitialised)
                {
                    Value = UsefulExtensions.GetClass<T>(_saveString, _defaultValue);
                }
                return _currentValue;
            }
            set
        {
                _currentValue = value;
                if (_saveOnChange)
                    Save();
                _onChanged?.Invoke();
            }
        }

    public virtual void Save()
    {
        throw new NotImplementedException();
    }

    public T Get()
    {
        return _currentValue;
    }


  

}

