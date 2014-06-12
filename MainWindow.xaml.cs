using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace nocoSaiga
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ev_btn();
        }

        private XmlDataProvider _Prov { get { return (XmlDataProvider)Resources["newXML"]; } }

        private void ev_btn()
        {
            PNewBtn.Click += (s, e) =>
                {
                    if (PSel.SelectedIndex > 0)
                    {
                        PSel.SelectedIndex = 0;
                        _Prov.Source = null;
                    }
                    var ge = new getImg();
                    ge.new_illust();
                    _Prov.Source = new Uri(getImg.newXMLFile);
                };

            PSel.SelectionChanged += (s, e) =>
                {
                    if (PSel.SelectedIndex > 0)
                    {
                        _Prov.Source = null;
                    }
                    var sel = PSel.SelectedValue.ToString();
                    sel = Regex.Split(sel, ": ")[1];
                    var ge = new getImg();
                    if (sel != "선택")
                    {
                        switch (sel)
                        {
                            case "러브라이브!":
                                ge.sel_illust("ラブライブ！");
                                _Prov.Source = new Uri(getImg.newXMLFile);
                                break;
                            case "칸코레":
                                ge.sel_illust("艦これ");
                                _Prov.Source = new Uri(getImg.newXMLFile);
                                break;
                            case "창작":
                                ge.sel_illust("オリジナル");
                                _Prov.Source = new Uri(getImg.newXMLFile);
                                break;
                            case "선택":
                                _Prov.Source = null;
                                break;
                        }
                    }
                };
  
            PNew.MouseDoubleClick += (s, e) =>
                {
                    var imgSrc = PNew.SelectedValue.ToString();
                    imgSrc = Regex.Split(imgSrc, "http://")[1];
                    imgSrc = Regex.Split(imgSrc, "m출처")[0];
                    var author = PNew.SelectedValue.ToString();
                    author = Regex.Split(author, "http://")[0];
                    author = Regex.Split(author, " : ")[1];
                    var url = PNew.SelectedValue.ToString();
                    url = Regex.Split(url, "출처 : ")[1];
                    var res = System.Windows.MessageBox.Show("해당 일러스트를 내려 받으시겠습니까?", "일러스트 다운로드", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        using (var sv = new SaveFileDialog())
                        {
                            sv.Filter = @"일러스트 파일|*.jpg";
                            sv.Title = "일러스트 내려받기";

                            if (sv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                using (var wb = new WebClient())
                                {
                                    wb.Encoding = Encoding.UTF8;

                                    /*var filepath = sv.FileName.Replace(
                                        Regex.Split(sv.FileName, @".jpg").First(),
                                        Regex.Split(sv.FileName, @".jpg").First() + " - 게시자_" + author);*/
                                    wb.DownloadFile("http://" + imgSrc + 'l', sv.FileName);
                                    System.Windows.MessageBox.Show("다운로드 완료 했습니다." + "\n\n해당 일러스트 출처 입니다." +
                                    "\n출처 복사는 해당 메시지 창이" + 
                                    "\n활성화 된 상태에서 Ctrl+C 하시면 됩니다."+   
                                    "\n\n게시자 : " + author + "\n출처 : " + url , "일러스트 다운로드 완료");
                                    wb.Dispose();
                                }
                            }
                        }
                    }
                };
        }
    }
}
