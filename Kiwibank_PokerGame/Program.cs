using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kiwibank_PokerGame
{
    public class Deck
    {
        public string card;        
    }

    public class Player
    {
        public int id;

        public List<string> hand;

        public int score;

        public int handRank;

        public int highCardRank;

    }

    class Program
    {
        static string[] cards = { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
        static string[] suits = { "S", "C", "H", "D" };
        static List<Deck> deck = new List<Deck>();

        enum Rank { StraightFlush, Flush, Straight, Pair, HighCard };

        static void Main(string[] args)
        {
            
            int player = AskPlayers();
            int rounds = AskRounds();
            int currRound = 0;

            List<Player> players = new List<Player>();

            for(int i = 0; i < player; i++)
            {
                players.Add(new Player() { id = i, hand = new List<string>(), score = 0, handRank = 0, highCardRank = 0 });
            }            

            do
            {
                List<Deck> cards = PrepareDeck();
                List<Deck> shuffled = AskShuffle(cards);            

                int drawCount = 2;

                for (int x = 0; x < drawCount; x++)
                {
                    for (int y = 0; y < player; y++)
                    {
                        Player currPlayer = players.Where(p => p.id == y).FirstOrDefault();

                        currPlayer.hand.Add(Deal(shuffled));
                    }
                }

                for (int x = 0; x < player; x++)
                {
                    Player currPlayer = players.Where(p => p.id == x).FirstOrDefault();

                    currPlayer.handRank = AssessRank(currPlayer.hand);
                    currPlayer.highCardRank = AssessHighCardRank(currPlayer.hand);
                }

                int mark = player;

                foreach (Player p in players.OrderBy(x => x.handRank).ThenBy(x => x.highCardRank))
                {
                    if (p.handRank != (int)Rank.HighCard)
                    {
                        p.score += mark - 1;
                    } 
                    else
                    {
                        p.score += mark - 1;
                    }

                    mark--;
                }              

                Console.WriteLine("Round " + (currRound + 1) + ":\nShow Hands\n");

                foreach (Player p in players)
                {
                    Console.WriteLine("Player " + (p.id + 1) + "\n");

                    foreach(string card in p.hand)
                    {
                        Console.WriteLine(card + " ");
                    }

                    Console.WriteLine(GetEnumDescription((Rank)p.handRank) + "\n");
                }

                Console.WriteLine("Results for Round " + (currRound + 1) + ":\n");

                foreach(Player p in players)
                {
                    Console.WriteLine("Player " + (p.id + 1) + "- Score: " + p.score + "\n");
                }

                //clean hands
                foreach (Player p in players)
                {
                    p.hand = new List<string>();
                    p.handRank = 0;
                    p.highCardRank = 0;
                }

                currRound++;

            } while (currRound < rounds);        

            Console.WriteLine("Press Any Key to Exit");
            Console.ReadKey();
        }

        public static List<Deck> PrepareDeck()
        {
            //string[] cards = { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
            //string[] suits = { "S", "C", "H", "D" };

            //List<Deck> deck = new List<Deck>();

            int count = 0;

            deck = new List<Deck>();

            while (count < suits.Length)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    deck.Add(new Deck() { card = suits[count] + cards[i] });
                }

                count++;
            }

            return deck;
        }

        public static List<Deck> ShuffleDeck(List<Deck> list)
        {
            Random rnd = new Random();

            return list.OrderBy(i => rnd.Next()).ToList();
        }

        public static int AskPlayers()
        {
            string input = String.Empty;
            int players;

            do
            {
                Console.Write("How many players in this session? (min 2, max 6)");
                input = Console.ReadLine();

                if (Int32.TryParse(input, out players) == true)
                {
                    if (players >= 2 && players <= 6)
                    {
                        //Console.Write(players);
                        break;
                    }
                    else if (players < 2)
                    {
                        Console.Write("Insuffient players\n");

                    }
                    else if (players > 6)
                    {
                        Console.Write("Too many players\n");
                    }
                }
                else
                {
                    Console.Write("Invalid Input\n");
                }

            } while (true);

            return players;
        }

        public static int AskRounds()
        {
            string input = String.Empty;
            int rounds;

            do
            {
                Console.Write("How many rounds? (min 2, max 5)");
                input = Console.ReadLine();

                if (Int32.TryParse(input, out rounds) == true)
                {
                    if (rounds >= 2 && rounds <= 5)
                    {
                        //Console.Write(rounds);
                        break;
                    }
                    else if (rounds < 2)
                    {
                        Console.Write("Insuffient rounds\n");

                    }
                    else if (rounds > 5)
                    {
                        Console.Write("Too many rounds\n");
                    }
                }
                else
                {
                    Console.Write("Invalid Input\n");
                }

            } while (true);

            return rounds;
        }

        public static List<Deck> AskShuffle(List<Deck> cards)
        {
            string input = String.Empty;
            List<Deck> shuffled = ShuffleDeck(cards);

            do
            {
                Console.Write("Deck shuffled.  Do you want to shuffle some more (Y/N)?");
                input = Console.ReadLine();

                if (input.ToUpper() == "Y" || input.ToUpper() == "N")
                { 
                    if (input.ToUpper() == "Y")
                    {
                        shuffled = ShuffleDeck(shuffled);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.Write("Invalid Input\n");
                }

            } while (true);

            return shuffled;
        }

        public static string Deal(List<Deck> cards)
        {
            string firstCard = cards[0].card;
            cards.RemoveAt(0);

            return firstCard;
        }

        public static int AssessRank(List<string> hand)
        {
            string firstSuit = hand[0].Substring(0, 1);
            string secondSuit = hand[1].Substring(0, 1);

            string firstCard = hand[0].Substring(1, 1);
            string secondCard = hand[1].Substring(1, 1);

            int firstCardPos = Array.IndexOf(cards, firstCard);
            int secondCardPos = Array.IndexOf(cards, secondCard);

            if (firstSuit == secondSuit)
            {
                if (firstCardPos + 1 == secondCardPos || firstCardPos - 1 == secondCardPos )
                {
                    return (int)Rank.StraightFlush;
                }
                else
                {
                    return (int)Rank.Flush;
                }
            }
            else
            {
                if (firstCardPos + 1 == secondCardPos || firstCardPos - 1 == secondCardPos)
                {
                    return (int)Rank.Straight;
                }
                else if (firstCardPos == secondCardPos)
                {
                    return (int)Rank.Pair;
                }
                else
                {
                    return (int)Rank.HighCard;
                }
            }

        }

        public static int AssessHighCardRank(List<string> hand)
        {
            int firstCardDeckPos = deck.FindIndex(x => x.card == hand[0]);
            int secondCardDeckPos = deck.FindIndex(x => x.card == hand[1]);

            if (firstCardDeckPos < secondCardDeckPos)
            {
                return firstCardDeckPos;
            }
            else
            {
                return secondCardDeckPos;
            }

        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
