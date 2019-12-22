using System;
using System.IO;

namespace Console_Game
{
    class Program
    {
        static int mode; // кол-во колец
        static int[][] towers; // список башен
        static bool gameStatus = true; // статус игры
        static string filepath = "saved.txt";

        static void Main(string[] args)
        {
            Console.Title = "Tower of Hanoi";

            using (StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.Write("");
            }

            create_file_towers();

            // create_towers();
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
                    Console.WriteLine("Пожалуйста, следуйте правилам");
                    continue;
                }
                if (check_win())
                {
                    Console.WriteLine("Вы сделали это!");
                    gameStatus = false;
                    break;
                }
                autosave();
            }
        }

        // сохранение
        static void autosave()
        {
            using (StreamWriter sw = new StreamWriter(filepath, false))
            {
                sw.Write(mode);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < mode; j++)
                    {
                        sw.Write(towers[i][j]);
                    }
                }
            }
        }

        // создание башен
        static void create_file_towers()
        {
            if (!(last_game()))
            {
                using (StreamWriter sw = new StreamWriter(filepath, false))
                {
                    sw.Write("");
                }
                using (StreamWriter sw = new StreamWriter(filepath, true))
                {
                    choose_mode();
                    sw.Write(mode);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < mode; j++)
                        {
                            if (i == 0)
                            {
                                sw.Write(j + 1);
                            }
                            else
                            {
                                sw.Write(0);
                            }
                        }
                    }
                }
            }
            using (StreamReader sr = new StreamReader(filepath))
            {
                // перезапись башен из файла в список
                string temp = Convert.ToString((char)sr.Read());
                int temp1 = Int32.Parse(temp);
                mode = temp1;
                towers = new int[3][];
                for (int i = 0; i < 3; i++)
                {
                    towers[i] = new int[mode];
                    for (int j = 0; j < mode; j++)
                    {
                        temp = Convert.ToString((char)sr.Read());
                        temp1 = Int32.Parse(temp);
                        towers[i][j] = temp1;
                    }
                }
            }
        }

        // продолжить игру?
        static bool last_game()
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                if (sr.Peek() != -1)
                {
                    Console.WriteLine("Продолжить игру?");
                    Console.WriteLine("Да: 1 Нет: 2");
                    int answer = Convert.ToInt32(Console.ReadLine());
                    if (answer == 1)
                    {
                        Console.Clear();
                        return true;
                    }
                }
                return false;
            }
        }

        // выбор режима
        static void choose_mode()
        {
            Console.WriteLine("Выберите количество колец (от 1 до 9):");
            mode = Convert.ToInt32(Console.ReadLine());
            while (mode >= 10 || mode < 1)
            {
                Console.Clear();
                Console.WriteLine("Попробуйте ещё раз");
                Console.WriteLine("Выберите количество колец (от 1 до 9):");
                mode = Convert.ToInt32(Console.ReadLine());
            }
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
                if (temp_towers[i][mode - 1] == 0 && temp_towers[i][0] != 0)
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
                    using (StreamWriter sw = new StreamWriter(filepath, false))
                    {
                        sw.Write("");
                    }
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
            if (from > 3 || to > 3)
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
    }
}
