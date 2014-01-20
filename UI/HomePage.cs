using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AllInOne
{
    class HomePage
    {
        HtmlDocument doc;

        public HomePage(HtmlDocument document)
        {
            this.doc = document;
        }

        public HtmlElement LeftPanel { get { return doc.GetElementById("leftPanel"); } }

        public HtmlElement ButtonRun { get { return doc.GetElementById("btnRun"); } }

        public HtmlElement LableSample { get { return doc.GetElementById("lbSample"); } }

        public HtmlElement TextContent { get { return doc.GetElementById("txtContent"); } }

        public HtmlElement TextOutput { get { return doc.Window.Frames["txtOutput"].Document.Body; } }
    }
}
