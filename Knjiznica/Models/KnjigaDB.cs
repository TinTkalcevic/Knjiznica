using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class KnjigaDB
    {
        private static List<Knjiga> lista = new List<Knjiga>();

        private static bool listaInicijalizirana = false;


        public KnjigaDB() {

            if(listaInicijalizirana == false)
            {
                listaInicijalizirana = true;

                lista.Add(new Knjiga()
                {
                    KnjigaId = 1,
                    NaslovKnjige = "Vlak u Snijegu",
                    ImeAutora = "Mato Lovrak",
                    GodinaObjave = new DateTime(2000, 1, 1),
                    BrojStranica = 100,
                    IzdavackaKucaId = IzdavackaKuca.Algoritam

                });

                lista.Add(new Knjiga()
                {
                    KnjigaId = 2,
                    NaslovKnjige = "Duh u mocvari",
                    ImeAutora = "Anto Gardaš",
                    GodinaObjave = new DateTime(2002, 2, 25),
                    BrojStranica = 150,
                    IzdavackaKucaId = IzdavackaKuca.Fraktura

                });

                lista.Add(new Knjiga()
                {
                    KnjigaId = 3,
                    NaslovKnjige = "Segrt Hlapic",
                    ImeAutora = "Ivana Brlić-Mažuranić",
                    GodinaObjave = new DateTime(2001, 1, 1),
                    BrojStranica = 100,
                    IzdavackaKucaId = IzdavackaKuca.Skolska_Knjiga

                });

                lista.Add(new Knjiga()
                {
                    KnjigaId = 4,
                    NaslovKnjige = "Lord od the rings",
                    ImeAutora = "John Ronald",
                    GodinaObjave = new DateTime(1986, 1, 1),
                    BrojStranica = 100,
                    IzdavackaKucaId = IzdavackaKuca.Profil

                });
            }


            
            
            
        }

        public List<Knjiga> VratiListu()
        {
            return lista;
        }

        public void AzurirajKnjigu(Knjiga knjiga)
        {
            int knjigaIndex = lista.FindIndex(x => x.KnjigaId == knjiga.KnjigaId);
            lista[knjigaIndex] = knjiga;
        }

    }
}