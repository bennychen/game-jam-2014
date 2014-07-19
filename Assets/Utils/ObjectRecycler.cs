using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fangtang.Utils
{
	public class ObjectRecycler
	{
#if UNITY_EDITOR
        public static bool WarningForInsufficientCapacityAtRuntime { get; set; }
#endif

		public ObjectRecycler(GameObject gameObject, int initialCapacity, 
				GameObject parentObject, Action<GameObject> onInstantiate)
		{
			_objectList = new List<RecyclableObject>(initialCapacity);
			_objectToRecycle = gameObject;
            _parentObject = parentObject;
            OnInstantiate = onInstantiate;

			for (int i = 0; i < initialCapacity; i++)
			{
                InstantiateNewRecyclable();
			}
		}

        public ObjectRecycler(GameObject gameObject, int initialCapacity,
            GameObject parentObject)
            : this(gameObject, initialCapacity, parentObject, null) { }

		public GameObject GetNextFree(bool dontInstantiateNew = false)
		{
            RecyclableObject freeObject = null;

            for (int i = 0; i < _objectList.Count; i++)
            {
                if (_objectList[i].IsAvailabe == true)
                {
                    freeObject = _objectList[i];
                    break;
                }
            }

			if (freeObject == null && !dontInstantiateNew)
			{
#if UNITY_EDITOR
                if (WarningForInsufficientCapacityAtRuntime && Application.isPlaying)
                {
                    Debug.LogWarning(string.Format("Pool size for [{0}] is not sufficient, and instantiation is kicked out at run-time",
                        _objectToRecycle.name));
                }
#endif
                freeObject = InstantiateNewRecyclable();
			}

            if (freeObject != null)
            {
                freeObject.IsAvailabe = false;
                return freeObject.gameObject;
            }
            else
            {
                return null;
            }
        }

        public void FreeAllObjects()
        {
            foreach (RecyclableObject recyclable in _objectList)
            {
                recyclable.IsAvailabe = true;
            }
        }

        public void DestroyAllObjects()
        {
            foreach (RecyclableObject recyclable in _objectList)
            {
                if (recyclable != null)
                {
                    recyclable.gameObject.DestroyBasedOnRunning();
                }
            }
            if (_parentObject != null)
            {
                _parentObject.DestroyBasedOnRunning();
            }
            _objectList.Clear();
        }

		public static void FreeObject(GameObject objectToFree)
		{
            if (objectToFree.GetComponent<RecyclableObject>() != null)
            {
                objectToFree.GetComponent<RecyclableObject>().IsAvailabe = true;
            }
            else
            {
                Debug.LogError("Try to free an object that is not recyclable.");
            }
        }

        private RecyclableObject InstantiateNewRecyclable()
        {
            GameObject newGameObject = GameObject.Instantiate(_objectToRecycle) as GameObject;
            RecyclableObject recyclable = newGameObject.AddComponent<RecyclableObject>();

            if (_parentObject != null)
            {
                recyclable.transform.parent = _parentObject.transform;
            }
            recyclable.IsAvailabe = true;
            _objectList.Add(recyclable);

            if (OnInstantiate != null)
            {
                OnInstantiate(newGameObject);
            }

            return recyclable;
        }

        private Action<GameObject> OnInstantiate { get; set; }

		private List<RecyclableObject> _objectList;
		private GameObject _objectToRecycle;
		private GameObject _parentObject;
	}
}
