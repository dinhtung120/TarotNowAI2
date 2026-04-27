(() => {
  if (typeof window === "undefined") {
    return;
  }

  const resolveNonce = () => {
    const currentScript = document.currentScript;
    if (currentScript && typeof currentScript.nonce === "string" && currentScript.nonce.length > 0) {
      return currentScript.nonce;
    }

    const firstNonceScript = document.querySelector("script[nonce]");
    if (firstNonceScript && typeof firstNonceScript.nonce === "string" && firstNonceScript.nonce.length > 0) {
      return firstNonceScript.nonce;
    }

    return undefined;
  };

  let cachedNonce =
    typeof window.__nonce__ === "string" && window.__nonce__.length > 0
      ? window.__nonce__
      : undefined;

  const lockNonce = (value) => {
    if (!value) {
      return;
    }

    cachedNonce = value;
    Object.defineProperty(window, "__nonce__", {
      value,
      writable: true,
      configurable: true,
    });
  };

  if (cachedNonce) {
    lockNonce(cachedNonce);
    return;
  }

  Object.defineProperty(window, "__nonce__", {
    configurable: true,
    enumerable: false,
    get() {
      const nonce = cachedNonce ?? resolveNonce();
      if (nonce) {
        lockNonce(nonce);
      }
      return nonce;
    },
    set(value) {
      if (typeof value === "string" && value.length > 0) {
        lockNonce(value);
      }
    },
  });

  const eagerNonce = resolveNonce();
  if (eagerNonce) {
    lockNonce(eagerNonce);
  }
})();
