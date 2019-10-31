package com.calling.angel;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import com.example.android.trivialdrivesample.util.IabHelper;
import com.example.android.trivialdrivesample.util.IabResult;
import com.example.android.trivialdrivesample.util.Inventory;
import com.example.android.trivialdrivesample.util.Purchase;
import android.app.Activity;
import android.app.AlertDialog.Builder;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.pm.Signature;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import com.unity3d.player.UnityPlayer;
import com.yymoon.game.GameActivity;

public class MainActivity extends GameActivity {

	IabHelper mHelper;
	static final String TAG = "unity";
	boolean mIsPremium = false;
	String m_purchse_token = "";
	String m_google_orderid = "";
	String m_rid = "";
	boolean mSubscribedToInfiniteGas = false;
	static final String SKU_PREMIUM = "premium";
	static String SKU_GAS = "gas";
	static final String SKU_INFINITE_GAS = "infinite_gas";
	static final int RC_REQUEST = 10001;
	static final int TANK_MAX = 4;
	int mTank;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		this.Sdkinit();
	}

	private void Sdkinit()
	{
		String base64EncodedPublicKey = "";
		try {
			base64EncodedPublicKey = getPublicBaseKey();
			Log.d("Unity", base64EncodedPublicKey);
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		Log.d(TAG, "Creating IAB helper.");
		mHelper = new IabHelper(this, base64EncodedPublicKey);
		mHelper.enableDebugLogging(true);
		Log.d(TAG, "Starting setup.");
		mHelper.startSetup(new IabHelper.OnIabSetupFinishedListener() {
			public void onIabSetupFinished(IabResult result) {
				Log.d(TAG, "Setup finished.");
				if (!result.isSuccess()) {
					complain("roblem setting up in-app billing: " + result);
					return;
				}
				if (mHelper == null) return;
				Log.d(TAG, "Setup successful. Querying inventory.");
			}
		});
	}

	public static String printKeyHash(Activity context) {
		PackageInfo packageInfo;
		String key = null;
		try {
			String packageName = context.getApplicationContext().getPackageName();
			packageInfo = context.getPackageManager().getPackageInfo(packageName,
					PackageManager.GET_SIGNATURES);
			Log.e("Package Name=", context.getApplicationContext().getPackageName());
			for (Signature signature : packageInfo.signatures) {
				MessageDigest md = MessageDigest.getInstance("SHA");
				md.update(signature.toByteArray());
				key = new String(Base64.encode(md.digest(), 0));
				Log.e("Key Hash=", key);
			}
		} catch (NameNotFoundException e1) {
			Log.e("Name not found", e1.toString());
		} catch (NoSuchAlgorithmException e) {
			Log.e("No such an algorithm", e.toString());
		} catch (Exception e) {
			Log.e("Exception", e.toString());
		}
		return key;
	}

	IabHelper.QueryInventoryFinishedListener mGotInventoryListener = new IabHelper.QueryInventoryFinishedListener() {
		public void onQueryInventoryFinished(IabResult result, Inventory inventory) {
			Log.d(TAG, "Query inventory finished.");

			if (mHelper == null) return;

			if (result.isFailure()) {
				complain("Failed to query inventory: " + result);
				return;
			}

			Log.d(TAG, "Query inventory was successful.");

			Purchase premiumPurchase = inventory.getPurchase(SKU_PREMIUM);
			mIsPremium = (premiumPurchase != null && verifyDeveloperPayload(premiumPurchase));
			Log.d(TAG, "User is " + (mIsPremium ? "PREMIUM" : "NOT PREMIUM"));

			// Do we have the infinite gas plan?
			Purchase infiniteGasPurchase = inventory.getPurchase(SKU_INFINITE_GAS);
			mSubscribedToInfiniteGas = (infiniteGasPurchase != null &&
					verifyDeveloperPayload(infiniteGasPurchase));
			Log.d(TAG, "User " + (mSubscribedToInfiniteGas ? "HAS" : "DOES NOT HAVE")
					+ " infinite gas subscription.");
			if (mSubscribedToInfiniteGas) mTank = TANK_MAX;

			// Check for gas delivery -- if we own gas, we should fill up the tank immediately

			Purchase gasPurchase = inventory.getPurchase(SKU_GAS);
			if (gasPurchase != null && verifyDeveloperPayload(gasPurchase)) {
				Log.d(TAG, "We have gas. Consuming it.");
				mHelper.consumeAsync(inventory.getPurchase(SKU_GAS), mConsumeFinishedListener);
				return;
			}

			setWaitScreen(false);
			Log.d(TAG, "Initial inventory query finished; enabling main UI.");
		}
	};

	IabHelper.OnConsumeFinishedListener mConsumeFinishedListener = new IabHelper.OnConsumeFinishedListener() {
		public void onConsumeFinished(Purchase purchase, IabResult result) {
			Log.d(TAG, "Consumption finished. Purchase: " + purchase + ", result: " + result);
			if (mHelper == null) return;
			if (result.isSuccess()) {
				Log.d(TAG, "Consumption successful. Provisioning.");
				mTank = mTank == TANK_MAX ? TANK_MAX : mTank + 1;
				saveData();
			} else {
				complain("Error while consuming: " + result);
			}
			setWaitScreen(false);
			Log.d(TAG, "End consumption flow.");
		}
	};


	IabHelper.OnIabPurchaseFinishedListener mPurchaseFinishedListener = new IabHelper.OnIabPurchaseFinishedListener() {
		public void onIabPurchaseFinished(IabResult result, Purchase purchase) {
			Log.d(TAG, "Purchase finished: " + result + ", purchase: " + purchase);
			Log.d("unIty", result + ":" + result.getResponse());
			if(purchase != null)
			{
				Log.d("unIty", purchase + ":");
			}
			Log.d("unIty", "chongzhi11");
			if (mHelper == null) return;
			if (result.isFailure()) {
				if(result.getResponse() == 7)
				{
					Log.d("unitY",result.getResponse() + "");
					mHelper.queryInventoryAsync(mGotInventoryListener);
					return;
				}
				Log.d("unIty","result");
				Log.d("unIty", result + "");
				setWaitScreen(false);
				UnityPlayer.UnitySendMessage("game_node", "recharge_cancel", "");
				return;
			}
			m_purchse_token = purchase.getToken();
			m_google_orderid = m_purchse_token + "*" + purchase.getOrderId();
			if (!verifyDeveloperPayload(purchase)) {
				setWaitScreen(false);
				return;
			}
			Log.d(TAG, "Purchase successful.");
			Log.d(TAG, purchase.getSku());
			if (purchase.getSku().equals(SKU_GAS)) {
				Log.d(TAG, "Purchase is gas. Starting gas consumption.");
				mHelper.consumeAsync(purchase, mConsumeFinishedListener);
			}
			else if (purchase.getSku().equals(SKU_PREMIUM)) {
				alert("Thank you for upgrading to premium!");
				mIsPremium = true;
				setWaitScreen(false);
			}
			else if (purchase.getSku().equals(SKU_INFINITE_GAS)) {
				alert("Thank you for subscribing to infinite gas!");
				mSubscribedToInfiniteGas = true;
				mTank = TANK_MAX;
				setWaitScreen(false);
			}
			UnityPlayer.UnitySendMessage("game_node", "recharge_done", m_purchse_token);
		}
	};

	void saveData() {
		SharedPreferences.Editor spe = getPreferences(MODE_PRIVATE).edit();
		spe.putInt("tank", mTank);
		spe.commit();
		Log.d(TAG, "Saved data: tank = " + String.valueOf(mTank));
	}

	boolean verifyDeveloperPayload(Purchase p) {
		return true;
	}

	void setWaitScreen(boolean set) {

	}

	void complain(String message) {
		Log.e(TAG, "**** TrivialDrive Error: " + message);
		alert("Error: " + message);
	}

	void alert(String message) {
		Builder bld = new Builder(this);
		bld.setMessage(message);
		bld.setNeutralButton("OK", null);
		Log.d(TAG, "Showing alert dialog: " + message);
		bld.create().show();
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		Log.d(TAG, "onActivityResult(" + requestCode + "," + resultCode + "," + data);
		if (mHelper == null) return;
		if (!mHelper.handleActivityResult(requestCode, resultCode, data)) {
			Log.d(TAG, "googlepay_result");
			super.onActivityResult(requestCode, resultCode, data);
		} else {
			Log.d(TAG, "onActivityResult handled by IABUtil.");
		}
	}

	String[] m_skus;

	/*public void returnPrice(String m_Sku) {
		m_skus = m_Sku.split("_");
		List<String> productNameList = new ArrayList<String>();
		for (int i = 0; i < m_skus.length; i++)
		{
			productNameList.add(m_skus[i]);
		}

		mHelper.queryInventoryAsync(true, productNameList, new IabHelper.QueryInventoryFinishedListener() {
			@Override
			public void onQueryInventoryFinished(IabResult result, Inventory inventory) {
				String id_price = "";
				try {
					for (int i = 0; i < m_skus.length; i++) {
						if (inventory.getSkuDetails(m_skus[i]) != null) {
							id_price += inventory.getSkuDetails(m_skus[i]).getSku() + "_" + inventory.getSkuDetails(m_skus[i]).getPrice() + "?";
						}
					}
					UnityPlayer.UnitySendMessage("game_node", "get_price", id_price);
				}catch(Exception e){
					Log.d("unity", e.toString());
				}
			}
		});
	}*/

	@Override
    public void game_pay(String body)
    {
    	 String[] s = body.split("_"); 
    	 SKU_GAS = s[3];
    	 Log.d("unity", body);
    	String payload = s[0] + "_" + s[1] + "_" + s[2] + "_" + s[4] + "_" + s[5];
    	 Log.d("unity", payload);
    	 mHelper.launchPurchaseFlow(this, SKU_GAS, RC_REQUEST,
                 mPurchaseFinishedListener, payload);
    }

	@Override
    public void game_logout()
    {
    	UnityPlayer.UnitySendMessage("game_node", "platform_logout","");
    }

	public String getPublicBaseKey() throws NameNotFoundException
	{
		ApplicationInfo appInfo = this.getPackageManager().getApplicationInfo(getPackageName(), PackageManager.GET_META_DATA);
		String s = appInfo.metaData.getString("publicbasekey");
		Log.d("unity", s);
		return s + "";

	}
}
