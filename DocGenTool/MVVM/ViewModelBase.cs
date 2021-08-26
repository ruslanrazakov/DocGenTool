﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DocGenTool.MVVM
{
	/// <summary>
	/// Implements INotifyPropertyChanged interface for property binding in viewmodels
	/// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
				return false;
			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}
	}
}
