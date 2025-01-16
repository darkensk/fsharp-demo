const initPayFrame = (payFrameBundleUrl, siteKey) => {
  (function (e, t, n, a, o, i) {
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
    'avardaMyPagesWidgetInit',
    payFrameBundleUrl,
  );

  window.avardaMyPagesWidgetInit({
    domain: 'se',
    rootNode: document.getElementById('pay-frame'),
    siteKey: siteKey,
    language: 'en',
  })
}