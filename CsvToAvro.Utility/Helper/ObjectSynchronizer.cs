//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text.RegularExpressions;

//namespace CCS.Level7.Connect.Library
//{

//    public static class ObjectSynchronizer
//    {

//        private static readonly Type EnumerableType = typeof(IEnumerable);

//        /// <summary>
//        /// Can synchronize a potential edited object.
//        /// </summary>
//        /// <param name="modelState">ModelState - the list of properties that might be changed</param>
//        /// <param name="source">The partially filled object with the new data</param>
//        /// <param name="target">The original object. Use this to save the object (for example)</param>
//        public static void Synchronize(IEnumerable<string> keys,object source, object target)
//        {

//            if (eys.Count > 0) //throw new ArgumentOutOfRangeException("modelState");
//            {


//                //the real thing
//                var keyInfos = ParseKeys(modelState.Keys);
//                SyncObject(keyInfos, source, target);

//                //the after fix
//                var fix = new ArrayFixRemoveEmptyElements(target);
//                fix.Execute();
//            }
//        }

//        // Copy properties from object a to object b
//        public static void copyPropertiesFrom(object to, object from, string[] excludedProperties)
//        {
//            Type targetType = to.GetType();
//            Type sourceType = from.GetType();

//            if (targetType.GetInterface("IEnumerable") != null)
//            {
//                targetType = targetType.GetElementType();
//            }

//            if (sourceType.GetInterface("IEnumerable") != null)
//            {
//                sourceType = sourceType.GetElementType();
//            }

//            PropertyInfo[] sourceProps = sourceType.GetProperties();
//            foreach (var propInfo in sourceProps)
//            {
//                //filter the properties
//                if (excludedProperties != null
//                  && excludedProperties.Contains(propInfo.Name))
//                    continue;

//                //Get the matching property from the target
//                PropertyInfo toProp =
//                  (targetType == sourceType) ? propInfo : targetType.GetProperty(propInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

//                //If it exists and it's writeable
//                if (toProp != null && toProp.CanWrite && HandleAsPrimitive(toProp.PropertyType))
//                {
//                    //Copy the value from the source to the target
//                    Object value = propInfo.GetValue(from, null);
//                    toProp.SetValue(to, Convert.ChangeType(value, toProp.PropertyType), null);
//                }
//            }
//        }

//        #region key parsing

//        private static KeyInfoCollection ParseKeys(ICollection<string> keys)
//        {
//            var keyInfos = new KeyInfoCollection();

//            foreach (string key in keys)
//            {
//                if (!string.IsNullOrEmpty(key)) keyInfos.Add(ParseKey(key));
//            }

//            return keyInfos;
//        }

//        private static KeyInfo ParseKey(string key)
//        {
//            string pattern = @"(?<name>[^\[\]\.]+)((\[(?<index>\d+)\]\.(?<remainder>.+))|\.(?<remainder>.+)){0,1}";
//            Regex regex = new Regex(pattern);

//            if (regex.IsMatch(key))
//            {
//                string name = regex.Match(key).Groups["name"].Value;
//                string index = regex.Match(key).Groups["index"].Value;
//                string remainder = regex.Match(key).Groups["remainder"].Value;

//                KeyInfo keyInfo = new KeyInfo(name, index);

//                if (!string.IsNullOrEmpty(remainder))
//                {
//                    keyInfo.Properties.Add(ParseKey(remainder));
//                }

//                return keyInfo;
//            }

//            throw new ArgumentException(string.Format("invalid key. value={0}", key));
//        }

//        public class KeyInfo
//        {
//            private int _index;

//            public string Name { get; private set; }
//            public bool IsArrayElement { get; private set; }

//            public int Index
//            {
//                get
//                {
//                    if (IsArrayElement)
//                    {
//                        return _index;
//                    }

//                    throw new InvalidOperationException("Index is only valid on type Array.");
//                }
//            }

//            public KeyInfoCollection Properties = new KeyInfoCollection();

//            public KeyInfo(string name, string index)
//            {
//                name.ThrowIfArgumentNull("name");
//                Name = name;
//                IsArrayElement = int.TryParse(index, out _index);
//            }

