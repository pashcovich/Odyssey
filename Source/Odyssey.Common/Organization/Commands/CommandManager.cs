using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SharpDX.Mathematics;

namespace Odyssey.Organization.Commands
{
    public class CommandManager : Component, IEnumerable<Command>
    {
        private readonly LinkedList<Command> commands;

        public CommandManager(IEnumerable<Command> commands)
        {
            this.commands = new LinkedList<Command>(commands);
        }

        public CommandManager()
        {
            this.commands = new LinkedList<Command>();
        }

        public int Count
        {
            get { return commands.Count; }
        }

        public LinkedListNode<Command> First
        {
            get { return commands.First; }
        }

        public bool IsEmpty
        {
            get
            {
                return commands.Count == 0;
            }
        }

        public bool IsInited { get; protected set; }

        public Command this[int index]
        {
            get { return commands.ElementAt(index); }
        }

        public void AddBefore(Command command, Command newCommand)
        {
            Contract.Requires<ArgumentNullException>(command != null);
            Contract.Requires<ArgumentNullException>(newCommand != null);
            commands.AddBefore(commands.Find(command), ToDispose(newCommand)); 
        }

        public void AddFirst(Command command)
        {
            Contract.Requires<ArgumentNullException>(command != null);
            commands.AddFirst(ToDispose(command));
        }

        public void AddLast(Command command)
        {
            Contract.Requires<ArgumentNullException>(command != null);
            commands.AddLast(ToDispose(command));
        }

        public void AddLast(IEnumerable<Command> collection)
        {
            foreach (Command command in collection)
                AddLast(command);
        }

        public void Clear()
        {
            Unload();
            commands.Clear();
        }

        public IEnumerator<Command> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Initialize()
        {
            foreach (Command command in commands.Where(c => !c.IsInited))
                command.Initialize();

            IsInited = commands.All(c=> c.IsInited);
        }

        public void Run()
        {
            foreach (Command command in commands)
                command.Execute();
        }

        public void RunOnce(Command command)
        {
            if (!command.IsInited)
                command.Initialize();
            command.Execute();
        }

        public void RunOnce(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
                RunOnce(command);
        }

        public void Unload()
        {
            foreach (var command in commands)
                command.Dispose();
            IsInited = false;
        }
    }
}