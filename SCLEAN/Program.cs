using System;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Game snakeGame = new Game();
            snakeGame.Run();
        }
    }

    class Game
    {
        private const int WindowWidth = 32;
        private const int WindowHeight = 16;
        private const int InitialSnakeLength = 5;

        private int score;
        private int gameOver;
        private pixel head;
        private List<int> xPosList;
        private List<int> yPosList;
        private int berryX;
        private int berryY;

        public Game()
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            score = InitialSnakeLength;
            gameOver = 0;

            head = new pixel
            {
                XPos = WindowWidth / 2,
                YPos = WindowHeight / 2,
                Color = ConsoleColor.Red
            };

            xPosList = new List<int>();
            yPosList = new List<int>();

            GenerateBerry();
        }

        public void Run()
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            string buttonPressed = "no";

            while (true)
            {
                Console.Clear();
                DrawBoundaries();

                Console.ForegroundColor = ConsoleColor.Green;
                CheckCollision();

                if (gameOver == 1)
                {
                    break;
                }

                DrawSnake();
                DrawBerry();

                endTime = DateTime.Now;
                buttonPressed = HandleKeyPress(startTime, endTime, buttonPressed);

                MoveSnake();

                UpdateSnakePosition();
            }

            ShowGameOverMessage();
        }

        private void DrawBoundaries()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, WindowHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(WindowWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawSnake()
        {
            Console.SetCursorPosition(head.XPos, head.YPos);
            Console.ForegroundColor = head.Color;
            Console.Write("■");

            for (int i = 0; i < xPosList.Count; i++)
            {
                Console.SetCursorPosition(xPosList[i], yPosList[i]);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            Console.SetCursorPosition(berryX, berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private void GenerateBerry()
        {
            Random randomNumber = new Random();
            berryX = randomNumber.Next(1, WindowWidth - 2);
            berryY = randomNumber.Next(1, WindowHeight - 2);
        }

        private void CheckCollision()
        {
            if (head.XPos == WindowWidth - 1 || head.XPos == 0 || head.YPos == WindowHeight - 1 || head.YPos == 0)
            {
                gameOver = 1;
            }

            if (berryX == head.XPos && berryY == head.YPos)
            {
                score++;
                GenerateBerry();
            }

            for (int i = 0; i < xPosList.Count; i++)
            {
                if (xPosList[i] == head.XPos && yPosList[i] == head.YPos)
                {
                    gameOver = 1;
                }
            }
        }

        private string HandleKeyPress(DateTime startTime, DateTime endTime, string buttonPressed)
        {
            while (true)
            {
                endTime = DateTime.Now;

                if (endTime.Subtract(startTime).TotalMilliseconds > 500)
                {
                    break;
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);

                    if (keyPressed.Key.Equals(ConsoleKey.UpArrow) && head.Direction != "DOWN" && buttonPressed == "no")
                    {
                        head.Direction = "UP";
                        buttonPressed = "yes";
                    }
                    if (keyPressed.Key.Equals(ConsoleKey.DownArrow) && head.Direction != "UP" && buttonPressed == "no")
                    {
                        head.Direction = "DOWN";
                        buttonPressed = "yes";
                    }
                    if (keyPressed.Key.Equals(ConsoleKey.LeftArrow) && head.Direction != "RIGHT" && buttonPressed == "no")
                    {
                        head.Direction = "LEFT";
                        buttonPressed = "yes";
                    }
                    if (keyPressed.Key.Equals(ConsoleKey.RightArrow) && head.Direction != "LEFT" && buttonPressed == "no")
                    {
                        head.Direction = "RIGHT";
                        buttonPressed = "yes";
                    }
                }
            }

            return buttonPressed;
        }

        private void MoveSnake()
        {
            switch (head.Direction)
            {
                case "UP":
                    head.YPos--;
                    break;
                case "DOWN":
                    head.YPos++;
                    break;
                case "LEFT":
                    head.XPos--;
                    break;
                case "RIGHT":
                    head.XPos++;
                    break;
            }
        }

        private void UpdateSnakePosition()
        {
            xPosList.Add(head.XPos);
            yPosList.Add(head.YPos);

            if (xPosList.Count > score)
            {
                xPosList.RemoveAt(0);
                yPosList.RemoveAt(0);
            }
        }

        private void ShowGameOverMessage()
        {
            Console.SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
        }

        class pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor Color { get; set; }
            public string Direction { get; set; } = "RIGHT";
        }
    }
}
