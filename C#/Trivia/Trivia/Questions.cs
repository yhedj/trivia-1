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

        public void AddLast(string s)
        {
        }

        public void AskQuestion()
        {
            Console.WriteLine(popQuestions.First());
            popQuestions.RemoveFirst();

            Console.WriteLine(scienceQuestions.First());
            scienceQuestions.RemoveFirst();

            Console.WriteLine(sportsQuestions.First());
            sportsQuestions.RemoveFirst();

            Console.WriteLine(rockQuestions.First());
            rockQuestions.RemoveFirst();

        }
    }
}
