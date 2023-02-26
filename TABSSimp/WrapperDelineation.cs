using System;
using System.Collections.Generic;
using System.Linq;

namespace ModdingForDummies.TABSSimp
{
    public class WrapperDelineation<T> where T : ModdingClass<T>
    {
        private List<T> internalObject = new List<T>();

        private Action<List<T>> onUpdate;

        private Func<string, T> query;

        public void Add(T item)
        {
            internalObject.Add(item);
            onUpdate(internalObject);
        }

        public void Add(string itemName)
        {
            T item = query(itemName);
            internalObject.Add(item);
            onUpdate(internalObject);
        }

        public void Add(params T[] items)
        {
            foreach(var item in items)
            {
                internalObject.Add(item);
            }
            
            onUpdate(internalObject);
        }

        public void Add(params string[] itemNames)
        {
            foreach(var itemName in itemNames)
            {
                T item = query(itemName);
                internalObject.Add(item);
            }
            
            onUpdate(internalObject);
        }
        
        public void AddCloned(T item)
        {
            internalObject.Add(item.Clone());
            onUpdate(internalObject);
        }

        public void AddCloned(string itemName)
        {
            T item = query(itemName);
            internalObject.Add(item.Clone());
            onUpdate(internalObject);
        }

        public void AddCloned(params T[] items)
        {
            foreach(var item in items)
            {
                internalObject.Add(item.Clone());
            }
            
            onUpdate(internalObject);
        }

        public void AddCloned(params string[] itemNames)
        {
            foreach(var itemName in itemNames)
            {
                T item = query(itemName);
                internalObject.Add(item.Clone());
            }
            
            onUpdate(internalObject);
        }

        public void Remove(T item)
        {
            internalObject.Remove(item);
            onUpdate(internalObject);
        }

        public void Remove(string itemName)
        {
            internalObject.Remove(internalObject.Where(t => itemName == Utilities.GetProperName(t.Name)).FirstOrDefault());
            onUpdate(internalObject);
        }

        public void RemoveAll(T item)
        {
            internalObject.RemoveAll(i => i == item);
            onUpdate(internalObject);
        }

        public void RemoveAll(string itemName)
        {
            internalObject.Where(t => itemName == Utilities.GetProperName(t.Name)).ForEach(t => internalObject.Remove(t));
            onUpdate(internalObject);
        }
        
        public void Clear()
        {
            internalObject = new List<T>();
            onUpdate(internalObject);
        }

        private List<T> ListGet() => (from T item in internalObject select item).ToList();

        private void ListSet(List<T> list)
        {
            internalObject.Clear();
            foreach(T item in list) internalObject.Add(item);
            onUpdate(internalObject);
        }

        public List<T> List
        {
            get => ListGet();
            set => ListSet(value);
        }

        public WrapperDelineation(Action<List<T>> onUpdate, Func<string, T> query)
        {
            this.onUpdate = onUpdate;
            this.query = query;
        }
    }
}
