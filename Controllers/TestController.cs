using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
namespace JazzApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("PDF")]
        public IActionResult PDF()
        {
#if DEBUG
            PreviewPdf();
#endif
            byte[] pdfBytes;

            using (var stream = new MemoryStream())
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text("Hellocgcgc gt PDF!")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Spacing(20);
                                x.Item().Text(Placeholders.LoremIpsum());
                                x.Item().Image(Placeholders.Image(200, 100));
                            });

                        page.Footer()
                            .AlignRight()
                            .Height(3, Unit.Centimetre)
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                                x.Span(" de ");
                                x.TotalPages();
                            });
                    });
                });

                document.GeneratePdf(stream);
                pdfBytes = stream.ToArray();
            }

            var fileName = "hello.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

#if DEBUG
        private void PreviewPdf()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignRight()
                        .Height(3, Unit.Centimetre)
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
                });
            });

            QuestPDF.Previewer.Extensions.ShowInPreviewer(document);
        }
#endif
    }
}
