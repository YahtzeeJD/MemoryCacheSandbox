using System;
using System.Collections;
using System.Runtime.Caching;

namespace MemoryCacheSandbox
{
    public class Program
    {
        private static MemoryCache _cache = new MemoryCache("CacheExample");
        private static readonly CacheItemPolicy _policy = new CacheItemPolicy();

        static void Main(string[] args)
        {
            _cache = MemoryCache.Default;
            _policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1);

            Console.WriteLine("Adding 3 items to cache");

            AddObjectToCache("John", 1.23, 50);
            AddObjectToCache("Paul", 4.56, 75);
            AddObjectToCache("George", 7.89, 32);

            //if (!_cache.Contains(cachedObject.Name))
            //{
            //    CacheItemPolicy policy = new CacheItemPolicy();
            //    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1);
            //    _cache.Set(cachedObject.Name, cachedObject, policy);
            //}

            Console.WriteLine();
            Console.WriteLine("Getting the count of cache items");

            var countOfCachedItems = _cache.GetCount();
            Console.WriteLine($"Count of cached items = {countOfCachedItems}");

            Console.WriteLine();
            Console.WriteLine("Getting John from cache");

            var cachedObject = _cache.Get("John") as Person;
            Console.WriteLine($"Name = {cachedObject.Name}, Salary = {cachedObject.Salary}, Age = {cachedObject.Age}");

            Console.WriteLine();
            Console.WriteLine("Getting George from cache");

            cachedObject = _cache.Get("George") as Person;
            Console.WriteLine($"Name = {cachedObject.Name}, Salary = {cachedObject.Salary}, Age = {cachedObject.Age}");

            Console.WriteLine();
            Console.WriteLine("Getting all cache items using GetEnumerator");

            IDictionaryEnumerator cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)_cache).GetEnumerator();
            while (cacheEnumerator.MoveNext())
                Console.WriteLine($"{((Person)cacheEnumerator.Value).Name}");

            Console.WriteLine();
            Console.WriteLine("Getting all cache items using foreach");

            foreach (var cacheItem in _cache)
            {
                var person = cacheItem.Value as Person;
                Console.WriteLine($"Name = {person.Name}, Salary = {person.Salary}, Age = {person.Age}");
            }

            Console.ReadLine();
        }

        private static void AddObjectToCache(string name, double salary, int age)
        {
            var cachedObject = new Person(name, salary, age);
            //_cache.AddOrGetExisting(cachedObject.Name, cachedObject, _policy);
            _cache.Set(cachedObject.Name, cachedObject, _policy);
            Console.WriteLine($"Caching item with the key {cachedObject.Name}");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public int Age { get; set; }

        public Person(string name, double salary, int age)
        {
            Name = name;
            Salary = salary;
            Age = age;
        }
    }
}
