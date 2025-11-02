using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using Grid = Microsoft.Maui.Controls.Grid;
using LabCalculator;

namespace MyExcelMAUIApp
{

    public partial class MainPage : ContentPage
    {
        private int CountColumn = 5;
        private int CountRow = 5;
        private CellManager cellManager = new();

        public MainPage()
        {
            InitializeComponent();
            CreateGrid();
        }

        private void CreateGrid()
        {
            AddColumnsAndColumnLabels();
            grid.RowDefinitions.Add(new RowDefinition());
            AddRowsAndCellEntries();
        }
        private void AddColumnsAndColumnLabels()
        {
            for (int col = 0; col < CountColumn + 1; col++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                if (col > 0)
                {
                    var label = new Label
                    {
                        Text = GetColumnName(col),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Grid.SetRow(label, 0);
                    Grid.SetColumn(label, col);
                    grid.Children.Add(label);
                }
            }
        }
        private void AddRowsAndCellEntries()
        {
            for (int row = 0; row < CountRow; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                var label = new Label
                {
                    Text = (row + 1).ToString(),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Grid.SetRow(label, row + 1);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);

                for (int col = 0; col < CountColumn; col++)
                {
                    var entry = new Entry
                    {
                        Text = "",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    entry.Focused += Entry_Focused;
                    entry.Unfocused += Entry_Unfocused;

                    Grid.SetRow(entry, row + 1);
                    Grid.SetColumn(entry, col + 1);
                    grid.Children.Add(entry);
                }
            }
        }

        private void AddColumnButton_Clicked(object sender, EventArgs e)
        {
            CountColumn++;
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            var label = new Label
            {
                Text = GetColumnName(CountColumn),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, CountColumn);
            grid.Children.Add(label);

            for (int row = 0; row < CountRow; row++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                entry.Focused += Entry_Focused;
                entry.Unfocused += Entry_Unfocused;

                Grid.SetRow(entry, row + 1);
                Grid.SetColumn(entry, CountColumn);
                grid.Children.Add(entry);
            }
        }


        private string GetColumnName(int colIndex)
        {
            int dividend = colIndex;
            string columnName = string.Empty;
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26; columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }
        private void Entry_Unfocused(object? sender, FocusEventArgs e)
        {
            var entry = (Entry)sender!;
            var row = Grid.GetRow(entry) - 1;
            var col = Grid.GetColumn(entry) - 1;
            var cellName = $"{GetColumnName(col + 1)}{row + 1}";
            var content = entry.Text?.Trim();
            
            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    cellManager.SetExpression(cellName, content);
                    double result = cellManager.GetValue(cellName);

                    entry.TextColor = double.IsNaN(result) ? Colors.OrangeRed
                                  : result != 0.0 ? Colors.Green
                                  : Colors.Red;

                    entry.Text = cellManager.GetDisplayText(cellName);

                   
                }
                catch (Exception ex)
                {
                    entry.TextColor = Colors.OrangeRed;
                    entry.Text = "Error";
                    Console.WriteLine($"Помилка: {ex.GetType().Name} — {ex.Message}");
                }
            }
            else
            {
                entry.Text = "";
            }
        }

        private void Entry_Focused(object? sender, FocusEventArgs e)
        {
            var entry = (Entry)sender!;
            int row = Grid.GetRow(entry) - 1;
            int col = Grid.GetColumn(entry) - 1;

            if (row >= 0 && col >= 0)
            {
                string cellName = $"{GetColumnName(col + 1)}{row + 1}";
                entry.Text = cellManager.GetExpression(cellName);
            }
        }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти ? ", "Так", "Ні");
            if (answer)
            {
                System.Environment.Exit(0);
            }
        }
        private async void HelpButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Довідка", "Лабораторна робота 1. Варіант 60. Виконав Давиденко Ярослав", "OK");
        }

        private void AddRowButton_Clicked(object sender, EventArgs e)
        {
            CountRow++;
            grid.RowDefinitions.Add(new RowDefinition());

            var label = new Label
            {
                Text = CountRow.ToString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, CountRow);
            Grid.SetColumn(label, 0);
            grid.Children.Add(label);

            for (int col = 0; col < CountColumn; col++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                entry.Focused += Entry_Focused;
                entry.Unfocused += Entry_Unfocused;

                Grid.SetRow(entry, CountRow);
                Grid.SetColumn(entry, col + 1);
                grid.Children.Add(entry);
            }
        }




    }
}