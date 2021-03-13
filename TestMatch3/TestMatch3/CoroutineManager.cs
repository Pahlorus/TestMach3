using MatchCore;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TestMatch3
{
    public class CoroutineManager
    {
        private TestMatch3Game _game;
        public static GameTime Time { get; private set; }
        public static List<Coroutine> _coroutines;

        public CoroutineManager(TestMatch3Game game)
        {
            _game = game;
            _game.OnUpdate += Update;
            _coroutines = new List<Coroutine>();
        }

        public static Coroutine StartCoroutine(object obj, IEnumerator<float> coroutine)
        {
            var newCoroutine = new Coroutine(obj, coroutine);
            _coroutines.Add(newCoroutine);
            return newCoroutine;
        }

        public void Stop(Coroutine coroutine)
        {
            coroutine.IsDone = true;
        }
        
        public void Wait(float seconds, Action callBack=null)
        {
            StartCoroutine(seconds, WaitCoroutine(seconds, callBack));
        }

        private static IEnumerator<float> WaitCoroutine(float seconds, Action callBack=null)
        {
            yield return seconds;
            callBack?.Invoke();
        }

        public void Update(GameTime gameTime)
        {
            Time = gameTime;
            for (int i=0; i< _coroutines.Count;i++)
            {
                _coroutines[i].Update(gameTime);
            }
            _coroutines.RemoveAll(cor => cor.IsDone);
        }
    }
}