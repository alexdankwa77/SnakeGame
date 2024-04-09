using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new SnakeGame();
            game.Run();
        }
    }

    class SnakeGame
    {
        private const int WindowWidth = 32;
        private const int WindowHeight = 16;
        private const int InitialSnakeLength = 5;

        private int _score;
        private int _gameOver;
        private SnakeSegment _head;
        private List<SnakeSegment> _snakeSegments;
        private int _berryX;
        private int _berryY;

        public SnakeGame()
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            _score = InitialSnakeLength;
            _gameOver = 0;

            _head = new SnakeSegment
            {
                XPos = WindowWidth / 2,
                YPos = WindowHeight / 2,
                Color = ConsoleColor.Red,
                Direction = "RIGHT"
            };

            _snakeSegments = new List<SnakeSegment>();

            GenerateBerry();
        }

        public void Run()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    HandleKeyPress(key);
                }

                MoveSnake();

                if (_gameOver == 1)
                {
                    ShowGameOverMessage();
                    break;
                }

                Thread.Sleep(200); // Adjust the speed of the game (slowed down further)
                Console.Clear();
                DrawBoundaries();
                CheckCollision();
                DrawSnake();
                DrawBerry();
                UpdateSnakePosition();
            }
        }

        private void HandleKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (_head.Direction != "DOWN")
                        _head.Direction = "UP";
                    break;
                case ConsoleKey.DownArrow:
                    if (_head.Direction != "UP")
                        _head.Direction = "DOWN";
                    break;
                case ConsoleKey.LeftArrow:
                    if (_head.Direction != "RIGHT")
                        _head.Direction = "LEFT";
                    break;
                case ConsoleKey.RightArrow:
                    if (_head.Direction != "LEFT")
                        _head.Direction = "RIGHT";
                    break;
            }
        }

        private void DrawBoundaries()
        {
            // Draw horizontal boundaries
            for (int i = 0; i < WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, WindowHeight - 1);
                Console.Write("■");
            }

            // Draw vertical boundaries
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
            // Draw snake head
            Console.SetCursorPosition(_head.XPos, _head.YPos);
            Console.ForegroundColor = _head.Color;
            Console.Write("■");

            // Draw snake body segments
            foreach (var segment in _snakeSegments)
            {
                Console.SetCursorPosition(segment.XPos, segment.YPos);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            // Draw berry
            Console.SetCursorPosition(_berryX, _berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private void GenerateBerry()
        {
            // Generate random position for the berry
            Random randomNumber = new Random();
            _berryX = randomNumber.Next(1, WindowWidth - 2);
            _berryY = randomNumber.Next(1, WindowHeight - 2);
        }

        private void CheckCollision()
        {
            // Check collision with boundaries
            if (_head.XPos == WindowWidth - 1 || _head.XPos == 0 || _head.YPos == WindowHeight - 1 || _head.YPos == 0)
            {
                _gameOver = 1;
            }

            // Check collision with berry
            if (_berryX == _head.XPos && _berryY == _head.YPos)
            {
                _score++;
                GenerateBerry();
            }

            // Check collision with snake body
            foreach (var segment in _snakeSegments)
            {
                if (segment.XPos == _head.XPos && segment.YPos == _head.YPos)
                {
                    _gameOver = 1;
                }
            }
        }

        private void MoveSnake()
        {
            // Move snake body segments
            foreach (var segment in _snakeSegments)
            {
                segment.XPrev = segment.XPos;
                segment.YPrev = segment.YPos;
                segment.XPos = segment.NextX;
                segment.YPos = segment.NextY;
            }

            // Move snake head
            _head.XPrev = _head.XPos;
            _head.YPrev = _head.YPos;

            switch (_head.Direction)
            {
                case "UP":
                    _head.YPos--;
                    break;
                case "DOWN":
                    _head.YPos++;
                    break;
                case "LEFT":
                    _head.XPos--;
                    break;
                case "RIGHT":
                    _head.XPos++;
                    break;
            }

            // Wrap around the screen
            if (_head.XPos >= WindowWidth)
                _head.XPos = 1;
            else if (_head.XPos <= 0)
                _head.XPos = WindowWidth - 2;

            if (_head.YPos >= WindowHeight)
                _head.YPos = 1;
            else if (_head.YPos <= 0)
                _head.YPos = WindowHeight - 2;
        }

        private void UpdateSnakePosition()
        {
            // Update snake body position
            _snakeSegments.Insert(0, new SnakeSegment { XPos = _head.XPrev, YPos = _head.YPrev });

            // Remove excess segments
            if (_snakeSegments.Count > _score)
            {
                _snakeSegments.RemoveAt(_snakeSegments.Count - 1);
            }
        }

        private void ShowGameOverMessage()
        {
            // Show game over message
            Console.SetCursorPosition(WindowWidth / 5, WindowHeight / 2);
            Console.WriteLine("Game over, Score: " + _score);
            Console.SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
        }

        class SnakeSegment
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public int XPrev { get; set; }
            public int YPrev { get; set; }
            public ConsoleColor Color { get; set; }
            public string Direction { get; set; }
            public int NextX { get { return Direction == "LEFT" ? XPos - 1 : Direction == "RIGHT" ? XPos + 1 : XPos; } }
            public int NextY { get { return Direction == "UP" ? YPos - 1 : Direction == "DOWN" ? YPos + 1 : YPos; } }
        }
    }
}
