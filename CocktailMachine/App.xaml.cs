using CocktailMachine.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace CocktailMachine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            MessageBuilder messageBuilder = new MessageBuilder('#', '%');
            Drinks drinks = new Drinks();
            messageBuilder.Add(drinks.ArduinoDrinksString());
        }
    }
}
