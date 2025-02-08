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
            

            for (int i = 0; i < dungeonMap.Length; i++)
            {
                dungeonMap[i] = rnd.Next(1, 5); //заполнене испытаниями


            }

            /// <summary>
            /// Метод мониторинга статистики пройденных уровней
            /// </summary>
            /// <param name="roomsCompleted"></param>
        }
            static void GameStatistic(int roomsCompleted)
            {
                int totalRooms = 10;
                int roomsCount = totalRooms - roomsCompleted; // Осталось комнат
                Console.WriteLine($"Комнат пройдено: {roomsCompleted}\nОсталось комнат: {roomsCount}");
            }

    }
}
