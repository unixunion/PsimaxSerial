using System;

namespace Psimax
{
	#pragma warning disable 436
	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoTODOAttribute : Attribute {

		string comment;

		public MonoTODOAttribute ()
		{
		}

		public MonoTODOAttribute (string comment)
		{
			this.comment = comment;
		}

		public string Comment {
			get { return comment; }
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoDocumentationNoteAttribute : MonoTODOAttribute {

		public MonoDocumentationNoteAttribute (string comment)
			: base (comment)
		{
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoExtensionAttribute : MonoTODOAttribute {

		public MonoExtensionAttribute (string comment)
			: base (comment)
		{
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoInternalNoteAttribute : MonoTODOAttribute {

		public MonoInternalNoteAttribute (string comment)
			: base (comment)
		{
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoLimitationAttribute : MonoTODOAttribute {

		public MonoLimitationAttribute (string comment)
			: base (comment)
		{
		}
	}

	[AttributeUsage (AttributeTargets.All, AllowMultiple=true)]
	internal class MonoNotSupportedAttribute : MonoTODOAttribute {

		public MonoNotSupportedAttribute (string comment)
			: base (comment)
		{
		}
	}
	#pragma warning restore 436	
}

