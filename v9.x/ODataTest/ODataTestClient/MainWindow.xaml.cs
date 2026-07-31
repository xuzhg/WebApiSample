using Default;
using System.Diagnostics;
using System.Windows;

namespace ODataTestClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var db   = new Container(new Uri("http://localhost:5084/main"));
        //var bill = db.Orders.Where(x => x.Id == 1).ToList().FirstOrDefault();
        //Debug.WriteLine(bill.Id);

        var amount = db.Orders.Sum(x => x.Amount);
        MessageBox.Show("amount=" + amount);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}