using System;
using System.IO;


namespace zgadywaniezwierzatdrzewobinarne
{
    internal class Program
    {
        class Dane
        {
            public string tresc;
            public bool czyPyt;
            public Dane tak, nie;

            public Dane(string nTresc, bool nCzyPyt)
            {
                tresc = nTresc;
                czyPyt = nCzyPyt;
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            //zmienne
            Dane poczatek = null, aktywnePyt;
            bool koniecDef = false, koniecCykl;
            string odp, nazwaPliku;
            StreamReader plikDoOdczytu;
            StreamWriter plikDoZapisu;


            //funkcje
            void NoweZwierze()
            {
                do
                {
                    Console.WriteLine("podaj, proszę, nazwę zwierzęcia, o którym myślałxś: ");
                    odp = Console.ReadLine().ToLower();
                } while (odp == "");
                if (odp == "stop")
                {
                    koniecDef = true;
                    return;
                }

                string roznica;

                do
                {
                    Console.WriteLine("napisz jeszcze, czym różni się zwierzę o nazwie " + odp + " od zwierzęcia o nazwie " + aktywnePyt.tresc + ": ");
                    roznica = Console.ReadLine().ToLower();
                } while (roznica == "");
                if (roznica == "stop")
                {
                    koniecDef = true;
                    return;
                }
                aktywnePyt.nie = new Dane(aktywnePyt.tresc, false);
                aktywnePyt.tak = new Dane(odp, false);
                aktywnePyt.tresc = roznica;
                aktywnePyt.czyPyt = true;
                plikDoZapisu = File.CreateText(nazwaPliku);
                Zapisz(poczatek);
                plikDoZapisu.Dispose();
            }

            //zapisywanie danych
            void Zapisz(Dane pytanie)
            {
                if (pytanie.czyPyt)
                {
                    plikDoZapisu.WriteLine(pytanie.tresc);
                    Zapisz(pytanie.tak);
                    Zapisz(pytanie.nie);
                }
                else
                {
                    plikDoZapisu.WriteLine("!" + pytanie.tresc);
                }
            }

            //wczytywanie danych
            Dane Wczytaj(StreamReader plikDanych)
            {
                string tresc = plikDoOdczytu.ReadLine();
                Dane nowePytanie;
                if (tresc[0] == '!')
                {
                    tresc = tresc.Substring(1);
                    nowePytanie = new Dane(tresc, false);
                }
                else
                {
                    nowePytanie = new Dane(tresc, true)
                    {
                        tak = Wczytaj(plikDanych),
                        nie = Wczytaj(plikDanych)
                    };
                }
                return nowePytanie;
            }

            //program
            nazwaPliku = AppDomain.CurrentDomain.BaseDirectory + "dane.txt";
            if (File.Exists(nazwaPliku))
            {
                plikDoOdczytu = File.OpenText(nazwaPliku);
                poczatek = Wczytaj(plikDoOdczytu);
                plikDoOdczytu.Dispose();
            }
            else poczatek = new Dane("płaszczka", false);
            Console.WriteLine("dzień dobry. spróbuję zgadnąć, jakie zwierzę masz na myśli.\njeśli będziesz chciałx skończyć, wpisz proszę \"stop\"");

            do
            {
                koniecCykl = true;
                aktywnePyt = poczatek;
                Console.WriteLine("\n---\nwymyśl zwierzę.\n");

                do
                {
                    if (aktywnePyt.czyPyt)
                    {
                        do
                        {
                            Console.WriteLine("czy zwierzę, o którym myślisz, " + aktywnePyt.tresc + "? (tak/nie)");
                            odp = Console.ReadLine().ToLower();
                        } while (odp != "tak" && odp != "nie" && odp != "stop");
                        if (odp == "stop") koniecDef = true;
                        else
                        {
                            if (odp == "tak") aktywnePyt = aktywnePyt.tak;
                            else aktywnePyt = aktywnePyt.nie;
                        }
                    }

                    else
                    {
                        do
                        {
                            Console.WriteLine("czy zwierzę, o którym myślisz, to " + aktywnePyt.tresc + "? (tak/nie)");
                            odp = Console.ReadLine().ToLower();
                        } while (odp != "tak" && odp != "nie" && odp != "stop");
                        if (odp == "stop") koniecDef = true;
                        else
                        {
                            if (odp == "tak")
                            {
                                Console.WriteLine("miło.");
                                koniecCykl = false;
                            }
                            else
                            {
                                Console.WriteLine("och.");
                                koniecCykl = false;
                                NoweZwierze();
                            }
                        }
                    }

                } while (!koniecDef && koniecCykl);

            } while (!koniecDef);
        }
    }
}