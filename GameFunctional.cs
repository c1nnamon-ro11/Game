using System;
using System.Windows.Forms;
using System.Drawing;

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
        public static string texturePackPath = TexturePackLoader.EnteringTexturePack("Galaxy Highways");

        //The initial number of objects with the ability to change them during the game
        static bool controlUp, controlDown, controlRight, controlLeft, isShipDestroyed;
        static int initialNumberOfAsteroids = 5;
        static int initialNumberOfStars = 5;
        static int initialNumberOfRockets = 5;
        public static int bonusTime = 1;
        public static bool isBossFight = false; //boss flag
        public static bool isPlasmaWeapon;

        //Random number generation and timers for screen refresh, points, automatic player and boss shots
        static Timer timer = new Timer();
        static Timer scoreTimer = new Timer();
        static Timer shotTimer = new Timer();
        static Timer bossShotTimer = new Timer();
        static Timer startGameMenu = new Timer();
        static Timer destroyShip = new Timer();

        //Game fucntional consts
        private const int SCORE_FOR_BONUS = 500;
        private const int BOSS_SPAWN_TIME = 300;

        //Timer`s values
        private const int GAME_RATE = 10;
        private const int SCORE_ADDITION_RATE = 5000;
        private const int FIRING_RATE = 1000;
        private const int BOSS_FIRING_RATE = 1000;
        private const int GAME_MENU_RATE = 10;
        private const int DESTROY_SHIP = 3000;

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
            Controller.ControlerInitialization();

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
            Ship.MessageDie += Finish;                  //End Game
            if (!Controller.IsControlerConnected)
            {
                GameScreen.KeyDown += GameScreen_KeyDown;   //Ship control
                GameScreen.KeyUp += GameScreen_KeyUp;
            }
            CountPlus += Game_Counter;                  //Bullet count
            Shot += ShotTimer_Tick;
        }

        //Start game
        public static void GameStart()
        {
            if (!startGame)
            {
                Clear();
                Load();
                MusicEffects.BackMusic();
                startGame = true;
            }
        }

        //Keyboard ship control
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

        //Keyboard ship control
        private static void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            GameStart();
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

        //Counting number of shots
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
            timer.Interval = GAME_RATE;
            timer.Start();
            timer.Tick += Timer_Tick;

            //Time of additional points during survival
            scoreTimer.Interval = SCORE_ADDITION_RATE;
            scoreTimer.Start();
            scoreTimer.Tick += ScoreTimer_Tick;

            //Automatic firing rate
            shotTimer.Interval = FIRING_RATE;
            shotTimer.Start();
            if (Controller.IsAutoFire)
            {
                shotTimer.Tick += ShotTimer_Tick;
            }

            //Automatic boss firing rate
            bossShotTimer.Interval = BOSS_FIRING_RATE;
            bossShotTimer.Start();
            bossShotTimer.Tick += BossShotTimer_Tick;
        }

        //Game menu timer
        static public void startGameMessage()
        {
            //Game menu
            startGameMenu.Interval = GAME_MENU_RATE;
            startGameMenu.Start();
            startGameMenu.Tick += StartGameMenu_Tick;
        }

        //Start game menu
        private static void StartGameMenu_Tick(object sender, EventArgs e)
        {
            //Stop game menu, if game starts
            if (startGame)
            {
                startGameMenu.Stop();
            }
            //Visualize game menu
            buffer.Graphics.DrawString(
                "Press any key to start", new Font(FontFamily.GenericSansSerif, 30),
                Brushes.White, Width / 3, Height / 4 * 3);
            buffer.Render();
        }

        //Ship bullet exception handler
        private static void ShotTimer_Tick(object sender, EventArgs e)
        {
            ShotTimer_Tick();
        }

        //Firing method
        public static void ShotTimer_Tick()
        {
            if (startGame)
            {
                MusicEffects.ShotSound();
                //Shot mechanic (Lvl - ship level and number of bullets according to the standard principle)
                int numberOfBullets = Ship.ship.Lvl;
                bool isSuperBullet = false;
                //Determining type of bullets
                if (numberOfBullets > 6)
                {
                    numberOfBullets = 6;
                }
                if (numberOfBullets > 3)
                {
                    isSuperBullet = true;
                    numberOfBullets -= 3;
                }

                //Generating bullets according to ship lvl
                for (int i = -numberOfBullets / 2; i <= numberOfBullets / 2; i++)
                {
                    //First type of bullets
                    if (numberOfBullets % 2 == 0 && i == 0 && numberOfBullets != 0) { continue; }
                    if (isSuperBullet)
                    {
                        Bullet.bullets.Add(
                            new Bullet(
                                new Point(Ship.ship.Rect.X + Ship.ship.WidthSize/2, Ship.ship.Rect.Y + i * 8 + Ship.ship.HeightSize / 2 - 7), new Point(4, 0), new Size(25, 15), true));//6,2
                    }

                    //Second type of bullets
                    else
                    {
                        Bullet.bullets.Add(
                        new Bullet(
                            new Point(Ship.ship.Rect.X + Ship.ship.WidthSize / 2, Ship.ship.Rect.Y + Ship.ship.HeightSize / 2 - 7), new Point(4, i), new Size(14, 7), false)); //4,1
                    }
                }
                CountPlus();
            }
        }

        //Extra points exception handler
        private static void ScoreTimer_Tick(object sender, EventArgs e)
        {
            if (startGame)
            {
                Ship.ship.ScoreUp(10);
            }
        }

        //Boss shot exception handler
        private static void BossShotTimer_Tick(object sender, EventArgs e)
        {
            if (isBossFight)  //Is boss
            {
                int numberOfBullets = 2; //parameter number of bullets per tick
                EnemyBullets.LoadObjects(Boss.boss.PosX, Boss.boss.PosY, Boss.boss.WidthSize, Boss.boss.HeightSize, numberOfBullets);
                if (isPlasmaWeapon)
                {
                    BossWeapon.LoadObjects(Boss.boss.PosX, Boss.boss.PosY, Boss.boss.WidthSize, Boss.boss.HeightSize);
                } 
                isPlasmaWeapon = !isPlasmaWeapon;
            }
        }

        //Screen refresh exception handler
        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (controlUp)
            {
                Ship.ship.Up();
            }
            if (controlDown)
            {
                Ship.ship.Down();
            }
            if (controlLeft)
            {
                Ship.ship.Left();
            }
            if (controlRight)
            {
                Ship.ship.Right();
            }
            //Force call garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Drawing();
            Update();
        }

        //Ship destroing adn game ending
        private static void DestroyShip(object sender, EventArgs e)
        {
            Ship.ship.Die();
        }

        //Display all objects
        static public void Drawing()
        {
            //Static (non-interacting objects)
            buffer.Graphics.Clear(Color.Black);
            Background.BackGround();
            if (Controller.IsControlerConnected)
            {
                Controller.SerialPort.WriteLine(Ship.ship.Score.ToString());
                Controller.controllerOperation(Ship.ship);
                if (Controller.IsShot)
                {
                    ShotTimer_Tick();
                    Controller.IsShot = false;
                }
            }

            //Dynamic
            if (startGame)
            {
                Ship.ship.Drawing();
            }
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
            foreach (Bullet bullet in Bullet.bullets)
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
            if (Boss.boss != null)
            {
                Boss.boss.Drawing();
            }
            foreach (RocketV2 obj in RocketV2.rocketsV2)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (EnemyBullets obj in EnemyBullets.enemyBullets)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (BossWeapon obj in BossWeapon.bossWeapons)
            {
                if (obj != null) obj.Drawing();
            }
            //Output game information (HP of the ship, number of points, number of shots)
            Background.DisplayOutputGameInformation(buffer, Ship.ship, counter);
        }


        //Changing the status of objects
        static public void Update()
        {
            //The end
            if (Ship.ship.Energy <= 0 && !isShipDestroyed)
            {
                isShipDestroyed = true;
                Ship.DestroyingObject();
                StopTimers(scoreTimer, shotTimer);

                destroyShip.Interval = DESTROY_SHIP;
                destroyShip.Start();
                destroyShip.Tick += DestroyShip;
            }

            //Asteroids
            Asteroid.Interaction();
            int asteroidDifference = initialNumberOfAsteroids - Asteroid.asteroids.Count;
            if (asteroidDifference > 0 && !isBossFight)
            {
                Asteroid.LoadObjects(asteroidDifference);
            }

            //Asteroids charges
            AsteroidCharge.Interaction();

            //Stars
            Star.Interaction();
            if (Star.stars.Count == 0 && !isBossFight)
            {
                Star.LoadObjects(initialNumberOfStars);
                initialNumberOfStars++;
            }

            //Rocket
            Rocket.Interaction();
            if (Rocket.rockets.Count == 0 && !isBossFight)
            {
                Rocket.LoadObjects(initialNumberOfRockets);
                initialNumberOfRockets++;
            }

            //Bonus
            BonusUp.Interaction();
            if (SCORE_FOR_BONUS * bonusTime < Ship.ship.Score)
            {
                BonusUp.LoadObject();
                bonusTime++;
            }

            //Boss
            Boss.Interaction();
            //if(Boss.boss != null && Boss.boss.ulta && Boss.boss.Power < Boss.DEFAULT_POWER/2)
            if(Boss.boss != null && Boss.boss.ulta)
            {
                Boss.boss.ulta = false;
                RocketV2.LoadObjects(15);
            }

            if (!isBossFight && Ship.ship.BossTime >= BOSS_SPAWN_TIME)
            {
                Boss.LoadObjects();
                MusicEffects.BossFightMusic();
            }

            //Enemy Bullets
            EnemyBullets.Interaction();

            //Boss weapon
            BossWeapon.Interaction();

            //RocketV2
            RocketV2.Interaction();           

            //Visual Effects
            VisualEffect.Interaction();

            //Bullets
            Bullet.Interaction();
        }

        //Create and upload objects to the screen
        static public void Load()
        {
            Asteroid.LoadObjects(initialNumberOfAsteroids);
            Star.LoadObjects(initialNumberOfStars);
            Rocket.LoadObjects(initialNumberOfRockets);
            isBossFight = false;
        }

        //Clear game area from all objects
        static public void Clear()
        {
            Asteroid.asteroids.Clear();
            Star.stars.Clear();
            Rocket.rockets.Clear();
        }

        //Method that stops all timers
        static public void StopTimers(params Timer[] timers)
        {
            foreach (var timer in timers)
            {
                timer.Stop();
            }
        }

        //End game method
        static public void Finish()
        {
            //Stop all timers
            StopTimers(timer, bossShotTimer, destroyShip);
            //Display information
            Background.DisplayFinishGameStats(buffer);
            //ScoreMenu
            if (MessageBox.Show("Want to save your record at scoreboard?", "Asteroids belt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ScoreMenu scoreMenu = new ScoreMenu(350, 200, Ship.ship.Score);
                scoreMenu.ShowDialog();
            }
            Application.Exit();
        }
    }
}
