using System;

namespace FirstGame
{
    //Class with all sound effects and backsounds
    public class MusicEffects
    {
        //Itialization game sounds
        static System.Windows.Media.MediaPlayer menuMusic = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer gameMusic = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer bossMusic = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer hitSound = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer shotSound = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer bossShotSound = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer bonusSound = new System.Windows.Media.MediaPlayer();
        static System.Windows.Media.MediaPlayer gameOverSound = new System.Windows.Media.MediaPlayer();

        //Event of ending backsound
        private static void Media_Ended(object sender, EventArgs e)
        {
            if (menuMusic.NaturalDuration <= menuMusic.Position)
            {
                menuMusic.Position = TimeSpan.Zero;
                menuMusic.Play();
            }

            if (gameMusic.NaturalDuration <= gameMusic.Position)
            {
                gameMusic.Position = TimeSpan.Zero;
                gameMusic.Play();
            }
        }

        //Sounds
        static public void BackMusic()
        {
            menuMusic.Stop();
            menuMusic.Close();
            gameMusic.Open(new System.Uri("Content\\Sounds\\MusicBack.mp3", UriKind.Relative));
            gameMusic.Volume = 0.1;
            gameMusic.MediaEnded += new EventHandler(Media_Ended);
            gameMusic.Play();
        }

        static public void BossFightMusic()
        {
            bossMusic.Open(new System.Uri("Content\\Sounds\\BossStart.mp3", UriKind.Relative));
            bossMusic.Volume = 0.1;
            bossMusic.Play();
        }

        static public void HitSound()
        {
            hitSound.Open(new System.Uri("Content\\Sounds\\Hit.mp3", UriKind.Relative));
            hitSound.Volume = 0.1;
            hitSound.Play();
        }

        static public void ShotSound()
        {
            shotSound.Open(new System.Uri("Content\\Sounds\\Shot.mp3", UriKind.Relative));
            shotSound.Volume = 0.1;
            shotSound.Play();
        }

        static public void BossShotSound()
        {
            bossShotSound.Open(new System.Uri("Content\\Sounds\\PowerShot.mp3", UriKind.Relative));
            bossShotSound.Volume = 0.1;
            bossShotSound.Play();
        }

        static public void BonusSound()
        {
            bonusSound.Open(new System.Uri("Content\\Sounds\\Bonus.mp3", UriKind.Relative));
            bonusSound.Volume = 0.2;
            bonusSound.Play();
        }

        //Initialization sounds at GameFunctional
        static public void MusicInitialization()
        {
            menuMusic.Open(new System.Uri("Content\\Sounds\\EricSkiffUnderclocked.mp3", UriKind.Relative));
            menuMusic.Volume = 0.1;
            menuMusic.MediaEnded += new EventHandler(Media_Ended);
            menuMusic.Play();
        }

        //Initialization sounds at GameFunctional
        static public void MusicStopMethod()
        {
            gameMusic.Stop();
            gameOverSound.Open(new System.Uri("Content\\Sounds\\GameOver.mp3", UriKind.Relative));
            gameOverSound.Volume = 0.1;
            gameOverSound.Play();
        }
    }
}
