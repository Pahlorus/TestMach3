using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MatchCore
{
    public class Coroutine
    {
        public object Object { get; }
        public bool IsDone { get; set; }
        public float Delay { get; set; }
        public IEnumerator<float> RoutineEnum { get; set; }

        public Coroutine(object obj, IEnumerator<float> routineEnum)
        {
            Object = obj;
            RoutineEnum = routineEnum;
        }

        public void Update(GameTime gameTime)
        {
            if (Object == null)
            {
                IsDone = true;
                return;
            }

            if (RoutineEnum.MoveNext())
            {
                Delay = RoutineEnum.Current;
                if (Delay > 0f || Mathf.Approximately(Delay, 0f)) return;

            }
            IsDone = true;
        }
    }
}
