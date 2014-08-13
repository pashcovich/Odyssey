using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Odyssey.UserInterface.Controls;

namespace Odyssey.UserInterface.Xml
{
    public abstract class XmlContainerControl : XmlUIElement
    {
        List<XmlUIElement> xmlControlList;

        protected XmlContainerControl(Control control) : base(control)
        {
            xmlControlList = new List<XmlUIElement>();
        }

        [XmlArray("Controls")]
        public virtual List<XmlUIElement> XmlControlList
        {
            get {
                return xmlControlList.Count > 0 ? xmlControlList : null;
            }
            set { xmlControlList = value; }
        }

        //void CreateWrapperForControl<TWrapperClass>(BaseControl ctl)
        //    where TWrapperClass : XmlBaseControl, new()
        //{
        //    TWrapperClass wrapper = new TWrapperClass();
        //    wrapper.FromControl(ctl);
        //    xmlControlList.Add(wrapper);
        //}

        //internal override void FromControl(BaseControl control)
        //{
        //    base.FromControl(control);

        //    xmlControlList = new List<XmlBaseControl>();
        //    ContainerControl containerControl = (ContainerControl) control;

        //    foreach (BaseControl childControl in containerControl.Controls)
        //    {

        //        Type wrapperType = UIParser.GetWrapperTypeForControl(childControl.GetType());
        //        if (wrapperType == null) continue;

        //        MethodInfo mi =
        //            typeof (XmlContainerControl).GetMethod("CreateWrapperForControl",
        //                                                   BindingFlags.NonPublic | BindingFlags.InstanceFrame);
                
        //        mi = mi.MakeGenericMethod();
        //        mi.Invoke(this, new object[]
        //                      {
        //                          childControl
        //                      });
        //    }
        //}

        public virtual void WriteContainerCSCode(StringBuilder sb)
        {
            foreach (XmlUIElement xmlBaseControl in XmlControlList)
            {
                XmlContainerControl xmlContainerControl = xmlBaseControl as XmlContainerControl;
                if ( xmlContainerControl!= null)
                    xmlContainerControl.WriteContainerCSCode(sb);
                    
                sb.AppendFormat("{0}.Add({1});\n", this.Id, xmlBaseControl.Id);

            }
        }
    }

    
    [XmlType("Panel")]
    public class XmlPanel : XmlContainerControl
    {
        public XmlPanel(Control control)
            : base(control)
        {

        }

        
    }

    //[XmlType(TypeName = "GroupBox")]
    //public class XmlGroupBox : XmlContainerControl
    //{
    //    string caption;

    //    public XmlGroupBox() : base()
    //    {
    //        caption = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Caption
    //    {
    //        get { return caption; }
    //        set { caption = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        GroupBox groupBox = control as GroupBox;
    //        caption = groupBox.Caption;
    //    }
    //}

    //public class XmlTabPanel : XmlContainerControl
    //{
    //    string textStyleClass;
    //}

    //[XmlType("Window")]
    //public class XmlWindow : XmlContainerControl
    //{
    //    string title;

    //    public XmlWindow() : base()
    //    {
    //        title = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Title
    //    {
    //        get { return title; }
    //        set { title = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        Window window = control as Window;
    //        title = window.Title;
    //    }
    //}

    
    [XmlType("Overlay")]
    public class XmlOverlay : XmlContainerControl
    {
        public XmlOverlay(Overlay Overlay) : base(Overlay)
        {
        }

        public virtual void WriteCSharpCode(StringBuilder sb)
        {
            // TODO Implement export UI to C#
        }


    }
}