﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace FellSky.Services
{
    public class CoroutineService : ICoroutineService
    {
        private LinkedList<Coroutine> Coroutines = new LinkedList<Coroutine>();

        public Coroutine StartCoroutine(IEnumerable routine)
        {
            var r = new Coroutine(routine);
            Coroutines.AddLast(r);
            r.Resume();
            return r;
        }

        public void RunCoroutines(TimeSpan deltaTime)
        {
            LinkedListNode<Coroutine> node = Coroutines.First;
            while (node != null)
            {
                var thisNode = node;
                node = node.Next;

                if (thisNode.Value.Status == CoroutineStatus.Stopped)
                {
                    Coroutines.Remove(thisNode);
                    continue;
                }

                if (!thisNode.Value.Run(deltaTime))
                {
                    Coroutines.Remove(thisNode);
                    if (thisNode.Value.OnDone != null)
                        thisNode.Value.OnDone();
                }
            }
        }
    }

}
