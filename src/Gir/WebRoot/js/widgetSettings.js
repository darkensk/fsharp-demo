const buttonAprWidget = document.querySelector('[data-widget-name="avarda-apr-widget"] .btn');
const buttonPaymentWidget = document.querySelector('[data-widget-name="avarda-payment-widget"] .btn');

const changeAprWidgetAttributes = () => {
  const aprWidgetElement = document.querySelector('avarda-apr-widget');

  if (!aprWidgetElement) return;

  const settings = [...document.querySelectorAll('[data-widget-name="avarda-apr-widget"] input[type="radio"]')]
  const accountClass = document.querySelector('[data-widget-name="avarda-apr-widget"] input[id="account-class"]').value;
  const price = document.querySelector('[data-widget-name="avarda-apr-widget"] input[id="price"]').value;

  const checkedPaymentMethod = settings.find(item => {
    return item.checked;
  })

  if (accountClass === "") {
    aprWidgetElement.removeAttribute('account-class');
  } else {
    aprWidgetElement.setAttribute('account-class', accountClass);
  }

  if (price !== "") {
    aprWidgetElement.setAttribute('price', price)
  }

  if (checkedPaymentMethod) {
    aprWidgetElement.setAttribute('payment-method', checkedPaymentMethod.value)
  }
}

const changePaymentWidgetAttributes = () => {
  const paymentWidgetElement = document.querySelector('avarda-payment-widget');

  if (!paymentWidgetElement) return;

  const accountClass = document.querySelector('[data-widget-name="avarda-payment-widget"] input[type="text"]').value;

  if (accountClass === "") {
    paymentWidgetElement.removeAttribute('account-class');
  } else {
    paymentWidgetElement.setAttribute('account-class', accountClass);
  }
}

if (buttonAprWidget) buttonAprWidget.addEventListener('click', () => changeAprWidgetAttributes())
if (buttonPaymentWidget) buttonPaymentWidget.addEventListener('click', () => changePaymentWidgetAttributes())