//            public string GetModelStateName()
//            {
//                if (IsArrayElement)
//                {
//                    return Name + "[" + _index + "]";
//                }

//                return Name;
//            }

//        }

//        public class KeyInfoCollection : IEnumerable<KeyInfo>
//        {
//            public Dictionary<string, KeyInfo> _items = new Dictionary<string, KeyInfo>();

//            public int Count
//            {
//                get
//                {
//                    return _items.Values.Count;
//                }
//            }

//            public void Add(KeyInfo item)
//            {
//                string key = GetKey(item);

//                if (_items.ContainsKey(key))
//                {
//                    KeyInfo existing = _items[key];

//                    foreach (KeyInfo child in item.Properties)
//                    {
//                        existing.Properties.Add(child);
//                    }
//                }
//                else
//                {
//                    _items.Add(key, item);
//                }
//            }

//            private string GetKey(KeyInfo item)
//            {
//                if (item.IsArrayElement)
//                {
//                    return string.Format("{0}-{1}", item.Name, item.Index);
//                }

//                return item.Name;
//            }

//            #region IEnumerable<KeyInfo> Members

//            public IEnumerator<KeyInfo> GetEnumerator()
//            {
//                return _items.Values.GetEnumerator();
//            }

//            #endregion

//            #region IEnumerable Members

//            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//            {
//                return ((System.Collections.IEnumerable)_items.Values).GetEnumerator();
//            }

//            #endregion
//        }

//        #endregion

//        private static void SyncObject(KeyInfoCollection keyInfos, object source, object target)
//        {
//            keyInfos.ThrowIfArgumentNull("keyInfos");
//            source.ThrowIfArgumentNull("source");
//            target.ThrowIfArgumentNull("target");

//            bool changed = false;

//            foreach (KeyInfo keyInfo in keyInfos)
//            {
//                if (SyncPropery(keyInfo, source, target))
//                    changed = true;
//            }

//            if (changed && target.HasProperty("pc"))
//            {
//                if ((int)target.GetPropertyValue("pc") == 3)
//                    target.SetPropertyValue("pc", 0);
//            }
//        }

//        private static bool SyncPropery(KeyInfo keyInfo, object source, object target)
//        {
//            keyInfo.ThrowIfArgumentNull("keyInfo");
//            source.ThrowIfArgumentNull("source");
//            target.ThrowIfArgumentNull("target");

//            var sourceType = source.GetType();
//            var propertyInfo = sourceType.GetProperty(keyInfo.Name);
//            if (propertyInfo == null)
//                return false;
//            var propertyType = propertyInfo.PropertyType;
//            var sourceValue = propertyInfo.GetValue(source);
//            var targetValue = propertyInfo.GetValue(target);

//            if (sourceValue != null && sourceValue.Equals(targetValue))
//            {
//                return false;
//            }
//            else if (HandleAsPrimitive(propertyType))
//            {
//                propertyInfo.SetValue(target, sourceValue);
//            }
//            else if (propertyType.IsArray && keyInfo.IsArrayElement)
//            {
//                SyncArrayElement(keyInfo, propertyInfo, target, sourceValue as object[], targetValue as object[]);
//            }
//            else if (sourceValue == null || targetValue == null)
//            {
//                propertyInfo.SetValue(target, sourceValue);
//            }
//            else if (IsGenericList(propertyType))
//            {
//                SyncListElement(keyInfo, propertyInfo, target, sourceValue as IList, targetValue as IList);
//            }
//            else if (propertyType.IsClass)
//            {
//                SyncObject(keyInfo.Properties, sourceValue, targetValue);
//            }
//            else
//            {
//                throw new Exception("not handled: " + propertyType.Name);
//            }

//            return true;
//        }

//        private static void SyncArrayElement(KeyInfo keyInfo, PropertyInfo propertyInfo, object parent, object[] sourceArray, object[] targetArray)
//        {
//            keyInfo.ThrowIfArgumentNull("keyInfo");
//            propertyInfo.ThrowIfArgumentNull("propertyInfo");
//            sourceArray.ThrowIfArgumentNull("sourceArray");

