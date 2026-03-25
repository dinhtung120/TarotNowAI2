export type JwtPayload = Record<string, unknown> & {
 exp?: number | string;
};

function base64UrlDecodeToUtf8(base64Url: string): string {
 const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
 const padded = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');

 if (typeof atob !== 'function') {
  throw new Error('Base64 decoder is not available in this runtime');
 }

 const binary = atob(padded);
 const bytes = Uint8Array.from(binary, (char) => char.charCodeAt(0));
 return new TextDecoder().decode(bytes);
}

export function tryDecodeJwtPayload(token: string): JwtPayload | null {
 if (!token) return null;
 const parts = token.split('.');
 if (parts.length < 2) return null;

 try {
  const json = base64UrlDecodeToUtf8(parts[1] ?? '');
  return JSON.parse(json) as JwtPayload;
 } catch {
  return null;
 }
}

export function getJwtExpiryMs(token: string): number | null {
 const payload = tryDecodeJwtPayload(token);
 const exp = payload?.exp;
 if (typeof exp === 'number' && Number.isFinite(exp)) return exp * 1000;
 if (typeof exp === 'string') {
  const parsed = Number(exp);
  if (Number.isFinite(parsed)) return parsed * 1000;
 }

 return null;
}

export function isJwtExpired(token: string, leewaySeconds = 0): boolean {
 const expMs = getJwtExpiryMs(token);
 if (!expMs) return true;
 const leewayMs = Math.max(0, leewaySeconds) * 1000;
 return Date.now() >= expMs - leewayMs;
}
