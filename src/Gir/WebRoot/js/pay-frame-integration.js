const initPayFrame = (function (e, t, n, a, o, i) {
  e[a] =
    e[a] ||
    function () {
      (e[a].q = e[a].q || []).push(arguments);
    };
  i = t.createElement(n);
  i.async = 1;
  i.type = 'module';
  i.src = o + '?ts=' + Date.now();
  t.body.prepend(i);
})(
  window,
  document,
  'script',
  'avardaPayFrameInit',
  'https://pay-frame.test.avarda.com/cdn/pay-frame.js',
);

window.avardaPayFrameInit({
  domain: 'se',
  rootNode: document.getElementById('pay-frame'),
  siteKey: '0aaef98d-0ab8-437b-8851-61f1e5a73589',
  language: 'en',
});