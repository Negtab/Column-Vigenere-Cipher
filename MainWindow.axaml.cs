using Avalonia.Controls;
using Crypto.Algorithms;
using Avalonia.Platform.Storage;
using System.IO;
using System.Text;


namespace Crypto;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void EncryptClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (InputText.Text is null || KeyText.Text is null)
            return;
        string text = InputText.Text;
        string key = KeyText.Text;

        if (AlgorithmBox.SelectedIndex == 0)
        {
            text = TextFilter.FilterRussian(text);
            key = TextFilter.FilterRussian(key);

            if (text != "" && key != "")
                ResultText.Text = VigenereRussian.Encrypt(text, key);
            else
                ResultText.Text = "";
        }
        else
        {
            text = TextFilter.FilterEnglish(text);
            key = TextFilter.FilterEnglish(key);

            if (text != "" && key != "")
                ResultText.Text = ColumnarProgressive.Encrypt(text, key);
            else
                ResultText.Text = "";
        }
    }

    private void DecryptClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (InputText.Text is null || KeyText.Text is null)
            return;
        
        string text = InputText.Text;
        string key = KeyText.Text;

        if (AlgorithmBox.SelectedIndex == 0)
        {
            text = TextFilter.FilterRussian(text);
            key = TextFilter.FilterRussian(key);

            if (text != "" && key != "")
                ResultText.Text = VigenereRussian.Decrypt(text, key);
            else
                ResultText.Text = "";
        }
        else
        {
            text = TextFilter.FilterEnglish(text);
            key = TextFilter.FilterEnglish(key);

            if (text != "" && key != "")
                ResultText.Text = ColumnarProgressive.Decrypt(text, key);
            else
                ResultText.Text = "";
        }
    }
    
    private async void OpenFileClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open text file",
            AllowMultiple = false
        });

        if (files.Count == 0)
            return;

        await using var stream = await files[0].OpenReadAsync();
        using var reader = new StreamReader(stream);

        InputText.Text = await reader.ReadToEndAsync();
    }
    
    private async void SaveFileClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(string.IsNullOrWhiteSpace(ResultText.Text))
            return;
        
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save result",
            SuggestedFileName = "result.txt"
        });

        if (file == null)
            return;

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream, Encoding.UTF8);

        await writer.WriteAsync(ResultText.Text);
    }
}