using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace FirstGame
{
    public static class GameFunctional
    {
        //Delegete,event and value for bullets counting
        public delegate void Count();
        public static event Count CountPlus;
        public static event Count Shot; //Needed if change autofire to press some button
        static int counter;
        static bool startGame;

        //Class fields 
        //Objects that displayed and interact
        //static BaseObject[] objs;
        static List<VisualEffect> visualEffects = new List<VisualEffect>();
        static Ship ship = new Ship(new Point(30, 450), new Point(3, 3), new Size(20, 15));
        static List<Bullet> bullets = new List<Bullet>();
        static int index = 0; //Bullet control (2 types of bullet)
        static List<Asteroid> asteroids = new List<Asteroid>();
        static List<Rocket> images = new List<Rocket>();
        static List<Star> stars = new List<Star>();
        static List<BonusUp> bonus = new List<BonusUp>();
        static List<Boss> boss = new List<Boss>();
        static List<EnemyBullets> enemyBullets = new List<EnemyBullets>();
        static List<AsteroidCharge> charges = new List<AsteroidCharge>();

        //The initial number of objects with the ability to change them during the game
        static bool controlUp, controlDown, controlRight, controlLeft;   
        static int initialNumberOfAsteroids = 3;
        static int initialNumberOfStars = 4;
        static int initialNumberOfImages = 5;
        static int bonusTime = -1;
        static bool isBossFight = false; //boss flag

        //Random number generation and timers for screen refresh, points, automatic player and boss shots
        static Timer timer = new Timer();
        static Timer scoreTimer = new Timer();
        static Timer shotTimer = new Timer();
        static Timer bossShotTimer = new Timer();
        static Timer startGameMenu = new Timer();
        static public Random rnd = new Random();

        //Buffers that change and output
        static BufferedGraphicsContext context;
        static public BufferedGraphics buffer;

        //Properties
        //Height and width of the playing field
        static public int Width { get; set; }
        static public int Height { get; set; }

        //Constructor
        static GameFunctional()
        {
        }

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
            if(startGame) counter++;
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
            shotTimer.Interval =1000;
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
            if(startGame) startGameMenu.Stop();
            buffer.Graphics.DrawString(
                "Press any key to start", new Font(FontFamily.GenericSansSerif, 30),
                Brushes.White, Width / 3, Height / 4*3);
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
            if (startGame) ship.ScoreUp(10);
        }

        //Boss shot exception handler
        private static void BossShotTimer_Tick(object sender, EventArgs e)
        {
            if (isBossFight)  //Is boss
            {
                int n = 2; //parameter number of bullets per tick
                for (int ich = -n; ich <= n; ich++)
                {
                    for (int jch = -n; jch <= n; jch++)
                    {
                        if (ich == 0 && jch == 0) { continue; }
                        if(Math.Abs(ich) + Math.Abs(jch) == n + 1 || (ich ==0 && Math.Abs(jch)==n) || (jch == 0 && Math.Abs(ich) == n))
                            enemyBullets.Add(new EnemyBullets(
                                new Point(boss[0].PosX + boss[0].Size / 2 + ich * boss[0].Size / 4, boss[0].PosY + boss[0].Size / 2 + jch * boss[0].Size / 4),
                                new Point(2*ich ,2*jch), new Size(20, 14)));
                    }
                }
                MusicEffects.BossShotSound();
            }
        }

        //Screen refresh exception handler
        private static void Timer_Tick(object sender,EventArgs e)
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
            foreach (Rocket obj in images)
            {
                if (obj != null) obj.Drawing();
            }
            foreach (Asteroid obj in asteroids)
            {
                if (obj !=null) obj.Drawing();
            }
            foreach (Star obj in stars)
            {
               if(obj!=null) obj.Drawing();
            }
            foreach(Bullet bullet in bullets)
            {
                if(startGame) bullet.Drawing();
            }
            foreach(BonusUp obj in bonus)
            {
                if (obj != null) obj.Drawing();
            }
            foreach(AsteroidCharge obj in charges)
            {
                if (obj != null) obj.Drawing();
            }
            foreach(VisualEffect obj in visualEffects)
            {
                if (obj != null) obj.Drawing();
            }
            foreach(Boss obj in boss)
            {
                if (obj != null) obj.Drawing();
            }
            foreach(EnemyBullets obj in enemyBullets)
            {
                if (obj != null) obj.Drawing();
            }
            if(startGame) ship.Drawing();

            //Output game information (HP of the ship, number of points, number of shots)
            Background.DisplayOutputGameInformation(buffer, ship, ref counter);
        }
        //Попробувати засунути в клас астероїду
        //Події в окремий клас
        //Позбутися реф, перенесши змінні в класи
        //Changing the status of objects
        static public void Update()
        {
            //Asteroids
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update();
                if (startGame)
                {
                    //Collision of object and bullet 
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(asteroids[i]))
                        {
                            MusicEffects.HitSound();
                            asteroids[i].PowerLow(bullets[j].Power);    //Object "power" reduction

                            if (asteroids[i].Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                ObjectLoader.VisualEffect(2, 
                                    asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2, visualEffects);  //Spawn in place of the object"visual effects"
                                                                                                                                        //Spawn on the asteroid site asteroid charges
                                int n = 1;
                                for (int ich = -n; ich <= n; ich++)
                                {
                                    for (int jch = -n; jch <= n; jch++)
                                    {
                                        if (ich == 0 && jch == 0) { continue; }
                                        if (ich == 0 || jch == 0) { continue; }
                                        charges.Add(new AsteroidCharge(
                                        new Point(asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2),
                                        new Point(3 * ich, 3 * jch), new Size(asteroids[i].Size / 4, asteroids[i].Size / 4)));
                                    }
                                }

                                //Scoring points for destroyed object and the approach of the boss spawn, if the boss flag is false
                                ship.ScoreUp(asteroids[i].Size);
                                if (!isBossFight)
                                {
                                    ship.BossTimeUp(asteroids[i].Size);
                                }
                                //Remove an object from the collection and load new objects if the boss flag is false
                                asteroids.RemoveAt(i);
                                if (asteroids.Count < initialNumberOfAsteroids && !isBossFight)
                                {
                                    ObjectLoader.LoadAsteroids(1,asteroids);
                                }
                                if (i != 0)
                                {
                                    i--;
                                }
                                else break;
                            }
                            //The procedure for destroying bullets that hit the object
                            bullets.RemoveAt(j);
                            j--;
                            index--;
                        }
                    }
                    //Collision of object and ship
                    if (asteroids.Count > 0 && ship.Collision(asteroids[i]))
                    {
                        MusicEffects.HitSound();
                        ObjectLoader.VisualEffect(2, 
                            asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2, visualEffects);  //Spawn in place of the object"visual effects"
                                                                                                                                //Changing the characteristics of the ship
                        ship.EnergyLow(asteroids[i].Damage);
                        ship.LvlUp(-1);
                        if (ship.Energy <= 0)
                        {
                            ship.Die();
                        }
                        ship.ScoreUp(asteroids[i].Size);
                        if (!isBossFight)
                        {
                            ship.BossTimeUp(asteroids[i].Size);
                        }
                        //A similar procedure destroying object
                        asteroids.RemoveAt(i);
                        if (asteroids.Count < initialNumberOfAsteroids && !isBossFight)
                        {
                            ObjectLoader.LoadAsteroids(1, asteroids);
                        }
                        if (i > 1)
                        {
                            i--;
                        }
                        else break;
                    }
                }                
            }

            //Asteroids charges
            for (int i = 0; i < charges.Count; i++)
            {
                charges[i].Update();
                if (startGame)
                {
                    //Collision of object and bullet  
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(charges[i]))
                        {
                            MusicEffects.HitSound();
                            charges[i].PowerLow(bullets[j].Power);  //Object "power" reduction
                            if (charges[i].Power <= 0) //Procedure or destroyingobject (if power less then zero)
                            {
                                ObjectLoader.VisualEffect(2,
                                    charges[i].PosX + charges[i].Size / 2, charges[i].PosY + charges[i].Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                                if (!isBossFight)
                                {
                                    ship.ScoreUp(charges[i].Size);
                                    ship.BossTimeUp(charges[i].Size);
                                }
                                charges.RemoveAt(i);
                                if (i != 0)
                                {
                                    i--;
                                }
                                else break;
                            }
                            bullets.RemoveAt(j);
                            j--; index--;
                        }
                    }
                    //Collision of object and ship
                    if (ship.Collision(charges[i]))
                    {
                        MusicEffects.HitSound();
                        ObjectLoader.VisualEffect(2,
                            charges[i].PosX + charges[i].Size / 2, charges[i].PosY + charges[i].Size / 2, visualEffects);
                        ship.EnergyLow(charges[i].Damage);
                        if (ship.Energy <= 0)
                        {
                            ship.Die();
                        }
                        ship.ScoreUp(10);
                        if (!isBossFight)
                        {
                            ship.BossTimeUp(charges[i].Size);
                        }
                        charges.RemoveAt(i);
                        if (i > 1)
                        {
                            i--;
                        }
                        else break;
                    }
                }              
            }
            //Stars
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Update();
                if (startGame)
                {
                    //Collision of object and bullet  
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(stars[i]))
                        {
                            MusicEffects.HitSound();
                            stars[i].PowerLow(bullets[j].Power);    //Object "power" reduction
                            if (stars[i].Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                            {
                                ObjectLoader.VisualEffect(2,
                                    stars[i].PosX + stars[i].Size / 2, stars[i].PosY + stars[i].Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                                ship.ScoreUp(stars[i].Size);
                                if (!isBossFight)
                                {
                                    ship.BossTimeUp(stars[i].Size);
                                }
                                stars.RemoveAt(i);
                                if (i != 0)
                                {
                                    i--;
                                }
                            }
                            bullets.RemoveAt(j);
                            j--; index--;
                            if (stars.Count == 0 && !isBossFight)
                            {
                                ObjectLoader.LoadStars(initialNumberOfStars ,stars);
                            }
                            if (i != 0)
                            {
                                i--;
                            }
                            else break;
                        }
                    }
                    //Collision of object and ship 
                    if (stars.Count > 0 && ship.Collision(stars[i]))
                    {
                        MusicEffects.HitSound();
                        ObjectLoader.VisualEffect(2,
                            stars[i].PosX + stars[i].Size / 2, stars[i].PosY + stars[i].Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                        ship.EnergyLow(stars[i].Damage);
                        if (ship.Energy <= 0)
                        {
                            ship.Die();
                        }
                        ship.ScoreUp(stars[i].Size);
                        if (!isBossFight)
                        {
                            ship.BossTimeUp(stars[i].Size);
                        }
                        stars.RemoveAt(i);
                        if (stars.Count == 0 && !isBossFight)
                        {
                            ObjectLoader.LoadStars(initialNumberOfStars, stars);
                        }
                        if (i > 1)
                        {
                            i--;
                        }
                        else break;                       
                    }
                }
            }

            //Rocket
            for (int i = 0; i < images.Count; i++)
            {
                images[i].Update();
                if (startGame)
                {
                    //Collision of object and bullet
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].Collision(images[i]))
                        {
                            MusicEffects.HitSound();
                            images[i].PowerLow(bullets[j].Power);   //Object "power" reduction
                            if (images[i].Power <= 0)   //Procedure or destroyingobject (if power less then zero)
                            {
                                ObjectLoader.VisualEffect(2,
                                    images[i].PosX + images[i].Size / 2, images[i].PosY + images[i].Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                                ship.ScoreUp(images[i].Size);
                                if (!isBossFight)
                                {
                                    ship.BossTimeUp(images[i].Size);
                                }
                                images.RemoveAt(i);
                                if (i != 0)
                                {
                                    i--;
                                }
                            }
                            bullets.RemoveAt(j);
                            j--; index--;
                            if (images.Count == 0 && !isBossFight)
                            {
                                ObjectLoader.LoadRockets(initialNumberOfImages, images);
                            }
                            if (i != 0)
                            {
                                i--;
                            }
                            else break;
                        }
                    }
                    //Collision of object and ship
                    if (images.Count >0 && ship.Collision(images[i]))
                    {
                        MusicEffects.HitSound();
                        ObjectLoader.VisualEffect(2,
                            images[i].PosX + images[i].Size / 2, images[i].PosY + images[i].Size / 2, visualEffects); //Spawn in place of the object"visual effects"
                        ship.EnergyLow(images[i].Damage);
                        if (ship.Energy <= 0)
                        {
                            ship.Die();
                        }
                        ship.ScoreUp(images[i].Size);
                        if (!isBossFight)
                        {
                            ship.BossTimeUp(images[i].Size);
                        }
                        images.RemoveAt(i);
                        if (ship.Energy <= 0)
                        {
                            ship.Die();
                        }
                        if (i > 1)
                        {
                            i--;
                        }
                        else break;
                        if (images.Count == 0 && !isBossFight)
                        {
                            ObjectLoader.LoadRockets(initialNumberOfImages, images);
                        }
                    }
                }
            }

            //Bonus
            for (int i = 0; i < bonus.Count; i++)
            {
                bonus[i].Update();
                if (startGame)
                {
                    //Collision of object and ship
                    if (ship.Collision(bonus[i]))
                    {
                        ship.LvlUp(1);//Lvl+
                        ship.ScoreUp(30);
                        if (!isBossFight)
                        {
                            ship.BossTimeUp(30);
                        }
                        bonus.RemoveAt(i);
                        i--;
                        if (ship.Lvl > 6)
                        {
                            ship.EnergyLow(-50); //increase HP if the level is greater than the specified
                            ship.Lvl--;
                        }
                        MusicEffects.BonusSound();
                    }
                }                
            }

            //Boss
            for (int i = 0; i < boss.Count; i++)
            {
                if(startGame)
                if (isBossFight)
                {
                    boss[i].Update();
                }     
                for (int j = 0; j < bullets.Count; j++)
                {
                    //Collision of object and bullet
                    if (bullets[j].Collision(boss[i]))
                    {
                        MusicEffects.HitSound();
                        boss[i].PowerLow(bullets[j].Power); //Object "power" reduction
                        bullets.RemoveAt(j);
                        j--; index--;
                        //Spawn in boss place charges
                        int n = 1;
                        for (int ich = -n; ich <= n; ich++)
                        {
                            for (int jch = -n; jch <= n; jch++)
                            {
                                if (ich == 0 && jch == 0) { continue; }
                                //if (ich == 0 || jch == 0) { continue; }
                                charges.Add(new AsteroidCharge(
                                        new Point(boss[i].PosX + boss[i].Size / 2+ich* boss[i].Size / 4, boss[i].PosY + boss[i].Size / 2+jch*boss[i].Size / 4),
                                        new Point(6 * ich+rnd.Next(-7, 7), 3 * jch+ rnd.Next(-7, 7)), new Size(50, 50)));
                            }
                        }
                        //Boss destoring procedure
                        if (boss[i].Power <= 0)
                        {
                            MusicEffects.BonusSound();
                            ship.LvlUp(1);
                            isBossFight = false;
                            ObjectLoader.VisualEffect(5,
                                boss[i].PosX + boss[i].Size / 2, boss[i].PosY + boss[i].Size / 2, visualEffects);  //Spawn in place of the object of "visual effects
                            ship.ScoreUp(5000);
                            bonusTime += 25;
                            boss.RemoveAt(i);
                            bossShotTimer.Stop();
                            Load();
                            if (i != 0) i--;
                            else break;
                        }
                    }                   
                }
                //End the game if ship collides with boss
                if (boss.Count > 0 && ship.Collision(boss[i]))
                {
                    ship.Energy = 0;
                    if (ship.Energy <= 0) ship.Die();
                }
            }

            //Enemy Bullets
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                enemyBullets[i].Update();
                //Remove bullets from the collection if they out of game area (to save memory)
                if (enemyBullets[i].PosX > Width || enemyBullets[i].PosX < 0 || enemyBullets[i].PosY > Height || enemyBullets[i].PosY < 0)
                {
                    enemyBullets.RemoveAt(i);
                    if (i != 0)
                        i--;
                    else break;
                }
                //Collision of object and ship
                if (ship.Collision(enemyBullets[i]))
                {
                    MusicEffects.HitSound();
                    ship.EnergyLow(enemyBullets[i].Damage);
                    ObjectLoader.VisualEffect(2,
                        enemyBullets[i].PosX + enemyBullets[i].Size / 2, enemyBullets[i].PosY + enemyBullets[i].Size / 2, visualEffects); //Spawn in place of the object of "visual effects
                    if (ship.Energy <= 0) ship.Die();
                    enemyBullets.RemoveAt(i);
                    if (i != 0) i--; else break;
                }
            }

            if (!isBossFight && ship.BossTime >= 3000)
            {
                ObjectLoader.LoadBoss(ref isBossFight, boss, ship);
                MusicEffects.BossFightMusic();
            }
            //Boss spawn
            if (bonusTime*300<ship.Score)
            {
                ObjectLoader.LoadBonus(ref bonusTime, bonus);//Bonus spawn
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

            for(int i=0;i<visualEffects.Count;i++)
            {
                visualEffects[i].Update();
                if (visualEffects[i].PosX > Width || visualEffects[i].PosX < 0 || visualEffects[i].PosY > Height || visualEffects[i].PosY < 0)
                { visualEffects.RemoveAt(i);
                    if (i != 0)
                    {
                        i--;
                    }
                    else break;
                }
            }
        }

        //Create and upload objects to the screen
        static public void Load()
        {
            ObjectLoader.LoadAsteroids(initialNumberOfAsteroids, asteroids);
            ObjectLoader.LoadStars(initialNumberOfStars, stars);
            ObjectLoader.LoadRockets(initialNumberOfImages, images);
            ObjectLoader.LoadBonus(ref bonusTime, bonus);
            ObjectLoader.LoadBoss(ref isBossFight, boss, ship);
            bonus.Clear();
            boss.Clear();
            isBossFight = false;
        }

        static public void Clear()
        {
            asteroids.Clear();
            stars.Clear();
            images.Clear();
            bonus.Clear();
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
                ScoreMenu scoreMenu = new ScoreMenu(350,200, ship.Score);
                scoreMenu.ShowDialog();
            }
            Application.Exit();
        }       
    }
}
