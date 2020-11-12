using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;


namespace FirstGame
{
    static class GameFunctional
    {
        //Delegete,event and value for bullets counting
        public delegate void Count();
        public static event Count CountPlus;
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
        //
        static int initialNumberOfAsteroids = 3;
        static int initialNumberOfStars = 4;
        static int initialNumberOfImages = 5;
        static int bonusTime = 0;
        static bool bossFight = false; //boss flag

        //Random number generation and timers for screen refresh, points, automatic player and boss shots
        static Timer timer = new Timer();
        static Timer scoreTimer = new Timer();
        static Timer shotTimer = new Timer();
        static Timer bossShotTimer = new Timer();
        static Timer startGameMenu = new Timer();
        static public Random rnd = new Random();

        //buffers that change and output
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
            GameScreen.KeyUp += GameScreen_KeyUp; ;
            Ship.MessageDie += Finish;                  //End Game
            CountPlus += Game_Counter;                  //Bullet count
        }

        private static void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (!startGame)
            {
                Clear();
                Load();
                startGame = true;
            }
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
            //Shot
            //Abuse
            /*//Shot mechanic (Lvl - ship level and number of bullets according to the standard principle)
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
                if (superBullet) bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + i * 8 + ship.Size / 2-1), new Point(4, 0), new Size(6, 2)));

                //Second type of bullets
                else bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y +ship.Size/2-1), new Point(4, i), new Size(4, 1)));

                //Data transfer to constructor
                bullets[index].Lvl = ship.Lvl;
                index++;
            }
            CountPlus();*/

