async function onApplePayButtonClicked() {
  // Consider falling back to Apple Pay JS if Payment Request is not available.
  if (!PaymentRequest) {
    console.log("Payment Request not available");
    return;
  }

  try {
    // Define PaymentMethodData
    const paymentMethodData = [
      {
        supportedMethods: "https://apple.com/apple-pay",
        data: {
          version: 3,
          merchantIdentifier: "merchant.com.avarda.sandbox",
          merchantCapabilities: ["supports3DS"],
          supportedNetworks: ["masterCard", "visa"],
          countryCode: "SE",
        },
      },
    ];
    // Define PaymentDetails
    const paymentDetails = {
      total: {
        label: "Demo (Card is not charged)",
        amount: {
          value: "27.50",
          currency: "SEK",
        },
      },
    };
    // Define PaymentOptions
    const paymentOptions = {
      requestPayerName: false,
      requestBillingAddress: false,
      requestPayerEmail: false,
      requestPayerPhone: false,
      requestShipping: false,
      shippingType: "shipping",
    };

    // Create PaymentRequest
    const request = new PaymentRequest(
      paymentMethodData,
      paymentDetails,
      paymentOptions
    );

    request.onmerchantvalidation = (event) => {
      let validationUrl_ = event.validationURL;

      let authorizeMerchantRequest = {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ validationUrl: validationUrl_ }),
      };

      const merchantSessionPromise = fetch(
        // For local development purposes
        // "https://checkout-dot-com.local/api/apple/payment-session",
        "https://demo-shop.test.avarda.com/authorize-merchant",
        authorizeMerchantRequest
      )
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
          }
          return response;
        })
        .catch((error) => console.error("Error:", error));

      event.complete(merchantSessionPromise);
    };

    request.onpaymentmethodchange = (event) => {
      if (event.methodDetails.type !== undefined) {
        // Define PaymentDetailsUpdate based on the selected payment method.
        // No updates or errors needed, pass an object with the same total.
        const paymentDetailsUpdate = {
          total: paymentDetails.total,
        };
        event.updateWith(paymentDetailsUpdate);
      } else if (event.methodDetails.couponCode !== undefined) {
        // Define PaymentDetailsUpdate based on the coupon code.
        const total = calculateTotal(event.methodDetails.couponCode);
        const displayItems = calculateDisplayItem(
          event.methodDetails.couponCode
        );
        const shippingOptions = calculateShippingOptions(
          event.methodDetails.couponCode
        );
        const error = calculateError(event.methodDetails.couponCode);

        event.updateWith({
          total: total,
          displayItems: displayItems,
          shippingOptions: shippingOptions,
          modifiers: [
            {
              data: {
                additionalShippingMethods: shippingOptions,
              },
            },
          ],
          error: error,
        });
      }
    };

    request.onshippingoptionchange = (event) => {
      // Define PaymentDetailsUpdate based on the selected shipping option.
      // No updates or errors needed, pass an object with the same total.
      const paymentDetailsUpdate = {
        total: paymentDetails.total,
      };
      event.updateWith(paymentDetailsUpdate);
    };

    request.onshippingaddresschange = (event) => {
      // Define PaymentDetailsUpdate based on a shipping address change.
      const paymentDetailsUpdate = {};
      event.updateWith(paymentDetailsUpdate);
    };

    const response = await request.show();
    console.log("--FINAL RESPONSE--");
    console.log(response);
    console.dir(response, null);

    const appleResponseRawTextArea = document.getElementById("apple-response");
    appleResponseRawTextArea.value = JSON.stringify(response);

    const status = "success";
    const completeResult = await response.complete(status);
    console.log(completeResult);
  } catch (e) {
    console.log(e);
  }
}
