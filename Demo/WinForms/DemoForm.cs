// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.Demo.Common;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.WinForms;
using PdfSharp;
using System.Drawing.Printing;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace TheArtOfDev.HtmlRenderer.Demo.WinForms
{
    public partial class DemoForm : Form
    {
        PdfGenerateConfig config = new PdfGenerateConfig();
        Size orgPageSize = new Size();
        Size pageSize = new Size();

        double scrollOffset = 0;
        int iPage = 0;
        private bool bFirstPagePrinting = false;

        HtmlRenderer.WinForms.HtmlContainer hc = new HtmlRenderer.WinForms.HtmlContainer();

        PrintDocument pd = new PrintDocument();
        #region Fields/Consts

        /// <summary>
        /// the private font used for the demo
        /// </summary>
        private readonly PrivateFontCollection _privateFont = new PrivateFontCollection();

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public DemoForm()
        {
            SamplesLoader.Init(HtmlRenderingHelper.IsRunningOnMono() ? "Mono" : "WinForms", typeof(HtmlRender).Assembly.GetName().Version.ToString());

            InitializeComponent();

            Icon = GetIcon();
            _openSampleFormTSB.Image = Common.Properties.Resources.form;
            _showIEViewTSSB.Image = Common.Properties.Resources.IE;
            _openInExternalViewTSB.Image = Common.Properties.Resources.chrome;
            _useGeneratedHtmlTSB.Image = Common.Properties.Resources.code;
            _generateImageSTB.Image = Common.Properties.Resources.image;
            _generatePdfTSB.Image = Common.Properties.Resources.pdf;
            _runPerformanceTSB.Image = Common.Properties.Resources.stopwatch;

            StartPosition = FormStartPosition.CenterScreen;
            var size = Screen.GetWorkingArea(Point.Empty);
            Size = new Size((int)(size.Width * 0.7), (int)(size.Height * 0.8));

            LoadCustomFonts();

            _showIEViewTSSB.Enabled = !HtmlRenderingHelper.IsRunningOnMono();
            pd.PrintPage += Pd_PrintPage;
        }


        /// <summary>
        /// Load custom fonts to be used by renderer HTMLs
        /// </summary>
        private void LoadCustomFonts()
        {
            // load custom font font into private fonts collection
            var file = Path.GetTempFileName();
            File.WriteAllBytes(file, Resources.CustomFont);
            _privateFont.AddFontFile(file);

            // add the fonts to renderer
            foreach (var fontFamily in _privateFont.Families)
                HtmlRender.AddFontFamily(fontFamily);
        }

        /// <summary>
        /// Get icon for the demo.
        /// </summary>
        internal static Icon GetIcon()
        {
            var stream = typeof(DemoForm).Assembly.GetManifestResourceStream("HtmlRenderer.Demo.WinForms.html.ico");
            return stream != null ? new Icon(stream) : null;
        }

        private void OnOpenSampleForm_Click(object sender, EventArgs e)
        {
            using (var f = new SampleForm())
            {
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Toggle if to show split view of HtmlPanel and WinForms WebBrowser control.
        /// </summary>
        private void OnShowIEView_ButtonClick(object sender, EventArgs e)
        {
            _showIEViewTSSB.Checked = !_showIEViewTSSB.Checked;
            _mainControl.ShowWebBrowserView(_showIEViewTSSB.Checked);
        }

        /// <summary>
        /// Open the current html is external process - the default user browser.
        /// </summary>
        private void OnOpenInExternalView_Click(object sender, EventArgs e)
        {
            var tmpFile = Path.ChangeExtension(Path.GetTempFileName(), ".htm");
            File.WriteAllText(tmpFile, _mainControl.GetHtml());
            Process.Start(tmpFile);
        }

        /// <summary>
        /// Toggle the use generated html button state.
        /// </summary>
        private void OnUseGeneratedHtml_Click(object sender, EventArgs e)
        {
            _useGeneratedHtmlTSB.Checked = !_useGeneratedHtmlTSB.Checked;
            _mainControl.UseGeneratedHtml = _useGeneratedHtmlTSB.Checked;
            _mainControl.UpdateWebBrowserHtml();
        }

        /// <summary>
        /// Open generate image form for the current html.
        /// </summary>
        private void OnGenerateImage_Click(object sender, EventArgs e)
        {
            using (var f = new GenerateImageForm(_mainControl.GetHtml()))
            {
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Create PDF using PdfSharp project, save to file and open that file.
        /// </summary>
        private void OnGeneratePdf_Click(object sender, EventArgs e)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.PageSize = PageSize.A4;
            config.SetMargins(20);

            var doc = PdfGenerator.GeneratePdf(_mainControl.GetHtml(), config, null, DemoUtils.OnStylesheetLoad, HtmlRenderingHelper.OnImageLoadPdfSharp);
            var tmpFile = Path.GetTempFileName();
            tmpFile = Path.GetFileNameWithoutExtension(tmpFile) + ".pdf";
            doc.Save(tmpFile);
            Process.Start(tmpFile);
        }

        /// <summary>
        /// Execute performance test by setting all sample HTMLs in a loop.
        /// </summary>
        private void OnRunPerformance_Click(object sender, EventArgs e)
        {
            _mainControl.UpdateLock = true;
            _toolStrip.Enabled = false;
            Application.DoEvents();

            var msg = DemoUtils.RunSamplesPerformanceTest(html =>
            {
                _mainControl.SetHtml(html);
                Application.DoEvents(); // so paint will be called
            });

            Clipboard.SetDataObject(msg);
            MessageBox.Show(msg, "Test run results");

            _mainControl.UpdateLock = false;
            _toolStrip.Enabled = true;
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            if (bFirstPagePrinting)
            {
                bFirstPagePrinting = false;
                hc.PerformLayout(e.Graphics);
            }
            if (hc.PageListCount > 0)
            {
                if (iPage <= hc.PageListCount-1)
                {
                    if (iPage == hc.PageListCount - 1)
                    {
                        hc.PerformPrint(e.Graphics,iPage);
                        e.HasMorePages = false;
                    }
                    else
                    {
                        hc.PerformPrint(e.Graphics, iPage);
                        e.HasMorePages = true;
                        iPage++;
                    }
                    
                }
                else
                {

                }
            }
            else
            { 
                XSize e_Graphics_XSize = new XSize(Convert.ToDouble(e.PageSettings.PrintableArea.Width), Convert.ToDouble(e.PageSettings.PrintableArea.Height));
                e.Graphics.IntersectClip(new RectangleF(config.MarginLeft, config.MarginTop, pageSize.Width, pageSize.Height));
                hc.ScrollOffset = new Point(0, Convert.ToInt32(scrollOffset));
                hc.PerformPaint(e.Graphics);
                scrollOffset -= pageSize.Height;
                if (scrollOffset > -hc.ActualSize.Height)
                {
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
        }

        private void btn_Pages_Click(object sender, EventArgs e)
        {

        }

        private void tsb_Print_Click(object sender, EventArgs e)
        {
            PrintDialog pdlg = new PrintDialog();
            pdlg.Document = pd;
            DialogResult dlgRes = pdlg.ShowDialog();
            if (dlgRes == DialogResult.OK)
            {
                config.PageSize = PageSize.A4;
                config.SetMargins(20);
                XSize orgPageSize = PageSizeConverter.ToSize(config.PageSize);
                orgPageSize = new Size(Convert.ToInt32(orgPageSize.Width), Convert.ToInt32(orgPageSize.Height));
                pageSize = new Size(Convert.ToInt32(orgPageSize.Width - config.MarginLeft - config.MarginRight), Convert.ToInt32(orgPageSize.Height - config.MarginTop - config.MarginBottom));
                hc.SetHtml(_mainControl._htmlPanel.Text);
                hc.Location = new PointF(config.MarginLeft, config.MarginTop);
                hc.MaxSize = new Size(Convert.ToInt32(pageSize.Width), 0);
                hc.SetHtml(_mainControl._htmlPanel.Text);

                scrollOffset = 0;

                hc.UseGdiPlusTextRendering = true;

                //SizeF szf = new SizeF(pd.DefaultPageSettings.PaperSize.Width, pd.DefaultPageSettings.PaperSize.Height);
                //hc.MaxSize = szf;
                iPage = 0;
                bFirstPagePrinting = true;
                pd.Print();

            }
        }

    }
}