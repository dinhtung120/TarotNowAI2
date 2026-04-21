const FACEBOOK_HOSTS = new Set([
 'facebook.com',
 'www.facebook.com',
 'm.facebook.com',
 'fb.com',
 'www.fb.com',
]);

const INSTAGRAM_HOSTS = new Set([
 'instagram.com',
 'www.instagram.com',
]);

const TIKTOK_HOSTS = new Set([
 'tiktok.com',
 'www.tiktok.com',
 'm.tiktok.com',
 'vt.tiktok.com',
]);

const ACCEPTED_PROTOCOLS = new Set(['http:', 'https:']);

export interface ReaderSocialLinksInput {
 facebookUrl?: string | null;
 instagramUrl?: string | null;
 tikTokUrl?: string | null;
}

export function normalizeOptionalSocialUrl(value: string | null | undefined): string {
 return value?.trim() ?? '';
}

export function hasAtLeastOneSocialLink(links: ReaderSocialLinksInput): boolean {
 return (
  normalizeOptionalSocialUrl(links.facebookUrl).length > 0
  || normalizeOptionalSocialUrl(links.instagramUrl).length > 0
  || normalizeOptionalSocialUrl(links.tikTokUrl).length > 0
 );
}

export function isValidFacebookUrl(url: string | null | undefined): boolean {
 return isValidPlatformUrl(url, FACEBOOK_HOSTS);
}

export function isValidInstagramUrl(url: string | null | undefined): boolean {
 return isValidPlatformUrl(url, INSTAGRAM_HOSTS);
}

export function isValidTikTokUrl(url: string | null | undefined): boolean {
 return isValidPlatformUrl(url, TIKTOK_HOSTS);
}

function isValidPlatformUrl(
 rawUrl: string | null | undefined,
 supportedHosts: ReadonlySet<string>,
): boolean {
 const url = normalizeOptionalSocialUrl(rawUrl);
 if (url.length === 0) {
  return true;
 }

 try {
  const parsed = new URL(url);
  return ACCEPTED_PROTOCOLS.has(parsed.protocol) && supportedHosts.has(parsed.hostname);
 } catch {
  return false;
 }
}
