using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaiveBayes
{
    public class NaiveBayes
    {
        Dictionary<int, double> documents_sentiment_count = new Dictionary<int, double>();
        public static Dictionary<string, int> vocabulary = new Dictionary<string, int>();
        public static Dictionary<int, Dictionary<string, int>> word_counts = new Dictionary<int, Dictionary<string, int>>();

        public NaiveBayes()
        {
            documents_sentiment_count[0] = 0.0; // npr. Pozitivno osecanje
            documents_sentiment_count[1] = 0.0; // npr. Negativno osecanje
            documents_sentiment_count[2] = 0.0; // npr. Neutralno osecanje
            word_counts[0] = new Dictionary<string, int>();
            word_counts[1] = new Dictionary<string, int>();
            word_counts[2] = new Dictionary<string, int>();
        }

        /// <summary>
        /// Formiranje globalnog recnika, recnika iz pozitivnih i negativnih recenzija 
        /// </summary>
        /// <param name="model">train model ucitan iz tsv datoteke</param>
        public void fit(DataModel model) // U DataModelu je bas lepo isparsirano
        {
            for (int i = 0; i < model.Text.Count; i++) // Za sve recenice
            {
                string text = model.Text[i]; // Uzmi tu recenicu
                int sentiment = model.Sentiment[i]; // i njeno osecanje

                documents_sentiment_count[sentiment] += 1; // povecaj broj recenica sa tim osecanjem
                string[] words = TextUtil.Tokenize(text); // rastavi recenicu na reci
                Dictionary<string, int> counts = TextUtil.CountWords(words); // kreira mapu <rec, broj_ponavljanja_te_reci>

                foreach (KeyValuePair<string, int> item in counts) // za svaki par <rec, broj_ponavljanja_te_reci>
                {
                    string word = item.Key; // Ovo je rec
                    int count = item.Value; // Ovo je broj ponavljanja

                    //TODO 5 - Popuniti globalni recnik svih reci, kao i recnike za odredjene sentimente
                    if (vocabulary.ContainsKey(word))
                    {
                        vocabulary[word] += count; // Povecaj
                    } else
                    {
                        vocabulary[word] = count; // Inicijalizuj
                    }

                    // Recnik za sentiment
                    Dictionary<string, int> sentiment_count = word_counts[sentiment];
                    if (sentiment_count.ContainsKey(word))
                    {
                        sentiment_count[word] += count; // Povecaj
                    } else
                    {
                        sentiment_count[word] = count; // Inicijalizuj
                    }
                }
            }
        }
        /// <summary>
        /// Racunanje verovatnoca za prosledjeni tekst
        /// </summary>
        /// <param name="text">Tekst koji se klasifikuje</param>
        public void predict(string text)
        {
            string[] words = TextUtil.Tokenize(text); // Uneta recenica rastavljena na reci
            Console.WriteLine("\t\tDEBUG1: " + words.Count());

            var counts = TextUtil.CountWords(words); //  kreira mapu <rec, broj_ponavljanja_te_reci> za unetu recenicu
            Console.WriteLine("\t\tDEBUG2: " + counts.Count());

            double documentCount = documents_sentiment_count.Values.Sum(); // Broj recenica u train setu
            //TODO 6 - Izracunati verovatnoce da je dokument za predikciju bas pozitivnog ili negativnog sentimenta - P(cj)
            double Pcj_neg = documents_sentiment_count[0] / documentCount;
            double Pcj_pos = documents_sentiment_count[1] / documentCount;
            double Pcj_neutral = documents_sentiment_count[2] / documentCount;

            Console.WriteLine("Pcj_neg: " + Pcj_neg + " Pcj_pos: " + Pcj_pos + " Pcj_neutral: " + Pcj_neutral);

            double log_prob_neg = 0.0;
            double log_prob_pos = 0.0;
            double log_prob_neutral = 0.0;

            double sum_count_neg = word_counts[0].Values.Sum();
            double sum_count_pos = word_counts[1].Values.Sum();
            double sum_count_neutral = word_counts[2].Values.Sum();

            double V = vocabulary.Count; // Ukupan broj reci

            foreach (KeyValuePair<string, int> item in counts)
            {
                string w = item.Key;
                int cnt = item.Value;
                //TODO 7.1 - Iterativno racunati logaritamski zbir verovatnoca sentimenta svake reci
                double count_neg = word_counts[0].ContainsKey(w) ? word_counts[0][w] : 0;
                double count_pos = word_counts[1].ContainsKey(w) ? word_counts[1][w] : 0;
                double count_neutral = word_counts[2].ContainsKey(w) ? word_counts[2][w] : 0;

                log_prob_neg += Math.Log(count_neg + 1) - Math.Log(sum_count_neg + V);
                log_prob_pos += Math.Log(count_pos + 1) - Math.Log(sum_count_pos + V);
                log_prob_neutral += Math.Log(count_neutral + 1) - Math.Log(sum_count_neg + V);
            }

            //TODO 7.2 Izracunati konacnu vrednost verovatnoce sentimenta prosledjenog teksta
            log_prob_neg += Math.Log(Pcj_neg);
            log_prob_pos += Math.Log(Pcj_pos);
            log_prob_neutral += Math.Log(Pcj_neutral);
            //TODO 8 - Ispisati vrednosti predikcije za pozitivan i negativan sentiment teksta
            Console.WriteLine("positive: {0}\nnegative: {1}\nsolution: {2}", log_prob_pos, log_prob_neg, log_prob_pos > log_prob_neg ? "POSITIVE" : "NEGATIVE");
        }
    }
}
