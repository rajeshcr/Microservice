﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xigadee;

namespace Test.Xigadee
{
    public abstract class WrapperBase : IConsolePersistence
    {
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public abstract ServiceStatus Status { get; }

        public IRepositoryAsync<Guid, MondayMorningBlues> Persistence { get; protected set; }

        public abstract string Name { get; protected set; }


        public abstract void Start();

        public abstract void Stop();

        protected void OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            var serv = sender as IConsolePersistence;

            StatusChanged?.Invoke(sender, e);
            //sMenuMain.Value.AddInfoMessage($"{serv.Name}={e.StatusNew.ToString()}{e.Message}", true);
        }
    }
}