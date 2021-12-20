using System;
using System.Reflection;

namespace Singelton
{
    [Serializable]
    public class SingletonException : Exception
    {
        public SingletonException(string message) : base(message) { }
        public SingletonException(string message, Exception exception) : base(message, exception) { }
    }

    public abstract class Singleton<T> where T : Singleton<T>
    {
        static Singleton() { }
        protected Singleton() { }

        class Nested
        {
            internal static volatile T _instatnce = null;
            internal static readonly object _lock = new object();
        }
        public static T Instance
        {
            get
            {
                if (Nested._instatnce == null)
                {
                    lock (Nested._lock)
                    {
                        if (Nested._instatnce == null)
                        {
                            Type type = typeof(T);

                            if (type == null || !type.IsSealed)
                            {
                                throw new SingletonException($"{type.Name} must be a seald class");
                            }

                            ConstructorInfo ctor = null;
                            try
                            {
                                ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                           null, Type.EmptyTypes, null);
                            }
                            catch (Exception exception)
                            {
                                throw new SingletonException($"A private/protected constructor is missing for {type.Name}", exception);
                            }

                            if (ctor == null || ctor.IsAssembly)
                            {
                                throw new SingletonException($"A private/protected constructor is missing for {type.Name}");
                            }

                            Nested._instatnce = (T)ctor.Invoke(null);
                        }
                    }
                }
                return Nested._instatnce;
            }
        }
    }
}
