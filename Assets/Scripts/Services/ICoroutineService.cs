using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public interface ICoroutineService
    {
        void RunAsync(IEnumerator operation);
    }

    public class CoroutineService : MonoBehaviour, ICoroutineService
    {
        public void RunAsync(IEnumerator operation)
        {
            StartCoroutine(operation);
        }
    }
}
