using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PackingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] humanBox = new char[60]; // kullanıcının blockları yerleştirmesi için string tipinde array (humanBox[0] => box1)
            char[] computerBox = new char[60]; // bilgisayar için boxlar
            char[] bestScoreBox = new char[60]; // en iyi skoru tutacak olan boxlar
            char[] humanBox0 = new char[108]; // kullanıcı tarasındaki blockları tutacak olan box0 (humanBox0[0] => A harflerini tutan 1. eleman)
            char[] computerBox0 = new char[108]; // bilgisayar için box0
            char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' }; // harf dizimiz
            int[] letterIndexes = new int[12]; // harflerin başladığı index i tutacak dizi kutudan çıkarma işlemlerinde işimize yarayacak
            char[] command = new char[2]; // girilen komutları tutacak char dizi
            char[] block, box; // block human ve computer için box0 da blockları görüntülemede kullanılacak,Box kutulara yerleştirmede kullanılacak
            int humanScore = 0, computerScore = 0, bestScore = 0, time = 60; // skorlar ve süreyi tutan değişkenler
            Random rnd; // random oluşturmak için değişken
            int index = 0; // harfleri rastgele atarken dizinin index değerini tutacak
            int karakterSayi = 0, puan; // block oluştururken karakterSayı, box oluştururken puan hesaplanacak

            // rastgele kutular dolduruluyor
            for (int i = 0; i < letters.Length; i++)
            {
                rnd = new Random(Guid.NewGuid().GetHashCode()); // aynı sayıları üretmekten kaçınmak için bir yöntem
                int random = rnd.Next(4, 9); // 4, 9 arası random sayı alınıyor
                letterIndexes[i] = index; // index tutulan diziye harflerin başlangıç indexleri kaydediliyor
                // random sayı adeti kadar box0 lara harf atanıyor
                for (int j = 0; j < random; j++)
                {
                    humanBox0[index] = letters[i];
                    computerBox0[index] = letters[i];
                    index++;
                }
            }

            // tanımlanan diziler boş olarak dolduruluyor
            for (int i = 0; i < 60; i++)
            {
                humanBox[i] = ' ';
                computerBox[i] = ' ';
                bestScoreBox[i] = ' ';
            }

            // İşlem döngüsü ------------------------------------------------

            DateTime dt = DateTime.Now;

            while (true)
            {
                // süre bittiyse sonlandır
                if (time == 0)
                    break;

                DateTime dt2 = DateTime.Now;
                TimeSpan ts = dt2 - dt;
                if (ts.TotalMilliseconds >= 1000)
                {
                    time--;
                    // Console.Write(time);
                    dt = dt2;
                }


                #region screen
                Console.Clear(); // console temizleniyor
                // Ekrana bilgiler yazdırılıyor...
                // 1. satır -------------------------------------------------
                addSpace(17); // boşluk ekleme fonksiyonu (17 boşluk bırakır)
                Console.Write("Human");
                addSpace(20);
                Console.Write("Time=" + time.ToString("00"));
                addSpace(15);
                Console.Write("Computer");
                Console.WriteLine();
                // 2. satır -------------------------------------------------
                addSpace(17);
                Console.Write("----------");
                addSpace(37);
                Console.Write("----------");
                Console.WriteLine();
                // 3. satır -------------------------------------------------
                addSpace(17);
                Console.Write("Score=  " + humanScore.ToString("00"));
                addSpace(19);
                Console.Write("|");
                addSpace(17);
                Console.Write("Score=  " + computerScore.ToString("00"));
                Console.WriteLine();
                // 4. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                Console.WriteLine();
                // 5. satır -------------------------------------------------
                Console.Write("  [0]");
                addSpace(41);
                Console.Write("|");
                Console.Write("  [0]");
                Console.WriteLine();
                // 6. satır -------------------------------------------------
                // human için
                block = blockOlustur(humanBox0, letters[0], out karakterSayi); // A harfi için block oluşturuluyor
                Console.Write(karakterSayi + " "); // elde edilen karakter sayısı yazdırılıyor
                printBlock(block); // block yazdırılıyor
                addSpace(44 - block.Length); //block genişliğine göre boşluk bırakılıyor
                Console.Write("|");
                // computer için
                block = blockOlustur(computerBox0, letters[0], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - computerBox0.Length);
                Console.WriteLine();
                // 7. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[1], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(19 - block.Length);
                Console.Write("5   10   15   20         |");
                block = blockOlustur(computerBox0, letters[1], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(19 - block.Length);
                Console.Write("5   10   15   20");
                Console.WriteLine();
                // 8. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[2], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(15 - block.Length);
                Console.Write("----+----+----+----+         |");
                block = blockOlustur(computerBox0, letters[2], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(15 - block.Length);
                Console.Write("----+----+----+----+");
                Console.WriteLine();
                // 9. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[3], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[1] ");
                box = boxOlustur(humanBox, 1, out puan); // kutu oluşturuluyor
                printBlock(box); // kutu yazdırılıyor
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00") + "      |"); // puan yazdırılıyor
                // Computer-----------------------------------------------------------
                block = blockOlustur(computerBox0, letters[3], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[1] ");
                box = boxOlustur(computerBox, 1, out puan);
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();
                // 10. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[4], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[2] ");
                box = boxOlustur(humanBox, 2, out puan);
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00") + "      |");
                block = blockOlustur(computerBox0, letters[4], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[2] ");
                box = boxOlustur(computerBox, 2, out puan);
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();
                // 11. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[5], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[3] ");
                box = boxOlustur(humanBox, 3, out puan);
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00") + "      |");
                block = blockOlustur(computerBox0, letters[5], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(11 - block.Length);
                Console.Write("[3] ");
                box = boxOlustur(computerBox, 3, out puan);
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();
                // 12. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[6], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[6], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - block.Length);
                Console.WriteLine();
                // 13. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[7], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[7], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - block.Length);
                Console.WriteLine();
                // 14. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[8], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[8], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - block.Length);
                Console.WriteLine();
                // 15. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[9], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[9], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - block.Length);
                Console.WriteLine();
                // 16. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[10], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[10], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(15 - block.Length);
                Console.Write("Best Score=  " + bestScore.ToString("00"));
                Console.WriteLine();
                // 17. satır -------------------------------------------------
                block = blockOlustur(humanBox0, letters[11], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(44 - block.Length);
                Console.Write("|");
                block = blockOlustur(computerBox0, letters[11], out karakterSayi);
                Console.Write(karakterSayi + " ");
                printBlock(block);
                addSpace(48 - block.Length);
                Console.WriteLine();
                // 18. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                addSpace(21);
                Console.Write("5   10   15   20");
                Console.WriteLine();
                // 19. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                addSpace(17);
                Console.Write("----+----+----+----+");
                Console.WriteLine();
                // 20. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                addSpace(13);
                box = boxOlustur(bestScoreBox, 1, out puan);
                Console.Write("[1] ");
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();
                // 21. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                addSpace(13);
                box = boxOlustur(bestScoreBox, 2, out puan);
                Console.Write("[2] ");
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();
                // 22. satır -------------------------------------------------
                addSpace(46);
                Console.Write("|");
                addSpace(13);
                box = boxOlustur(bestScoreBox, 3, out puan);
                Console.Write("[3] ");
                printBlock(box);
                addSpace(20 - box.Length);
                Console.Write(" " + puan.ToString("00"));
                Console.WriteLine();

                // Son satır -------------------------------------------------
                Console.WriteLine();
                Console.WriteLine();

                #endregion

                // human ve computer için alt kısıma box0 ekleniyor
                char[] humanTemp = new char[108];
                char[] compTemp = new char[108];
                index = 0;
                for (int i = 0; i < humanBox0.Length; i++)
                    if (humanBox0[i] != ' ')
                        humanTemp[index++] = humanBox0[i];

                int count = 0;
                for (int i = 0; i < 46; i++)
                {
                    Console.Write(humanTemp[i]);
                    count++;
                }
                addSpace(47 - count);

                index = 0;
                for (int i = 0; i < computerBox0.Length; i++)
                    if (computerBox0[i] != ' ')
                        compTemp[index++] = computerBox0[i];

                for (int i = 0; i < 46; i++)
                    Console.Write(compTemp[i]);

                Console.WriteLine();
                index = 46;
                count = 0;
                for (int i = 0; i < 46; i++)
                {
                    Console.Write(humanTemp[index++]);
                    count++;
                }
                addSpace(47 - count);

                index = 46;
                for (int i = 0; i < 46; i++)

                    Console.Write(compTemp[index++]);

                Console.WriteLine();
                Console.WriteLine("Please enter the command:");
                //------------------------------------------------------------


                #region   bilgisayarın hamlesi



                // random harf ve kutu id'si oluştur
                rnd = new Random(Guid.NewGuid().GetHashCode());
                int letterIndex = rnd.Next(12); // random harf seçimi
                int boxNumber = rnd.Next(4); // random işlem seçimi
                char letter = letters[letterIndex]; // harf diziden alınıyor

                // human kısmıyla aynı
                // çıkarma işlemiyse
                if (boxNumber == 0)
                {
                    computerScore -= karakterSayisi(computerBox, letter);
                    kutudanCikar(ref computerBox, ref computerBox0, letterIndexes[letterIndex], letter);
                }
                else // ekleme işlemiyse
                {

                    if (!mevcut(computerBox, letter))
                    {
                        kutuyaEkle(ref computerBox, ref computerBox0, letter, boxNumber);
                        computerScore += karakterSayisi(computerBox, letter);
                    }
                }

                // eğer skor en iyiden büyükse en iyi skoru güncelle
                if (computerScore >= bestScore)
                {
                    bestScore = computerScore;
                    // en iyi kutusuna güncel kutuyu ata
                    for (int i = 0; i < computerBox.Length; i++)
                        bestScoreBox[i] = computerBox[i];
                }


                #endregion


                #region  Kullanıcıdan komut bekleniyor -----------------------------

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo cki = Console.ReadKey();
                    if (command[0] == '\0')
                        command[0] = cki.KeyChar;
                    else if (command[1] == '\0')
                        command[1] = cki.KeyChar;

                    if (command[0] != '\0' && command[1] != '\0')
                    {
                        // harf sayısı kadar döngü
                        for (int i = 0; i < 12; i++)
                        {
                            // girilen harf harf dizimizde mevcutsa gerçekleştir
                            if (command[0] == letters[i])
                            {
                                // çıkarma işlemiyse puanı eksilt ve çıkarma işlemini gerçekleştir
                                if (command[1] == '0')
                                {
                                    humanScore -= karakterSayisi(humanBox, letters[i]);
                                    kutudanCikar(ref humanBox, ref humanBox0, letterIndexes[i], letters[i]);
                                }

                                // ekleme işlemiyse gerçekleştir
                                if (command[1] >= '1' & command[1] <= '3')
                                {
                                    // eğer eklenmek istenen karakter kutuda yoksa gerçekleştir
                                    if (!mevcut(humanBox, letters[i]))
                                    {
                                        // ilgili kutuya ekle ve puanı arttır
                                        int boxId = Convert.ToInt32(command[1].ToString());
                                        kutuyaEkle(ref humanBox, ref humanBox0, letters[i], boxId);
                                        humanScore += karakterSayisi(humanBox, letters[i]);
                                    }
                                }
                            }
                        }

                        command[0] = '\0';
                        command[1] = '\0';
                    }
                }
                #endregion
            }


            // süre dolduysa bu gerçekleşecek komutlar
            Console.WriteLine("Game Over!");
            if (computerScore > humanScore)
                Console.WriteLine("Computer win!");
            else
                Console.WriteLine("You win!");

            Console.ReadKey();
            Console.ReadKey();
        }

        // boşluk ekleme fonksiyonu kendisine gönderilen int değişken değeri kadar boşluk bırakır
        private static void addSpace(int length)
        {
            for (int i = 0; i < length; i++)
                Console.Write(" ");
        }

        // blockları ekrana basma fonksiyonu
        private static void printBlock(char[] block)
        {
            for (int i = 0; i < block.Length; i++)
                Console.Write(block[i]);
        }

        // kendisine gönderilen dizide istenilen karakterin sayısını dönen fonksiyon
        private static int karakterSayisi(char[] dizi, char karakter)
        {
            int toplam = 0;

            for (int i = 0; i < dizi.Length; i++)
                if (dizi[i] == karakter)
                    toplam++;

            return toplam;
        }
        // dizi de istenilen karakter var mı sonucu true veya false döner
        private static bool mevcut(char[] dizi, char karakter)
        {
            bool var = false;

            for (int i = 0; i < dizi.Length; i++)
            {
                if (dizi[i] == karakter)
                {
                    var = true;
                    break;
                }
            }

            return var;
        }
        // kutuya ekleme fonksiyonu
        private static void kutuyaEkle(ref char[] kutu, ref char[] block, char karakter, int boxId)
        {
            int puan = 0, karakterSayi, bIndex = 0;
            char[] tempBlock = blockOlustur(block, karakter, out karakterSayi); // geçici block oluşturuluyor
            char[] box = boxOlustur(kutu, boxId, out puan); // geçici box oluşturuluyor (sadece boşluk sayısını hesaplamak için)
            int bosluk = box.Length - puan; // kutudaki boşluğu tutan değişken

            // eğer kutumuz 1. kutu ise bIndex 0 olacak değilse bu kısım gerçekleşecek
            if (boxId == 2)
                bIndex = 20;
            if (boxId == 3)
                bIndex = 40;

            // kutuda yeteri kadar boşluk var ise gerçekleştir
            if (bosluk >= karakterSayi)
            {
                // eklenmek istenen karakterin yerlerini blockta boşlukla değiştir
                for (int i = 0; i < block.Length; i++)
                    if (block[i] == karakter)
                        block[i] = ' ';

                // kutudaki boş kısımlara karakterler ekleniyor
                for (int i = 0; i < box.Length; i++)
                {
                    if (kutu[bIndex] == ' ')
                    {
                        // geçici blocktaki karakterler kutu dizimize atanıyor
                        for (int j = 0; j < tempBlock.Length; j++)
                        {
                            kutu[bIndex] = tempBlock[j];
                            bIndex++;
                        }
                        break;
                    }
                    bIndex++;
                }
            }
        }

        // kutudan çıkarma fonksiyonu
        private static void kutudanCikar(ref char[] kutu, ref char[] block, int letterIndex, char karakter)
        {
            int iterasyon = 0; // döngüler için ilgili kutuya göre döngü sayısını belirleyecek değişken
            int karakterSayi = 0, karakterIndex = 0, karakterSonIndex;
            bool bulundu = false; // eğer çıkarılmak istenen karakter var ise işlem yapılacak

            // karakter ve başladığı index bulunuyor
            for (int i = 0; i < kutu.Length; i++)
            {
                if (kutu[i] == karakter)
                {
                    karakterIndex = i;
                    bulundu = true;
                    break;
                }
            }
            // bulunduysa gerçekleştir
            if (bulundu)
            {
                // eğer bulduğumuz index 20 den küçük ise yani 1. kutudaysa iterasyon 20 yap (ilk kutu için işlem yapılacak)
                if (karakterIndex < 20)
                    iterasyon = 20;
                else if (karakterIndex < 40) // 2. kutuysa
                    iterasyon = 40;
                else // 3. kutuysa
                    iterasyon = 60;

                karakterSayi = karakterSayisi(kutu, karakter);
                karakterSonIndex = karakterIndex + karakterSayi; // karakterden sonraki karakterleri okumak için

                char[] tempKutu = new char[20]; // karakterden önceki ve sonrakiler tutacak dizi
                int tempIndex = 0; // tempKutu için indexi tutan değişken

                // çıkarılmak istenen karaktere kadar karakterleri tempKutu ya ata
                for (int i = iterasyon - 20; i < karakterIndex; i++)
                    tempKutu[tempIndex++] = kutu[i];

                // karakterden sonraki karakterleri tempKutu ya ata
                for (int i = karakterSonIndex; i < iterasyon; i++)
                    tempKutu[tempIndex++] = kutu[i];

                // kalan boşlukları doldur
                for (int i = tempIndex; i < 20; i++)
                    tempKutu[i] = ' ';

                tempIndex = 0; // tempIndex sıfırla
                // max iterasyonun 20 eksiğinden kendisine kadar döngü
                // iterasyon 20 ise 0'dan 20ye kadar(yani 1. kutu için), 40 ise 20'den 40a kadar(2. kutu için) 60 ise 40'tan 60a 
                // kadar(3. kutu için) tempKutu dizi karakterterleri kutu dizimize atanıyor
                for (int i = iterasyon - 20; i < iterasyon; i++)
                    kutu[i] = tempKutu[tempIndex++];

                // çıkarılan karakterin box0daki indexinden karakter sayısı kadar block dizimize atanıyor
                for (int i = letterIndex; i < letterIndex + karakterSayi; i++)
                    block[i] = karakter;

            }
        }
        // box0 a eklenen karakterler için block oluşturur
        private static char[] blockOlustur(char[] dizi, char karakter, out int karakterSayi)
        {
            char[] block; // karakteri tutacak dizi
            int bIndex = 0; // block için index tutar

            karakterSayi = karakterSayisi(dizi, karakter); // karakter sayisi
            block = new char[karakterSayi]; // karakter sayısı uzunluğunda diziyi oluştur
            // kendisine gönderilen diziden istenilen karakteri block dizisine at ve bIndex i arttır
            for (int i = 0; i < dizi.Length; i++)
                if (dizi[i] == karakter)
                    block[bIndex++] = dizi[i];

            return block; // oluşan diziyi dön
        }
        // kutuları oluşturan fonksiyon
        private static char[] boxOlustur(char[] dizi, int boxId, out int puan)
        {
            char[] box = new char[20]; // kutu için dizi değişkeni
            int bIndex = 0; // kutu için index, box Id 1 ise 1. kutudur ve başlama index i 0dır
            //  2 ise 20, 3 ise 40tır.
            if (boxId == 2)
                bIndex = 20;
            if (boxId == 3)
                bIndex = 40;

            puan = 0;
            // ilgili kutu içinde kaç tane ilgili karakter var ise puanı o kadar arttır
            for (int i = 0; i < box.Length; i++)
            {
                box[i] = dizi[bIndex++];
                if (box[i] != ' ')
                    puan++;
            }

            // puan değişkeni out olarak gönderildiğinden burda yapılan değişiklikler gönderildiği yerde de geçerli olacaktır

            return box; // oluşturulan kutuyu dön
        }
    }
}
