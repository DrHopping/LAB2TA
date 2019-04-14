using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB2TA
{

    public class HashTable<TKey, TValue>
    {
        private LinkedList<Tuple<TKey, TValue>>[] _items;
        private int _fillFactor = 1;
        private int _size = 4;
        private int _elements;
        public int count = 0;

        public HashTable()
        {
            _items = new LinkedList<Tuple<TKey, TValue>>[4];
        }

        private HashTable(int length)
        {
            _items = new LinkedList<Tuple<TKey, TValue>>[length];
            _size = length;
        }

        public void Add(TKey key, TValue value)
        {
            var pos = GetPosition(key);
            if (_items[pos] == null)
            {
                _items[pos] = new LinkedList<Tuple<TKey, TValue>>();
            }
            if (_items[pos].Any(x => x.Item1.Equals(key)))
            {
                throw new Exception("Duplicate key, cannot insert.");
            }
            _elements++;
            if (NeedToGrow())
            {
                GrowAndReHash();
            }
            pos = GetPosition(key);
            if (_items[pos] == null)
            {
                _items[pos] = new LinkedList<Tuple<TKey, TValue>>();
            }
            _items[pos].AddFirst(new Tuple<TKey, TValue>(key, value));
        }

        public void Remove(TKey key)
        {
            var pos = GetPosition(key);
            if (_items[pos] != null)
            {
                var objToRemove = _items[pos].FirstOrDefault(item => item.Item1.Equals(key));
                if (objToRemove == null) return;
                _items[pos].Remove(objToRemove);
                _elements--;
            }
            else
            {
                throw new Exception("Value not in HashTable.");
            }
        }

        public TValue Get(TKey key)
        {
            var pos = GetPosition(key);
            //foreach (var item in _items[pos].Where(item => item.Item1.Equals(key)))
            //{
            //    return item.Item2;
            //}
            for (int i = 0; i < _items[pos].Count; i++)
            {
                count++;
                if (_items[pos].ElementAt(i).Item1.Equals(key))
                    return _items[pos].ElementAt(i).Item2;
            }

            throw new Exception("Key does not exist in HashTable.");
        }

        private void GrowAndReHash()
        {
            _size = _size * 2;
            var newHashTable = new HashTable<TKey, TValue>(_size);
            foreach (var item in _items.Where(x => x != null))
            {
                foreach (var value in item)
                {
                    newHashTable.Add(value.Item1, value.Item2);
                }
            }
            _items = newHashTable._items;
        }

        public int GetPosition(TKey key)
        {
            var hash = key.GetHashCode();
            var pos = Math.Abs(hash);
            pos = (int)(_size * ((pos * 0.618033) % 1));
            return pos;
        }

        private bool NeedToGrow()
        {
            return _elements/_size >= _fillFactor;
        }
    }

    class Program
    {

        static Random rand = new Random();

        static int HashFunction(int k)
        {
            int N = 4;
            double A = 0.618033;
            int h = (int)(N * ((k * A) % 1));
            return h;
        }

        static string RandElement(int length)
        {
            var Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[length];
            stringChars[0] = Chars[rand.Next(Chars.Length)];
            for (int i = 1; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }

            return new String(stringChars);

        }

        static void Main(string[] args)
        {
            var hashTable = new HashTable<int, string>();
            hashTable.Add(21037, "Pirogova");
            int elements = 100;
            for (int i = 0; i < elements; i++)
            {
                var element = RandElement(100);
                hashTable.Add(element.GetHashCode(), element);
            }

            Console.WriteLine("Number of elements: " + elements);
            Console.WriteLine("Insert Key to find: 21037");
            Console.WriteLine("Value with key 21037:" + hashTable.Get(21037));
            Console.WriteLine("Number of hits: " + 1);
            Console.WriteLine("Number of comparisons: " + hashTable.count);
            Console.ReadLine();
        }
    }
}
