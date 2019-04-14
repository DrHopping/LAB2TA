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
        private int _fillFactor = 3;
        private int _size;

        public HashTable()
        {
            _items = new LinkedList<Tuple<TKey, TValue>>[4];
        }

        public void Add(TKey key, TValue value)
        {
            var pos = GetPosition(key, _items.Length);
            if (_items[pos] == null)
            {
                _items[pos] = new LinkedList<Tuple<TKey, TValue>>();
            }
            if (_items[pos].Any(x => x.Item1.Equals(key)))
            {
                throw new Exception("Duplicate key, cannot insert.");
            }
            _size++;
            if (NeedToGrow())
            {
                GrowAndReHash();
            }
            pos = GetPosition(key, _items.Length);
            if (_items[pos] == null)
            {
                _items[pos] = new LinkedList<Tuple<TKey, TValue>>();
            }
            _items[pos].AddFirst(new Tuple<TKey, TValue>(key, value));
        }

        public void Remove(TKey key)
        {
            var pos = GetPosition(key, _items.Length);
            if (_items[pos] != null)
            {
                var objToRemove = _items[pos].FirstOrDefault(item => item.Item1.Equals(key));
                if (objToRemove == null) return;
                _items[pos].Remove(objToRemove);
                _size--;
            }
            else
            {
                throw new Exception("Value not in HashTable.");
            }
        }

        public TValue Get(TKey key)
        {
            var pos = GetPosition(key, _items.Length);
            foreach (var item in _items[pos].Where(item => item.Item1.Equals(key)))
            {
                return item.Item2;
            }
            throw new Exception("Key does not exist in HashTable.");
        }

        private void GrowAndReHash()
        {
            _fillFactor *= 2;
            var newItems = new LinkedList<Tuple<TKey, TValue>>[_items.Length * 2];
            foreach (var item in _items.Where(x => x != null))
            {
                foreach (var value in item)
                {
                    var pos = GetPosition(value.Item1, newItems.Length);
                    if (newItems[pos] == null)
                    {
                        newItems[pos] = new LinkedList<Tuple<TKey, TValue>>();
                    }
                    newItems[pos].AddFirst(new Tuple<TKey, TValue>(value.Item1, value.Item2));
                }
            }
            _items = newItems;
        }

        private int GetPosition(TKey key, int length)
        {
            var hash = key.GetHashCode();
            var pos = Math.Abs(hash);
            pos = (int)(_size * ((pos * 0.618033) % 1));
            return pos;
        }

        private bool NeedToGrow()
        {
            return _size >= _fillFactor;
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
            int elements = 10;
            for (int i = 0; i < elements; i++)
            {
                var element = RandElement(8);
                hashTable.Add(i, element);
            }

            Console.WriteLine(hashTable.Get(10));
            Console.ReadLine();
        }
    }
}
