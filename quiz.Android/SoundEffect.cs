using Android.Media;
using quiz;
using quiz.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Sound01.Droid.SoundEffect))]
namespace Sound01.Droid
{
    public class SoundEffect : ISoundEffect
    {
        SoundPool soundPool;
        int hitSoundId;
        int correctSoundId;
        int incorrectSoundId;
        int resultSoundId;

        public SoundEffect()
        {
            int SOUND_POOL_MAX = 6;

            AudioAttributes attr = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                .SetContentType(AudioContentType.Music)
                .Build();
            soundPool = new SoundPool.Builder()
               .SetAudioAttributes(attr)
               .SetMaxStreams(SOUND_POOL_MAX)
               .Build();
            hitSoundId = soundPool.Load(Android.App.Application.Context, Resource.Raw.touch, 1);
            correctSoundId = soundPool.Load(Android.App.Application.Context, Resource.Raw.maru, 1);
            incorrectSoundId = soundPool.Load(Android.App.Application.Context, Resource.Raw.batu, 1);
            resultSoundId = soundPool.Load(Android.App.Application.Context, Resource.Raw.result, 1);
        }

        public void HitSound()
        {
            soundPool.Play(hitSoundId, 1.0F, 1.0F, 0, 0, 1.0F);
        }

        public void CorrectSound()
        {
            soundPool.Play(correctSoundId, 1.0F, 1.0F, 0, 0, 1.0F);
        }

        public void IncorrectSound()
        {
            soundPool.Play(incorrectSoundId, 1.0F, 1.0F, 0, 0, 1.0F);
        }
        public void ResultSound()
        {
            soundPool.Play(resultSoundId, 1.0F, 1.0F, 0, 0, 1.0F);
        }
    }
}