﻿using System;
using System.Collections.Generic;
using System.Linq;

#if XAMCORE_2_0
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using MonoTouch.NUnit.UI;
using NUnit.Framework.Internal.Filters;

#if !__WATCHOS__

public static partial class TestLoader
{
	static partial void AddTestAssembliesImpl (BaseTouchRunner runner);

	public static void AddTestAssemblies (BaseTouchRunner runner)
	{
		AddTestAssembliesImpl (runner);
	}
}

namespace BCL.Tests
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		TouchRunner runner;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			runner = new TouchRunner (window);
			runner.Filter = new NotFilter (new CategoryExpression ("MobileNotWorking,NotOnMac,NotWorking,ValueAdd,CAS,InetAccess").Filter);

			// register every tests included in the main application/assembly
			runner.Add (System.Reflection.Assembly.GetExecutingAssembly ());
			TestLoader.AddTestAssemblies (runner);

			window.RootViewController = new UINavigationController (runner.GetViewController ());
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}
#endif // !__WATCHOS__
