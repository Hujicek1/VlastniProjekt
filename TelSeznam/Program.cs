
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TelSeznam
{
    public class TelefonniSeznam
    {
        private Dictionary<string, int> seznam;

        public TelefonniSeznam()
        {
            seznam = new Dictionary<string, int>();
        }

        public void PridatKontakt()
        {
            while (true)
            {
                Console.WriteLine("Zadejte jméno a příjmení (nechte prázdné pro ukončení): ");
                string jmeno = Console.ReadLine();
                if (jmeno.Any(c => char.IsDigit(c)))
                {
                    Console.WriteLine("Jméno je neplatné.");
                    break;
                }
                if (string.IsNullOrEmpty(jmeno)) break;
                Console.WriteLine("Zadejte číslo:");
                int cislo;
                while (!int.TryParse(Console.ReadLine(), out cislo))
                {
                    Console.WriteLine("Neplatné číslo, zkuste to znovu:");
                }
                int pocetCisel = CountDigits(cislo);
                while (pocetCisel != 9)
                {
                    Console.WriteLine("Neplatné číslo, zkuste to znovu:");
                    Console.WriteLine("Zadejte číslo:");
                    cislo = int.Parse(Console.ReadLine());
                    pocetCisel = CountDigits(cislo);
                }
                if (seznam.ContainsKey(jmeno))
                {
                    Console.WriteLine("Kontakt s tímto jménem již existuje.");
                }
                else
                {
                    seznam.Add(jmeno, cislo);
                    Console.WriteLine("Kontakt byl úspěšně přidán.");
                }
            }
        }

        public int? ZiskatCislo(string jmeno)
        {
            if (seznam.TryGetValue(jmeno, out int cislo))
            {
                return cislo;
            }
            else
            {
                Console.WriteLine("Kontakt nenalezen.");
                return null;
            }
        }

        public void VypisSeznam()
        {
            if (seznam.Count == 0)
            {
                Console.WriteLine("Telefonní seznam je prázdný.");
            }
            else
            {
                foreach (var kontakt in seznam)
                {
                    Console.WriteLine($"{kontakt.Key}: {kontakt.Value}");
                }
            }
        }

        public void SmazatKontakt(string jmeno)
        {
            if (seznam.Remove(jmeno))
            {
                Console.WriteLine("Kontakt byl úspěšně smazán.");
            }
            else
            {
                Console.WriteLine("Kontakt nenalezen.");
            }
        }

        public void SmazatCelySeznam()
        {
            seznam.Clear();
            Console.WriteLine("Všechny kontakty byly smazány.");
        }

        public void UpravitKontakt()
        {
            Console.WriteLine("Zadejte jméno kontaktu, který chcete upravit:");
            string jmeno = Console.ReadLine();
            if (seznam.ContainsKey(jmeno))
            {
                Console.WriteLine("Zadejte nové číslo:");
                int cislo;
                while (!int.TryParse(Console.ReadLine(), out cislo))
                {
                    Console.WriteLine("Neplatné číslo, zkuste to znovu:");
                }
                int pocetCisel = CountDigits(cislo);
                while (pocetCisel != 9)
                {
                    Console.WriteLine("Neplatné číslo, zkuste to znovu:");
                    Console.WriteLine("Zadejte číslo:");
                    cislo = int.Parse(Console.ReadLine());
                    pocetCisel = CountDigits(cislo);
                }
                seznam[jmeno] = cislo;
                Console.WriteLine("Kontakt byl úspěšně upraven.");
            }
            else
            {
                Console.WriteLine("Kontakt nenalezen.");
            }
        }

        public void VyhledatKontakty()
        {
            Console.WriteLine("Zadejte část jména pro vyhledávání:");
            string castJmena = Console.ReadLine();
            var nalezeneKontakty = seznam.Where(k => k.Key.Contains(castJmena)).ToList();
            if (nalezeneKontakty.Count > 0)
            {
                foreach (var kontakt in nalezeneKontakty)
                {
                    Console.WriteLine($"{kontakt.Key}: {kontakt.Value}");
                }
            }
            else
            {
                Console.WriteLine("Žádné kontakty nenalezeny.");
            }
        }

        public void ExportSeznamu(string soubor)
        {
            using (StreamWriter writer = new StreamWriter(soubor))
            {
                foreach (var kontakt in seznam)
                {
                    writer.WriteLine($"{kontakt.Key},{kontakt.Value}");
                }
            }
            Console.WriteLine("Telefonní seznam byl exportován.");
        }

        public void ImportSeznamu(string soubor)
        {
            if (File.Exists(soubor))
            {
                using (StreamReader reader = new StreamReader(soubor))
                {
                    string radek;
                    while ((radek = reader.ReadLine()) != null)
                    {
                        var data = radek.Split(',');
                        if (data.Length == 2 && int.TryParse(data[1], out int cislo))
                        {
                            seznam[data[0]] = cislo;
                        }
                    }
                }

                Console.WriteLine("Telefonní seznam byl importován.");
            }
            else
            {
                Console.WriteLine("Soubor nenalezen.");
            }
        }

        private int CountDigits(int number)
        {
            number = Math.Abs(number);
            if (number == 0)
            {
                return 1;
            }
            int count = 0;
            while (number > 0)
            {
                count++;
                number /= 10;
            }
            return count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TelefonniSeznam seznam = new TelefonniSeznam();

            while (true)
            {
                Console.WriteLine("Vyberte akci: 1 - Přidat kontakt, 2 - Získat číslo, 3 - Vypsat seznam, 4 - Smazat kontakt, 5 - Smazat celý seznam, 6 - Upravit kontakt, 7 - Vyhledat kontakty, 8 - Exportovat seznam, 9 - Importovat seznam, 10 - Konec");
                string volba = Console.ReadLine();

                switch (volba)
                {
                    case "1":
                        seznam.PridatKontakt();
                        break;
                    case "2":
                        Console.WriteLine("Zadejte jméno: ");
                        string jmeno = Console.ReadLine();
                        int? cislo = seznam.ZiskatCislo(jmeno);
                        if (cislo.HasValue)
                        {
                            Console.WriteLine($"{jmeno}: {cislo.Value}");
                        }
                        break;
                    case "3":
                        seznam.VypisSeznam();
                        break;
                    case "4":
                        Console.WriteLine("Zadejte jméno: ");
                        jmeno = Console.ReadLine();
                        seznam.SmazatKontakt(jmeno);
                        break;
                    case "5":
                        seznam.SmazatCelySeznam();
                        break;
                    case "6":
                        seznam.UpravitKontakt();
                        break;
                    case "7":
                        seznam.VyhledatKontakty();
                        break;
                    case "8":
                        Console.WriteLine("Zadejte název souboru pro export:");
                        string soubor = Console.ReadLine();
                        seznam.ExportSeznamu(soubor);
                        break;
                    case "9":
                        Console.WriteLine("Zadejte název souboru pro import:");
                        soubor = Console.ReadLine();
                        seznam.ImportSeznamu(soubor);
                        break;
                    case "10":
                        return;  // Ukončení programu
                    default:
                        Console.WriteLine("Neplatná volba.");
                        break;
                }
            }
        }
    }
}

