using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class WaitForEndOfLifecycleStep : CustomYieldInstruction
    {
        private bool _ended = false;
        private GameProcessor _processor;

        public WaitForEndOfLifecycleStep (GameProcessor processor)
        {
            _processor = processor;
            _processor.AddLifecycleStepEndedListener(StepEnded);
        }

        public override bool keepWaiting
        {
            get
            {
                return _ended;
            }
        }

        public void StepEnded()
        {
            _ended = true;
        }

        public class Factory : Factory<WaitForEndOfLifecycleStep> { }
    }
}
