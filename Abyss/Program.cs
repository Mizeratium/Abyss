using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Abyss
{
    public class Program
    {
        static void Main(string[] args)
        {
            // ТЕХНИЧЕСКАЯ ЧАСТЬ: ОБЪЯВЛЕНИЕ ПЕРЕМННЫХ, НАПОЛНЕНИЕ МАССИВОВ, РАНДОМАЙЗЕР    
            int room = 1; //количество пройденных комнат
            int[] dungeonMap = new int[10];
            string poison = "Восстановительное зелье", none = "---"; //заполнение инвентаря амуницией и пустотой
            string[] inventory = new string[5] { poison, poison, poison, none, none };
            int health = 100, gold = 30, arrow = 5; //HP
            Random rnd = new Random();

            // НАЧАЛО ИГРЫ
            Console.WriteLine("Добро пожаловать в игру Подземелье!\nПройдите все 9 комнат, прежде чем сразиться с боссом в десятой комнате!");
            GameStatistic(room);

            for (int i = 0; i < dungeonMap.Length; i++)
            {
                dungeonMap[i] = rnd.Next(1, 5); //заполнене испытаниями
            }

            for (int i = 0; i < dungeonMap.Length; i++)
            {
                Console.WriteLine($"\n----------------------------------Комната {i}----------------------------------");
                switch (dungeonMap[i])
                {
                    case 1: Monster(ref health, ref arrow, inventory, poison, none); break;
                    case 2: Chester(ref gold, ref arrow, inventory, poison, none); break;
                    case 3: Pitfall(ref health); break;
                    case 4: Trader(inventory, ref gold, inventory, poison, none); break;
                }
            }

            Console.WriteLine("\nВы прошли все комнаты! Подготовьтесь к битве с боссом в десятой комнате!\n");
            Boss(ref health, ref arrow, inventory, poison, none);
        }

        /// <summary>
        /// Метод мониторинга статистики пройденных уровней
        /// </summary>
        /// <param name="roomsCompleted"></param>
        static void GameStatistic(int roomsCompleted)
        {
            int totalRooms = 10;
            int roomsCount = totalRooms - roomsCompleted; // Осталось комнат
            Console.WriteLine($"Комнат пройдено: {roomsCompleted}\nОсталось комнат: {roomsCount}");
        }

        /// <summary>
        /// Показ инвентаря, взаимодействие ДОДЕЛАТЬ
        /// </summary>
        /// <param name="inv"></param>
        static void ShowInventory(string[] inv)
        {
            int i = 1;
            Console.WriteLine("\nИНВЕНТАРЬ\n");
            foreach (var elem in inv)
            {
                Console.WriteLine($"[{i}]" + elem);
                i++;
            }
        }

        /// <summary>
        /// Ивент битвы с монстром
        /// </summary>
        /// <param name="health"> Здоровье игрока </param>
        /// <param name="arrow"> Стрелы игрока </param>
        /// <param name="inventory"> Инвентарь игрока </param>
        /// <param name="poison"> Восстанавливающее зелье </param>
        /// <param name="none"> --- </param>
        /// <returns></returns>
        static int Monster(ref int health, ref int arrow, string[] inventory, string poison, string none)
        {
            int damage, heal; //Получаемый урон от монстра и получаемые НР от зелья
            Random random = new Random();
            Console.WriteLine("\nНа пути к выходу Вас настиг Монстр!\n");
            int monsterHealth = random.Next(20, 50); //случайное кол-во НР 
            Console.WriteLine("Выберите оружие:\n1) Меч - наносит 10-20 урона\n2) Лук - наносит 5-15 урона\n");
            int weapon = int.Parse(Console.ReadLine());
            if (weapon == 1)//алгоритм для меча
            {
                while (monsterHealth > 0)
                {
                    if (health <= 0) //если у игрока нет НР, игра закончится
                    {
                        Console.WriteLine("Вы проиграли! =(");
                        break;
                    }
                    Console.WriteLine($"Ваше здоровье {health}\tЗдоровье противника {monsterHealth}");
                    Console.WriteLine($"[ Выберите действие ]\t1 - Удар\t 2 - Использовать зелье\n");
                    if (Console.ReadLine() == "1")
                    {
                        monsterHealth -= random.Next(10, 20);
                        damage = random.Next(5, 15);
                        Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                        health -= damage;
                    }
                    else if (Console.ReadLine() == "2")
                    {
                        if (inventory.Contains(poison)) //проверка на наличие зелья в инвентаре
                        {
                            if (health >= 100) //если здоровье полное, зелье не потратится, игрок пропустит ход
                            {
                                Console.WriteLine("\n[ У вас максимальное HP ]\n");
                                damage = random.Next(5, 15);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                            else
                            {
                                heal = random.Next(5, 20);
                                health += heal;
                                Console.WriteLine($"\n[ Вы использовали Восстановительное зелье ]\t [+ {heal} HP]\n");
                                inventory[Array.IndexOf(inventory, poison)] = none; //убираем 1 зелье из инвентаря игрока
                                if (health > 100) //здоровье игрока не может быть больше 100 НР
                                {
                                    health = 100;
                                }
                                damage = random.Next(5, 15);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                        }
                    }
                }
                if (monsterHealth < health)
                {
                    Console.WriteLine("[ Вы прошли комнату ]");
                }
            }
            else if (weapon == 2)//алгоритм для лука
            {
                while (monsterHealth > 0)
                {
                    if (health <= 0)
                    {
                        Console.WriteLine("Вы проиграли! =(");
                        break;
                    }
                    Console.WriteLine($"Ваше здоровье {health}\tЗдоровье противника {monsterHealth}");
                    Console.WriteLine($"[Осталось стрел - {arrow} ]\n[Выберите действие]\t1 - Удар\t 2 - Использовать зелье\n");
                    if (Console.ReadLine() == "1")
                    {
                        if (arrow == 0)
                        {
                            Console.WriteLine("\n[ Вы не можете атаковать ]\n");
                            damage = random.Next(5, 15);
                            Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                            health -= damage;
                        }
                        else
                        {
                            monsterHealth -= random.Next(10, 20);
                            damage = random.Next(5, 15);
                            Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                            health -= damage;
                            arrow--;
                        }
                    }
                    else
                    {
                        if (inventory.Contains(poison))
                        {
                            if (health >= 100)
                            {
                                Console.WriteLine("\n[ У вас максимальное HP ]\n");
                                damage = random.Next(5, 15);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                            else
                            {
                                heal = random.Next(5, 20);
                                health += heal;
                                Console.WriteLine($"\n[ Вы использовали Восстановительное зелье ]\t [+ {heal} HP]\n");
                                inventory[Array.IndexOf(inventory, poison)] = none;
                                if (health > 100)
                                {
                                    health = 100;
                                }
                                damage = random.Next(5, 15);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                        }
                    }
                }
                if (monsterHealth < health)
                {
                    Console.WriteLine("[ Вы прошли комнату ]");
                }
            }
            return health;
        }

        /// <summary>
        /// Ловушка
        /// </summary>
        static void Pitfall(ref int health)
        {
            Random random = new Random();
            int damage = random.Next(10, 20);
            health -= damage;
            Console.WriteLine($"\nВы попали в ловушку!\t [-{damage} HP]\nОсталось {health} HP");
            if (health <= 0)
            {
                Console.WriteLine("Вы проиграли! =(");
            }
        }

        /// <summary>
        /// Очень сложная математическая загадка
        /// </summary>
        static void Chester(ref int money, ref int arrow, string[] inventory, string poison, string none)
        {
            Random rnd = new Random();
            int bonus;
            Console.WriteLine("\n[ Отгадайте загадку, чтобы идти дальше ]\nСколько хвостов у девяти котов?");
            while (Console.ReadLine() != "9")
            {
                Console.WriteLine("\tПопробуйте снова.");
            }
            Console.WriteLine("\nВы прошли комнату!\n");
            switch (rnd.Next(1, 3)) //Случайный подарок из сундука
            {
                case 1:
                    Console.WriteLine("Вы нашли Зелье!\t[Получено Зелье 1 шт.]");
                    if (inventory.Contains(none))
                    {
                        inventory[Array.IndexOf(inventory, none)] = poison;
                    }
                    break;
                case 2:
                    bonus = rnd.Next(10, 30);
                    Console.WriteLine($"[ Получено Золото {bonus} шт.]\n\n[Всего золота - {money + bonus} шт. ]");
                    break;
                case 3:
                    bonus = rnd.Next(1, 5); Console.WriteLine($"Вы нашли Стрелы!\t[ Получено Стрелы {bonus} шт. ]");
                    arrow += bonus;
                    break;
            }
        }

        /// <summary>
        /// Торговец зельями
        /// </summary>
        /// <param name="inv"></param>
        /// <param name="money"></param>
        static void Trader(string[] inv,ref int money, string[] inventory, string poison, string none)
        {
            Console.WriteLine("Вы наткнулись на торговца!");
            ShowInventory(inv);
            Console.WriteLine("\nМАГАЗИН\n\nВосстновительное зелье (1 шт.)\t 30 золота");
            Console.WriteLine($"[ У Вас есть {money} зол. ]\n1 - купить\t2 - идти дальше");
            if (Console.ReadLine() == "1")
            {
                if (money >= 30)
                {
                    if (inventory.Contains(none))
                    {
                        money -= 30;
                        Console.WriteLine($"Вы приобрели [ Восстановительное зелье ]\tБаланс - {money} зол.");
                        if (inventory.Contains(none))
                        {
                            inventory[Array.IndexOf(inventory, none)] = poison;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n[ Нет места ]\n");
                    }
                    
                }
                else
                {
                    Console.WriteLine("\n[ Недостаточно средств ]\n");
                }
            }
            else
            {
                Console.WriteLine("\nВы прошли комнату!");
            }
        }

        /// <summary>
        /// Событие битвы с финальным боссом
        /// </summary>
        /// <param name="health"> Здоровье игрока </param>
        /// <param name="arrow"> Стрелы игрока </param>
        /// <param name="inventory"> Инвентарь игрока </param>
        /// <param name="poison"> Восстанавливающие зелья </param>
        /// <param name="none"> --- </param>
        /// <returns></returns>
        static int Boss(ref int health, ref int arrow, string[] inventory, string poison, string none)
        {
            int damage, heal;
            Random random = new Random();
            Console.WriteLine("\nВы столкнулись с Боссом!\n");
            int monsterHealth = 80; //НР Босса
            Console.WriteLine("Выберите оружие:\n1) Меч - наносит 10-20 урона\n2) Лук - наносит 5-15 урона\n");
            int weapon = int.Parse(Console.ReadLine());
            if (weapon == 1)//алгоритм для меча
            {
                while (monsterHealth > 0)
                {
                    if (health <= 0) //если у игрока нет НР, игра закончится
                    {
                        Console.WriteLine("[ Вы проиграли ]");
                        break;
                    }
                    Console.WriteLine($"Ваше здоровье {health}\tЗдоровье противника {monsterHealth}");
                    Console.WriteLine("[ Выберите действие ]\t1 - Удар\t 2 - Использовать зелье\n");
                    if (Console.ReadLine() == "1")
                    {
                        monsterHealth -= random.Next(10, 20);
                        damage = random.Next(10, 20);
                        Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                        health -= damage;
                    }
                    else
                    {
                        if (inventory.Contains(poison)) //проверка на наличие зелья в инвентаре
                        {
                            if (health >= 100) //если здоровье полное, зелье не потратится, игрок пропустит ход
                            {
                                Console.WriteLine("\n[У вас максимальное HP]\n");
                                damage = random.Next(10, 20);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                            else
                            {
                                heal = random.Next(5, 20);
                                health += heal;
                                Console.WriteLine($"\n[Вы использовали Восстановительное зелье]\t [+ {heal} HP]\n");
                                inventory[Array.IndexOf(inventory, poison)] = none; //убираем 1 зелье из инвентаря игрока
                                if (health > 100) //здоровье игрока не может быть больше 100 НР
                                {
                                    health = 100;
                                }
                                damage = random.Next(10, 20);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n[ У Вас закончились зелья ]\n");
                        }
                    }
                }
                if (monsterHealth < health)
                {
                    Console.WriteLine("[ Вы прошли комнату ]");
                }
            }
            else if (weapon == 2)//алгоритм для лука
            {
                while (monsterHealth > 0)
                {
                    if (health <= 0)
                    {
                        Console.WriteLine("Вы проиграли! =(");
                        break;
                    }
                    Console.WriteLine($"Ваше здоровье {health}\tЗдоровье противника {monsterHealth}");
                    Console.WriteLine($"[ Осталось стрел - {arrow} ]\n[ Выберите действие ]\t1 - Удар\t 2 - Использовать зелье\n");
                    if (Console.ReadLine() == "1")
                    {
                        if (arrow == 0)
                        {
                            Console.WriteLine("\n[ Вы не можете атаковать ]\n");
                            damage = random.Next(10, 20);
                            Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                            health -= damage;
                        }
                        else
                        {
                            monsterHealth -= random.Next(10, 20);
                            damage = random.Next(10, 20);
                            Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                            health -= damage;
                            arrow--;
                        }
                    }
                    else
                    {
                        if (inventory.Contains(poison))
                        {
                            if (health >= 100)
                            {
                                Console.WriteLine("\n[ У вас максимальное HP ]\n");
                                damage = random.Next(10, 20);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                            else
                            {
                                heal = random.Next(5, 20);
                                health += heal;
                                Console.WriteLine($"\n[ Вы использовали Восстановительное зелье ]\t [+ {heal} HP]\n");
                                inventory[Array.IndexOf(inventory, poison)] = none;
                                if (health > 100)
                                {
                                    health = 100;
                                }
                                damage = random.Next(10, 20);
                                Console.WriteLine($"\nМонстр атакует!\t -{damage}HP\n");
                                health -= damage;
                            }
                        }
                    }
                }
                if (monsterHealth < health)
                {
                    Console.WriteLine("[ Вы победили ]");
                }
            }
            return health;
        }
    }
}
