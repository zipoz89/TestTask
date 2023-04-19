using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Scripts
{
    [System.Serializable]
    public class AnimalNeed
    {
        [SerializeField] private string name;
        public string Name => name;
        public AnimalAction[] ActionChain;
        public Action OnNeedFulfiled;

        private int currentChain = -1;
        
        public void ProgressActionChain()
        {
            currentChain++;
            
            if (currentChain == ActionChain.Length)
            {
                currentChain = -1;
                OnNeedFulfiled.Invoke();
                return;
            }
            ActionChain[currentChain].actionEvent.Invoke();
        }
    }

    [System.Serializable]
    public class AnimalAction
    {
        public UnityEvent actionEvent;
    }
    
}
