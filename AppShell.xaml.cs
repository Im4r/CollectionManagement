﻿namespace CollectionManagement
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CollectionPage), typeof(CollectionPage));
        }
    }
}
