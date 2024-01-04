﻿using System;

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
            var commandExecuter = new CommandExecuter(pathFinder.ElementsFound);

            switch (_name)
            {
                case "PRINT":
                    commandExecuter.Print();
                    break;
                case "SET":
                    commandExecuter.Set(_content);
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

            var length = 0;
            var counter = 0;

            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] == '"')
                {
                    for (int j = i + 1; j < command.Length; j++)
                    {
                        if (command[j] == '"')
                        {
                            counter++;

                            if (counter == 1)
                            {
                                _path = command.Substring(i + 1, length);
                            }
                            else
                            {
                                _content = command.Substring(i + 1, length);
                            }

                            length = 0;
                            i = j;
                            break;
                        }
                        length++;
                    }
                }
                else if (counter == 0 && command[i] != ' ')
                {
                    _name += command[i];
                }
            }
        }
    }
}
