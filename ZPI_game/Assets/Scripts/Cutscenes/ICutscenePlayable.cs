using System.Collections;
using System.Collections.Generic;

namespace Cutscenes
{
    public interface ICutscenePlayable
    {
        public IEnumerator Play();
    }
}