﻿namespace SistemaMedicoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = new AppShell();
            shell.GoToAsync("//LoginPage"); // Fuerza navegar a login
            return new Window(shell);
        }
    }
}