using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Simple
{

    public class Bridge
    {
        private static void RunBridgeGame()
        {
            var set = new BridgeSet();

            var game = new BridgeGame(PlayerPosition.East);

            var players = Enumerable.Range(0, 4).Select(n =>
            {
                var position = (PlayerPosition)n;
                return new Player(position, set.Cards[position]);
            }).ToArray();

            var table = new ConsoleBridgeTable();

            table.ShowTable(set);
            Console.WriteLine();
            Console.WriteLine("Game Started:");
            Console.WriteLine(string.Empty.PadLeft(Console.BufferWidth, '*'));
            while (!game.Finished)
            {
                Console.Write(game.TurnStarter.ToString()[0] + ": ");
                for (int i = 0; i < 4; i++)
                {
                    var player = players.First(p => p.Position == game.Next);
                    var card = player.Play(game);
                    game.RecordOutCard(card);

                    table.ShowCard(card);
                }
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
    #region Bridge

    interface IBridgeTable
    {
        void ShowTable(BridgeSet set);

        void ShowCard(Card c);
    }

    class ConsoleBridgeTable : IBridgeTable
    {
        const int SpaceMiddle = 18;
        const int SpaceRight = 36;
        ConsoleColor defaultColor;
        int currentSpace;

        public ConsoleBridgeTable()
        {
            defaultColor = Console.ForegroundColor;
            Console.OutputEncoding = Encoding.GetEncoding(866);
        }

        public void ShowTable(BridgeSet set)
        {
            currentSpace = SpaceMiddle;
            for (int i = 0; i < 4; i++)
            {
                WriteCardsInPlayerSuit(set, PlayerPosition.North, i);
                Console.WriteLine();
            }
            WriteLine(2);

            for (int i = 0; i < 4; i++)
            {
                currentSpace = 0;
                WriteCardsInPlayerSuit(set, PlayerPosition.West, i);
                currentSpace = SpaceRight - Console.CursorLeft;
                WriteCardsInPlayerSuit(set, PlayerPosition.East, i);
                Console.WriteLine();
            }
            WriteLine(1);

            currentSpace = SpaceMiddle;
            for (int i = 0; i < 4; i++)
            {
                WriteCardsInPlayerSuit(set, PlayerPosition.South, i);
                Console.WriteLine();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            }

            int squareLeft = SpaceMiddle, squareTop = 5, squareHeight = 4, squareWidth = 9;
            Console.SetCursorPosition(squareLeft, squareTop);
            Console.Write('┌' + string.Empty.PadLeft(squareWidth, '─') + '┐');

            Console.SetCursorPosition(squareLeft, squareTop + squareHeight);
            Console.Write('└' + string.Empty.PadLeft(squareWidth, '─') + '┘');

            for (int i = 0; i < squareHeight - 1; i++)
            {
                Console.SetCursorPosition(squareLeft, squareTop + 1 + i);
                Console.Write('│');
                Console.SetCursorPosition(squareLeft + squareWidth + 1, squareTop + 1 + i);
                Console.Write('│');
            }

            Console.SetCursorPosition(squareLeft + 1 + squareWidth / 2, squareTop + 1);
            Console.Write('N');
            Console.SetCursorPosition(squareLeft + 1 + squareWidth / 2, squareTop + squareHeight - 1);
            Console.Write('S');
            Console.SetCursorPosition(squareLeft + 1, squareTop + squareHeight/2);
            Console.Write('W');
            Console.SetCursorPosition(squareLeft + squareWidth, squareTop + squareHeight / 2);
            Console.Write('E');
            Console.SetCursorPosition(0, 15);
            currentSpace = 0;
        }

        private void WriteCardsInPlayerSuit(BridgeSet set,PlayerPosition position, int typeValue)
        {
            var type = (CardType)typeValue;
            Write(type);
            Console.Write(' ');
            var cardsInType = set.Cards[position].Where(c => c.Type == type).OrderBy(c => ~c.Value);

            if (cardsInType.Count() < 1) Console.Write("-");
            foreach (var card in cardsInType)
            {
                Console.Write(card.Name);
                Console.Write(' ');
            }
        }

        void Write(CardType type)
        {
            switch (type)
            {
                case CardType.Spade:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Write("♠");
                    break;
                case CardType.Heart:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Write("♥");
                    break;
                case CardType.Diamond:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Write("♦");
                    break;
                case CardType.Club:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Write("♣");
                    break;
            }
            Console.ForegroundColor = defaultColor;
        }

        void Write(string text)
        {
            Console.Write(string.Empty.PadLeft(currentSpace));
            Console.Write(text);
        }

        void WriteLine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine();
            }
        }


        public void ShowCard(Card c)
        {
            Write(c.Type);
            Console.Write(c.Name);
            Console.Write(' ');
        }
    }

    class Card
    {
        public int Value { get; set; }

        public CardType Type { get; set; }

        public Char Name
        {
            get
            {
                char name;
                switch (this.Value)
                {
                    case 10:
                        name = 'T'; break;
                    case 11:
                        name = 'J'; break;
                    case 12:   
                        name = 'Q'; break;
                    case 13:   
                        name = 'K'; break;
                    case 14:   
                        name = 'A'; break;
                    default:
                        name = Value.ToString()[0];
                        break;
                }
                return name;
            }
        }

        public override string ToString()
        {
            return Type.ToString()[0].ToString() + Name;
        }
    }

    class BridgeSet
    {
        public static List<Card> AllCards = new List<Card>(52);

        public const int Players = 4;

        static BridgeSet()
        {
            Func<CardType, IEnumerable<Card>> getCards = type => Enumerable.Range(2, 13).Select(n=> new Card { Value = n, Type= type });
            AllCards.AddRange(getCards(CardType.Club));
            AllCards.AddRange(getCards(CardType.Diamond));
            AllCards.AddRange(getCards(CardType.Heart));
            AllCards.AddRange(getCards(CardType.Spade));     
        }

        public BridgeSet()
        {
            var randomCards = new List<Card>(AllCards.Count);
            var dealerRecords = new bool[AllCards.Count];

            var rnd = new Random();
            for (int i = 0; i < AllCards.Count; i++)
            {
                var n = rnd.Next(AllCards.Count);
                while (dealerRecords[n])
	            {
                    n++;
                    if (n == AllCards.Count) n = 0;
	            }
                dealerRecords[n] = true;
                randomCards.Add(AllCards[n]);
            }
            
            int countPerHand = AllCards.Count/4;

            var cards = new Dictionary<PlayerPosition, IReadOnlyCollection<Card>>();
            cards[PlayerPosition.East] = randomCards.Take(countPerHand).ToList().AsReadOnly();
            cards[PlayerPosition.West] = randomCards.Skip(countPerHand).Take(countPerHand).ToList().AsReadOnly();
            cards[PlayerPosition.South] = randomCards.Skip(countPerHand * 2).Take(countPerHand).ToList().AsReadOnly();
            cards[PlayerPosition.North] = randomCards.Skip(countPerHand * 3).Take(countPerHand).ToList().AsReadOnly();
            this.Cards = cards;
        }

        public IReadOnlyDictionary<PlayerPosition, IReadOnlyCollection<Card>> Cards { get; private set; }

        public PlayerPosition DealerPosition { get; private set; }

        public Vulnerable Vulnerable { get; private set; } 

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            Action<IEnumerable<Card>, string> getCardsDisplay = (cards, title) =>
            {
                sb.Append(title);
                sb.Append(": ");
                sb.AppendLine();

                for (int i = 0; i < 4; i++)
                {
                    var type = (CardType)i;
                    sb.Append(type.ToString()[0]);
                    sb.Append(": ");
                    foreach (var card in cards.Where(c=>c.Type == type).OrderBy(c=>~c.Value))
                    {
                        sb.Append(card.Name);
                        sb.Append(' ');
                    }
                    sb.AppendLine();
                }
            };

            foreach (var item in this.Cards)
            {
                getCardsDisplay(item.Value, item.Key.ToString());
            }

            return sb.ToString();
        }
    }

    class BridgeGame
    {
        public BridgeGame(PlayerPosition starter)
        {
            TurnWinners = new List<PlayerPosition>(BridgeSet.AllCards.Count/BridgeSet.Players);
            OutCards = new List<Card>(52);
            this.SetStarter = starter;
        }

        public List<PlayerPosition> TurnWinners { get; private set; }

        public List<Card> OutCards { get; private set; }

        public PlayerPosition SetStarter { get; private set; }

        public PlayerPosition TurnStarter
        {
            get
            {
                if (TurnWinners.Count < 1) return SetStarter;
                return TurnWinners.Last();
            }
        }

        public Card FirstTurnCard
        {
            get
            {
                var playedCards = OutCards.Count % 4;
                if (playedCards > 0)
                {
                    return OutCards[OutCards.Count - playedCards];
                }
                return null;
            }
        }

        public PlayerPosition Next
        {
            get
            {
                return (PlayerPosition)(((int)this.TurnStarter + OutCards.Count % 4) % 4);
            }
        }

        public bool Finished { get { return OutCards.Count == BridgeSet.AllCards.Count; } }

        public void RecordOutCard(Card card)
        {
            OutCards.Add(card);

            if (OutCards.Count % BridgeSet.Players == 0)
            {
                int maxCard = OutCards.Count - BridgeSet.Players;
                for (int i = OutCards.Count - BridgeSet.Players + 1; i < OutCards.Count ; i++)
                {
                    var c = OutCards[i];

                    if (c.Type == OutCards[maxCard].Type && c.Value > OutCards[maxCard].Value)
                    {
                        maxCard = i;
                    }
                }

                var winner = (PlayerPosition)(((int)this.TurnStarter + maxCard % 4) % 4);
                this.TurnWinners.Add(winner);
            }
        }

    }


    class Player
    {
        Dictionary<CardType, List<Card>> groupedCards;
        List<Card> cards;
        public Player(PlayerPosition position, IEnumerable<Card> cards)
        {
            this.Position = position;
            this.groupedCards = cards.GroupBy(c => c.Type).ToDictionary(c => c.Key, c => c.ToList());
            this.cards = cards.ToList();
        }

        public PlayerPosition Position { get; private set; }

        public Card Play(BridgeGame game)
        {
            if (cards.Count < 1) throw new InvalidOperationException();

            var rnd = new Random();
            Card c;
            if (game.FirstTurnCard == null)
            {
                c = cards[rnd.Next(cards.Count)];
            }
            else
            {
                var type = game.FirstTurnCard.Type;
                if (!groupedCards.ContainsKey(type) || groupedCards[type].Count < 1)    //No cards of the type remaining
                {  
                    c = cards[rnd.Next(cards.Count)];
                }
                else
                {
                    c = groupedCards[type][rnd.Next(groupedCards[type].Count)];
                }
            }
            groupedCards[c.Type].Remove(c);
            cards.Remove(c);

            return c;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    enum PlayerPosition { East, South ,West ,North }
    enum CardType { Spade, Heart, Diamond, Club }

    [Flags]
    enum Vulnerable { EW, SN }
    #endregion

}
