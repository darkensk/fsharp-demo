// Setup random amount for ApplePay mock items
const RANDOM_AMOUNT = Math.random().toFixed(2) * 100;
const AMOUNT = RANDOM_AMOUNT.toString();

async function swapTokens(payload) {
  let swapTokensRequest = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(payload),
  };

  const swapTokenPromise = fetch(
    "https://demo-shop.test.avarda.com/swap-tokens",
    swapTokensRequest
  ).then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  });

  return swapTokenPromise;
}

async function validateMerchant(validationUrl) {
  let authorizeMerchantRequest = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ validationUrl: validationUrl }),
  };

  const merchantSessionPromise = fetch(
    "https://demo-shop.test.avarda.com/authorize-merchant",
    authorizeMerchantRequest
  )
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .catch((error) => console.error("Error:", error));

  return merchantSessionPromise;
}

function onApplePayButtonClicked() {
  if (!ApplePaySession) {
    return;
  }

  const request = {
    countryCode: "SE",
    currencyCode: "SEK",
    merchantCapabilities: ["supports3DS"],
    supportedNetworks: ["visa", "masterCard", "amex", "discover"],
    total: {
      label: "Demo (Card is not charged)",
      type: "final",
      amount: AMOUNT,
    },
  };

  const session = new ApplePaySession(3, request);

  session.onvalidatemerchant = async (event) => {
    const validationUrl = event.validationURL;
    const merchantSession = await validateMerchant(validationUrl);

    session.completeMerchantValidation(merchantSession);
  };

  session.onpaymentmethodselected = (event) => {
    const update = {
      newTotal: {
        label: "Demo (Card is not charged)",
        amount: AMOUNT,
        type: "final",
      },
    };

    session.completePaymentMethodSelection(update);
  };

  session.onshippingmethodselected = (event) => {
    // Define ApplePayShippingMethodUpdate based on the selected shipping method.
    // No updates or errors are needed, pass an empty object.
    const update = {};
    session.completeShippingMethodSelection(update);
  };

  session.onshippingcontactselected = (event) => {
    // Define ApplePayShippingContactUpdate based on the selected shipping contact.
    const update = {};
    session.completeShippingContactSelection(update);
  };

  session.onpaymentauthorized = async (event) => {
    // Define ApplePayPaymentAuthorizationResult
    const result = {
      status: ApplePaySession.STATUS_SUCCESS,
    };

    session.completePayment(result);

    const paymentData = event.payment.token.paymentData;
    const swapTokenPayload = { type: "applepay", tokenData: paymentData };

    const swapTokenResponse = await swapTokens(swapTokenPayload);

    window.document.getElementById("tokenized-apple-pay-token").value =
      JSON.stringify(swapTokenResponse);
  };

  session.oncouponcodechanged = (event) => {
    // Define ApplePayCouponCodeUpdate
    const newTotal = calculateNewTotal(event.couponCode);
    const newLineItems = calculateNewLineItems(event.couponCode);
    const newShippingMethods = calculateNewShippingMethods(event.couponCode);
    const errors = calculateErrors(event.couponCode);

    session.completeCouponCodeChange({
      newTotal: newTotal,
      newLineItems: newLineItems,
      newShippingMethods: newShippingMethods,
      errors: errors,
    });
  };

  session.oncancel = (event) => {
    // Payment canceled by WebKit
    console.log(event);
    console.dir(event, null);
  };

  session.begin();
}
