using System;
using System.Collections.Generic;
using System.Text;

namespace quiz
{
    public interface ISoundEffect
    {
        void HitSound();
        void CorrectSound();
        void IncorrectSound();
        void ResultSound();
    }
}
