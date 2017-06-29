//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("EsentInterop")]
[assembly: AssemblyDescription("Managed interop code for esent.dll")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("EsentInterop")]
[assembly: AssemblyCopyright("Copyright (c)  Microsoft")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//
// Version history:
// 1.6.0.0
// 1.7.0.0
// 1.8.0.0 Support new Windows user interface (As of Win8 Beta build).
// 1.8.1.0 2012.11.18. Some minor updates, including some perf updates.
// 1.8.2.0 2012.11.19. Signed.
// 1.8.3.0 2013.03.25. Signed and Strong Named. Renamed 'Metro' to 'Wsa' (Windows Store App)
// 1.8.4.0 2013.12.23. Updated for Windows 8.1.
// 1.9.0.0 2013.12.23. Go back to targetting framework 4.0.
// 1.9.1.0 2014.07.18. PersistentDictionary gets binary blobs; added Isam layer.
// 1.9.2.0 2014.09.11. Isam is placed in the Microsoft.Database namespace.
// 1.9.3.0 2015.08.11. Dependence added from Collections to Isam dll for configsets.
// 1.9.3.2 2015.09.02. Some bug fixes; go back to Framework 4.0
// 1.9.3.3 2016.03.01. Some bug and perf fixes.
// 1.9.4   2016.06.28. Some bug fixes.
[assembly: AssemblyVersion("1.9.4")]
[assembly: AssemblyFileVersion("1.9.4")]

#if STRONG_NAMED
[assembly: InternalsVisibleTo("EsentInteropWsaTests,    PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]
[assembly: InternalsVisibleTo("Esent.Isam,              PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]
[assembly: InternalsVisibleTo("InteropApiTests,         PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]
[assembly: InternalsVisibleTo("IsamUnitTests,         PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]
[assembly: InternalsVisibleTo("Pixie,                   PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]
[assembly: InternalsVisibleTo("PixieTests, 		        PublicKey=0024000004800000940000000602000000240000525341310004000001000100B5FC90E7027F67871E773A8FDE8938C81DD402BA65B9201D60593E96C492651E889CC13F1415EBB53FAC1131AE0BD333C5EE6021672D9718EA31A8AEBD0DA0072F25D87DBA6FC90FFD598ED4DA35E44C398C454307E8E33B8426143DAEC9F596836F97C8F74750E5975C64E2189F45DEF46B2A2B1247ADC3652BF5C308055DA9")]

// This assembly is generated by Rhino.Mocks
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
#else
[assembly: InternalsVisibleTo("InteropApiTests")]
[assembly: InternalsVisibleTo("IsamUnitTests")]
[assembly: InternalsVisibleTo("EsentInteropTestsImmersive")]
[assembly: InternalsVisibleTo("EsentInteropWsaTests")]
[assembly: InternalsVisibleTo("Esent.Isam")]
[assembly: InternalsVisibleTo("Pixie")]
[assembly: InternalsVisibleTo("PixieTests")]

// This assembly is generated by Rhino.Mocks
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif
