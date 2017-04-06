using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    class Questions
    {
        LinkedList<String> popQuestions = new LinkedList<string>();
        LinkedList<String> scienceQuestions = new LinkedList<string>();
        LinkedList<String> sportsQuestions = new LinkedList<string>();
        LinkedList<String> rockQuestions = new LinkedList<string>();

        private readonly Dictionary<int, string> _categories = new Dictionary<int, string>() { { 0, "Pop" }, { 1, "Science" }, { 2, "Sports" }, { 3, "Rock" } };
        private object _players;

        public void AddLast(string s)
        {
        }

        private void AskQuestion(int CurrentPlayer)
        {
            if (CurrentCategory(CurrentPlayer) == "Pop")
            {
                popQuestions.AskQuestion();
            }
            if (CurrentCategory(CurrentPlayer) == "Science")
            {
                scienceQuestions.AskQuestion();
            }
            if (CurrentCategory(CurrentPlayer) == "Sports")
            {
                sportsQuestions.AskQuestion();
            }
            if (CurrentCategory(CurrentPlayer) == "Rock")
            {
                rockQuestions.AskQuestion();
            }
        }
        private string CurrentCategory(int currentPlayer)
        {
            return _categories[_players.Current.Place % 4];
        }

    }
}
