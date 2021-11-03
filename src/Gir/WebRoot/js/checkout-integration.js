const styles = {
  buttons: {
    primary: {
      base: {
        backgroundColor: "#fbb710",
        color: "#ffffff",
        borderColor: "#fbb710",
      },
      hover: {
        backgroundColor: "#131212",
        borderColor: "#131212",
        color: "#ffffff",
      },
      boxShadow: {
        hOffset: 0,
        vOffset: 0,
        blur: 0,
        spread: 0,
        color: "#000000",
      },
      disabled: {
        backgroundColor: "#FDE9B7",
        borderColor: "#FDE9B7",
        color: "#ffffff",
      },
      borderWidth: 0,
      fontSize: 18,
      lineHeight: 56,
      minHeight: 55,
      fontWeight: 400,
      padding: {
        top: 10,
        right: 16,
        bottom: 10,
        left: 16,
      },
      borderRadius: 0,
    },
    secondary: {
      base: {
        backgroundColor: "#131212",
        borderColor: "#131212",
        color: "#ffffff",
      },
      hover: {
        backgroundColor: "#131212",
        borderColor: "#131212",
        color: "#ffffff",
      },
      boxShadow: {
        hOffset: 0,
        vOffset: 0,
        blur: 0,
        spread: 0,
        color: "#000000",
      },
      disabled: {
        backgroundColor: "#FDE9B7",
        borderColor: "#FDE9B7",
        color: "#ffffff",
      },
      borderWidth: 0,
      fontSize: 18,
      lineHeight: 56,
      minHeight: 55,
      fontWeight: 400,
      padding: {
        top: 10,
        right: 16,
        bottom: 10,
        left: 16,
      },
      borderRadius: 0,
    },
  },
  fontFamilies: ["HelveticaNeue-Medium", "sans-serif"],
  headings: {
    h1: {
      fontSize: 30,
      lineHeight: 32,
      display: "block",
      fontWeight: 400,
      color: "#000000",
      alignment: "left",
    },
    h2: {
      fontSize: 30,
      lineHeight: 32,
      display: "block",
      fontWeight: 400,
      color: "#000000",
      alignment: "left",
    },
    h3: {
      fontSize: 18,
      lineHeight: 20,
      display: "block",
      fontWeight: 400,
      color: "#000000",
      alignment: "left",
    },
    h4: {
      fontSize: 16,
      lineHeight: 16,
      display: "block",
      fontWeight: 400,
      color: "#000000",
      alignment: "left",
    },
  },
  input: {
    height: 50,
    fontSize: 16,
    fontWeight: 400,
    backgroundColorValid: "#ffffff",
    backgroundColorInvalid: "#fee7e7",
    borderColor: "#b2b2b2",
    borderWidth: 1,
    borderRadius: 0,
    focusOutlineColor: "#fbb710",
    disabled: {
      backgroundColor: "#f5f5f5",
      borderColor: "#aeaeae",
      color: "#aeaeae",
    },
    placeholderColor: "#aeaeae",
  },
  links: {
    default: {
      fontSize: 13,
      fontWeight: 400,
      color: "#777777",
      textDecoration: "underline",
      hover: {
        color: "#fbb710",
        textDecoration: "underline",
      },
      disabled: {
        color: "#aeaeae",
        textDecoration: "underline",
      },
    },
    blue: {
      fontSize: 13,
      fontWeight: 400,
      color: "#131212",
      textDecoration: "underline",
      hover: {
        color: "#fbb710",
        textDecoration: "underline",
      },
      disabled: {
        color: "#aeaeae",
        textDecoration: "underline",
      },
    },
    biggerBlue: {
      fontSize: 16,
      fontWeight: 400,
      color: "#131212",
      textDecoration: "underline",
      hover: {
        color: "#fbb710",
        textDecoration: "underline",
      },
      disabled: {
        color: "#aeaeae",
        textDecoration: "underline",
      },
    },
    smallNoDecoration: {
      fontSize: 11,
      fontWeight: 400,
      color: "#4b4b4b",
      textDecoration: "none",
      hover: {
        color: "#fbb710",
        textDecoration: "none",
      },
      disabled: {
        color: "#aeaeae",
        textDecoration: "underline",
      },
    },
  },
  select: {
    base: {
      backgroundColor: "#ffffff",
      color: "#4b4b4b",
      borderColor: "#b2b2b2",
      selectArrowUrl:
        "https://avdonl0p0documentation.blob.core.windows.net/static/default_selectArrow.svg",
    },
    disabled: {
      backgroundColor: "#f5f5f5",
      color: "#aeaeae",
      borderColor: "#aeaeae",
      selectArrowUrl:
        "https://avdonl0p0documentation.blob.core.windows.net/static/default_selectArrow.svg",
    },
    fontSize: 16,
    lineHeight: 30,
    height: 50,
    borderWidth: 1,
    fontWeight: 400,
  },
  labels: {
    active: {
      color: "#fbb710",
    },
    disabled: {
      color: "#aeaeae",
    },
    error: {
      color: "#e20000",
    },
  },
  footer: {
    fontSize: 13,
    fontWeight: 400,
    color: "#b2b2b2",
  },
  icons: {
    labelColor: "#fbb710",
    card: {
      color: "#fbb710",
      width: 91,
      height: 24,
    },
    loanPayment: {
      color: "#fbb710",
      width: 30,
      height: 30,
    },
    partPayment: {
      color: "#fbb710",
      width: 30,
      height: 30,
    },
    invoice: {
      color: "#fbb710",
      width: 30,
      height: 30,
    },
    directBank: {
      color: "#fbb710",
      width: 30,
      height: 30,
    },
    payOnDelivery: { color: "#fbb710", width: 30, height: 30 },
  },
  paymentMethods: {
    selected: {
      labelColor: "#fbb710",
      borderColor: "#fbb710",
      borderWidth: 1,
      backgroundColor: "#FEFAF0",
      partPaymentPaymentTermSelect: {
        selected: {
          labelColor: "#131212",
          borderColor: "#131212",
          backgroundColor: "#FDE9B7",
        },
        unselected: {
          labelColor: "#000000",
          borderColor: "#b2b2b2",
          backgroundColor: "#ffffff",
        },
      },
      bulletIconColor: "#00a0ba",
      radioButtonColor: "#fbb710",
    },
    unselected: {
      labelColor: "#000000",
      backgroundColor: "",
      borderColor: "#b2b2b2",
      radioButtonColor: "#fbb710",
    },
  },
  amountToPayColor: "#fbb710",
  backgroundBorderRadius: 0,
  commonBorderColor: "#b2b2b2",
  paymentSection: { activeBorderWidth: 2, borderRadius: 5 },
  checkbox: {
    primary: {
      width: 20,
      height: 20,
      checked: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryChecked.svg",
      },
      unchecked: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryUnchecked.svg",
      },
      checkedDisabled: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryCheckedDisabled.svg",
      },
      uncheckedDisabled: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxPrimaryUncheckedDisabled.svg",
      },
      checkedInvalid: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedChecked.svg",
      },
      uncheckedInvalid: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedUnchecked.svg",
      },
      focusOutlineColor: "#fbb710",
      checkedLabelColor: "#fbb710",
    },
    secondary: {
      width: 20,
      height: 20,
      checked: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryChecked.svg",
      },
      unchecked: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryUnchecked.svg",
      },
      checkedDisabled: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryCheckedDisabled.svg",
      },
      uncheckedDisabled: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxSecondaryUncheckedDisabled.svg",
      },
      checkedInvalid: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedChecked.svg",
      },
      uncheckedInvalid: {
        backgroundUrl:
          "https://avdonl0p0documentation.blob.core.windows.net/static/custom_giraffe_CheckboxRedUnchecked.svg",
      },
      focusOutlineColor: "#fbb710",
      checkedLabelColor: "#fbb710",
    },
  },
  spinnerColor: "#aeaeae",
};

