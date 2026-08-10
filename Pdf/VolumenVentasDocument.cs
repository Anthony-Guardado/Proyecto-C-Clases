using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InventaMeCF.Pdf
{
    public class VolumenVentasDocument : IDocument
    {
        public VolumenVentasModel Model { get; }

        public VolumenVentasDocument(VolumenVentasModel model)
        {
            Model = model;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("InventaMeCF")
                        .Bold().FontSize(22).FontColor(Colors.Blue.Darken2);
                    column.Item().Text(string.IsNullOrEmpty(Model.Titulo) ? "Reporte de Volumen de Ventas" : Model.Titulo)
                        .SemiBold().FontSize(14).FontColor(Colors.Grey.Darken1);
                });

                row.ConstantItem(180).Column(column =>
                {
                    column.Item().AlignRight().Text($"Fecha: {Model.FechaGeneracion:dd/MM/yyyy HH:mm}")
                        .FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Spacing(15);

                // --- SECCIÓN: GRÁFICO VISUAL DE BARRAS DE VENTAS ---
                if (Model.Items != null && Model.Items.Any())
                {
                    column.Item().Background(Colors.Grey.Lighten4).Padding(12).Column(chartColumn =>
                    {
                        chartColumn.Spacing(8);
                        chartColumn.Item().Text("📊 Gráfico de Volumen de Ventas por Producto")
                            .Bold().FontSize(12).FontColor(Colors.Blue.Darken3);

                        var maxCantidad = Model.Items.Max(i => i.Volumen);
                        if (maxCantidad == 0) maxCantidad = 1;

                        foreach (var item in Model.Items)
                        {
                            var porcentaje = (float)item.Volumen / maxCantidad;
                            var pctEntero = Math.Max(1, (int)(porcentaje * 100));

                            chartColumn.Item().Row(row =>
                            {
                                row.ConstantItem(130).Text(item.Nombre)
                                    .FontSize(10).SemiBold();

                                row.RelativeItem().PaddingVertical(2).Row(barRow =>
                                {
                                    barRow.RelativeItem(pctEntero)
                                        .Height(14)
                                        .Background(Colors.Blue.Medium);

                                    if (pctEntero < 100)
                                    {
                                        barRow.RelativeItem(100 - pctEntero)
                                            .Height(14)
                                            .Background(Colors.Grey.Lighten3);
                                    }
                                });

                                row.ConstantItem(70).AlignRight().Text($"{item.Volumen} unids")
                                    .FontSize(10).Bold().FontColor(Colors.Grey.Darken2);
                            });
                        }
                    });
                }

                // --- SECCIÓN: TABLA DE DETALLE ---
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Producto
                        columns.RelativeColumn(2); // Volumen / Cantidad
                        columns.RelativeColumn(2); // Monto Total
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderStyle).Text("PRODUCTO");
                        header.Cell().Element(HeaderStyle).AlignRight().Text("VOLUMEN VENDIDO");
                        header.Cell().Element(HeaderStyle).AlignRight().Text("MONTO TOTAL");

                        static IContainer HeaderStyle(IContainer c) =>
                            c.Background(Colors.Blue.Darken2)
                             .Padding(6)
                             .DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White));
                    });

                    if (Model.Items != null && Model.Items.Any())
                    {
                        foreach (var item in Model.Items)
                        {
                            table.Cell().Element(CellStyle).Text(item.Nombre);
                            table.Cell().Element(CellStyle).AlignRight().Text(item.Volumen.ToString("N0"));
                            table.Cell().Element(CellStyle).AlignRight().Text($"$ {item.MontoTotal:N2}");

                            static IContainer CellStyle(IContainer c) =>
                                c.BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2)
                                 .Padding(6);
                        }
                    }
                    else
                    {
                        table.Cell().ColumnSpan(3).Element(CellStyle).Text("No hay ventas registradas.");
                        static IContainer CellStyle(IContainer c) => c.Padding(6);
                    }
                });

                // --- TOTAL GENERAL ---
                column.Item().AlignRight().Text($"Gran Total: $ {Model.GranTotal:N2}")
                    .Bold().FontSize(13).FontColor(Colors.Green.Darken2);
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Text("InventaMeCF - Reportes del Sistema")
                    .FontSize(9).FontColor(Colors.Grey.Medium);
                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("Página ").FontSize(9).FontColor(Colors.Grey.Medium);
                    x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });
        }
    }
}
