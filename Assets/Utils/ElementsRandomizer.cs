using System.Collections.Generic;
using UnityEngine;

namespace Fangtang.Utils
{
    class ElementsRandomizer<T>
    {
        public ElementsRandomizer(T[] elements, float[] probabilities, bool debug = false)
        {
            _elements = new List<T>();
            _probabilityBounds = new List<float>();
            _debug = debug;
            ResetElements(elements, probabilities);
        }

        public void ResetElements(T[] elements)
        {
            _elements.Clear();
            _probabilityBounds.Clear();
            _isEven = true;

            foreach (var element in elements)
            {
                _elements.Add(element);
            }
        }

        public void ResetElements(T[] elements, float[] probabilities)
        {
            if (elements.Length != probabilities.Length + 1)
            {
                Debug.LogError("(Count of probability numbers) must be (count of elements - 1)");
                return;
            }

            float sum = 0;
            foreach (var p in probabilities)
            {
                sum += p;
            }
            if (sum > 1)
            {
                Debug.LogError("Probabilities' sum must be less than 1");
                return;
            }

            _elements.Clear();
            _probabilityBounds.Clear();
            _isEven = false;

            string debugStr = "{ ";
            for (int i = 0; i < elements.Length; i++)
            {
                _elements.Add(elements[i]);

                if (i < elements.Length - 1)
                {
                    float probability = probabilities[i];
                    _probabilityBounds.Add(i == 0 ? probability : _probabilityBounds[i - 1] + probability);
                }
                else
                {
                    _probabilityBounds.Add(1);
                }

                debugStr += (i == 0 ? ("[0 - " + _probabilityBounds[0] + "] ") : 
                                      ("[" + _probabilityBounds[i - 1] + " - " + _probabilityBounds[i] + "] "));
            }
            debugStr += "}";

            if (_debug)
            {
                Debug.Log(debugStr);
            }
        }

        public T Get()
        {
            if (_elements.Count == 0)
            {
                return default(T);
            }

            if (_isEven)
            {
                return _elements[Random.Range(0, _elements.Count)];
            }
            else
            {
                float number = Random.Range(0f, 1f);
                if (number < _probabilityBounds[0])
                {
                    if (_debug)
                    {
                        Debug.Log("random number [" + number + "] is in range [0," +
                            _probabilityBounds[0] + "], return[" + _elements[0] + "]");
                    }
                    return _elements[0];
                }
                else
                {
                    for (int i = 1; i < _probabilityBounds.Count; i++)
                    {
                        if (number > _probabilityBounds[i - 1] &&
                            number <= _probabilityBounds[i])
                        {
                            if (_debug)
                            {
                                Debug.Log("random number [" + number + "] is in range [" +
                                    _probabilityBounds[i - 1] + "," + _probabilityBounds[i] +
                                    "], return[" + _elements[i] + "]");
                            }
                            return _elements[i];
                        }
                    }
                    Debug.LogError("Code should never run here");
                    return default(T);
                }
            }
        }

        private List<float> _probabilityBounds;
        private List<T> _elements;
        private bool _isEven;
        private bool _debug = false;
    }
}
