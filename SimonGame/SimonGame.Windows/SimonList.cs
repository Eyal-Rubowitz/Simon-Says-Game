using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonGame
{
    public class SimonList : IEnumerable
    {
        public SimonLink _head;
        public SimonLink currLink;
        public int _linksCounter;
        int _simonSaysColor;

        Random gameStatus = new Random();

        public SimonList()
        {
            _head = null;
            currLink = null;
            _linksCounter = 0;
        }

        public void AddGameStep()
        {
            if (_head != null)
            {
                SimonLink tempLink = _head;
                while (tempLink._next != null)
                {
                    tempLink = tempLink._next;
                }
                int nextGameStep = SimonSays();
                SimonLink newLink = new SimonLink(nextGameStep, null);
                tempLink._next = newLink;
                _linksCounter++;
            }
            else
            {
                int status = SimonSays();
                SimonLink firstLink = new SimonLink(gameStatus: status, next: null);
                _head = firstLink;
                currLink = _head;
                _linksCounter++;
            }
        }


        public int SimonSays()
        {
            _simonSaysColor =  gameStatus.Next(1, 5);
            return _simonSaysColor;
        }

        public void EndOfGame()
        {
            if (_head != null)
            {
                _head = null;
                currLink = null;
                _linksCounter = 0;
                _simonSaysColor = 0;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_head != null)
            {
                SimonLink tempLink = _head;
                int tempData;
                while (tempLink != null)
                {
                    tempData = Convert.ToInt32(tempLink._data);

                    yield return tempData;
                    Task.Delay(1200);
                    tempLink = tempLink._next;
                }
            }
        }
    }
}
