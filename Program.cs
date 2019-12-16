using System;

namespace Console_Game
{
    class Program
    {
        static int mode; // кол-во колец
        static int[][] towers; // список башен
        static bool gameStatus = true; // статус игры

        static void Main(string[] args)
        {
            Console.Title = "Tower of Hanoi";
            Console.WriteLine("Choose the game mode (from 1 to 9):");
            mode = Convert.ToInt32(Console.ReadLine());
            while (mode >= 10 || mode < 1)
            {
                Console.Clear();
                Console.WriteLine("Try again");
                Console.WriteLine("Choose the game mode (from 1 to 9):");
                mode = Convert.ToInt32(Console.ReadLine());
            }
            create_towers();
            int from, to;
            render();
            
            // начало игры
            while (gameStatus)
            {
                Console.WriteLine("From:");
                from = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.WriteLine("To:");
                to = Convert.ToInt32(Console.ReadLine()) - 1;
                if (can_put(from, to))
                {
                    put(from, to);
                    render();
                }
                else
                {
                    render();
                    Console.WriteLine("Please, follow the rules");
                    continue;
                }
                if (check_win())
                {
                    break;
                }
            }
            Console.WriteLine("You did it!");
        }

        // вывод башен
        static void render()
        {
            Console.Clear();
            // сдвиг башни для правильной отрисовки
            int[][] temp_towers;
            temp_towers = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                temp_towers[i] = new int[mode];
                for (int j = 0; j < mode; j++)
                {
                    temp_towers[i][j] = towers[i][j];
                }
                if (temp_towers[i][mode - 1] == 0 && temp_towers [i][0] != 0)
                {
                    int cnt = 0;
                    while (temp_towers[i][mode - 1] == 0)
                    {
                        for (int k = mode - 1; k >= 1; k--)
                        {
                            temp_towers[i][k] = temp_towers[i][k - 1];
                        }
                        temp_towers[i][cnt] = 0;
                        cnt += 1;
                    }
                }
            }

            // отрисовка башни
            for (int i = 0; i < mode + 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 0) // первая линия (шпиль) |
                    {
                        for (int k = 0; k < mode + 1; k++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write("|");
                        for (int k = 0; k < mode + 1; k++)
                        {
                            Console.Write(" ");
                        }
                    }
                    else if (i == mode + 1) // последняя линия -----
                    {
                        for (int k = 0; k < ((mode + 1) * 2 + 1); k++)
                        {
                            Console.Write("-");
                        }
                    }
                    else // башня
                    {
                        for (int k = 0; k < mode + 1 - temp_towers[j][i - 1]; k++)
                        {
                            Console.Write(" ");
                        }
                        for (int k = 0; k < temp_towers[j][i - 1]; k++)
                        {
                            Console.Write("*");
                        }

                        if (temp_towers[j][i - 1] != 0)
                        {
                            Console.Write(temp_towers[j][i - 1]);
                        }
                        else
                        {
                            Console.Write("|");
                        }

                        for (int k = 0; k < temp_towers[j][i - 1]; k++)
                        {
                            Console.Write("*");
                        }
                        for (int k = 0; k < mode + 1 - temp_towers[j][i - 1]; k++)
                        {
                            Console.Write(" ");
                        }
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        
        // проверка конца игры
        static bool check_win()
        {
            for (int i = 1; i < 3; i++)
            {
                if (towers[i][mode - 1] != 0)
                {
                    return true;
                }
            }
            return false;

        }
        
        // перестановка колец
        static void put(int from, int to)
        {
            for (int i = mode - 1; i >= 1; i--)
            {
                towers[to][i] = towers[to][i - 1];
            }
            towers[to][0] = towers[from][0];
            for (int i = 1; i < mode; i++)
            {
                towers[from][i - 1] = towers[from][i];
            }
            towers[from][mode - 1] = 0;
        }
        
        // проверка соблюдений правил
        static bool can_put(int from, int to)
        {
            if (from > mode || to > mode)
            {
                return false;
            }
            else if (towers[to][mode - 1] != 0 || towers[from][0] == 0)
            {
                return false;
            }
            else if (towers[to][0] == 0)
            {
                return true;
            }
            else if (towers[to][0] < towers[from][0])
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // создание башен
        static void create_towers()
        {
            towers = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                towers[i] = new int[mode];
                for (int j = 0; j < mode; j++)
                    {
                    if (i == 0)
                    {
                        towers[i][j] = j + 1;
                    }
                    else
                    {
                        towers[i][j] = 0;
                    }
                }
            }
        }
    }
}