const initCheckout = (
  checkoutFrontendBundle,
  purchaseJwt,
  disableFocus,
  useCustomStyles,
  isBeforeSubmitCallbackEnabled,
  isDeliveryAddressChangedCallbackEnabled
) => {
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

  const completedUrl = window.location.origin + "/cart/completed";
  const sessionExpiredUrl = window.location.origin + "/cart/sessionExpired";
  const cartUrl = window.location.origin + "/cart/";

  const completedCallback = () => {
    fetch(completedUrl)
      .then((response) => {
        return response.text();
      })
      .then((data) => {
        console.log(data);
        window.location.replace(cartUrl);
      });
  };

  var handleByMerchantCallback = function (avardaCheckoutInstance) {
    console.log("Handle external payment here");

    // Un-mount Checkout 3.0 frontend app from the page when external payment is handled
    avardaCheckoutInstance.unmount();
    // Display success message instead of Checkout 3.0 frontend application
    document.getElementById("checkout-form").innerHTML =
      "<br><h2>External payment handled by partner!</h2><br>";

    completedCallback();
  };

  const sessionExpired = () => {
    fetch(sessionExpiredUrl)
      .then((response) => {
        return response.text();
      })
      .then((data) => {
        console.log(data);
        window.location.replace(cartUrl);
      });
  };

  var sessionTimedOutCallback = function (avardaCheckoutInstance) {
    console.log("Session Timed Out - Handle here!");

    // Un-mount Checkout 3.0 frontend app from the page
    avardaCheckoutInstance.unmount();
    // Start Session Expired handling process
    sessionExpired();
  };

  const redirectUrlCallback = () =>
    window.location.origin + "/cart/#checkout-form";

  var beforeSubmitCallback = function (
    { zip, country, selectedPaymentMethodId },
    checkoutInstance
  ) {
    var confirmAlert = confirm(
      `Before Submit Callback Triggered:\nZip: "${zip}"\nCountry: "${country}"\nSelectedPaymentMethod: "${selectedPaymentMethodId}"\nPress 'OK' to continue or 'Cancel' to abort!`
    );
    if (confirmAlert == true) {
      checkoutInstance.beforeSubmitContinue();
    } else {
      checkoutInstance.beforeSubmitAbort();
    }
  };

  var deliveryAddressChangedCallback = function (
    { zip, country },
    checkoutInstance
  ) {
    var confirmAlert = confirm(
      `Delivery Address Changed Callback Triggered:\nZip: "${zip}"\nCountry: "${country}"\nPress 'OK' to continue or 'Cancel' to abort!`
    );
    if (confirmAlert == true) {
      checkoutInstance.deliveryAddressChangedContinue();
    } else {
      checkoutInstance.refreshForm();
    }
  };

  window.avardaCheckoutInit({
    purchaseJwt: purchaseJwt,
    rootElementId: "checkout-form",
    redirectUrl: redirectUrlCallback,
    styles: useCustomStyles ? styles : {},
    disableFocus: disableFocus,
    handleByMerchantCallback: handleByMerchantCallback,
    completedPurchaseCallback: completedCallback,
    sessionTimedOutCallback: sessionTimedOutCallback,
    beforeSubmitCallback: isBeforeSubmitCallbackEnabled
      ? beforeSubmitCallback
      : false,
    deliveryAddressChangedCallback: isDeliveryAddressChangedCallbackEnabled
      ? deliveryAddressChangedCallback
      : false,
  });
};
