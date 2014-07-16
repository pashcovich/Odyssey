#region Using Directives

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion Using Directives

// General Information about an assembly is controlled through the following set of attributes.
// Change these attribute values to modify the information associated with an assembly.

[assembly: AssemblyTitle("Odyssey.Common")]
[assembly: AssemblyDescription("Common classes for the Odyssey Engine.")]
[assembly: AssemblyCompany("Avengers Utd - http://www.avengersutd.com/")]
[assembly: AssemblyCopyright("GPLv3")]
[assembly: NeutralResourcesLanguage("en")]

// Version information for an assembly consists of the following four values:
//
// Major Version Minor Version Build Number Revision
//
// You can specify all the values or you can default the Build and Revision Numbers by using the '*'
// as shown below: [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.2.*")]
[assembly: InternalsVisibleTo("Odyssey.2D")]
[assembly: InternalsVisibleTo("Odyssey.Engine")]
[assembly: InternalsVisibleTo("Odyssey.Content")]
[assembly: InternalsVisibleTo("Odyssey.Talos")]
[assembly: InternalsVisibleTo("Odyssey.Renderer")]
[assembly: InternalsVisibleTo("Odyssey.Renderer2D")]
[assembly: InternalsVisibleTo("Odyssey.Engine.Tests")]
[assembly: InternalsVisibleTo("Odyssey.Windows")]
[assembly: InternalsVisibleTo("Odyssey.Windows.WPF")]
[assembly: InternalsVisibleTo("Daedalus")]
[assembly: ComVisible(false)]