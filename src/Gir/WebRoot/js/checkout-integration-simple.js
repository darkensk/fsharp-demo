const initCheckout = (checkoutFrontendBundle, purchaseJwt) => {
  (function (e, t, n, a, s, c, o, i, r) {
    e[a] =
      e[a] ||
      function () {
        (e[a].q = e[a].q || []).push(arguments);
      };
    e[a].i = s;
    i = t.createElement(n);
    i.async = 1;
    i.src = o + "?v=" + c + "&ts=" + 1 * new Date();
    r = t.getElementsByTagName(n)[0];
    r.parentNode.insertBefore(i, r);
  })(
    window,
    document,
    "script",
    "avardaCheckoutInit",
    "avardaCheckout",
    "1.0.0",
    checkoutFrontendBundle
  );

  var handleByMerchantCallback = function (avardaCheckoutInstance) {
    console.log("Handle external payment here");

    // Un-mount Checkout 3.0 frontend app from the page when external payment is handled
    avardaCheckoutInstance.unmount();
    // Display success message instead of Checkout 3.0 frontend application
    document.getElementById("checkout-form").innerHTML =
      "<br><h2>External payment handled by partner!</h2><br>";
  };

  var completedPurchaseCallback = function (avardaCheckoutInstance) {
    console.log("Purchase Completed Successfully - Handle here!");

    // Un-mount Checkout 3.0 frontend app from the page
    avardaCheckoutInstance.unmount();
    // Display success message instead of Checkout 3.0 frontend application
    document.getElementById("checkout-form").innerHTML =
      "<br><h2>Purchase Completed Successfully!</h2><br>";
  };

  var sessionTimedOutCallback = function (avardaCheckoutInstance) {
    console.log("Session Timed Out - Handle here!");

    // Un-mount Checkout 3.0 frontend app from the page
    avardaCheckoutInstance.unmount();
    // Display success message instead of Checkout 3.0 frontend application
    document.getElementById("checkout-form").innerHTML =
      "<br><h2>Session Timed Out - handled by partner!</h2><br>";
  };

  const redirectUrlCallback = () => window.location.href;

  window.avardaCheckoutInit({
    purchaseJwt: purchaseJwt,
    rootElementId: "checkout-form",
    redirectUrl: redirectUrlCallback,
    styles: {},
    handleByMerchantCallback: handleByMerchantCallback,
    completedPurchaseCallback: completedPurchaseCallback,
    sessionTimedOutCallback: sessionTimedOutCallback,
  });
};
