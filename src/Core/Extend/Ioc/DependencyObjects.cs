// /*****************************************************
// (c)2008-2013 Copy right www.Agebull.com
// ����:bull2
// ����:CodeRefactor-Agebull.Common.WpfMvvmBase
// ����:2014-11-29
// �޸�:2014-11-29
// *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.EntityModel.Common
{
    /// <summary>
    ///     ���������ֵ�
    /// </summary>
    /// <remarks>
    ///     ��������ΪIgnoreDataMember����,�������������л�
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class DependencyObjects
    {
        [JsonIgnore]
        private readonly Dictionary<Type, object> _dictionary = new Dictionary<Type, object>();

        /// <summary>
        ///     ����һ�����Ͷ���
        /// </summary>
        /// <remarks>
        ///     ���ַ���ֻ����һ��,����θ���,ֻ�����һ������
        /// </remarks>
        public void Annex<T>(T value)
        {
            var type = typeof(T);
            if (_dictionary.ContainsKey(type))
            {
                if (Equals(value, default(T)))
                {
                    _dictionary.Remove(type);
                }
                else
                {
                    _dictionary[type] = value;
                }
            }
            else if (!Equals(value, default(T)))
            {
                _dictionary.Add(type, value);
            }
        }

        /// <summary>
        ///     ȡ��һ�����͵���չ����(�����Զ��������ǰ����)
        /// </summary>
        /// <returns></returns>
        public T AutoDependency<T>() where T : class, new()
        {
            if (_dictionary.TryGetValue(typeof(T), out var value1))
            {
                return value1 as T;
            }
            var value = new T();
            _dictionary.Add(typeof(T), value);
            return value;
        }

        /// <summary>
        ///     ȡ��һ�����͵���չ����(��Ҫ����)
        /// </summary>
        /// <returns></returns>
        public T Dependency<T>() where T : class
        {
            return _dictionary.TryGetValue(typeof(T), out var value1) ? value1 as T : null;
        }

        /// <summary>
        ///     ȡ��һ�����͵���չ����(��Ҫ����)
        /// </summary>
        /// <returns></returns>
        public T Dependency<T>(Func<T> def) where T : class
        {
            if (!_dictionary.TryGetValue(typeof(T), out var value1))
            {
                _dictionary.Add(typeof(T), value1 = def());
            }
            return value1 as T;
        }
    }
}