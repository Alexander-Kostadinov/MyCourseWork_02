using System;

namespace MyCourseWork_02
{
    public class Command
    {
        private string _name;
        private string _path;
        private string _content;
        private HtmlElement _element;

        public Command(string command, HtmlElement element)
        {
            _element = element;
            GetCommandElements(command);
        }

        public void Execute()
        {
            var pathFinder = new PathFinder(_path, _element);
            var commandExecuter = new CommandExecuter(pathFinder.Elements);

            switch (_name)
            {
                case "PRINT":
                    commandExecuter.Print();
                    break;
                case "SET":
                    break;
                case "COPY":
                    break;
                case "SAVE":
                    break;
                default:
                    break;
            }
        }

        private void GetCommandElements(string command)
        {
            if (command == null) return;

            var counter = 0;
            var index = 0;
            var commandElements = new string[3];

            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] == ' ' || i == command.Length - 1)
                {
                    if (i == command.Length - 1) i++;

                    commandElements[counter] = command.Substring(index, i - index);
                    index = i + 1;
                    counter++;
                }
            }

            _name = commandElements[0];
            _path = commandElements[1];
            _content = commandElements[2];
        }
    }
}
