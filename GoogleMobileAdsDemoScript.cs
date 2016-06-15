using System;
using UnityEngine;
using GoogleMobileAds; 
using GoogleMobileAds.Api;  

public class GoogleMobileAdsDemoHandler : IInAppPurchaseHandler
{
	private readonly string[] validSkus = { "android.test.purchased" };
	
	//Will only be sent on a success.
	public void OnInAppPurchaseFinished(IInAppPurchaseResult result)
	{
		result.FinishPurchase();
		GoogleMobileAdsDemoScript.OutputMessage = "Purchase Succeeded! Credit user here.";
	}
	
	//Check SKU against valid SKUs.
	public bool IsValidPurchase(string sku)
	{ 
		foreach(string validSku in validSkus) {
			if (sku == validSku) {
				return true; 
			}
		}
		return false;
	}
	
	//Return the app's public key.
	public string AndroidPublicKey
	{
		//In a real app, return public key instead of null.
		get { return null; }
	}
}

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{
	 
	public static BannerView bannerView;
	public static InterstitialAd interstitial;
	public static string outputMessage = ""; 
	
	public static string OutputMessage
	{
		set { outputMessage = value; }
	}
	
	/*	void OnGUI()  
	{
		// Puts some basic buttons onto the screen.
		GUI.skin.button.fontSize = (int) (0.05f * Screen.height);
		GUI.skin.label.fontSize = (int) (0.025f * Screen.height);
		
		Rect requestBannerRect = new Rect(0.1f * Screen.width, 0.05f * Screen.height,
		                                  0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(requestBannerRect, "Request Banner"))
		{
			RequestBanner();
		}
		
		Rect showBannerRect = new Rect(0.1f * Screen.width, 0.175f * Screen.height,
		                               0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showBannerRect, "Show Banner"))
		{
			bannerView.Show();
		}
		
		Rect hideBannerRect = new Rect(0.1f * Screen.width, 0.3f * Screen.height,
		                               0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(hideBannerRect, "Hide Banner"))
		{
			bannerView.Hide();
		}
		
		Rect destroyBannerRect = new Rect(0.1f * Screen.width, 0.425f * Screen.height,
		                                  0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(destroyBannerRect, "Destroy Banner"))
		{
			bannerView.Destroy();
		}
		
		Rect requestInterstitialRect = new Rect(0.1f * Screen.width, 0.55f * Screen.height,
		                                        0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(requestInterstitialRect, "Request Interstitial"))
		{
			RequestInterstitial();
		}
		
		Rect showInterstitialRect = new Rect(0.1f * Screen.width, 0.675f * Screen.height,
		                                     0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(showInterstitialRect, "Show Interstitial"))
		{
			ShowInterstitial();
		}
		
		Rect destroyInterstitialRect = new Rect(0.1f * Screen.width, 0.8f * Screen.height,
		                                        0.8f * Screen.width, 0.1f * Screen.height);
		if (GUI.Button(destroyInterstitialRect, "Destroy Interstitial"))
		{
			interstitial.Destroy();
		}
		
		Rect textOutputRect = new Rect(0.1f * Screen.width, 0.925f * Screen.height,
		                               0.8f * Screen.width, 0.05f * Screen.height);
		GUI.Label(textOutputRect, outputMessage);
	}
	*/
	
	public static void RequestBanner() 
	{ 
		#if UNITY_EDITOR
		string adUnitId = "ca-app-pub-4841594385449270/5128691545";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-4841594385449270/5128691545";      
		#elif UNITY_IPHONE  
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE"; 
		#else
		string adUnitId = "unexpected_platform";
		#endif 
		
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom); 
		// Register for ad events.
		bannerView.AdLoaded += HandleAdLoaded;
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		bannerView.AdOpened += HandleAdOpened;
		bannerView.AdClosing += HandleAdClosing; 
		bannerView.AdClosed += HandleAdClosed;
		bannerView.AdLeftApplication += HandleAdLeftApplication;
		// Load a banner ad.
		bannerView.LoadAd(createAdRequest());
	}
	
	public static void RequestInterstitial()  
	{
		#if UNITY_EDITOR
		string adUnitId = "ca-app-pub-3940256099942544/1033173712";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1033173712";         
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create an interstitial.
		interstitial = new InterstitialAd(adUnitId);
		// Register for ad events.
		interstitial.AdLoaded += HandleInterstitialLoaded;
		interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
		interstitial.AdOpened += HandleInterstitialOpened;
		interstitial.AdClosing += HandleInterstitialClosing;
		interstitial.AdClosed += HandleInterstitialClosed;
		interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
		GoogleMobileAdsDemoHandler handler = new GoogleMobileAdsDemoHandler();
		interstitial.SetInAppPurchaseHandler(handler);
		// Load an interstitial ad.
		interstitial.LoadAd(createAdRequest());
	}
	
	// Returns an ad request with custom ad targeting.
	public static AdRequest createAdRequest()
	{
		return new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
				.AddKeyword("game")
				.SetGender(Gender.Male)
				.SetBirthday(new DateTime(1985, 1, 1))
				.TagForChildDirectedTreatment(false)
				.AddExtra("color_bg", "9B30FF")
				.Build();
		
	}
	
	public static void ShowInterstitial()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
		else
		{
			print("Interstitial is not ready yet.");
		}
	}
	
	#region Banner callback handlers
	
	public static void HandleAdLoaded(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");
	}
	
	public static void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}
	
	public static void HandleAdOpened(object sender, EventArgs args)
	{
		print("HandleAdOpened event received");
	}
	
	public static void HandleAdClosing(object sender, EventArgs args)
	{
		print("HandleAdClosing event received");
	}
	
	public static void HandleAdClosed(object sender, EventArgs args)
	{
		print("HandleAdClosed event received");
	}
	
	public static void HandleAdLeftApplication(object sender, EventArgs args)
	{
		print("HandleAdLeftApplication event received");
	}
	
	#endregion
	
	#region Interstitial callback handlers
	
	public static void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		print("HandleInterstitialLoaded event received.");
	} 
	
	public static void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	}
	
	public static void HandleInterstitialOpened(object sender, EventArgs args)
	{
		print("HandleInterstitialOpened event received");
	}
	
	public static void HandleInterstitialClosing(object sender, EventArgs args)
	{
		print("HandleInterstitialClosing event received");
	}
	
	public static void HandleInterstitialClosed(object sender, EventArgs args)
	{
		print("HandleInterstitialClosed event received");
	}
	
	public static void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		print("HandleInterstitialLeftApplication event received");
	}
	
	#endregion
}