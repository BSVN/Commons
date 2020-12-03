using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BSN.Commons.Infrastructure
{
	public class UpdateConfig<T> : IUpdateConfig<T> where T : class
	{
		public bool IncludeAllPropertiesEnabled
		{
			get
			{
				return _includeAllPropertiesEnabled;
			}
			private set
			{
				_includeAllPropertiesEnabled = value;
				if (value == true)
					_autoDetectChangedPropertiesEnabled = false;
			}
		}

		public bool AutoDetectChangedPropertiesEnabled
		{
			get
			{
				return _autoDetectChangedPropertiesEnabled;
			}
			private set
			{
				_autoDetectChangedPropertiesEnabled = value;
				if (value == true)
					_includeAllPropertiesEnabled = false;
			}
		}

		public IList<string> PropertyNames { get; }

		public UpdateConfig()
		{
			PropertyNames = new List<string>();
		}

		public void IncludeAllProperties()
		{
			IncludeAllPropertiesEnabled = true;
		}

		public void AutoDetectChangedProperties()
		{
			AutoDetectChangedPropertiesEnabled = true;
		}

        /// <summary>
        /// <see cref="PropertyNames"/>
        /// </summary>
        /// <typeparam name="TProperty">Property Type</typeparam>
        /// <param name="propertyAccessExpression">Property Access Expression</param>
        /// <returns>Update Config</returns>
		public IUpdateConfig<T> IncludeProperty<TProperty>(Expression<Func<T, TProperty>> propertyAccessExpression)
		{
			PropertyNames.Add(
				(propertyAccessExpression.Body as MemberExpression)?.Member.Name ??
					throw new ArgumentException(nameof(propertyAccessExpression))
			);

			AutoDetectChangedPropertiesEnabled = false;
			IncludeAllPropertiesEnabled = false;
			return this;
		}

		#region Fields

		private bool _includeAllPropertiesEnabled;

		private bool _autoDetectChangedPropertiesEnabled;

		#endregion
	}
}
