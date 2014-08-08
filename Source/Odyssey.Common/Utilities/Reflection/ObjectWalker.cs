using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Odyssey.Utilities.Reflection
{

    public class ObjectWalker
    {
        delegate object ReadMethod();
        private readonly object root;
        private TypeInfo currentTypeInfo;

        private PropertyInfo currentProperty;
        private FieldInfo currentField;
        private int index;
        private ReadMethod readMethod;

        private object currentObject;


        public string Name
        {
            get { return CurrentType.Name; }
        }

        public Type CurrentType { get { return CurrentMember.GetType(); }}

        public Type ContainingType { get { return CurrentType.DeclaringType; } }

        public MemberInfo CurrentMember { get; private set; }

        public ObjectWalker(object obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null, "obj");
            this.root = obj;
            currentTypeInfo = root.GetType().GetTypeInfo();
            currentObject = root;

        }

        void Reset()
        {
            CurrentMember = null;
        }

        bool ShouldAdvance(MemberInfo member)
        {
            if (member != null)
            {
                CurrentMember = member;
                return true;
            }
            else return false;
        }

        public void FollowPath(string propertyPath)
        {
            foreach (string subPath in propertyPath.Split('.'))
            {
                string expression = subPath;
                string arrayName;

                if (IsArray(subPath, out arrayName, out index))
                    expression = arrayName;

                if (!AdvanceTo(expression))
                    throw new InvalidOperationException(string.Format("Type '{0}' does not contain a property or field '{1}'", Name, expression));

                currentObject = Read();
            }
        }


        bool AdvanceTo(string memberName)
        {
            return AdvanceToProperty(memberName) || AdvanceToField(memberName);
        }

        bool AdvanceToProperty(string propertyName)
        {
            var member = currentTypeInfo.GetDeclaredProperty(propertyName);
            bool result = ShouldAdvance(member);
            if (result)
            {
                currentProperty = member;
                currentField = null;
                readMethod = ReadPropertyValue;
            }
            return result;
        }

        bool AdvanceToField(string fieldName)
        {
            var member = currentTypeInfo.GetDeclaredField(fieldName);
            bool result = ShouldAdvance(member);
            if (result)
            {
                currentField = member;
                currentProperty = null;
                readMethod = ReadFieldValue;
            }
            return result;
        }

        object ReadPropertyValue()
        {
            return index >= 0 ? currentProperty.GetValue(currentObject, new object[] {index}) : currentProperty.GetValue(currentObject);
        }

        object ReadFieldValue()
        {
            var value = currentField.GetValue(currentObject);

            if (index >= 0)
            {
                Array array = (Array) value;
                value = array.GetValue(new[] {index});
            }
            return value;
        }

        object Read()
        {
            return readMethod();
        }

        public TValue ReadValue<TValue>()
        {
            return (TValue) Read();
        }

        static bool IsArray(string expression, out string arrayName, out int index)
        {
            const string rArrayPattern = @"(?<array>\w*)\[(?<index>\d+)\]";
            Regex rArray = new Regex(rArrayPattern);

            Match match = rArray.Match(expression);
            if (match.Success)
            {
                arrayName = match.Groups["array"].Value;
                index = int.Parse(match.Groups["index"].Value);
                return true;
            }
            else
            {
                arrayName = string.Empty;
                index = -1;
                return false;
            }
        }
  
    }
}
