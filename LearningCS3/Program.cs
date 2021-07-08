using System;
namespace LearningCS3
{
    class TicTacToe
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Tic Tac Toe Game!\nWould you like to play easy (random) or hard (minimax) mode?");
            int alpha;
            Console.Write("Choose:\n1. Easy (Random)\n2. Hard (MiniMax)\nYour Choice: ");
            alpha = int.Parse(Console.ReadLine());
            bool minimax = alpha == 2;
            int[,] grid = new int[3, 3];
            while (GameOver(grid) == 0 && !CheckFull(grid))
            {
                ShowBoard(grid);
                Console.WriteLine("Using coordinates, type in your move! (Of form: 0 2 (This is the top right corner))");
                int[] x = new int[2];
                string myString = Console.ReadLine();
                x[0] = int.Parse(myString.Substring(0, myString.IndexOf(' ')).Trim());
                x[1] = int.Parse(myString.Substring(myString.IndexOf(' '), myString.Length-1).Trim());
                if (ValidLoc(grid, x))
                {
                    grid[x[0], x[1]] = 1;
                }
                else
                {
                    Console.WriteLine("Invalid, Computer goes for free!!!");
                }
                if (GameOver(grid) == 0 && !CheckFull(grid))
                {
                    int[] o = minimax ? ComputerMiniMaxGuess(grid) : ComputerRandomGuess(grid);
                    if (o[0] != -1)
                    {
                        grid[o[0], o[1]] = -1;
                    }
                }
            }

            ShowBoard(grid);
            string resp = GameOver(grid) == 0 ? "It's a tie!" : GameOver(grid) > 0 ? "You Win!" : "Computer Wins!";
            Console.WriteLine($"Game Over! {resp} Would you like to play another!?!\ny/n");
            char r = Char.Parse(Console.ReadLine());
            if (r == 'y')
            {
                Main(args);
            }
        }

        private static bool CheckFull(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 0) return false;
                }
            }
            return true;
        }

        private static bool ValidLoc(int[,] grid, int[] x) { return grid[x[0], x[1]] == 0; }

        //----------------RANDOM----------------//
        private static int[] ComputerRandomGuess(int[,] grid)
        {
            int x, y;
            Random rand = new();
            x = rand.Next(grid.GetLength(0));
            y = rand.Next(grid.GetLength(1));
            int[] guess = new int[2];
            guess[0] = x;
            guess[1] = y;

            int count = 0;
            while (!ValidLoc(grid, guess))
            {
                x = rand.Next(grid.GetLength(0));
                y = rand.Next(grid.GetLength(1));
                guess[0] = x;
                guess[1] = y;
                if (count > 100)
                {
                    Console.Write("COMPUTER COULD NOT FIND VALID SOLUTION\n");
                    break;
                }
                count++;
            }
            if (count >= 100)
            {
                guess[0] = -1;
                guess[1] = -1;
            }
            return guess;
        }

        //----------------MiniMax----------------//
        private static int[] ComputerMiniMaxGuess(int[,] grid)
        {
            int bestScore = -9999;
            int[] move = new int[2];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 0)
                    {
                        grid[i, j] = -1;
                        int score = FindBestGuess(grid, 0, false);
                        grid[i, j] = 0;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            move[0] = i;
                            move[1] = j;
                        }
                    }
                }
            }
            return move;
        }

        private static int FindBestGuess(int[,] grid, int depth, bool maximize)
        {
            int result = GameOver(grid);
            if (CheckFull(grid))
            {
                return -result;
            }
            int player = maximize ? 1 : -1;
            int bestScore = -player * 9999;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 0)
                    {
                        grid[i, j] = -player;
                        int score = FindBestGuess(grid, depth + 1, !maximize);
                        grid[i, j] = 0;
                        if (maximize)
                        {
                            bestScore = Max(score, bestScore);
                        }
                        else
                        {
                            bestScore = Min(score, bestScore);
                        }
                    }
                }
            }
            return bestScore;
        }

        private static int Max(int num1, int num2)
        {
            return num1 > num2 ? num1 : num2;
        }
        private static int Min(int num1, int num2)
        {
            return num1 < num2 ? num1 : num2;
        }





        private static void ShowBoard(int[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        Console.Write(" x ");
                    }
                    else if (grid[i, j] == -1)
                    {
                        Console.Write(" o ");
                    }
                    else
                    {
                        Console.Write(" _ ");
                    }
                    if (j < cols - 1)
                    {
                        Console.Write("|");
                    }
                }
                if (i < rows - 1) Console.Write("\n-----------");
                Console.Write("\n");
            }
        }

        //--------------------CHECK ENDING--------------------//
        private static int GameOver(int[,] grid)
        {
            int ret = 0;
            int d = CheckDiag(grid);
            int r = CheckRows(grid);
            int c = CheckCols(grid);
            if (d == 1 || d == -1)
            {
                ret = d;
            }
            else if (r == 1 || r == -1)
            {
                ret = r;
            }
            else if (c == 1 || c == -1)
            {
                ret = c;
            }
            return ret;
        }

        private static int CheckCols(int[,] grid)
        {
            int ret = 0;
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    if (grid[i, j] == 1)
                    {
                        if (ret == -1)
                        {
                            ret = 0;
                            break;
                        }
                        ret = 1;
                    }
                    else if (grid[i, j] == -1)
                    {
                        if (ret == 1)
                        {
                            ret = 0;
                            break;
                        }
                        ret = -1;
                    }
                    else
                    {
                        ret = 0;
                        break;
                    }
                }
                if (ret == 1 || ret == -1)
                {
                    break;
                }
            }
            return ret;
        }

        private static int CheckDiag(int[,] grid)
        {
            int ret = 1;
            // Checks if User won.
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (grid[i, i] == 1)
                {
                    ret = 1;
                }
                else
                {
                    ret = 0;
                    break;
                }
            }
            if (ret == 0)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[2 - i, i] == 1)
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret = 0;
                        break;
                    }
                }
            }

            // Checks if Comp won.
            if (ret == 0)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, i] == -1)
                    {
                        ret = -1;
                    }
                    else
                    {
                        ret = 0;
                        break;
                    }
                }

                if (ret == 0)
                {
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        if (grid[2 - i, i] == -1)
                        {
                            ret = -1;
                        }
                        else
                        {
                            ret = 0;
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        private static int CheckRows(int[,] grid)
        {
            int ret = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 1)
                    {
                        if (ret == -1)
                        {
                            ret = 0;
                            break;
                        }
                        ret = 1;
                    }
                    else if (grid[i, j] == -1)
                    {
                        if (ret == 1)
                        {
                            ret = 0;
                            break;
                        }
                        ret = -1;
                    }
                    else
                    {
                        ret = 0;
                        break;
                    }
                }
                if (ret == 1 || ret == -1)
                {
                    break;
                }
            }
            return ret;
        }
    }
}