//            var targetLength = (targetArray != null) ? targetArray.Length : 0;

//            if (!keyInfo.IsArrayElement || keyInfo.Index > sourceArray.Length - 1)
//            {
//                throw new Exception("can't process ArrayElement");
//            }
//            if (keyInfo.Index > targetLength - 1)
//            {
//                var sourceElement = sourceArray[keyInfo.Index];
//                var pc = (int)sourceElement.GetPropertyValue("pc");

//                //Only add new elements
//                if (pc == 4)
//                {
//                    var tempArray = (object[])Array.CreateInstance(propertyInfo.PropertyType.GetElementType(), targetLength + 1);

//                    if (targetArray != null)
//                    {
//                        Array.Copy(targetArray, tempArray, targetArray.Length);
//                    }

//                    tempArray[targetLength] = sourceElement;

//                    propertyInfo.SetValue(parent, tempArray);
//                }
//                else
//                {
//                    // When array item is not copied, remove item from Modelstate
//                    ObjectModelState.Keys.Where(key => key.StartsWith(keyInfo.GetModelStateName())).ToList().ForEach(m => ObjectModelState.Remove(m));
//                }
//            }
//            else if (targetArray != null)
//            {
//                var sourceElement = sourceArray[keyInfo.Index];
//                var targetElement = targetArray[keyInfo.Index];
//                var pcSource = (int)sourceElement.GetPropertyValue("pc");
//                var pcTarget = (int)targetElement.GetPropertyValue("pc");

//                if (pcSource == 1 && pcTarget == 4)
//                {
//                    // direct delete of new items
//                    targetArray[keyInfo.Index] = null;
//                }
//                else
//                {
//                    SyncObject(keyInfo.Properties, sourceElement, targetElement);
//                }
//            }
//        }

//        private static void SyncListElement(KeyInfo keyInfo, PropertyInfo propertyInfo, object parent, IList sourceArray, IList targetArray)
//        {
//            keyInfo.ThrowIfArgumentNull("keyInfo");
//            propertyInfo.ThrowIfArgumentNull("propertyInfo");
//            sourceArray.ThrowIfArgumentNull("sourceArray");
//            targetArray.ThrowIfArgumentNull("targetArray");

//            if (!keyInfo.IsArrayElement || keyInfo.Index > sourceArray.Count - 1)
//            {
//                throw new Exception("can't process ArrayElement");
//            }
//            else if (keyInfo.Index > targetArray.Count - 1)
//            {
//                var sourceElement = sourceArray[keyInfo.Index];
//                var pc = (int)sourceElement.GetPropertyValue("pc");

//                //Only add new elements
//                if (pc == 4)
//                {
//                    targetArray.Add(sourceElement);
//                }
//                else
//                {
//                    // When array item is not copied, remove item from Modelstate
//                    ObjectModelState.Keys.Where(key => key.StartsWith(keyInfo.GetModelStateName())).ToList().ForEach(m => ObjectModelState.Remove(m));
//                }
//            }
//            else
//            {
//                var sourceElement = sourceArray[keyInfo.Index];
//                var targetElement = targetArray[keyInfo.Index];
//                SyncObject(keyInfo.Properties, sourceElement, targetElement);
//            }
//        }

//        public static bool HandleAsPrimitive(Type type)
//        {
//            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
//            return isNullable || type.IsPrimitive || type.IsEnum || type == typeof(String) || type == typeof(Decimal) || type == typeof(DateTime);
//        }

//        private static bool IsGenericList(Type type)
//        {
//            return type != null && IsEnumerable(type) && type.IsGenericType && type.GetGenericArguments().Count() == 1;
//        }

//        public static bool IsEnumerable(Type t)
//        {
//            return EnumerableType.IsAssignableFrom(t);
//        }

//        /// <summary> 
//        /// Copies the data of one object to another. The target object gets properties of the first.  
//        /// Any matching properties (by name) are written to the target. 
//        /// </summary> 
//        /// <param name="source">The source object to copy from</param>
//        /// <param name="target">The target object to copy to</param>
//        public static void CopyObjectData(object source, object target)
//        {
//            CopyObjectData(source, target, String.Empty, BindingFlags.Public | BindingFlags.Instance);
//        }

