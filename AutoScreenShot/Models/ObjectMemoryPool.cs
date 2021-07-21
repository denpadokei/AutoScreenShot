﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AutoScreenShot.Models
{
    public class ObjectMemoryPool<T> where T : MonoBehaviour
    {
        private ConcurrentBag<T> _objects = new ConcurrentBag<T>();
        private Action<T> ReInitialize;
        private Action<T> Initialize;
        public T Spawn()
        {
            if (this._objects.TryTake(out var result)) {
                this.ReInitialize?.Invoke(result);
                result.gameObject.SetActive(true);
                return result;
            }
            else {
                result = new GameObject(nameof(T), typeof(T)).GetComponent<T>();
                this.Initialize?.Invoke(result);
                this.ReInitialize?.Invoke(result);
                result.gameObject.SetActive(true);
                return result;
            }
        }

        public void Despawn(T item)
        {
            item.gameObject.SetActive(false);
            this._objects.Add(item);
        }

        public ObjectMemoryPool(Action<T> init, Action<T> reInit, int size) 
        {
            this.ReInitialize = reInit;
            this.Initialize = init;
            for (int i = 0; i < size; i++) {
                var item = new GameObject(nameof(T), typeof(T)).GetComponent<T>();
                this.Initialize?.Invoke(item);
                item.gameObject.SetActive(false);
                this._objects.Add(item);
            }
        }
    }
}