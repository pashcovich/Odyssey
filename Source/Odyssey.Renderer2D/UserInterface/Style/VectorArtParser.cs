#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace Odyssey.UserInterface.Style
{
    public static class VectorArtParser
    {
        /// <summary>
        ///     Parses a string representing a XAML Path object in the Abbreviated Geometry Syntax
        /// </summary>
        /// <param name="pathString"></param>
        public static IEnumerable<VectorCommand> ParsePathData(string pathString)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(pathString), "pathString");

            const string separators = @"(?=[FMLHVCQSTAZmlhvcqstaz])";
            var tokens = Regex.Split(pathString, separators).Where(t => !string.IsNullOrEmpty(t));

            var result = tokens.Select(Parse);

            return result;
        }

        private static VectorCommand Parse(string pathString)
        {
            var cmd = pathString.Cast<char>().Take(1).Single();
            string remainingargs = pathString.Substring(1);
            const string separators = @"[\s,]|(?=(?<!e)-)";

            var splitArgs = Regex
                .Split(remainingargs, separators)
                .Where(t => !string.IsNullOrEmpty(t));

            float[] floatArgs = splitArgs.Select(float.Parse).ToArray();
            bool relative;
            var primitiveType = Convert(cmd, out relative);
            return new VectorCommand(primitiveType, floatArgs, relative);
        }

        private static CommandType Convert(char cmd, out bool relative)
        {
            relative = char.IsLower(cmd);
            char invCmd = char.ToLower(cmd);

            switch (invCmd)
            {
                case 'f':
                    return CommandType.FillRule;
                case 'l':
                    return CommandType.Line;
                case 'h':
                    return CommandType.HorizontalLine;
                case 'a':
                    return CommandType.EllipticalArc;
                case 'm':
                    return CommandType.Move;
                case 'v':
                    return CommandType.VerticalLine;
                case 'c':
                    return CommandType.CubicBezierCurve;
                case 'q':
                    return CommandType.QuadraticBezierCurve;
                case 's':
                    return CommandType.SmoothCubicBezierCurve;
                case 't':
                    return CommandType.SmoothQuadraticBezierCurve;
                case 'z':
                    return CommandType.Close;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Command '{0}' is not valid", cmd));
            }
        }

        internal static char ConvertBack(CommandType command, bool relative)
        {
            char cmd = '?';
            switch (command)
            {
                case CommandType.FillRule:
                    cmd = 'f';
                    break;
                case CommandType.Move:
                    cmd = 'm';
                    break;
                case CommandType.Line:
                    cmd = 'l';
                    break;
                case CommandType.HorizontalLine:
                    cmd = 'h';
                    break;
                case CommandType.VerticalLine:
                    cmd = 'v';
                    break;
                case CommandType.CubicBezierCurve:
                    cmd = 'c';
                    break;
                case CommandType.QuadraticBezierCurve:
                    cmd = 'q';
                    break;
                case CommandType.SmoothCubicBezierCurve:
                    cmd = 's';
                    break;
                case CommandType.SmoothQuadraticBezierCurve:
                    cmd = 't';
                    break;
                case CommandType.EllipticalArc:
                    cmd = 'a';
                    break;
                case CommandType.Close:
                    cmd = 'z';
                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format("Command '{0}' is not valid", cmd));
            }

            return relative ? cmd : char.ToUpper(cmd);
        }
    }
}