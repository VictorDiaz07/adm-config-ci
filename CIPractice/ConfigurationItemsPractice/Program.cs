using System;
using System.IO;
using System.Linq;

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
			Console.WriteLine("Gracias por utilizar nuestro programa. ");
        }

        private static void ConfigureDependencies()
        {
            int option = 0;
            Console.WriteLine("Escoja una de las siguientes opciones");
            Console.WriteLine("1. Agregar dependencias");
            Console.WriteLine("2. Eliminar dependencias");
            option = int.Parse(Console.ReadLine());

            Console.WriteLine("Escoja el CI deseado");
            GetAllCIs();
            int selectedCi = int.Parse(Console.ReadLine());
            if (option == 1)
            {
                Console.WriteLine("Escoja el CI dependiente del anterior");
                GetCIs(selectedCi);
                int dependantCi = int.Parse(Console.ReadLine());
                ConfigureDependency(selectedCi, dependantCi, option);
            }

            if (option == 2)
            {
				if (HasDependencies(selectedCi))
				{
					Console.WriteLine("Escoja el CI cuya dependencia desea eliminar");
					GetDependencies(selectedCi);
					int dependantCi = int.Parse(Console.ReadLine());
					ConfigureDependency(selectedCi, dependantCi, option);
				}
				else
				{
					Console.WriteLine("El CI seleccionado no posee dependencias. ");
					Console.WriteLine("Presione Enter para volver al menú");
					Console.ReadLine();
					StartupMenu();
				}
                

            }

        }

		private static bool HasDependencies(int selectedCi)
		{
			string[] registeredCIs = File.ReadAllLines("CI.txt");
			var ci = registeredCIs[selectedCi];
			int counter = 0;
			if(ci.Split("|").Length == 2)
			{
				return true;
			}
			else
			{
				var dependencies = ci.Split("|")[2].Split(',').ToList();

				foreach (var dependency in dependencies)
				{
					if (string.IsNullOrWhiteSpace(dependency))
					{
						continue;
					}
					else
					{
						counter++;
					}
				}
				if (counter == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		private static void GetDependencies(int selectedCi)
        {
			int counter = 0;
            if (File.Exists("CI.txt"))
            {
                string[] registeredCis = File.ReadAllLines("CI.txt");

                var ci = registeredCis[selectedCi];

                if (ci.Split("|").Length == 2)
                {
                    Console.WriteLine("No hay dependencias configuradas");
                }
                else
                {
                    var dependencies = ci.Split("|")[2].Split(',').ToList();

                    foreach (var dependency in dependencies)
                    {
						if (string.IsNullOrWhiteSpace(dependency))
						{
							continue;
						}
                        var Name = registeredCis[int.Parse(dependency)].Split('|')[0];
                        Console.WriteLine(dependency + ". " + Name);
						counter++;

                    }
					if (counter == 0)
					{
						Console.WriteLine("No hay dependencias configuradas. ");
					}
                }
               
            }
            else
            {
                Console.WriteLine("No hay CIs registrados");
                StartupMenu();
            }
        }

        private static void ConfigureDependency(int selectedCi, int dependantCi, int option)
        {
            if (option == 1)
            {
                string[] configurationItems = File.ReadAllLines("CI.txt");
                string configurationItem = configurationItems[selectedCi];
                var ci = configurationItem.Split("|");

                if (ci.Length == 2)
                {
                    configurationItem += "|" + dependantCi;
                }

                else if (ci.Length == 3)
                {
                    configurationItem += "," + dependantCi;
                }

                configurationItems[selectedCi] = configurationItem;
                File.WriteAllLines("CI.txt", configurationItems);

            }

            if (option == 2)
            {
                string[] configurationItems = File.ReadAllLines("CI.txt");
                string configurationItem = configurationItems[selectedCi];
                var ci = configurationItem.Split("|");

                if (ci.Length == 2)
                {
                    Console.WriteLine("No hay dependencias configuradas para este CI");
                }

                else if (ci.Length == 3)
                {
                    var dependencies = ci[2].Split(",").ToList();
                    dependencies.Remove(dependantCi.ToString());
                    string remainingDependencies = "";

                    foreach (var dependency in dependencies)
                    {
                        remainingDependencies += dependency + ",";
                    }

                    ci[2] = remainingDependencies.ToString();
                    string line = ci[0] + "|" + ci[1] + "|" + ci[2];
                    configurationItems[selectedCi] = line;

                    File.WriteAllLines("CI.txt", configurationItems);

                }
            }

            Console.WriteLine("Dependencia configurada exitosamente");

            StartupMenu();

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

        private static void GetCIs(int selectedCi)
        {
            if (File.Exists("CI.txt"))
            {
                string[] registeredCis = File.ReadAllLines("CI.txt");
                for (int i = 0; i < registeredCis.Length; i++)
                {
                    if (i != selectedCi)
                    {
                        var Name = registeredCis[i].Split('|')[0];
                        Console.WriteLine(i + ". " + Name);
                    }

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
            var CiInfo = new ConfigurationItem(ciName, ciVersion);

            if (!File.Exists("CI.txt"))
            {
                StreamWriter sw = new StreamWriter("CI.txt");
                sw.WriteLine(CiInfo.Name + "|" + ciVersion);
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(File.Open("CI.txt", FileMode.Append));
                sw.WriteLine(CiInfo.Name + "|" + ciVersion);
                sw.Close();
            }
			Console.Clear(); 
			StartupMenu();
        }

        static void Main(string[] args)
        {
            StartupMenu();
        }
    }
}