            //Ship displacement in any of 4 directions
            /*if (e.KeyCode == Keys.Up)
            {
                ship.Up();
                if (controlRight) { ship.Right(); }
                if (controlLeft) { ship.Left(); }
                controlUp = true; controlDown = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                ship.Down();
                if (controlRight) { ship.Right(); }
                if (controlLeft) { ship.Left(); }
                controlDown = true;  controlUp = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                ship.Left();
                if (controlUp) { ship.Up(); }
                if (controlDown) { ship.Down(); }
                controlLeft = true; controlRight = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                ship.Right();
                if (controlUp) { ship.Up(); }
                if (controlDown) { ship.Down(); }
                controlRight = true; controlLeft = false;
            }*/
            if (e.KeyCode == Keys.Up)
            {
                controlUp = true;
                //ship.Up();
            }
            if (e.KeyCode == Keys.Down)
            {
                controlDown = true;
                //ship.Down();
            }
            if (e.KeyCode == Keys.Left)
            {
                controlLeft = true;
                //ship.Left();
            }
            if (e.KeyCode == Keys.Right)
            {
                controlRight = true;
                //ship.Down();
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
            shotTimer.Tick += ShotTimer_Tick;

            //Automatic boss firing rate
            bossShotTimer.Interval = 1000;
            bossShotTimer.Start();
            bossShotTimer.Tick += BossShotTimer_Tick;
        }
        static public void startGameMessage()
        {
            //Game menu
            startGameMenu.Interval = 10;
            startGameMenu.Start();
            startGameMenu.Tick += StartGameMenu_Tick;
        }
        private static void StartGameMenu_Tick(object sender, EventArgs e)
        {
            if(startGame) startGameMenu.Stop();
            buffer.Graphics.DrawString(
                "Press any key to start", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline),
                Brushes.White, Width / 2 - 200, Height / 2 - 100);
            buffer.Render();
        }


        //Ship bullet exception handler
        private static void ShotTimer_Tick(object sender, EventArgs e)
        {
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
                if (superBullet) bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + i * 8 + ship.Size / 2-1), new Point(4, 0), new Size(6, 2)));

                //Second type of bullets
                else bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y +ship.Size/2-1), new Point(4, i), new Size(4, 1)));

                //Data transfer to constructor
                bullets[index].Lvl = ship.Lvl;
                index++;
            }
            CountPlus();
        }

        //Previous version of shooting
        /*int numberOfBullets;
        if (ship.Lvl > 3) numberOfBullets = 1;
        else numberOfBullets = ship.Lvl/2;

        for (int i = -numberOfBullets; i <= numberOfBullets; i++)
        {
            if (ship.Lvl % 2 == 0 && i == 0) { continue; }
            bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + 4), new Point(4, i), new Size(4, 1)));
            bullets[index].Lvl = ship.Lvl;
            bullets[index].Power = 10;
            index++;
        }
        CountPlus();*/

        /*int numberOfBullets=ship.Lvl;
        bool superBullet = false;
        if (numberOfBullets > 3) superBullet = true;
        //if (numberOfBullets > 6) numberOfBullets=6;

        if (superBullet)
        {
            int numberOfSuperBullets = numberOfBullets - 3;
            for (int i = -numberOfSuperBullets/2; i <= numberOfSuperBullets/2;i++)
            {
                if (numberOfSuperBullets % 2 == 0 && i==0 && numberOfSuperBullets !=0) { continue; }
                bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + i*8+4), new Point(4, 0), new Size(6, 2)));
                PowerOfBullet();
            }
            CountPlus();
        }
        else
        {
            for (int i = -numberOfBullets / 2; i <= numberOfBullets / 2; i++)
            {
                if (numberOfBullets % 2 == 0 && i == 0 && numberOfBullets !=0) { continue; }
                bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + 4), new Point(4, i), new Size(4, 1)));
                PowerOfBullet();
            }
            CountPlus();
        }*/

        //bullets.Add(new Bullet(new Point(ship.Rect.X + 10, ship.Rect.Y + 4), new Point(4, i), new Size(4, 1)));
        //CountPlus();

        //Extra points exception handler
        private static void ScoreTimer_Tick(object sender, EventArgs e)
        {
            if (startGame) ship.ScoreUp(10);
        }

        //Boss shot exception handler
        private static void BossShotTimer_Tick(object sender, EventArgs e)
        {
            if (bossFight)  //Is boss
            {
                int n = 2; //parameter number of bullets per tick
                for (int ich = -n; ich <= n; ich++)
                {
                    for (int jch = -n; jch <= n; jch++)
                    {
                        if (ich == 0 && jch == 0) { continue; }
                        //if (ich == 0 || jch == 0) { continue; }
                        //if(Math.Abs(ich) + Math.Abs(jch) == n+1 || ich==0 || jch==0)
                        if(Math.Abs(ich) + Math.Abs(jch) == n + 1 || (ich ==0 && Math.Abs(jch)==n) || (jch == 0 && Math.Abs(ich) == n))
                            enemyBullets.Add(new EnemyBullets(
                                new Point(boss[0].PosX + boss[0].Size / 2 + ich * boss[0].Size / 4, boss[0].PosY + boss[0].Size / 2 + jch * boss[0].Size / 4),
                                new Point(2*ich ,2*jch), new Size(20, 14)));
                    }
                }
            }
        }

        //Screen refresh exception handler
        private static void Timer_Tick(object sender,EventArgs e)
        {
            if (controlUp) ship.Up();
            if (controlDown) ship.Down();
            if (controlLeft) ship.Left();
            if (controlRight) ship.Right();
            //Примусова сборка мусора (наче допомогло)
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
            BackGround();

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
            buffer.Graphics.DrawString("Energy:" + ship.Energy, SystemFonts.DefaultFont,
            Brushes.White, 0, 0);
            buffer.Graphics.DrawString("Score:" + ship.Score, SystemFonts.DefaultFont,
            Brushes.White, 100, 0);
            buffer.Graphics.DrawString("Number of shots:" + counter, SystemFonts.DefaultFont,
            Brushes.White, 200, 0);
            buffer.Render();
        }

        //Changing the status of objects
        static public void Update()
        {
            //Asteroids
            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update();
                if(startGame)
                //Collision of object and bullet 
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bullets[j].Collision(asteroids[i]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        asteroids[i].PowerLow(bullets[j].Power);    //Object "power" reduction

                        if (asteroids[i].Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                        {
                            VisualEffect(2, asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2);  //Spawn in place of the object"visual effects"
                            //Spawn on the asteroid site asteroid charges
                            int n = 1;
                            for(int ich = -n; ich <= n; ich++)
                            {
                                for (int jch = -n ; jch <= n; jch++)
                                {
                                    if (ich == 0 && jch == 0) { continue; }
                                    if (ich == 0 || jch == 0) { continue; }
                                        charges.Add(new AsteroidCharge(
                                        new Point(asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2),
                                        new Point(3*ich,3*jch), new Size(asteroids[i].Size / 4, asteroids[i].Size / 4)));
                                }
                            }

                            //Scoring points for destroyed object and the approach of the boss spawn, if the boss flag is false
                            ship.ScoreUp(asteroids[i].Size);    
                            if (!bossFight) ship.BossTimeUp(asteroids[i].Size);

                            //Remove an object from the collection and load new objects if the boss flag is false
                            asteroids.RemoveAt(i);
                            if (asteroids.Count < initialNumberOfAsteroids && !bossFight)
                            {
                                LoadAsteroids(1);
                            }
                            else
                            {
                                Console.WriteLine(asteroids.Count);
                                Console.WriteLine(!bossFight);
                                Console.WriteLine(initialNumberOfAsteroids);
                            }
                            if (i != 0) i--;
                            else break;
                        }

                        //The procedure for destroying bullets that hit the object
                        bullets.RemoveAt(j);
                        j--; index--;
                    }

                    //Collision of object and ship
                    if (ship.Collision(asteroids[i]))
                    {
                        VisualEffect(2, asteroids[i].PosX + asteroids[i].Size / 2, asteroids[i].PosY + asteroids[i].Size / 2);  //Spawn in place of the object"visual effects"
                        //Changing the characteristics of the ship
                        ship.EnergyLow(15);
                        ship.LvlUp(-1);
                        ship.ScoreUp(asteroids[i].Size);
                        if (!bossFight) ship.BossTimeUp(asteroids[i].Size);

                        //A similar procedure destroying object
                        asteroids.RemoveAt(i);
                        if (ship.Energy <= 0) ship.Die();
                        if (asteroids.Count < initialNumberOfAsteroids && !bossFight)
                        {
                            LoadAsteroids(1);
                        }
                        if (i != 0) i--;
                        else break;
                        System.Media.SystemSounds.Hand.Play();
                    }
                }
            }

            //Asteroids charges
            for (int i = 0; i < charges.Count; i++)
            {
                charges[i].Update();
                if(startGame)
                //Collision of object and bullet  
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bullets[j].Collision(charges[i]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        charges[i].PowerLow(bullets[j].Power);  //Object "power" reduction
                        if (charges[i].Power <= 0) //Procedure or destroyingobject (if power less then zero)
                        {
                            VisualEffect(2, charges[i].PosX + charges[i].Size / 2, charges[i].PosY + charges[i].Size / 2); //Spawn in place of the object"visual effects"
                            if (!bossFight)
                            {
                                ship.ScoreUp(charges[i].Size);
                                ship.BossTimeUp(charges[i].Size);
                            }
                            charges.RemoveAt(i);
                            if (i != 0) i--;
                            else break;
                        }
                        bullets.RemoveAt(j);
                        j--; index--;
                    }

                    //Collision of object and ship
                    if (ship.Collision(charges[i]))
                    {
                        VisualEffect(2, charges[i].PosX + charges[i].Size / 2, charges[i].PosY + charges[i].Size / 2);
                        ship.EnergyLow(5);
                        ship.ScoreUp(0);
                        if (!bossFight) ship.BossTimeUp(charges[i].Size);
                        charges.RemoveAt(i);
                        if (ship.Energy <= 0) ship.Die();
                        if (i != 0) i--;
                        else break;
                        System.Media.SystemSounds.Hand.Play();
                    }
                }
            }
            //Stars
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Update();
                if(startGame)
                //Collision of object and bullet  
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bullets[j].Collision(stars[i]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        stars[i].PowerLow(bullets[j].Power);    //Object "power" reduction
                        if (stars[i].Power <= 0)    //Procedure or destroyingobject (if power less then zero)
                        {
                            VisualEffect(2, stars[i].PosX + stars[i].Size / 2, stars[i].PosY + stars[i].Size / 2); //Spawn in place of the object"visual effects"
                            ship.ScoreUp(stars[i].Size);
                            if (!bossFight) ship.BossTimeUp(stars[i].Size);
                            stars.RemoveAt(i);
                            if (i != 0) i--;
                        }
                        bullets.RemoveAt(j);
                        j--; index--;
                        if (stars.Count == 0 && !bossFight)
                        {
                            LoadStars();
                        }
                        if (i != 0) i--;
                        else break;
                    }

                    //Collision of object and ship 
                    if (ship.Collision(stars[i]))
                    {
                        VisualEffect(2, stars[i].PosX + stars[i].Size / 2, stars[i].PosY + stars[i].Size / 2); //Spawn in place of the object"visual effects"
                        ship.EnergyLow(10);
                        ship.ScoreUp(stars[i].Size);
                        if (!bossFight) ship.BossTimeUp(stars[i].Size);
                        stars.RemoveAt(i);
                        if (ship.Energy <= 0) ship.Die();
                        if (stars.Count == 0 && !bossFight)
                        {
                            LoadStars();
                        }
                        if (i != 0) i--;
                        else break;
                        System.Media.SystemSounds.Hand.Play();
                        if (stars.Count == 0 && !bossFight) LoadStars();
                    } 
                }
            }

            //Rocket
            for (int i = 0; i < images.Count; i++)
            {
                images[i].Update();
                if(startGame)
                //Collision of object and bullet
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (bullets[j].Collision(images[i]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        images[i].PowerLow(bullets[j].Power);   //Object "power" reduction
                        if (images[i].Power <= 0)   //Procedure or destroyingobject (if power less then zero)
                        {
                            VisualEffect(2, images[i].PosX +images[i].Size / 2, images[i].PosY + images[i].Size / 2); //Spawn in place of the object"visual effects"
                            ship.ScoreUp(images[i].Size);
                            if (!bossFight) ship.BossTimeUp(images[i].Size);
                            images.RemoveAt(i);
                            if (i != 0) i--;
                        }
                        bullets.RemoveAt(j);
                        j--; index--;
                        if (images.Count == 0 && !bossFight)
                        {
                            LoadImages();
                        }
                        if (i != 0) i--;
                        else break;
                    }

                    //Collision of object and ship
                    if (ship.Collision(images[i]))
                    {
                        VisualEffect(2, images[i].PosX + images[i].Size / 2, images[i].PosY + images[i].Size / 2); //Spawn in place of the object"visual effects"
                        ship.EnergyLow(10);
                        ship.ScoreUp(images[i].Size);
                        if (!bossFight) ship.BossTimeUp(images[i].Size);
                        images.RemoveAt(i);
                        if (ship.Energy <= 0) ship.Die();
                        if (images.Count == 0 && !bossFight)
                        {
                            LoadImages();
                        }
                        if (i != 0) i--;
                        else break;
                        System.Media.SystemSounds.Hand.Play();
                        if (ship.Energy <= 0) ship.Die();
                        if (images.Count == 0 && !bossFight) LoadImages();
                    }
                }
            }

            //Bonus
            for (int i = 0; i < bonus.Count; i++)
            {
                bonus[i].Update();
                if(startGame)
                //Collision of object and ship
                if (ship.Collision(bonus[i]))
                {
                    ship.LvlUp(1);//Lvl+
                    ship.ScoreUp(30);
                    if (!bossFight) ship.BossTimeUp(30);
                    bonus.RemoveAt(i);
                    i--;
                    if (ship.Lvl > 6) { ship.EnergyLow(-100); }//increase HP if the level is greater than the specified
                    System.Media.SystemSounds.Beep.Play();
                }
            }

            //Boss
            for (int i = 0; i < boss.Count; i++)
            {
                if(startGame)
                if (bossFight)
                {
                    boss[i].Update();;
                }     
                for (int j = 0; j < bullets.Count; j++)
                {
                    //Collision of object and bullet
                    if (bullets[j].Collision(boss[i]))
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        boss[i].PowerLow(bullets[j].Power); //Object "power" reduction
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
                            bossFight = false;
                            VisualEffect(5, boss[i].PosX + boss[i].Size / 2, boss[i].PosY + boss[i].Size / 2);  //Spawn in place of the object of "visual effects
                            ship.ScoreUp(5000);
                            bonusTime += 25;
                            boss.RemoveAt(i);
                            Console.WriteLine("Boss defeated");
                            Console.WriteLine(bossFight);
                            bossShotTimer.Stop();
                            Load();
                            if (i != 0) i--;
                            else break;
                        }
                        bullets.RemoveAt(j);
                        j--; index--;
                    }
                    //End the game if ship collides with boss
                    if (ship.Collision(boss[i]))
                    {
                        ship.Energy = 0;
                        if (ship.Energy <= 0) ship.Die();
                    }
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
                    VisualEffect(2, enemyBullets[i].PosX + enemyBullets[i].Size / 2, enemyBullets[i].PosY + enemyBullets[i].Size / 2); //Spawn in place of the object of "visual effects
                    if (i != 0) i--; else break;
                        System.Media.SystemSounds.Hand.Play();
                        if (ship.Energy <= 0) ship.Die();
                }
            }

            if (!bossFight && ship.BossTime >= 1000) LoadBoss();//Boss spawn
            if (bonusTime*300<ship.Score) LoadBonus();//Bonus spawn

            //"Clearing the memory" of bullets that out of the playing field
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].PosX > Width)
                {
                    bullets.RemoveAt(i);
                    i--;index--;
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
                if (visualEffects[i].PosX > Width || visualEffects[i].PosX < 0 || visualEffects[i].PosY > Height || visualEffects[i].PosY < 0) { visualEffects.RemoveAt(i); if (i != 0) i--;
                    else break;
                }
            }
        }


        //Create and upload objects to the screen
        static public void Load()
        {
            LoadAsteroids();
            LoadStars();
            LoadImages();
            LoadBonus();
            LoadBoss();
            bonus.RemoveAt(0);
            boss.RemoveAt(0);
            bossFight = false;
        }
        static public void Clear()
        {
            asteroids.RemoveRange(0,asteroids.Count);
            stars.RemoveRange(0, stars.Count);
            images.RemoveRange(0, images.Count);
            bonus.RemoveRange(0, bonus.Count);
        }
        //Loading Asteroids
        static public void LoadAsteroids()
        {
            for (int i = 0; i < initialNumberOfAsteroids; i++)
            {
                int speedX = rnd.Next(2, 7);
                int speedY = rnd.Next(2, 7);
                //int size = rnd.Next(50, 150);
                int size = 70;
                asteroids.Add(new Asteroid(
                    new Point(
                        Width  + GameFunctional.rnd.Next(100, 400), Height / 2 - GameFunctional.rnd.Next(0, 400)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
            //initialNumberOfAsteroids++;
        }
        static public void LoadAsteroids(int n)
        {
            for (int i = 0; i < n; i++)
            {
                int speedX = rnd.Next(2, 7);
                int speedY = rnd.Next(2, 7);
                //int size = rnd.Next(50, 150);
                int size = 70;
                asteroids.Add(new Asteroid(
                    new Point(
                        Width + GameFunctional.rnd.Next(100, 400), Height / 2 - GameFunctional.rnd.Next(0, 400)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
        }
        //Loading Stars
        static public void LoadStars()
        {
            for (int i = 0; i < initialNumberOfStars; i++)
            {
                int speedX = rnd.Next(5, 7);
                int speedY = rnd.Next(7, 10);
                int size = 25;
                stars.Add(new Star(
                    new Point(
                        Width + GameFunctional.rnd.Next(1, 100) * 10, GameFunctional.rnd.Next(0, GameFunctional.Height)),
                    new Point(-speedX, speedY), new Size(size, size)));
            }
            initialNumberOfStars++;
        }
        //Loading Rocket
        static public void LoadImages()
        {
            for (int i = 0; i < initialNumberOfImages; i++)
            {
                int r = rnd.Next(10, 15);
                //int size = 30;
                images.Add(new Rocket(
                    new Point(
                        Width + GameFunctional.rnd.Next(1, 100) * 10, GameFunctional.rnd.Next(0, GameFunctional.Height)),
                    new Point(-r, r), new Size(70, 20)));
            }
            initialNumberOfImages++;
        }
        //Loading Bonus
        static public void LoadBonus()
        {
            int speedX = rnd.Next(2, 3);
            int speedY = rnd.Next(2, 3);
            //int size = rnd.Next(50, 150);
            int sizeX =45;
            int sizeY =25;
                bonus.Add(new BonusUp(
                    new Point(
                        Width + GameFunctional.rnd.Next(0, 400), Height / 2 + GameFunctional.rnd.Next(-400, 400)),
                    new Point(-speedX, speedY), new Size(sizeX, sizeY)));
            bonusTime++;
        }

        //Loading Boss
        static public void LoadBoss()
        {
            bossFight = true;
            boss.Add(new Boss(
                    new Point(
                        Width+rnd.Next(10,100), Height/2),
                    new Point(-3, 3), new Size(350, 350)));
            ship.BossTime = 0;
        }

        //End game method
        static public void Finish()
        {

            //Stop all timers
            timer.Stop();
            scoreTimer.Stop();
            shotTimer.Stop();
            bossShotTimer.Stop();
            //Display information
            buffer.Graphics.DrawString(
                "The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), 
                Brushes.White, Width / 2-200, Height / 2 - 100);
            buffer.Graphics.DrawString(
                "Your score:"+ship.Score, new Font(FontFamily.GenericSansSerif,30),
                Brushes.Bisque, Width / 2-200, Height / 2 );
            buffer.Render();
        }

        //Background
        static public void BackGround()
        {            
            GameFunctional.buffer.Graphics.DrawImage
                (Image.FromFile("pictures//planet.png"), new Rectangle(1000, 100, 100, 100));

            Random rnd = new Random();
                int Num = rnd.Next(10, 100);
                int[] coorX = new int[Num];
                int[] coorY = new int[Num];
                for (int i = 0; i < Num; i++)
                {
                    coorX[i] = rnd.Next(0, Width);
                    coorY[i] = rnd.Next(0, Height);
                    buffer.Graphics.FillEllipse(Brushes.Bisque, new Rectangle(coorX[i], coorY[i], 2, 2));
                }
        } 

        //"Visual effects"
        static public void VisualEffect(int n,int PosX,int PosY)
        {
            for (int ich = -n; ich <= n; ich++)
            {
                for (int jch = -n; jch <= n; jch++)
                {
                    if (ich == 0 && jch == 0) { continue; }
                    //if (ich == 0 || jch == 0) { continue; }
                    visualEffects.Add(new VisualEffect(
                        new Point(PosX,PosY),
                        new Point(3 * ich + rnd.Next(-7, 7), 3 * jch + rnd.Next(-7, 7)), new Size(5, 5)));
                }
            }
        }
    }
}
