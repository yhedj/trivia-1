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
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (CurrentCategory(CurrentPlayer) == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (CurrentCategory(CurrentPlayer) == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (CurrentCategory(CurrentPlayer) == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }
        private string CurrentCategory(int currentPlayer)
        {
            return _categories[currentPlayer % 4];
        }

    }
}
