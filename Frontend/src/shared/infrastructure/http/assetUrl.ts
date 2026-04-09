const UPLOADS_PREFIX = '/uploads/';
const API_UPLOADS_PREFIX = '/api/v1/uploads/';
const HTTP_URL_REGEX = /^https?:\/\//i;

function normalizeUploadsPath(pathname: string): string | null {
  const lowerPath = pathname.toLowerCase();
  const uploadsIndex = lowerPath.indexOf(UPLOADS_PREFIX);
  if (uploadsIndex >= 0) {
    return pathname.slice(uploadsIndex);
  }

  const apiUploadsIndex = lowerPath.indexOf(API_UPLOADS_PREFIX);
  if (apiUploadsIndex >= 0) {
    const suffix = pathname
      .slice(apiUploadsIndex + API_UPLOADS_PREFIX.length)
      .replace(/^\/+/, '');
    return `${UPLOADS_PREFIX}${suffix}`;
  }

  return null;
}

export function resolveAvatarUrl(value: string | null | undefined): string | null {
  if (!value) return null;
  const raw = value.trim();
  if (!raw) return null;

  if (raw.startsWith('blob:') || raw.startsWith('data:')) return raw;
  if (raw.startsWith(UPLOADS_PREFIX)) return raw;
  if (raw.startsWith('uploads/')) return `/${raw}`;
  if (raw.startsWith(API_UPLOADS_PREFIX)) {
    return raw.replace(API_UPLOADS_PREFIX, UPLOADS_PREFIX);
  }
  if (/^api\/v1\/uploads\//i.test(raw)) {
    return `/${raw.replace(/^api\/v1\//i, '')}`;
  }

  if (!HTTP_URL_REGEX.test(raw)) return raw;

  try {
    const parsed = new URL(raw);
    const uploadsPath = normalizeUploadsPath(parsed.pathname);
    if (!uploadsPath) return raw;
    return `${uploadsPath}${parsed.search}${parsed.hash}`;
  } catch {
    return raw;
  }
}
