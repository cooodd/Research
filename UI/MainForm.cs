/*
 * Created by SharpDevelop.
 * User: wxm
 * Date: 2014/1/18
 * Time: 0:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;

namespace AllInOne
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();						
		}
		
		void WbMainDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{				
			var leftPanel = wbMain.Document.GetElementById("leftPanel");
			foreach (var filename in Directory.GetFiles(samplePath)) {
				var element = wbMain.Document.CreateElement("div");
				element.InnerHtml = Path.GetFileName(filename);
				element.Click += (o,e1)=>{
					LoadSample((o as HtmlElement).InnerHtml);
				};
				leftPanel.AppendChild(element);
			}
			
			wbMain.Document.GetElementById("btnRun").Click += (o,e1)=>{
                var content = wbMain.Document.GetElementById("txtContent").InnerText;
                if (String.IsNullOrEmpty(content)) return;

                var p = this.InitProxy();

                var domain = DomainManager.SampleDomain;

                var type = typeof(SampleAdapter);
                var adapter = domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName) as SampleAdapter;

                adapter.Execute(content, p);
			};
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{							
			LoadPanel();		                     
		}
		
		void BtnLoadClick(object sender, EventArgs e)
		{
			LoadPanel();
		}
		
		#region Utility
		String samplePath = @"..\Resources\Samples";

		void LoadPanel(){
			wbMain.Url = new Uri(Environment.CurrentDirectory + "\\base.html");	
		}
		
		void LoadSample(string filename){
			var content = File.ReadAllText(Path.Combine(samplePath,filename));
			wbMain.Document.GetElementById("txtContent").InnerText = content;
			wbMain.Document.GetElementById("lbSample").InnerText = String.Format("Codes {0}:", filename);
		}

        Proxy InitProxy()
        {
            var proxy = new Proxy();

            proxy.ConsoleWrite = (content, newline) =>
            {
                var txtOutput = wbMain.Document.Window.Frames["txtOutput"].Document.Body;
                txtOutput.InnerHtml += content.Replace(" ", "&nbsp;").Replace("\t", "<span class='tab'></span>");
                if (newline) txtOutput.InnerHtml += "<br/>";
            };

            return proxy;
        }
		#endregion
	}
}
