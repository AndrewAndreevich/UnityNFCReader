using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class NFCReader : MonoBehaviour
{
	private AndroidJavaObject mActivity;
	private AndroidJavaObject mIntent;
	private string sAction;

	[SerializeField] private NFC_Interface handler;
	
	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
			try {
					// Create new NFC Android object
					mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
					AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
					mIntent = intent;
					sAction = mIntent.Call<String>("getAction"); // resulte are returned in the Intent object
					if (sAction == "android.nfc.action.NDEF_DISCOVERED")
					{
						reactToAction();
					}
				}
				catch (Exception ex) {
					Debug.Log(ex);
				}
		}
	}


	void reactToAction()
	{

		AndroidJavaObject[] mNdefMessage =
			mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
		if (mNdefMessage != null)
		{
			AndroidJavaObject[] mNdefRecord1 = mNdefMessage[0].Call<AndroidJavaObject[]>("getRecords");
			if (mNdefRecord1 != null)
			{
				string[] records  = new string[mNdefRecord1.Length];
				
				for (int i = 0; i < mNdefRecord1.Length; i++)
				{
					byte[] payLoad1 = mNdefRecord1[i].Call<byte[]>("getPayload");
					records[i] = System.Text.Encoding.UTF8.GetString(payLoad1);
				}
				
				handler.Proceed(records);
				
				mIntent.Call<AndroidJavaObject>("setAction", "");
				mIntent.Call<AndroidJavaObject>("setData", null);
				
				
				return;
			}
			else
			{

			}
		
		}
		else if (sAction == "android.nfc.action.TECH_DISCOVERED")
		{
			Debug.Log("No ID found !");
		}
		else if (sAction == "android.nfc.action.TAG_DISCOVERED")
		{
			Debug.Log("This type of tag is not supported !");

		}
		else
		{
			Debug.Log("No ID found !");
		}
	}
}