//        /// <summary> 
//        /// Copies the data of one object to another. The target object gets properties of the first.  
//        /// Any matching properties (by name) are written to the target. 
//        /// </summary> 
//        /// <param name="source">The source object to copy from</param>
//        /// <param name="target">The target object to copy to</param>
//        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
//        /// <param name="memberAccess">Reflection binding access</param>
//        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
//        {
//            string[] excluded = null;
//            if (!string.IsNullOrEmpty(excludedProperties))
//            {
//                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//            }

//            var miT = target.GetType().GetMembers(memberAccess);
//            foreach (var field in miT)
//            {
//                string name = field.Name;

//                // Skip over excluded properties 
//                if (excluded != null && (string.IsNullOrEmpty(excludedProperties) == false
//                                         && excluded.Contains(name)))
//                {
//                    continue;
//                }


//                if (field.MemberType == MemberTypes.Field)
//                {
//                    var sourcefield = source.GetType().GetField(name);
//                    if (sourcefield == null) { continue; }

//                    object sourceValue = sourcefield.GetValue(source);
//                    ((FieldInfo)field).SetValue(target, sourceValue);
//                }
//                else if (field.MemberType == MemberTypes.Property)
//                {
//                    var piTarget = field as PropertyInfo;
//                    var sourceField = source.GetType().GetProperty(name, memberAccess);
//                    if (sourceField == null) { continue; }

//                    if (piTarget != null && (piTarget.CanWrite && sourceField.CanRead))
//                    {
//                        var targetValue = piTarget.GetValue(target, null);
//                        var sourceValue = sourceField.GetValue(source, null);

//                        if (sourceValue == null) { continue; }

//                        if (sourceField.PropertyType.IsArray
//                            && piTarget.PropertyType.IsArray)
//                        {
//                            CopyArray(source, target, memberAccess, piTarget, sourceField, sourceValue);
//                        }
//                        else
//                        {
//                            CopySingleData(source, target, memberAccess, piTarget, sourceField, targetValue, sourceValue);
//                        }
//                    }
//                }
//            }
//        }
//        private static void CopySingleData(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object targetValue, object sourceValue)
//        {
//            //instantiate target if needed 
//            if (targetValue == null
//                && piTarget.PropertyType.IsValueType == false
//                && piTarget.PropertyType != typeof(string))
//            {
//                targetValue = Activator.CreateInstance(piTarget.PropertyType.IsArray ?
//                    piTarget.PropertyType.GetElementType() : piTarget.PropertyType);
//            }

//            if (piTarget.PropertyType.IsValueType == false
//                && piTarget.PropertyType != typeof(string))
//            {
//                CopyObjectData(sourceValue, targetValue, "", memberAccess);
//                piTarget.SetValue(target, targetValue, null);
//            }
//            else
//            {
//                if (piTarget.PropertyType.FullName == sourceField.PropertyType.FullName)
//                {
//                    object tempSourceValue = sourceField.GetValue(source, null);
//                    piTarget.SetValue(target, tempSourceValue, null);
//                }
//                else
//                {
//                    CopyObjectData(piTarget, target, "", memberAccess);
//                }
//            }
//        }

//        private static void CopyArray(object source, object target, BindingFlags memberAccess, PropertyInfo piTarget, PropertyInfo sourceField, object sourceValue)
//        {
//            var sourceLength = (int)sourceValue.GetType().InvokeMember("Length", BindingFlags.GetProperty, null, sourceValue, null);
//            var targetArray = Array.CreateInstance(piTarget.PropertyType.GetElementType(), sourceLength);
//            var array = (Array)sourceField.GetValue(source, null);

//            for (var i = 0; i < array.Length; i++)
//            {
//                var o = array.GetValue(i);
//                var tempTarget = Activator.CreateInstance(piTarget.PropertyType.GetElementType());
//                CopyObjectData(o, tempTarget, "", memberAccess);
//                targetArray.SetValue(tempTarget, i);
//            }
//            piTarget.SetValue(target, targetArray, null);
//        }

//    }
//}
