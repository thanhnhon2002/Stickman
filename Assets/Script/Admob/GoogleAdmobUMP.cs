//using GoogleMobileAds.Api;
//using GoogleMobileAds.Ump.Api;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GoogleAdmobUMP : MonoBehaviour
//{
//    void Start()
//    {
//        // Create a ConsentRequestParameters object.
//        ConsentRequestParameters request = new ConsentRequestParameters();
//        // Check the current consent information status.
//        ConsentInformation.Update(request, OnConsentInfoUpdated);
//    }

//    void OnConsentInfoUpdated(FormError consentError)
//    {
//        if (consentError != null)
//        {
//            // Handle the error.
//            UnityEngine.Debug.LogError(consentError);
//            return;
//        }

//        // If the error is null, the consent information state was updated.
//        // You are now ready to check if a form is available.
//        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
//        {
//            if (formError != null)
//            {
//                // Consent gathering failed.
//                UnityEngine.Debug.LogError(consentError);
//                return;
//            }

//            // Consent has been gathered.
//            if (ConsentInformation.CanRequestAds())
//            {
//                MobileAds.Initialize((InitializationStatus initstatus) =>
//                {
//                    // TODO: Request an ad.
//                });
//            }
//        });

//    }
//}