using System;
using System.IO;

namespace ConfigurationItemsPractice
{
    class Program
    {
        public static void StartupMenu()
        {
            Console.Clear();
            int option;
            Console.WriteLine("Bienvenido a CI Manager");
            Console.WriteLine("Opcion 1: Registrar un nuevo CI");
            Console.WriteLine("Opcion 2: Ver CIs registrados");
            Console.WriteLine("Opcion 3: Configurar dependencias");
            Console.WriteLine("Opcion 4: Salir");
            Console.Write("Inserte una de las siguientes opciones para continuar: ");
            string choosedOption = Console.ReadLine();
            option = int.Parse(choosedOption);
            switch (option)
            {
                case 1:
                    RegisterNewCI();
                    break;
                case 2:
                    GetAllCIs();
                    break;
                case 3:
                    ConfigureDependencies();
                    break;
                case 4:
                    ExitProgram();
                    break;
            }
        }

        private static void ExitProgram()
        {
            throw new NotImplementedException();
        }

        private static void ConfigureDependencies()
        {
            throw new NotImplementedException();
        }

        private static void GetAllCIs()
        {
            if (File.Exists("CI.txt"))
            {
                string[] registeredCis = File.ReadAllLines("CI.txt");
                for (int i = 0; i < registeredCis.Length; i++)
                {
                    var Name = registeredCis[i].Split('|')[0];
                    Console.WriteLine(i + ". " + Name);
                }
            }
            else
            {
                Console.WriteLine("No hay CIs registrados");
                StartupMenu();
            }
            
        }

        private static void RegisterNewCI()
        {
            string ciName, ciVersion;
            Console.Clear();
            Console.WriteLine("Inserte el nombre de su CI: ");
            ciName = Console.ReadLine();
            Console.WriteLine("Inserte el numero de version de su CI: ");
            ciVersion = Console.ReadLine();
            var CiInfo = new ConfigurationItem(ciName,ciVersion);

            if (!File.Exists("CI.txt"))
            {
                StreamWriter sw = new StreamWriter("CI.txt");
                sw.WriteLine(CiInfo.Name + " | " + ciVersion + " | ");
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(File.Open("CI.txt", FileMode.Append));
                sw.WriteLine(CiInfo.Name + " | " + ciVersion + " | ");
                sw.Close();
            }
        }

        static void Main(string[] args)
        {
            StartupMenu();
        }
    }
}
