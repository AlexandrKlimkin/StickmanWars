using System;
using System.Collections.Generic;

namespace Tools.BehaviourTree {
    [Serializable]
    public class Blackboard {
        private readonly Dictionary<Type, BlackboardData> _Container = new Dictionary<Type, BlackboardData>();

        public T Get<T>() where T : BlackboardData, new() {
            return Get(typeof(T)) as T;
        }

        public BlackboardData Get(Type type) {
            if (_Container.TryGetValue(type, out var result))
                return result;
            var instance = Activator.CreateInstance(type) as BlackboardData;
            Set(type, instance);
            return instance;
        }

        public void Set<T>(T parameter) where T : BlackboardData {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            Set(parameter.GetType(), parameter);
        }

        public void Set(Type type, BlackboardData parameter) {
            if (_Container.ContainsKey(type)) {
                _Container[type] = parameter;
            }
            else {
                _Container.Add(type, parameter);
            }
        }
    }
}