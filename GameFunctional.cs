using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public static class GameFunctional
    {
        #region [Initialization data]
        //Delegete,event and value for bullets counting
        public delegate void Count();
        public static event Count CountPlus;
        public static event Count Shot; //Needed if change autofire to press some button
        static int counter;
        public static bool startGame;

        //Class fields 
        static Ship ship = new Ship(new Point(30, 450), new Point(3, 3), new Size(20, 15));
        static List<Bullet> bullets = new List<Bullet>();
        static int index = 0; //Bullet control (2 types of bullet)

        //The initial number of objects with the ability to change them during the game
        static bool controlUp, controlDown, controlRight, controlLeft;
        static int initialNumberOfAsteroids = 3;
        static int initialNumberOfStars = 1;
        static int initialNumberOfRockets = 1;
        public static int bonusTime = 1;
        const int scoreForBonus = 300;
        public static bool isBossFight = false; //boss flag

        //Random number generation and timers for screen refresh, points, automatic player and boss shots
        static Timer timer = new Timer();
        static Timer scoreTimer = new Timer();
        static Timer shotTimer = new Timer();
        public static Timer bossShotTimer = new Timer();
        static Timer startGameMenu = new Timer();
        //static public Random rnd = new Random();

        //Buffers that change and output
        static BufferedGraphicsContext context;
        static public BufferedGraphics buffer;

        //Properties
        //Height and width of the playing field
        static public int Width { get; private set; }
        static public int Height { get; private set; }

        //Constructor
        static GameFunctional()
        {
        }
        #endregion

        //Initialization game screen
        static public void Initialization(Form GameScreen)
        {
            MusicEffects.MusicInitialization();
            Controler.ControlerInitialization();

            Graphics grx;
            context = BufferedGraphicsManager.Current;
            grx = GameScreen.CreateGraphics();
            Width = GameScreen.Width;
            Height = GameScreen.Height;
            buffer = context.Allocate(grx, new Rectangle(0, 0, Width, Height));
            Load();     //Initial loading of objects
            Timer();    //Initialize and run all timers
            startGameMessage();

            //Events
            GameScreen.KeyDown += GameScreen_KeyDown;   //Ship control
            GameScreen.KeyUp += GameScreen_KeyUp;
            Ship.MessageDie += Finish;                  //End Game
            CountPlus += Game_Counter;                  //Bullet count
            Shot += ShotTimer_Tick;
        }

        private static void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                controlUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                controlDown = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                controlLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                controlRight = false;
            }
        }

        //Control key exception handler
        private static void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            //Start game
            if (!startGame)
            {
                Clear();
                Load();
                MusicEffects.BackMusic();
                startGame = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                controlUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                controlDown = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                controlLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                controlRight = true;
            }
        }

        //Counting function
        static void Game_Counter()
        {
            if (startGame)
            {
                counter++;
            }
        }

        //Game timer
        static public void Timer()
        {
            //Screen refresh rate
            timer.Interval = 10;
            timer.Start();
            timer.Tick += Timer_Tick;

            //Time of additional points during survival
            scoreTimer.Interval = 5000;
            scoreTimer.Start();
            scoreTimer.Tick += ScoreTimer_Tick;

            //Automatic firing rate
            shotTimer.Interval = 1000;
            shotTimer.Start();
            if (Controler.IsAutoFire)
            {
                shotTimer.Tick += ShotTimer_Tick;
            }

            //Automatic boss firing rate
            bossShotTimer.Interval = 1000;
            bossShotTimer.Start();
            bossShotTimer.Tick += BossShotTimer_Tick;
        }

        static public void startGameMessage()
        {
            //Game menu
            startGameMenu.Interval = 1;
            startGameMenu.Start();
            startGameMenu.Tick += StartGameMenu_Tick;
        }

        private static void StartGameMenu_Tick(object sender, EventArgs e)
        {
            if (startGame) startGameMenu.Stop();
            buffer.Graphics.DrawString(
                "Press any key to start", new Font(FontFamily.GenericSansSerif, 30),
                Brushes.White, Width / 3, Height / 4 * 3);
            buffer.Render();
        }

        //Ship bullet exception handler
        private static void ShotTimer_Tick(object sender, EventArgs e)
        {
            if (startGame)
            {
                MusicEffects.ShotSound();

                //Shot mechanic (Lvl - ship level and number of bullets according to the standard principle)
                int numberOfBullets = ship.Lvl;

                //Determining type of bullets
                if (numberOfBullets > 6) numberOfBullets = 6;
                bool superBullet = false;
                if (numberOfBullets > 3)
                {
                    superBullet = true;
                    numberOfBullets -= 3;
                }

                //Generating bullets according to ship lvl
                for (int i = -numberOfBullets / 2; i <= numberOfBullets / 2; i++)
                {
                    //First type of bullets
                    if (numberOfBullets % 2 == 0 && i == 0 && numberOfBullets != 0) { continue; }
                    if (superBullet) bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + i * 8 + ship.Size / 2 - 1), new Point(4, 0), new Size(6, 2)));

                    //Second type of bullets
                    else bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + ship.Size / 2 - 1), new Point(4, i), new Size(4, 1)));

                    //Data transfer to constructor
                    bullets[index].Lvl = ship.Lvl;
                    index++;
                }
                CountPlus();
            }
        }

        public static void ShotTimer_Tick()
        {
            if (startGame)
            {
                MusicEffects.ShotSound();
                //Shot mechanic (Lvl - ship level and number of bullets according to the standard principle)
                int numberOfBullets = ship.Lvl;

                //Determining type of bullets
                if (numberOfBullets > 6) numberOfBullets = 6;
                bool superBullet = false;
                if (numberOfBullets > 3)
                {
                    superBullet = true;
                    numberOfBullets -= 3;
                }

                //Generating bullets according to ship lvl
                for (int i = -numberOfBullets / 2; i <= numberOfBullets / 2; i++)
                {
                    //First type of bullets
                    if (numberOfBullets % 2 == 0 && i == 0 && numberOfBullets != 0) { continue; }
                    if (superBullet)
                    {
                        bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + i * 8 + ship.Size / 2 - 1), new Point(4, 0), new Size(6, 2)));
                    }

                    //Second type of bullets
                    else bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + ship.Size / 2 - 1), new Point(4, i), new Size(4, 1)));

                    //Data transfer to constructor
                    bullets[index].Lvl = ship.Lvl;
                    index++;
                }
                CountPlus();
            }
        }

        //Extra points exception handler
        private static void ScoreTimer_Tick(object sender, EventArgs e)
        {
            if (startGame)
            {
                ship.ScoreUp(10);
            }
        }

        //Boss shot exception handler
        private static void BossShotTimer_Tick(object sender, EventArgs e)
        {
            if (isBossFight)  //Is boss
            {
                int numberOfBullets = 2; //parameter number of bullets per tick
                EnemyBullets.LoadObjects(Boss.boss.PosX, Boss.boss.PosY, Boss.boss.Size, numberOfBullets);

            }
        }

        //Screen refresh exception handler
        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (controlUp)
            {
                ship.Up();
            }
            if (controlDown)
            {
                ship.Down();
            }
            if (controlLeft)
            {
                ship.Left();
            }
            if (controlRight)
            {
                ship.Right();
            }
            //Force call garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Drawing();
            Update();
        }

        //Display all objects
        static public void Drawing()
        {
            //Static (non-interacting objects)
            buffer.Graphics.Clear(Color.Black);
            Background.BackGround();
            if (Controler.IsControlerConnected)
            {
                Controler.SerialPort.WriteLine(ship.Score.ToString());
                Controler.controlerOperation(ship);
                if (Controler.IsShot)
                {
                    ShotTimer_Tick();
                    Controler.IsShot = false;
                }
            }

            //Dynamic
            foreach (Rocket obj in Rocket.rockets)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (Asteroid obj in Asteroid.asteroids)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (Star obj in Star.stars)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (Bullet bullet in bullets)
            {
                if (startGame) bullet.Drawing();
            }
            foreach (BonusUp obj in BonusUp.bonusUp)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (AsteroidCharge obj in AsteroidCharge.asteroidCharges)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (VisualEffect obj in VisualEffect.visualEffects)
            {
                if (obj != null) obj.Drawing();
            }
            /*foreach(Boss obj in Boss.boss)
            {
                if (obj != null) obj.Drawing();
            }*/
            if (Boss.boss != null)
            {
                Boss.boss.Drawing();
            }
            foreach (EnemyBullets obj in EnemyBullets.enemyBullets)
            {
                if (obj != null) obj.Drawing();
            }
            if (startGame) ship.Drawing();

            //Output game information (HP of the ship, number of points, number of shots)
            Background.DisplayOutputGameInformation(buffer, ship, ref counter);
        }

        //Changing the status of objects
        static public void Update()
        {
            //The end
            if (ship.Energy <= 0)
            {
                ship.Energy = 0;
                ship.Die();
            }

            //Asteroids
            {
                Asteroid.Interaction(bullets, ship, ref index);
                int difference = initialNumberOfAsteroids - Asteroid.asteroids.Count;
                if (difference > 0 && !isBossFight)
                {
                    Asteroid.LoadObjects(difference);
                }
            }

            //Asteroids charges
            {
                AsteroidCharge.Interaction(bullets, ship, ref index);
            }

            //Stars
            {
                Star.Interaction(bullets, ship, ref index);
                if (Star.stars.Count == 0 && !isBossFight)
                {
                    Star.LoadObjects(initialNumberOfStars);
                    initialNumberOfStars++;
                }
            }

            //Rocket
            {
                Rocket.Interaction(bullets, ship, ref index);
                if (Rocket.rockets.Count == 0 && !isBossFight)
                {
                    Rocket.LoadObjects(initialNumberOfRockets);
                    initialNumberOfRockets++;
                }
            }

            //Bonus
            {
                BonusUp.Interaction(ship);
                if (scoreForBonus * bonusTime < ship.Score)
                {
                    BonusUp.LoadObject();
                    bonusTime++;
                }
            }

            //Boss
            {
                Boss.Interaction(bullets, ship, ref index);

                if (!isBossFight && ship.BossTime >= 60)
                {
                    Boss.LoadObjects(ship);
                    MusicEffects.BossFightMusic();
                }
            }

            //Enemy Bullets
            {
                EnemyBullets.Interaction(ship);
            }

            //Visual Effects
            {
                VisualEffect.Interaction(ship);
            }

            //"Clearing the memory" of bullets that out of the playing field
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].PosX > Width)
                {
                    bullets.RemoveAt(i);
                    i--;
                    index--;
                }
            }

            //Update bullet and "visual effects"positions 
            foreach (Bullet bullet in bullets)
            {
                if (startGame) bullet.Update();
            }
        }

        //Create and upload objects to the screen
        static public void Load()
        {
            Asteroid.LoadObjects(initialNumberOfAsteroids);
            Star.LoadObjects(initialNumberOfStars);
            Rocket.LoadObjects(initialNumberOfRockets);
            //Boss.LoadObjects(ship);
            //Boss.RemoveObjectFromCollection();
            isBossFight = false;
        }

        static public void Clear()
        {
            Asteroid.asteroids.Clear();
            Star.stars.Clear();
            Rocket.rockets.Clear();
        }

        //End game method
        static public void Finish()
        {
            //Ship ремув, спавн віжуал ефектів з специфічним кольором, звук кабуму, запуск таймеру на 5 сек, після чого кол оцього
            //Stop all timers
            timer.Stop();
            scoreTimer.Stop();
            shotTimer.Stop();
            bossShotTimer.Stop();

            MusicEffects.MusicStopMethod();
            //Display information
            Background.DisplayFinishGameStats(buffer, ship);
            //ScoreMenu
            if (MessageBox.Show("Want to save your record at scoreboard?", "Asteroids belt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ScoreMenu scoreMenu = new ScoreMenu(350, 200, ship.Score);
                scoreMenu.ShowDialog();
            }
            Application.Exit();
        }
    }
}
