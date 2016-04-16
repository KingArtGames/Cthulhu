using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class CoroutineService : MonoBehaviour
    {
        public void RunAsync(IEnumerator operation)
        {
            StartCoroutine(operation);
        }
    }
}
