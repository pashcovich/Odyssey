#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using Odyssey.UserInterface.Controls;

#endregion

namespace Odyssey.UserInterface.Style
{
    public class ControlDescription
    {
        public const int DefaultBorderSize = 1;
        internal const string Error = "Error";
        internal const string Empty = "Empty";

        #region Properties

        public string Name { get; internal set; }
        public string TextStyleClass { get; internal set; }
        public BorderStyle BorderStyle { get; internal set; }
        public Size Size { get; internal set; }
        public Thickness BorderSize { get; internal set; }
        public Thickness Padding { get; internal set; }
        public IGradient Enabled { get; internal set; }
        public IGradient Highlighted { get; internal set; }
        public IGradient BorderEnabled { get; internal set; }


        public IGradient GetFillGradient(ControlStatus status)
        {
            switch (status)
            {
                default:
                case ControlStatus.Enabled:
                    return Enabled;

                case ControlStatus.Highlighted:
                    return Highlighted;
            }
        }

        public IGradient GetStrokeGradient(ControlStatus status)
        {
            switch (status)
            {
                default:
                case ControlStatus.Enabled:
                    return BorderEnabled;

            }
        }




        public static ControlDescription EmptyDescription
        {
            get
            {
                return new ControlDescription
                           {
                               BorderSize = Thickness.Empty,
                               BorderStyle = BorderStyle.None,
                               Enabled = null,
                               BorderEnabled = null,
                               Highlighted = null,
                               Name = Empty,
                               Padding = Thickness.Empty,
                               Size = Size.Empty,
                               TextStyleClass = "Default"
                           };
            }
        }

        #endregion


    }
}